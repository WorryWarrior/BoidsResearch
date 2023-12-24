using System;
using Content.Boids.Interfaces;
using Content.StaticData.Converters;
using Newtonsoft.Json;
using UnityEngine;

namespace Content.StaticData
{
    [Serializable]
    public record StageStaticData
    {
        public string StageKey { get; set; }
        public string StageTitle{ get; set; }
        public string StageDescription{ get; set; }
        
        [JsonConverter(typeof(Vector3Converter))]
        public Vector3 CameraSpawnPoint { get; set; }
        
        public BoidsSimulationType BoidsSimulationType { get; set; }
    }
}