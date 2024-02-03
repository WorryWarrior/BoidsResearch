using RSG;
using UnityEngine;
using UnityEngine.UI;

namespace Content.UI.Windows
{
    public class OneButtonWindow : WindowBase
    {
        [Header("Buttons")]
        [SerializeField] private Button confirmButton = null;

        public override Promise<bool> Show<T>(T data, string titleText = "")
        {
            confirmButton.onClick.AddListener(Confirm);

            return base.Show(data, titleText);
        }

        protected override void Close()
        {
            confirmButton.onClick.RemoveAllListeners();
            base.Close();
        }

        private void Confirm()
        {
            UserAccepted = true;
            Close();
        }
    }
}