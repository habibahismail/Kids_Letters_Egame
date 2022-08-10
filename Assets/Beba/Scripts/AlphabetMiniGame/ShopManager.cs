using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace bebaSpace.AlphabetMiniGame
{
    public class ShopManager : MonoBehaviour
    {
        [SerializeField] private GameObject stickerShopContainer;
        [SerializeField] private GameObject stickerShopPrefab;

        [Header("AlertBox Properties")]
        [SerializeField] private GameObject alertBox;
        [SerializeField] private string buySuccessedText;
        [SerializeField] private string buyFailedText;

        private int stickersAvailable = 0;
        private CanvasGroup alertBoxCanvasGroup;
        private TMP_Text alertBoxText;
        private List<Sticker_SO> allStickers = new List<Sticker_SO>();
        private List<Sticker_SO> currentStickerList = new List<Sticker_SO>();
        private List<SaveGameSticker> saveGameStickers = new List<SaveGameSticker>();

        private void Awake()
        {
            allStickers = GameManager.Instance.allStickers;

            if (SaveLoad.SaveExist("Stickers"))
            {
                Load();
            }
            else
            {
                currentStickerList = allStickers;
            }
        }
    

        private void Start()
        {

            alertBoxCanvasGroup = alertBox.GetComponent<CanvasGroup>();
            alertBoxText = alertBox.transform.GetChild(1).GetComponent<TMP_Text>();

            for (int i = 0; i < currentStickerList.Count; i++)
            {
                if (!currentStickerList[i].isUnlocked) {

                    GameObject sticker = Instantiate(stickerShopPrefab);
                    sticker.transform.SetParent(stickerShopContainer.transform);

                    sticker.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                    sticker.GetComponentInChildren<Shop_StickerSlot>().SetStickerDetails(currentStickerList[i]);

                    stickersAvailable++;

                }
            }

            if(stickersAvailable == 0)
            {
                ShowAlertBox("Tahniah, anda telah mengumpul semua sticker yang ada.");
            }

            GameEvents.SaveInitiated += Save;
        }

        private void Save()
        {
            foreach (Sticker_SO sticker in allStickers)
            {
                saveGameStickers.Add(new SaveGameSticker(sticker));
            }

            SaveLoad.Save<List<SaveGameSticker>>(saveGameStickers, "Stickers");

            //Debug.Log("Stickers saved!");
        }

        private void Load()
        {
            if (SaveLoad.SaveExist("Stickers"))
            {
                List<SaveGameSticker> reloadedStickers = SaveLoad.Load<List<SaveGameSticker>>("Stickers");

                List<Sticker_SO> tempList = new List<Sticker_SO>();

                foreach (SaveGameSticker SavedSticker in reloadedStickers)
                {

                    for (int i = 0; i < allStickers.Count; i++)
                    {
                        if(string.Compare(allStickers[i].ID,SavedSticker.ID) == 0)
                        {
                            Sticker_SO loadedSticker = allStickers[i];

                            loadedSticker.isUnlocked = SavedSticker.IsUnlocked;

                            tempList.Add(loadedSticker);
                        }
                    }    
                }

                currentStickerList = tempList;
                //Debug.Log("Stickers Loaded!");
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

        public void BackToButton(string _sceneName)
        {
            PlayButtonBackSFX();
            GameManager.Instance.BGMPersist = true;
            SceneManager.LoadScene(_sceneName, LoadSceneMode.Single);

            GameEvents.OnSaveInitiated();
        }

        public void CloseAlertBox()
        {
            PlayButtonSFX();
            alertBoxCanvasGroup.alpha = 0;
            alertBoxCanvasGroup.interactable = false;
            alertBoxCanvasGroup.blocksRaycasts = false;
        }

        public bool HandleBuy(Sticker_SO theSticker)
        {
            bool status;
            string alertBoxText;

            if (GameManager.Instance.CurrentGameData.Gem >= theSticker.price)
            {
                SoundManager.Instance.PlaySoundUntilDone("SFX_Kaching");
                theSticker.isUnlocked = true;
                GameEvents.OnGemValueChanged(-theSticker.price);
                GameManager.Instance.UpdateTotalStickerUnlocked();
                GameManager.Instance.UpdateRecentlyUnlockedSticker(theSticker);
               
                alertBoxText = buySuccessedText;
                status = true;

            }
            else
            {
                SoundManager.Instance.PlaySoundUntilDone("SFX_FailedBuy");  
                alertBoxText = buyFailedText;
                status = false;
            }

            ShowAlertBox(alertBoxText);

            return status;
        }

        private void ShowAlertBox(string _alertBoxText)
        {
            alertBoxText.text = _alertBoxText;
            alertBoxCanvasGroup.alpha = 1;
            alertBoxCanvasGroup.interactable = true;
            alertBoxCanvasGroup.blocksRaycasts = true;
        }
    }
}
