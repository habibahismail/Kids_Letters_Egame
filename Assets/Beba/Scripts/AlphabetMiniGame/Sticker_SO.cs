using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace bebaSpace.AlphabetMiniGame
{
    [CreateAssetMenu(fileName = "New Sticker", menuName = "Unlockables/Sticker")]
    public class Sticker_SO : ScriptableObject
    {
        [SerializeField] private string id;
        [SerializeField] private string stickerName;

        public string ID { get { return id; } }
        public string StickerName { get { return stickerName; } }

        public int price;
        public Sprite stickerImage = null;

        public StickerSeries series;
        public bool isUnlocked = false;

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            string path = AssetDatabase.GetAssetPath(this);
            id = AssetDatabase.AssetPathToGUID(path);

            stickerName = stickerImage.name;
        }

#endif

    }

    public enum StickerSeries { Cat, SeaCreature, BackToSchool, Emoji, Dino, Dessert }

    [System.Serializable]
    public class SaveGameSticker
    {
        public string ID;
        public string StickerName;
        public bool IsUnlocked;

        public SaveGameSticker(Sticker_SO sticker)
        {
            ID = sticker.ID;
            StickerName = sticker.StickerName;
            IsUnlocked = sticker.isUnlocked;
            
        }
    }
}
