using System;
using Content.Boids.Interfaces;

namespace Content.StaticData
{
    [Serializable]
    public record StageStaticData
    {
        public string StageKey { get; set; }
        public string StageTitle{ get; set; }
        public string StageDescription{ get; set; }
        public BoidsSimulationType BoidsSimulationType { get; set; }
    }
}