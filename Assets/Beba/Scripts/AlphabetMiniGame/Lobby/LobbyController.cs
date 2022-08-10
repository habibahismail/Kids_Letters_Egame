using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace bebaSpace.AlphabetMiniGame
{
    public class LobbyController : MonoBehaviour
    {
        [Header("Audio")]
        [SerializeField] private string bgmName;

        [Header("Player Statistic Panel")]
        [SerializeField] private TMP_Text TotalGamesPlayed;
        [SerializeField] private TMP_Text mostAnsweredInTimedMode;
        [Space]
        [SerializeField] private TMP_Text _3StarsStatistic;
        [SerializeField] private TMP_Text _2StarsStatisctic;
        [SerializeField] private TMP_Text _1StarStatisctic;

        [Header("Recently Unlocked Sticker Panel")]
        [SerializeField] private List<Image> recentStickerPlaceholder;

        [Header("Menus")]
        [SerializeField] private GameObject settingsMenu;
        [SerializeField] private GameObject infoMenu;

        private CanvasGroup settingCanvasGroup;
        private CanvasGroup infoCanvasGroup;


        private void Awake()
        {
            SoundManager.Instance.PlaySound(bgmName);
        }

        private void Start()
        {
            settingCanvasGroup = settingsMenu.GetComponent<CanvasGroup>();
            infoCanvasGroup = infoMenu.GetComponent<CanvasGroup>();

            GameData currentGameData = GameManager.Instance.CurrentGameData; ;

            TotalGamesPlayed.text = currentGameData.TotalGamesInitiated.ToString();
            mostAnsweredInTimedMode.text = currentGameData.ChallengeModeBest.ToString();
            _3StarsStatistic.text = currentGameData.Total3StarsObtained.ToString();
            _2StarsStatisctic.text = currentGameData.Total2StarsObtained.ToString();
            _1StarStatisctic.text = currentGameData.Total1StarsObtained.ToString();

            PopulateRecentlyUnlockedPanel();

        }

        private void PopulateRecentlyUnlockedPanel()
        {
            List<Sticker_SO> recentStickers = GameManager.Instance.RecentlyUnlockedStickers;

            if(recentStickers.Count == 1)
            {
                recentStickerPlaceholder[3].sprite = recentStickers[0].stickerImage;
                recentStickerPlaceholder[3].color = new Color(1f, 1f, 1f, 1f);
            }
            else if(recentStickers.Count == 2) 
            {
                for (int i = 0; i < recentStickers.Count; i++)
                {
                    recentStickerPlaceholder[i+2].sprite = recentStickers[i].stickerImage;
                    recentStickerPlaceholder[i+2].color = new Color(1f, 1f, 1f, 1f);
                }
            }else if(recentStickers.Count == 3)
            {
                for (int i = 0; i < recentStickers.Count; i++)
                {
                    recentStickerPlaceholder[i + 1].sprite = recentStickers[i].stickerImage;
                    recentStickerPlaceholder[i + 1].color = new Color(1f, 1f, 1f, 1f);
                }
            }else 
            {       
                for (int i = 0; i < recentStickers.Count; i++)
                {
                    recentStickerPlaceholder[i].sprite = recentStickers[i].stickerImage;
                    recentStickerPlaceholder[i].color = new Color(1f, 1f, 1f, 1f);
                }
            }
        }

        private void ShowMenu(bool isTrue, string menu)
        {
            CanvasGroup currentMenu = settingCanvasGroup;

            switch (menu)
            {
                case "setting":

                    currentMenu = settingCanvasGroup;

                    break;
                case "info":

                    currentMenu = infoCanvasGroup;

                    break;
                default: break;
            }

            if (isTrue)
            {
                currentMenu.alpha = 1;
                currentMenu.blocksRaycasts = true;
                currentMenu.interactable = true;
            }
            else
            {
                currentMenu.alpha = 0;
                currentMenu.blocksRaycasts = false;
                currentMenu.interactable = false;
            }
        }

        private void PlayButtonSFX()
        {
            SoundManager.Instance.PlaySoundUntilDone("SFX_ButtonClicked");
        }

        private void PlayButtonBackSFX()
        {
            SoundManager.Instance.PlaySoundUntilDone("SFX_ButtonBack"); 
        }

        public void LoadGame(int sceneIndex)
        {
            GameManager.Instance.BGMPersist = false;
            SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
        }

        public void ZenModeGameChosen(bool isZenMode)
        {
            if (isZenMode)
            {
                GameManager.Instance.ZenMode = true;
            }
            else
            {
                GameManager.Instance.ZenMode = false;
            }

            PlayButtonSFX();
        }

        public void OpenMenu(string menu)
        {
            PlayButtonSFX();
            ShowMenu(true, menu);
        }

        public void HideMenu(string menu)
        {
            PlayButtonBackSFX();
            ShowMenu(false, menu);
        }

        public void SaveSettings()
        {
            PlayButtonBackSFX();
            ShowMenu(false, "setting");
            GameEvents.OnSaveInitiated();
        }

        public void ButtonGoTo(string _sceneName)
        {
            PlayButtonSFX();
            GameManager.Instance.BGMPersist = true;
            SceneManager.LoadScene(_sceneName, LoadSceneMode.Single);
        }
        public void QuitGame()
        {
            Application.Quit();
        }

    }

}
