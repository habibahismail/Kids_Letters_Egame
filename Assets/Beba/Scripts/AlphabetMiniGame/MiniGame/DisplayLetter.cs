using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace bebaSpace.AlphabetMiniGame
{
    public class DisplayLetter : MonoBehaviour, IPointerEnterHandler
    {
        [SerializeField] private bool upperCase;

        private char currentLetter;
        private bool playSound = true;


        public void OnPointerEnter(PointerEventData eventData)
        {
            if (playSound) 
            {
                string soundName;
                
                if(GameManager.Instance.CurrentGameData.UsePhonics)
                    soundName = currentLetter.ToString().ToUpper() + "_namaHuruf";  // temp until have the sound for phonics -- soundName = currentLetter.ToString().ToUpper() + "_phonic"; 
                else 
                    soundName = currentLetter.ToString().ToUpper() + "_namaHuruf";


                SoundManager.Instance.PlaySoundUntilDone(soundName);
            }
            
        }

        public void SetLetter(char letter)
        {
            enabled = true;
            playSound = true;

            currentLetter = letter;
            TMP_Text displayTextPlaceholder = GetComponent<TMP_Text>();

            if (upperCase)
                displayTextPlaceholder.text = letter.ToString().ToUpper();
            else
                displayTextPlaceholder.text = letter.ToString().ToLower();
        }

        public void DisableSound()
        {
            playSound = false;
        }
    }
}
