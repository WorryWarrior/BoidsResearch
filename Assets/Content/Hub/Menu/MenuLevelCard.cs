﻿using System;
using Content.StaticData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Content.Hub.Menu
{
    public class MenuLevelCard : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI cardTitle;
        [SerializeField] private TextMeshProUGUI cardDescription;
        [SerializeField] private Toggle selectToggle;
        [SerializeField] private Image cardImage;
        
        public event Action<StageStaticData> OnSelect; 
        
        public void Initialize(StageStaticData staticData, Sprite previewSprite, ToggleGroup toggleGroup)
        {
            cardTitle.text = staticData.StageTitle;
            cardDescription.text = staticData.StageDescription;
            cardImage.sprite = previewSprite;
            
            selectToggle.onValueChanged.AddListener(it => OnSelect?.Invoke(it ? staticData : null));
            selectToggle.group = toggleGroup;
        }
    }
}