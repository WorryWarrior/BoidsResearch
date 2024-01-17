using DG.Tweening;
using RSG;
using RSG.Extensions;
using UnityEngine;

namespace Content.UI.Windows.VisibilityToggles
{
    public class WindowSlideToggle : WindowVisibilityToggleBase
    {
        [SerializeField] private RectTransform windowPanel = null;
        [SerializeField] private float visibilityToggleDuration = 0.5f;
        
        public override Promise SetVisibility(bool value)
        {
            base.SetVisibility(value);
            
            Promise animationPromise = new Promise();

            DOTween.Sequence()
                .Append(windowPanel
                    .DOAnchorPos(value ? Vector2.zero : Vector2.left * windowPanel.rect.width,
                        visibilityToggleDuration))
                .Play()
                .onComplete += () => animationPromise.ResolveIfPending();
            
            return animationPromise;
        }

        public override void SetInitialVisibility()
        {
            windowPanel.anchoredPosition = Vector2.left * windowPanel.rect.width;
        }
    }
}