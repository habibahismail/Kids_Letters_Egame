using UnityEngine;

namespace bebaSpace.AlphabetMiniGame
{
    public class GameEvents : MonoBehaviour
    {
        public static System.Action SaveInitiated;
        public static System.Action<int> GemValueChanged;
        public static System.Action UpdateGemDisplay;

        public static System.Action TimesUp;

        public static void OnSaveInitiated()
        {
            SaveInitiated?.Invoke();
        }

        public static void OnGemValueChanged(int value)
        {
            GemValueChanged?.Invoke(value);
        }

        public static void OnUpdateGemDisplay()
        {
            UpdateGemDisplay?.Invoke();
        }

        public static void OnTimesUp()
        {
            TimesUp?.Invoke();
        }
    }
}