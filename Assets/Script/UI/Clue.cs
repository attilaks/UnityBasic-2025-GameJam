using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script.UI
{
    public class Clue : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject _cluePanel;
        [SerializeField] private GameObject _board;
        [SerializeField] private GameObject _spawnerBombs;
        

        public void OnPointerEnter(PointerEventData eventData)
        {
            _cluePanel.SetActive(true);
            _board.SetActive(false);
            _spawnerBombs.SetActive(false);
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            _cluePanel.SetActive(false);
            _board.SetActive(true);
            _spawnerBombs.SetActive(true);
        }
    }
}

