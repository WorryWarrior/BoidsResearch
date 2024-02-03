using Unity.Entities;

namespace Content.Boids.Impl_Native.Components
{
    public struct BoidParameterComponent : IComponentData
    {
        public int Id;
        public int BoidCount;
        public float MinSpeedValue;
        public float MaxSpeedValue;
        public float PerceptionRadius;
        public float AvoidanceRadius;
        public float MaxSteerForce;
        public float AlignmentWeight;
        public float CohesionWeight;
        public float SeparationWeight;
        public float TargetWeight;
        public float BoundsRadius;
        public float CollisionAvoidanceWeight;
        public float CollisionAvoidanceDistance;
    }
}