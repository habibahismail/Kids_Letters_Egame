using TMPro;
using UnityEngine;


namespace bebaSpace.AlphabetMiniGame
{
    public class GemDisplay : MonoBehaviour
    {
        private TMP_Text gemText;


        private void Start()
        {
            gemText = GetComponentInChildren<TMP_Text>();
            
            GameEvents.UpdateGemDisplay += UpdateGemText;
            GameEvents.OnGemValueChanged(0);
        }

        private void OnDisable()
        {
            GameEvents.UpdateGemDisplay -= UpdateGemText;
        }

        private void UpdateGemText()
        {
            gemText.text = GameManager.Instance.CurrentGameData.Gem.ToString();
        }
    }
}
