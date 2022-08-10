using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace bebaSpace.AlphabetMiniGame
{
    public class GameController : MonoBehaviour
    {
        [Header("Audio Properties")]
        [SerializeField] private string zenModeBgmName;
        [SerializeField] private string timedAttackBgmName;
        [SerializeField] private string nextRoundButtonClicked;

        [Header("Game Properties")]
        [SerializeField] private int correctAnswers = 5;
        [SerializeField] private int puzzlePerRound = 5;
        [SerializeField] private int maxCoins = 100;

        [Header("Placeholder Properties")]
        [SerializeField] private List<ClickableLetter> clickableLetters = new List<ClickableLetter>();
        [SerializeField] private List<DisplayLetter> displayLetters = new List<DisplayLetter>();
        [SerializeField] private GameObject timerDisplay;

        private char letter;
        private int correctClicks = 0;
        private int currentRound = 0;
        private int currentRoundClicks = 0;
        private int totalRoundsCompletedTimed = 0;
        private float percentage;

        private List<char> charsList = new List<char>();
        private Queue<char> previousChar = new Queue<char>();

        private MenuManager menuManager;
        private RemainingCounter remainingCounter;

        private CanvasGroup timerDisplayCanvasGroup;

        public char Letter { get => letter; private set { } }
        public int CurrentRoundClicks { get => currentRoundClicks; set => currentRoundClicks = value; }



        #region Instance declaration
        private static GameController _instance;

        public static GameController Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    _instance = obj.AddComponent<GameController>();
                    obj.name = typeof(GameController).ToString();
                }

                return _instance;
            }
        }
        #endregion

        #region Awake
        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }

            if (GameManager.Instance.ZenMode)
            {
                SoundManager.Instance.PlaySound(zenModeBgmName);
            }
            else
            {
                SoundManager.Instance.PlaySound(timedAttackBgmName);
            }
        }
        #endregion

  
        private void Start()
        {
            menuManager = GetComponent<MenuManager>();
            remainingCounter = GetComponent<RemainingCounter>();

            timerDisplayCanvasGroup = timerDisplay.GetComponent<CanvasGroup>();

            InitiateNewRound();
           
        }

        private void ShowTimerDisplay()
        {
            timerDisplayCanvasGroup.alpha = 1;
        }

        private void GenerateBoard()
        {
            for (int i = 0; i < correctAnswers; i++)
            {
                charsList.Add(letter);
            }

            for (int i = correctAnswers; i < clickableLetters.Count; i++)
            {
                var chosenLetter = GenerateInvalidRandomLetter();
                charsList.Add(chosenLetter);
            }

            ShuffleAnswerChoicesList();

            for (int i = 0; i < clickableLetters.Count; i++)
            {
                clickableLetters[i].SetLetter(charsList[i]);
                clickableLetters[i].gameObject.transform.DOPunchScale(new Vector3(1, 1, 1), Random.Range(0.05f,0.8f), 2, 0.3f).SetEase(Ease.InOutBounce);
            }

        }

        private char GenerateInvalidRandomLetter()
        {
            int a = Random.Range(0, 26);
            var randomLetter = (char)('a' + a);

            while( randomLetter == Letter)
            {
                a = Random.Range(0, 26);
                randomLetter = (char)('a' + a);
            }

            return randomLetter;

        }

        private char GenerateRandomLetter()
        {
            int a = Random.Range(0, 26);
            var randomLetter = (char)('a' + a);

            while (previousChar.Contains(randomLetter))
            {
                a = Random.Range(0, 26);
                randomLetter = (char)('a' + a);
            }

            if(previousChar.Count == 15)
                previousChar.Dequeue();

            previousChar.Enqueue(randomLetter);

            return randomLetter;
        }

        private void UpdateDisplayLetters()
        {
            foreach (var displayletter in displayLetters)
            {
                displayletter.SetLetter(Letter);
            }
        }

        private void DisableDisplayLettersSound()
        {
            foreach (var displayletter in displayLetters)
            {
                displayletter.DisableSound();
            }
        }

        private void ShuffleAnswerChoicesList()
        {
            for (int i = 0; i < charsList.Count; i++)
            {
                char temp = charsList[i];
                int randomIndex = Random.Range(i, charsList.Count);
                charsList[i] = charsList[randomIndex];
                charsList[randomIndex] = temp;
            }
        }

        private void DisableAllClickableLetters()
        {
            for (int i = 0; i < clickableLetters.Count; i++)
            {
                if (clickableLetters[i].isActiveAndEnabled)
                {
                    clickableLetters[i].enabled = false;
                }
            }
        }

        private void ShowEndPuzzleMenu()
        {

            SoundManager.Instance.PlaySound("RoundWin");
            DisableAllClickableLetters();
            DisableDisplayLettersSound();

            //just a precaution to avoid division by 0, if somehow player did not click on anything and the game ends with 0 click.
            if (currentRoundClicks == 0) { currentRoundClicks = 1; }

            if (!GameManager.Instance.ZenMode)
            {
                percentage = ((float)correctAnswers * totalRoundsCompletedTimed / currentRoundClicks) * 100;
            }
            else
            {
                percentage = ((float)correctAnswers * puzzlePerRound / currentRoundClicks) * 100;
            }

            int _coinsEarned = CalculateCoinsEarned();

            GameManager.Instance.CurrentGameData.TotalGamesInitiated++;

            if(totalRoundsCompletedTimed > GameManager.Instance.CurrentGameData.ChallengeModeBest)
            {
                GameManager.Instance.CurrentGameData.ChallengeModeBest = totalRoundsCompletedTimed;
            }

            GameManager.Instance.UpdateGem(_coinsEarned);

            menuManager.ShowEndLevelMenu(true,_coinsEarned);
        }

        private int CalculateCoinsEarned()
        {
            float coinsEarned = percentage / 100 * maxCoins;

            if (!GameManager.Instance.ZenMode)
            {
                coinsEarned *= 2;
            }

            return Mathf.RoundToInt(coinsEarned);
        }
        
        private void NewLetterPuzzle()
        {
            charsList.Clear();
            correctClicks = 0;

            remainingCounter.SetRemainingText(correctAnswers - correctClicks);

            letter = GenerateRandomLetter();

            UpdateDisplayLetters();
            GenerateBoard();
        }

        private void TimedChallengeEnded()
        {
            GameEvents.TimesUp -= TimedChallengeEnded;
            Invoke(nameof(ShowEndPuzzleMenu), 0.1f);
        }

        public void InitiateNewRound()
        {
            if (!GameManager.Instance.ZenMode)
            {
                //timed challenge mode
                puzzlePerRound = 2306; //some big number doesn't matter. 23/6 is my birthdate :)

                totalRoundsCompletedTimed = 0;
                ShowTimerDisplay();

                GameEvents.TimesUp += TimedChallengeEnded;
            }

            NewLetterPuzzle();
        }

        public void HandleCorrectLetterClick()
        {
            correctClicks++;
            
            remainingCounter.SetRemainingText(correctAnswers - correctClicks);

            if (correctClicks == correctAnswers)
            {
                currentRound++;
                totalRoundsCompletedTimed++;
                
                if(currentRound == puzzlePerRound)
                {
                    Invoke(nameof(ShowEndPuzzleMenu), 1.2f);
                }
                else
                {
                    Invoke(nameof(NewLetterPuzzle), 0.6f);
                }
            }
        }

        public void ResetCurrentRoundCounter()
        {
            currentRound = 0;
            currentRoundClicks = 0;
        }

        public int CalculateStarsObtained()
        {
            int stars = 1;

            if (percentage <= 30) 
            { 
                stars = 1;
                GameManager.Instance.CurrentGameData.Total1StarsObtained++;

            } else if(percentage > 30 && percentage <= 55)
            {
                stars = 2;
                GameManager.Instance.CurrentGameData.Total2StarsObtained++;

            }
            else if(percentage > 55)
            {
                stars = 3;
                GameManager.Instance.CurrentGameData.Total3StarsObtained++;

            }

            return stars;
        }


        public void PlayButtonSound()
        {
            SoundManager.Instance.PlaySound(nextRoundButtonClicked);
        }

    }
}
