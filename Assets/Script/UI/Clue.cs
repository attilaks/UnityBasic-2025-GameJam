using UnityEngine;
using UnityEngine.EventSystems;

namespace Script.UI
{
    public class Clue : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject _cluePanel;
        

        public void OnPointerEnter(PointerEventData eventData)
        {
            _cluePanel.SetActive(true);
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            _cluePanel.SetActive(false);
        }
    }
}

