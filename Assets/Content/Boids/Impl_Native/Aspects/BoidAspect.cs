using Content.Boids.Impl_Native.Components;
using Unity.Entities;
using Unity.Transforms;

namespace Content.Boids.Impl_Native.Aspects
{
    public readonly partial struct BoidAspect : IAspect
    {
        public readonly RefRW<LocalTransform> Transform;
        public readonly RefRW<BoidParameterComponent> Parameters;
        public readonly RefRW<VelocityComponent> Velocity;
        public readonly RefRW<AccelerationComponent> Acceleration;
        public readonly RefRW<FlockDataComponent> FlockData;
        public readonly RefRW<BoidMovementDataComponent> MovementData;
    }
}