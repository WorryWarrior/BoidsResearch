using System;
using Content.StaticData.Converters;
using Newtonsoft.Json;
using UnityEngine;

namespace Content.Data
{
    [Serializable]
    public class BoidsSettingsData
    {
        public int BoidCount { get; set; }
        public float SpawnRadius { get; set; }
        public float MinSpeed { get; set; }
        public float MaxSpeed { get; set; }
        public float PerceptionRadius { get; set; }
        public float AvoidanceRadius { get; set; }
        public float MaxSteerForce { get; set; }
        public float AlignmentWeight { get;  set; }
        public float CohesionWeight { get;  set; }
        public float SeparationWeight { get; set; }
        public float TargetWeight { get; set; }
        public float BoundsRadius { get; set; }
        public float CollisionAvoidanceWeight { get; set; }
        public float CollisionAvoidanceDistance { get; set; }
    }
}