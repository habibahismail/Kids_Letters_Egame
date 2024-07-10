using System.Collections.Generic;
using UnityEngine;

namespace bebaSpace.AlphabetMiniGame
{
    public class GameManager : MonoBehaviour
    {
        #region Instance declaration
        private static GameManager _instance;

        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    _instance = obj.AddComponent<GameManager>();
                    obj.name = typeof(GameManager).ToString();
                }

                return _instance;
            }
        }

        #endregion


        [Header("Game Properties")]
        public bool BGMPersist;
        
        [Space]
        public List<Sticker_SO> allStickers = new List<Sticker_SO>();

        private bool gameLoaded = false;
        private GameData currentGameData = new GameData();
        private List<SaveGameSticker> saveGameSticker = new List<SaveGameSticker>();
        private List<Sticker_SO> recentlyUnlockedStickers = new List<Sticker_SO>();

        public bool ZenMode = true;

        public GameData CurrentGameData { get => currentGameData; private set { } }

        public List<Sticker_SO> RecentlyUnlockedStickers { get => recentlyUnlockedStickers; private set { } }

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
                DontDestroyOnLoad(gameObject);
            }

            //Debug.Log("GameLoaded: " + gameLoaded);

            if (SaveLoad.SaveExist("GameData") && gameLoaded == false)
            {
                    Load();
                    gameLoaded = true;       
            }
            else 
            {
                InitializeGameData();
            }

        }
        #endregion


        private void Start()
        {
            BGMPersist = false;

            GameEvents.SaveInitiated += Save;
            GameEvents.GemValueChanged += UpdateGem;
        }

        private void Save()
        {
            saveGameSticker.Clear();

            foreach (Sticker_SO sticker in recentlyUnlockedStickers)
            {
                saveGameSticker.Add(new SaveGameSticker(sticker));
            }

            currentGameData.recentlyUnlockedStickers = saveGameSticker;

            SaveLoad.Save<GameData>(currentGameData, "GameData");

        }

        private void Load()
        {
            currentGameData = SaveLoad.Load<GameData>("GameData");

            List<SaveGameSticker> reloadedStickers = new List<SaveGameSticker>();
            reloadedStickers = currentGameData.recentlyUnlockedStickers;

            recentlyUnlockedStickers = new List<Sticker_SO>();

            foreach (SaveGameSticker SavedSticker in reloadedStickers)
            {
                for (int i = 0; i < allStickers.Count; i++)
                {
                    if (string.Compare(allStickers[i].ID, SavedSticker.ID) == 0)
                    {
                        Sticker_SO loadedSticker = allStickers[i];

                        loadedSticker.isUnlocked = SavedSticker.IsUnlocked;

                        recentlyUnlockedStickers.Add(loadedSticker);
                    }
                }
            }
        }

        private void InitializeGameData()
        {
            currentGameData.Gem = 200;
            currentGameData.TotalStickerUnlocked = 0;

            currentGameData.TotalGamesInitiated = 0;
            currentGameData.Total3StarsObtained = 0;
            currentGameData.Total2StarsObtained = 0;
            currentGameData.Total1StarsObtained = 0;
            currentGameData.ChallengeModeBest = 0;

            currentGameData.recentlyUnlockedStickers = new List<SaveGameSticker>();

            currentGameData.SoundLevel = 0.8f;
            currentGameData.UsePhonics = true;

        }

        private void OnApplicationQuit()
        {
            GameEvents.OnSaveInitiated();
        }

        public void UpdateGem(int value)
        {
            currentGameData.Gem += value;
            GameEvents.OnUpdateGemDisplay();
        }

        public void SaveButton()
        {
            GameEvents.OnSaveInitiated();
        }

        public void UpdateTotalStickerUnlocked()
        {
            currentGameData.TotalStickerUnlocked++;
        }

        public void UpdateRecentlyUnlockedSticker(Sticker_SO newSticker)
        {
            if (recentlyUnlockedStickers.Count >= 4)
            {
                recentlyUnlockedStickers.RemoveAt(0);
            }

            recentlyUnlockedStickers.Add(newSticker);
        }

    }

    [System.Serializable]
    public class GameData
    {
     
        //Player data properties;
        public int Gem;
        public int TotalStickerUnlocked;
        public int TotalGamesInitiated;
        
        public int Total3StarsObtained;
        public int Total2StarsObtained;
        public int Total1StarsObtained;

        public int ChallengeModeBest;

        public List<SaveGameSticker> recentlyUnlockedStickers;
        
        //App settings Properties
        public float SoundLevel;
        public bool UsePhonics;

    }

}