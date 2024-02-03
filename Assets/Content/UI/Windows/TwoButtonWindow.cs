using RSG;
using UnityEngine;
using UnityEngine.UI;

namespace Content.UI.Windows
{
    public class TwoButtonWindow : OneButtonWindow
    {
        [SerializeField] private Button cancelButton = null;

        public override Promise<bool> Show<T>(T data, string titleText = "")
        {
            cancelButton.onClick.AddListener(Cancel);
            return base.Show(data, titleText);
        }

        protected override void Close()
        {
            cancelButton.onClick.RemoveAllListeners();
            base.Close();
        }

        private void Cancel()
        {
            UserAccepted = false;
            Close();
        }
    }
}