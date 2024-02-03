using Entitas;
using Unity.Mathematics;
using UnityEngine;

namespace Content.Boids.Impl_Entitas.Components
{
    // Transform components
    [Game] public class PositionComponent : IComponent { public float3 value; }
    [Game] public class RotationComponent : IComponent { public float3 value; }
    // Movement components
    [Game] public class VelocityComponent : IComponent { public float3 value; }
    [Game] public class AccelerationComponent : IComponent { public float3 value; }
    // Flock data components
    [Game] public class FlockmateNumberComponent: IComponent { public int value; }
    [Game] public class FlockCenterComponent : IComponent { public float3 value; }
    [Game] public class AverageFlockDirectionComponent : IComponent { public float3 value; }
    [Game] public class AverageAvoidanceComponent : IComponent { public float3 value; }
    // Collision components
    [Game] public class IsOnCollisionTrajectoryComponent : IComponent { public bool value; }
    [Game] public class AvoidanceComponent : IComponent { public float3 value; }
    // Movement parameter components
    [Game] public class FollowingTargetComponent : IComponent { public float3 value; }
    [Game] public class AlignmentComponent : IComponent { public float3 value; }
    [Game] public class CohesionComponent : IComponent { public float3 value; }
    [Game] public class SeparationComponent : IComponent { public float3 value; }
    // Drift component
    [Game] public class TargetOffsetComponent : IComponent { public float3 value; }
    // Visualization component
    [Game] public class LinkedGOComponent : IComponent { public GameObject linkedGO; }
}