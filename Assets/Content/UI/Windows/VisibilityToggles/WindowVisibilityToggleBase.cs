using RSG;
using UnityEngine;

namespace Content.UI.Windows.VisibilityToggles
{
    public abstract class WindowVisibilityToggleBase : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup = null;
        
        public virtual Promise SetVisibility(bool value)
        {
            if (canvasGroup)
                canvasGroup.blocksRaycasts = value;
            
            return null;
        }

        public abstract void SetInitialVisibility();
    }
}