using System.Runtime.InteropServices;
using Content.Boids.Impl_Native.Aspects;
using Content.Boids.Impl_Native.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Content.Boids.Impl_Native.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(UpdateBoidsParametersSystem))]
    [StructLayout(LayoutKind.Auto)]
    public partial struct UpdateFlockDataSystem : ISystem
    {
        private EntityQuery _boidQuery;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            _boidQuery = SystemAPI.QueryBuilder().WithAspect<BoidAspect>().Build();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            // TODO: Switch back to arrays with capacity pulled out of entity component
            NativeList<float3> boidPositions = new NativeList<float3>(Allocator.TempJob);
            NativeList<float3> boidRotations = new NativeList<float3>(Allocator.TempJob);
            NativeList<int> ids = new NativeList<int>(Allocator.TempJob);

            foreach (BoidAspect boidAspect in SystemAPI.Query<BoidAspect>())
            {
                boidPositions.Add(boidAspect.Transform.ValueRO.Position);
                boidRotations.Add(math.forward(boidAspect.Transform.ValueRO.Rotation));
                ids.Add(boidAspect.Parameters.ValueRO.Id);
            }

            CalculateFlockDataJob calculateFlockDataJob = new CalculateFlockDataJob
            {
                Positions = boidPositions,
                Rotations = boidRotations,
                IDs = ids,
            };
            calculateFlockDataJob.ScheduleParallel(_boidQuery);

            boidPositions.Dispose(state.Dependency);
            boidRotations.Dispose(state.Dependency);
            ids.Dispose(state.Dependency);
        }

        [BurstCompile]
        [StructLayout(LayoutKind.Auto)]
        [RequireMatchingQueriesForUpdate]
        private partial struct CalculateFlockDataJob : IJobEntity
        {
            [ReadOnly] public NativeList<float3> Positions;
            [ReadOnly] public NativeList<float3> Rotations;
            [ReadOnly] public NativeList<int> IDs;

            private void Execute(ref BoidParameterComponent parameters,
                ref LocalTransform transform,
                ref FlockDataComponent flockData)
            {
                flockData.FlockmateCount = 0;
                flockData.FlockCenter = float3.zero;
                flockData.AverageFlockAvoidance = float3.zero;
                flockData.AverageFlockHeading = float3.zero;

                for (int i = 0; i < IDs.Length; i++)
                {
                    if (parameters.Id != i)
                    {
                        float3 otherPosition = Positions[i];
                        float sqrDst = math.distancesq(transform.Position, otherPosition);
                        if (sqrDst < parameters.PerceptionRadius * parameters.PerceptionRadius)
                        {
                            flockData.FlockmateCount++;
                            flockData.AverageFlockHeading += Rotations[i];
                            flockData.FlockCenter += otherPosition;

                            if (sqrDst < parameters.AvoidanceRadius * parameters.AvoidanceRadius)
                            {
                                flockData.AverageFlockAvoidance -= (otherPosition - transform.Position) / sqrDst;
                            }
                        }
                    }
                }
            }
        }
    }
}