using UnityEngine;
using UnityEngine.SceneManagement;

namespace bebaSpace.AlphabetMiniGame
{
    public class MainMenuManager : MonoBehaviour
    {
        [Header("Audio Properties")]
        [SerializeField] private string menuBGM;
        [SerializeField] private string startButtonSFX;
        [SerializeField] private CanvasGroup creditCanvasGroup;

        private void Awake()
        {
            SoundManager.Instance.PlaySound(menuBGM);
        }

        private void LoadGame()
        {
            SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
        }

        public void ClickPlayButton()
        {
          
            SoundManager.Instance.PlaySoundUntilDone(startButtonSFX);
            GameManager.Instance.BGMPersist = false;
            Invoke(nameof(LoadGame), 1f);
        }

        public void CloseAlertBox()
        {
            SoundManager.Instance.PlaySoundUntilDone("SFX_ButtonBack");
            creditCanvasGroup.alpha = 0;
            creditCanvasGroup.interactable = false;
            creditCanvasGroup.blocksRaycasts = false;
        }

        public void ShowAlertBox()
        {
            SoundManager.Instance.PlaySoundUntilDone("SFX_ButtonClicked");
            creditCanvasGroup.alpha = 1;
            creditCanvasGroup.interactable = true;
            creditCanvasGroup.blocksRaycasts = true;
        }
    }
}
