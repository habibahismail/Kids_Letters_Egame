using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace bebaSpace.AlphabetMiniGame
{
    public class ShowHint : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TMP_Text textPlaceholder;
        private Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();
            textPlaceholder.enabled = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(animator != null)
                animator.SetBool("mouseOver", true);

            textPlaceholder.enabled = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (animator != null)
                animator.SetBool("mouseOver", false);

            textPlaceholder.enabled = false;
        }
    }

}
