using Unity.Entities;
using Unity.Mathematics;

namespace Content.Boids.Impl_Native.Components
{
    public struct FlockDataComponent : IComponentData
    {
        public int FlockmateCount;
        public float3 FlockCenter;
        public float3 AverageFlockHeading;
        public float3 AverageFlockAvoidance;
    }
}