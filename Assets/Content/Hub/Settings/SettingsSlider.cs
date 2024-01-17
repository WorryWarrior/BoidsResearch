using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Content.Hub.Settings
{
    public class SettingsSlider : MonoBehaviour
    {
        [SerializeField] private Slider slider = null;
        [SerializeField] private TextMeshProUGUI sliderNameText = null;
        [SerializeField] private TextMeshProUGUI minValueText = null;
        [SerializeField] private TextMeshProUGUI maxValueText = null;
        [SerializeField] private TextMeshProUGUI currentValueText = null;
        
        public event Action<float> OnSliderValueChanged;

        private void Awake()
        {
            OnSliderValueChanged += it => currentValueText.text = it.ToString("F2");
            slider.onValueChanged.AddListener(it=> OnSliderValueChanged?.Invoke(it));
        }

        public void SetMinMaxValues(float minValue, float maxValue)
        {
            slider.minValue = minValue;
            slider.maxValue = maxValue;

            minValueText.text = minValue.ToString();
            maxValueText.text = maxValue.ToString();
        }

        public void SetSliderValue(float value)
        {
            slider.value = value;
        }

        public float GetSliderValue() => slider.value;

        public void SetSliderName(string value)
        {
            sliderNameText.text = value;
        }
    }
}