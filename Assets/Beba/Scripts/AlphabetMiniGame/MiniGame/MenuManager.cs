using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

namespace bebaSpace.AlphabetMiniGame
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject endLevelMenu;
        [SerializeField] private TMP_Text SuccessOrNotText;
        [SerializeField] private TMP_Text coinsEarnedText;

        [SerializeField] private GameObject star01, star02, star03;

        [SerializeField] private List<string> _3StarsText = new List<string>();
        [SerializeField] private List<string> _2StarsText = new List<string>();
        [SerializeField] private List<string> _1StarsText = new List<string>();

        private CanvasGroup endLevelMenuCanvasGroup;
        private readonly WaitForSeconds waitForSeconds = new WaitForSeconds(0.8f);
        private bool canClick = false;

        private void Start()
        {
            ShowEndLevelMenu(false);
        }

        public void ShowEndLevelMenu(bool isTrue, int coinsEarned)
        {
            endLevelMenuCanvasGroup = endLevelMenu.GetComponent<CanvasGroup>();

            if (isTrue)
            {
                endLevelMenuCanvasGroup.alpha = 1;
                endLevelMenuCanvasGroup.blocksRaycasts = true;
                endLevelMenuCanvasGroup.interactable = true;

                int stars = GameController.Instance.CalculateStarsObtained();

                StartCoroutine(ShowStarsCO(stars));
                ShowText(stars);

                coinsEarnedText.text = coinsEarned.ToString();

            }else
            {
                endLevelMenuCanvasGroup.alpha = 0;
                endLevelMenuCanvasGroup.blocksRaycasts = false;
                endLevelMenuCanvasGroup.interactable = false;

                HideAllStars();
                StopAllCoroutines();
                canClick = false;
            }

        }

        public void ShowEndLevelMenu(bool isTrue)
        {
            ShowEndLevelMenu(isTrue, 0);
        }

        private IEnumerator ShowStarsCO(int stars)
        {
            switch (stars)
            {
                case 1:
                    star01.SetActive(true);
                    canClick = true;
                    break;
                case 2:
                    star01.SetActive(true);
                    yield return waitForSeconds;
                    star02.SetActive(true);
                    canClick = true;
                    break;
                case 3:
                    star01.SetActive(true);
                    yield return waitForSeconds;
                    star02.SetActive(true);
                    yield return waitForSeconds;
                    star03.SetActive(true);
                    canClick = true;
                    break;
                default:
                    canClick = true;
                    break;
            }
        }

        private void ShowText(int stars)
        {
            int randno;

            switch (stars)
            {
                case 1:
                    randno = Random.Range(1, _1StarsText.Count);
                    SuccessOrNotText.text = _1StarsText[randno];
                    break;
                case 2:
                    randno = Random.Range(1, _2StarsText.Count);
                    SuccessOrNotText.text = _2StarsText[randno];
                    break;
                case 3:
                    randno = Random.Range(1, _3StarsText.Count);
                    SuccessOrNotText.text = _3StarsText[randno];
                    break;
                default:
                    randno = Random.Range(1, _1StarsText.Count);
                    SuccessOrNotText.text = _1StarsText[randno];
                    break;
            }
        }

        private void HideAllStars()
        {
            star01.SetActive(false);
            star02.SetActive(false);
            star03.SetActive(false);
        }

        private void PlayButtonSFX()
        {
            SoundManager.Instance.PlaySoundUntilDone("SFX_ButtonClicked");
        }

        private void PlayButtonBackSFX()
        {
            SoundManager.Instance.PlaySoundUntilDone("SFX_ButtonBack");
        }

        public void InitiateNewRound()
        {
            if (canClick) 
            { 
                PlayButtonSFX();
                ShowEndLevelMenu(false);
                GameController.Instance.ResetCurrentRoundCounter();
                GameController.Instance.InitiateNewRound();
            }
        }

        public void BackToLobbyButton()
        {
            if (canClick)
            {
                PlayButtonBackSFX();
                GameManager.Instance.BGMPersist = false;
                SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
            }
        }
    }
}
