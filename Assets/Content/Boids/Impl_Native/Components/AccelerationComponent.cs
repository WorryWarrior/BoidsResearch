using Unity.Entities;
using Unity.Mathematics;

namespace Content.Boids.Impl_Native.Components
{
    public struct AccelerationComponent : IComponentData
    {
        public float3 Value;
    }
}