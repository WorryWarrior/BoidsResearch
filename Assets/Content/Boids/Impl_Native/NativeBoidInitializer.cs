using System.Threading.Tasks;
using Content.Boids.Impl_Native.Components;
using Content.Infrastructure.Factories.Interfaces;
using Content.Infrastructure.Services.PersistentData;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Graphics;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

namespace Content.Boids.Impl_Native
{
    public class NativeBoidInitializer
    {
        private IPersistentDataService _persistentDataService;
        private IBoidFactory _boidFactory;

        [Inject]
        private void Construct(
            IPersistentDataService persistentDataService,
            IBoidFactory boidFactory)
        {
            _persistentDataService = persistentDataService;
            _boidFactory = boidFactory;
        }

        public async Task InitializeBoids()
        {
            World world = World.DefaultGameObjectInjectionWorld;
            EntityManager entityManager = world.EntityManager;
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);

            NativeArray<float3> boidPositions = new NativeArray<float3>(
                _persistentDataService.BoidSettings.BoidCount, Allocator.TempJob);
            GeneratePoints.RandomPointsInSphere(float3.zero,
                _persistentDataService.BoidSettings.SpawnRadius, boidPositions);

            NativeArray<int> boidIDs = new NativeArray<int>(
                CreateBoidIDs(_persistentDataService.BoidSettings.BoidCount), Allocator.TempJob);

            Entity boidPrototype = await CreateBoidPrototype(entityManager);

            CreateEntityJob createEntityJob = new CreateEntityJob
            {
                Prototype = boidPrototype,
                Positions = boidPositions,
                IDs = boidIDs,
                BoidCount = _persistentDataService.BoidSettings.BoidCount,
                MinSpeed = _persistentDataService.BoidSettings.MinSpeed,
                MaxSpeed = _persistentDataService.BoidSettings.MaxSpeed,
                Ecb = ecb.AsParallelWriter(),
            };

            JobHandle createEntityJobHandle = createEntityJob
                .Schedule(_persistentDataService.BoidSettings.BoidCount, 64);
            createEntityJobHandle.Complete();

            ecb.Playback(entityManager);
            ecb.Dispose();
            boidPositions.Dispose();
            boidIDs.Dispose();
            entityManager.DestroyEntity(boidPrototype);
        }

        private async Task<Entity> CreateBoidPrototype(EntityManager entityManager)
        {
            Entity boidPrototype = entityManager.CreateEntity();

            entityManager.AddComponentData(boidPrototype, new AccelerationComponent());
            entityManager.AddComponentData(boidPrototype, new FlockDataComponent());
            entityManager.AddComponentData(boidPrototype, new BoidMovementDataComponent
            {
                FollowTargetPosition = float3.zero,
            });

            GameObject prototypeBoidGO = await _boidFactory.Create(Vector3.zero);
            //float3 prototypeScale = prototypeBoidGO.transform.localScale;

            RenderMeshUtility.AddComponents(boidPrototype, entityManager, new RenderMeshDescription
                {
                    FilterSettings = RenderFilterSettings.Default,
                    LightProbeUsage = LightProbeUsage.Off,
                },
                new RenderMeshArray(new[] { prototypeBoidGO.GetComponent<MeshRenderer>().material },
                    new[] { prototypeBoidGO.GetComponent<MeshFilter>().mesh }),
                MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0)
            );

            Object.Destroy(prototypeBoidGO);

            return boidPrototype;
        }

        private int[] CreateBoidIDs(int boidCount)
        {
            int[] boidIDs = new int[boidCount];

            for (int i = 0; i < boidIDs.Length; i++)
            {
                boidIDs[i] = i;
            }

            return boidIDs;
        }

        [BurstCompile]
        private struct CreateEntityJob : IJobParallelFor
        {
            [ReadOnly] public Entity Prototype;
            [ReadOnly] public NativeArray<float3> Positions;
            [ReadOnly] public NativeArray<int> IDs;
            [ReadOnly] public int BoidCount;
            [ReadOnly] public float MinSpeed;
            [ReadOnly] public float MaxSpeed;
            //[ReadOnly] public float3 Scale;

            [WriteOnly] public EntityCommandBuffer.ParallelWriter Ecb;

            public void Execute(int index)
            {
                Entity e = Ecb.Instantiate(index, Prototype);
                float3 entityRotationEuler = math.forward(quaternion.Euler(Positions[index]));

                Ecb.AddComponent(index, e, new LocalTransform
                {
                    Position = Positions[index],
                    //Rotation = quaternion.LookRotationSafe(float3.zero - Positions[index], math.up()),
                    Rotation = quaternion.Euler(entityRotationEuler),
                    Scale = .25f
                });
                Ecb.AddComponent(index, e, new BoidParameterComponent
                {
                    Id = IDs[index],
                    BoidCount = BoidCount
                });
                Ecb.AddComponent(index, e, new VelocityComponent
                {
                    Value = entityRotationEuler * (MinSpeed + MaxSpeed) * 0.5f
                });
            }
        }
    }
}