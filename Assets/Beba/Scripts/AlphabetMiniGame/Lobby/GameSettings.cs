using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace bebaSpace.AlphabetMiniGame
{
    public class GameSettings : MonoBehaviour
    {

        [SerializeField] private AudioMixer masterMixer;
        [SerializeField] private Slider volumeSlider;


        private void Start()
        {
            SetSoundVolume(GameManager.Instance.CurrentGameData.SoundLevel);
            volumeSlider.value = GameManager.Instance.CurrentGameData.SoundLevel;
        }

        public void SetAlphabetSound(bool isPhonic)
        {
            if (isPhonic)
            {
                GameManager.Instance.CurrentGameData.UsePhonics = true;
            }else
            {
                GameManager.Instance.CurrentGameData.UsePhonics = false;
            }

            Debug.Log("GameManager.Instance.CurrentGameData.UsePhonics = " + GameManager.Instance.CurrentGameData.UsePhonics);
        }

        public void SetSoundVolume(float soundLevel)
        {
            masterMixer.SetFloat("MasterVol", Mathf.Log10(soundLevel) * 20);
            GameManager.Instance.CurrentGameData.SoundLevel = soundLevel;
        }
    }
}
