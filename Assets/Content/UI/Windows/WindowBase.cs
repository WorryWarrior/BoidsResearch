using Content.UI.Windows.VisibilityToggles;
using RSG;
using RSG.Extensions;
using TMPro;
using UnityEngine;

namespace Content.UI.Windows
{
    public abstract class WindowBase : MonoBehaviour
    {
        [SerializeField] private WindowVisibilityToggleBase visibilityToggle = null;
        [SerializeField] private TextMeshProUGUI windowTitle = null;
        
        protected bool UserAccepted;
        private Promise<bool> _isBeingShown;
        
        protected virtual void Awake()
        {
            visibilityToggle.SetInitialVisibility();
        }
        
        public virtual Promise<bool> Show<T>(T data, string titleText = "")
        {
            if (windowTitle != null && !string.IsNullOrEmpty(titleText))
                windowTitle.text = titleText;
            
            visibilityToggle.SetVisibility(true);
            
            return _isBeingShown = new Promise<bool>();
        }

        protected virtual void Close()
        {
            visibilityToggle.SetVisibility(false)
                .Then(() => _isBeingShown.ResolveIfPending(UserAccepted));
        }
    }
}