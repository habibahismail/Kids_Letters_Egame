using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


namespace bebaSpace.AlphabetMiniGame
{
    public class StickerBookManager : MonoBehaviour
    {
        [SerializeField] private Button lArrowButton;
        [SerializeField] private Button rArrowButton;

        [Space]
        [SerializeField] private Image stickerBookMainBG;
        [SerializeField] private Image stickerBookDisableImageContainer;
        [SerializeField] private Sprite bg_school, bg_cat, bg_dessert, bg_dino, bg_emoji, bg_seacreature;

        [SerializeField] private List<Sprite> stickerBG;
        [SerializeField] private List<Image> catStickerUnlocked = new List<Image>();
        [SerializeField] private List<Image> seaCreatureUnlocked = new List<Image>();
        [SerializeField] private List<Image>  schoolUnlocked = new List<Image>();
        [SerializeField] private List<Image> dinoUnlocked = new List<Image>();
        [SerializeField] private List<Image> emojiUnlocked = new List<Image>();
        [SerializeField] private List<Image> dessertUnlocked = new List<Image>();

        private int currentStickerIndex = 0;
        private readonly int maxStickerSeries = 5;
        private List<Image> currentStickerShown = new List<Image>();

        private List<Sticker_SO> currentStickerList = new List<Sticker_SO>();
        private List<Sticker_SO> allStickers = new List<Sticker_SO>();

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
            ShowSticker(currentStickerIndex);
            lArrowButton.interactable = false;
        }

        private void ShowSticker(int currentIndex)
        {
            Sprite bgImage = null;

            switch (currentIndex)
            {
                case (int)StickerSeries.BackToSchool:

                    bgImage = bg_school;
                    stickerBookMainBG.sprite = stickerBG[3];
                    currentStickerShown = schoolUnlocked;
                    
                    break;

                case (int)StickerSeries.Cat:

                    bgImage = bg_cat;
                    stickerBookMainBG.sprite = stickerBG[3];
                    currentStickerShown = catStickerUnlocked;

                    break;

                case (int)StickerSeries.Dessert:

                    bgImage = bg_dessert;
                    stickerBookMainBG.sprite = stickerBG[1];
                    currentStickerShown = dessertUnlocked;

                    break;

                case (int)StickerSeries.Dino:

                    bgImage = bg_dino;
                    stickerBookMainBG.sprite = stickerBG[0];
                    currentStickerShown = dinoUnlocked;

                    break;

                case (int)StickerSeries.Emoji:

                    bgImage = bg_emoji;
                    stickerBookMainBG.sprite = stickerBG[0];
                    currentStickerShown = emojiUnlocked;

                    break;

                case (int)StickerSeries.SeaCreature:

                    bgImage = bg_seacreature;
                    stickerBookMainBG.sprite = stickerBG[2];
                    currentStickerShown = seaCreatureUnlocked;

                    break;

                default: break;
            }

            stickerBookDisableImageContainer.sprite = bgImage;
            ShowUnlockedStickers();

        }

        private void ShowUnlockedStickers()
        {

           foreach (Image sticker in currentStickerShown)
            {

                for (int i = 0; i < currentStickerList.Count; i++)
                {
                    if(string.Compare(sticker.sprite.name, currentStickerList[i].stickerImage.name) == 0 && currentStickerList[i].isUnlocked){

                        sticker.enabled = true;
                    }
                }
           }
        }

        private void HideAllStickersInSeries()
        {
            foreach (Image sticker in currentStickerShown)
            {

                if (sticker.isActiveAndEnabled)
                {
                    sticker.enabled = false;
                }
            }
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
                        if (string.Compare(allStickers[i].ID, SavedSticker.ID) == 0)
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
            SoundManager.Instance.PlaySoundUntilDone("SFX_PageFlip");
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
        }

        public void OnClickButton(int number)
        {
            PlayButtonSFX();

            if (number == 1)
            {
                currentStickerIndex++;

                if(lArrowButton.interactable == false)
                {
                    lArrowButton.interactable = true;
                }

                if (currentStickerIndex >= maxStickerSeries) 
                { 
                    currentStickerIndex = maxStickerSeries;
                    rArrowButton.interactable = false;
                }
                    

            }else
            {
                currentStickerIndex--;

                if(rArrowButton.interactable == false)
                {
                    rArrowButton.interactable = true;
                }

                if (currentStickerIndex <= 0) 
                {
                    currentStickerIndex = 0;
                    lArrowButton.interactable = false;
                }
                  
            }

            HideAllStickersInSeries();
            ShowSticker(currentStickerIndex);
        }
    }
}
