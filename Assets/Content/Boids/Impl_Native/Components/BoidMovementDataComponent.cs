using Unity.Entities;
using Unity.Mathematics;

namespace Content.Boids.Impl_Native.Components
{
    public struct BoidMovementDataComponent : IComponentData
    {
        public float3 FollowTargetPosition;
        public float3 FollowTargetContribution;
        public float3 AlignmentContribution;
        public float3 CohesionContribution;
        public float3 SeparationContribution;
    }
}