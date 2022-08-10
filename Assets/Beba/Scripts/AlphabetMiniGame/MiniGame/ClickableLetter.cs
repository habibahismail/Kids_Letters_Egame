using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace bebaSpace.AlphabetMiniGame
{
    public class ClickableLetter : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject VFXprefab;

        private char randomLetter;
        private bool isCorrect = false;

        private TextMeshProUGUI letterPlaceholder;
        private Animator animator;
        private Sequence sequence;


        private void Start()
        {
            letterPlaceholder = gameObject.GetComponentInChildren<TextMeshProUGUI>();
            animator = GetComponentInChildren<Animator>();

            sequence = DOTween.Sequence();
        }

        private void PlayCorrectClickSound()
        {
            SoundManager.Instance.PlaySound("SFX_CorrectLetter");
        }


        public void SetLetter(char letter)
        {
            TMP_Text letterPlaceholder = GetComponentInChildren<TMP_Text>();
            letterPlaceholder.color = new Color(0.4150943f, 0.4150943f, 0.4150943f);

            isCorrect = false;
            enabled = true;

            randomLetter = letter;

            string theLetter = randomLetter.ToString();

            if (Random.Range(0, 100) > 50)
            {
                letterPlaceholder.text = theLetter;
            }
            else
            {
                letterPlaceholder.text = theLetter.ToUpper();
            }

            KillDoTweenSequence();

        }

        public void OnPointerClick(PointerEventData eventData)
        {
           
            GameController.Instance.CurrentRoundClicks++;

            if (randomLetter == GameController.Instance.Letter)
            {
                letterPlaceholder.color = new Color(0.2560036f, 0.5660378f, 0.1682093f);
                isCorrect = true;
                enabled = false;

                string soundName;

                if (GameManager.Instance.CurrentGameData.UsePhonics)
                    soundName = randomLetter.ToString().ToUpper() + "_namaHuruf";  // temp until have the sound for phonics -- soundName = currentLetter.ToString().ToUpper() + "_phonic"; 
                else
                    soundName = randomLetter.ToString().ToUpper() + "_namaHuruf";


                Instantiate(VFXprefab, transform);
                SoundManager.Instance.PlaySoundUntilDone(soundName);
                Invoke(nameof(PlayCorrectClickSound), 0.5f);
                KillDoTweenSequence();


                GameController.Instance.HandleCorrectLetterClick();

            }
            else
            {
                SoundManager.Instance.PlaySound("SFX_Error");
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            animator.SetBool("onMouseOver", true);

            Sequence _sequence = DOTween.Sequence();

            _sequence
                .Append(transform.DORotate(new Vector3(0, 0, 5f), 0.15f))
                .Append(transform.DORotate(new Vector3(0, 0, 0), 0.15f))
                .Append(transform.DORotate(new Vector3(0, 0, -5f), 0.15f));

            _sequence.SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);

            sequence = _sequence;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            animator.SetBool("onMouseOver", false);

            KillDoTweenSequence();

            if (isCorrect)
            {
                enabled = false;
                isCorrect = false;
            }

        }

        private void KillDoTweenSequence()
        {
            Sequence _sequence = DOTween.Sequence();
            _sequence.Append(transform.DORotate(Vector3.zero, 0.2f));

            sequence.Kill();
        }
    }
}
