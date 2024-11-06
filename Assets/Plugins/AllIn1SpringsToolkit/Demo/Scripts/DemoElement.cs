using UnityEngine;
using UnityEngine.EventSystems;

namespace AllIn1SpringsToolkit.Demo.Scripts
{
    public abstract class DemoElement : MonoBehaviour
    {
        [SerializeField] private GameObject firstSelectedUiElement;
        [SerializeField] private CanvasGroup buttonsCanvasGroup;
        protected bool isOpen;

        public virtual void Initialize(bool hideUi)
        {
            isOpen = true;
            EventSystem.current.SetSelectedGameObject(firstSelectedUiElement);
            
            if(hideUi && buttonsCanvasGroup != null)
            {
                buttonsCanvasGroup.alpha = 0f;
            }
        }

        public virtual void Close()
        {
            isOpen = false;
        }
        
        public bool IsOpen()
        {
            return isOpen;
        }
    }
}