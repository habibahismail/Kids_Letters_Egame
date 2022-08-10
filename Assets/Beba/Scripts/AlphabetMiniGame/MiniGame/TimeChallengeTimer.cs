using TMPro;
using UnityEngine;

namespace bebaSpace.AlphabetMiniGame
{
    public class TimeChallengeTimer : MonoBehaviour
    {
        [SerializeField] private float timedChallengeTimeLimit = 60f; // time in seconds
        [SerializeField] private TMP_Text timerText;


        private float timeLeft;

        private void Awake()
        {
            timeLeft = timedChallengeTimeLimit;
        }

        private void Update()
        {
            if (timeLeft < 0)
            {
                GameEvents.OnTimesUp();
            }
            else
            {
                timeLeft -= Time.deltaTime;
                timerText.text = (timeLeft).ToString("0");
            }
        }

        public void StartNewTimedChallenge()
        {
            timeLeft = timedChallengeTimeLimit;
        }
    }

}