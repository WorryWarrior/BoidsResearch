using Content.Data;
using Content.StaticData;
using Content.UI.Windows;
using RSG;
using UnityEngine;

namespace Content.Hub.Settings
{
    public class SettingsWindow : OneButtonWindow
    {
        private const string AlignmentSliderName = "Alignment";
        private const string CohesionSliderName = "Cohesion";
        private const string SeparationSliderName = "Separation";
        
        [Header("Settings Controls")]
        [SerializeField] private SettingsSlider alignmentSlider = null;
        [SerializeField] private SettingsSlider cohesionSlider = null;
        [SerializeField] private SettingsSlider separationSlider = null;
        
        private BoidSettingsData _boidSettings;
        private BoidSettingsStaticData _boidSettingsStaticData;

        private bool _isInitialized;
        
        public override Promise<bool> Show<T>(T data, string titleText = "")
        {
            Tuple<BoidSettingsData, BoidSettingsStaticData> dataTuple  = 
                data as Tuple<BoidSettingsData, BoidSettingsStaticData>;
            _boidSettings = dataTuple!.Item1;
            _boidSettingsStaticData = dataTuple!.Item2;
            
            SetupControls();
            
            return base.Show(data, titleText);
        }

        private void SetupControls()
        {
            alignmentSlider.SetMinMaxValues(_boidSettingsStaticData.BoidAlignmentWeightValues.x, 
                _boidSettingsStaticData.BoidAlignmentWeightValues.y);
            alignmentSlider.SetSliderValue(_boidSettings.AlignmentWeight);
            
            cohesionSlider.SetMinMaxValues(_boidSettingsStaticData.BoidCohesionWeightValues.x, 
                _boidSettingsStaticData.BoidCohesionWeightValues.y);
            cohesionSlider.SetSliderValue(_boidSettings.CohesionWeight);
            
            separationSlider.SetMinMaxValues(_boidSettingsStaticData.BoidSeparationWeightValues.x, 
                _boidSettingsStaticData.BoidSeparationWeightValues.y);
            separationSlider.SetSliderValue(_boidSettings.SeparationWeight);
            
            if (!_isInitialized)
            {
                SubscribeAndInvokeControls();
                SetControlsNames();
                _isInitialized = true;
            }
        }

        private void SubscribeAndInvokeControls()
        {
            alignmentSlider.OnSliderValueChanged += SetAlignmentWeight;
            cohesionSlider.OnSliderValueChanged += SetCohesionWeight;
            separationSlider.OnSliderValueChanged += SetSeparationWeight;
            
            SetAlignmentWeight(alignmentSlider.GetSliderValue());
            SetCohesionWeight(cohesionSlider.GetSliderValue());
            SetSeparationWeight(separationSlider.GetSliderValue());
        }

        private void SetControlsNames()
        {
            alignmentSlider.SetSliderName(AlignmentSliderName);
            cohesionSlider.SetSliderName(CohesionSliderName);
            separationSlider.SetSliderName(SeparationSliderName);
        }
        
        private void SetAlignmentWeight(float value) => 
            _boidSettings.AlignmentWeight = value;

        private void SetCohesionWeight(float value) => 
            _boidSettings.CohesionWeight = value;
        
        private void SetSeparationWeight(float value) => 
            _boidSettings.SeparationWeight = value;
    }
}
