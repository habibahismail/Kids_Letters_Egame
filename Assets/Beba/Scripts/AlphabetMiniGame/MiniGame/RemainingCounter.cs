using TMPro;
using UnityEngine;


namespace bebaSpace.AlphabetMiniGame
{
    public class RemainingCounter : MonoBehaviour
    {
        [SerializeField] private TMP_Text remainingText;

       public void SetRemainingText(int remainingCount)
        {
            if(remainingCount > 0)
            {
                remainingText.text = "x" + remainingCount.ToString();
            }else
            {
                remainingText.text = "";
            }

        }
    }
}
