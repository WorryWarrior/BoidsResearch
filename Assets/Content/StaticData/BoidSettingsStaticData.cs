using System;
using Content.StaticData.Converters;
using Newtonsoft.Json;
using UnityEngine;

namespace Content.StaticData
{
    [Serializable]
    public record BoidSettingsStaticData
    {
        [JsonConverter(typeof(Vector2IntConverter))]
        public Vector2Int BoidCountValues { get; set; }
        [JsonConverter(typeof(Vector2Converter))]
        public Vector2 BoidMinSpeedValues { get; set; }
        [JsonConverter(typeof(Vector2Converter))]
        public Vector2 BoidMaxSpeedValues { get; set; }
        [JsonConverter(typeof(Vector2Converter))]
        public Vector2 BoidAlignmentWeightValues { get; set; }
        [JsonConverter(typeof(Vector2Converter))]
        public Vector2 BoidCohesionWeightValues { get; set; }
        [JsonConverter(typeof(Vector2Converter))]
        public Vector2 BoidSeparationWeightValues { get; set; }
        [JsonConverter(typeof(Vector2Converter))]
        public Vector2 BoidTargetWeightValues { get; set; }
    }
}