using RSG;
using UnityEngine;
using UnityEngine.UI;

namespace Content.UI.Windows
{
    public class OneButtonWindow : WindowBase
    {
        [Header("Buttons")]
        [SerializeField] private Button acceptButton = null;

        public override Promise<bool> Show<T>(T data, string titleText = "")
        {
            acceptButton.onClick.AddListener(Accept);
            
            return base.Show(data, titleText);
        }

        protected override void Close()
        {
            acceptButton.onClick.RemoveAllListeners();
            base.Close();
        }

        private void Accept()
        {
            UserAccepted = true;
            Close();
        }
    }
}