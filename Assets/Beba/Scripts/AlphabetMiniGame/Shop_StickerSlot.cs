using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace bebaSpace.AlphabetMiniGame
{
    public class Shop_StickerSlot : MonoBehaviour
    {
        private Image stickerImage;
        private TMP_Text priceText;
        private Button buyButton;

        private Sticker_SO theSticker;

        private void Buy()
        {
            ShopManager shopManager = GameObject.Find("ShopManager").GetComponent<ShopManager>();

            bool status = shopManager.HandleBuy(theSticker);

            //if transaction succesfull
            if (status)
            {
                Destroy(gameObject);
            }
        }

        public void SetStickerDetails(Sticker_SO sticker)
        {
           
            stickerImage = gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
            priceText = GetComponentInChildren<TextMeshProUGUI>();
            buyButton = GetComponentInChildren<Button>();

            stickerImage.sprite = sticker.stickerImage;
            priceText.text = sticker.price.ToString();
            buyButton.onClick.AddListener(Buy);

            theSticker = sticker;

        }
    }

}