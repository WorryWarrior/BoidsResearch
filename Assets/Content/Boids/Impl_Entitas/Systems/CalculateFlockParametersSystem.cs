using Content.Infrastructure.Services.PersistentData;
using Entitas;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Content.Boids.Impl_Entitas.Systems
{
    public class CalculateFlockParametersSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _boidsGroup = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(
            GameMatcher.Position,
            GameMatcher.Rotation,
            GameMatcher.AverageFlockDirection,
            GameMatcher.FlockmateNumber,
            GameMatcher.FlockCenter,
            GameMatcher.AverageAvoidance));
        private readonly IPersistentDataService _persistentDataService;
        
        public CalculateFlockParametersSystem(
            IPersistentDataService persistentDataService)
        {
            _persistentDataService = persistentDataService;
        }

        public void Execute()
        {
            NativeArray<float3> _boidPositions = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<float3> _boidRotations = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<int> _flockmateNumbers = new NativeArray<int>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<float3> _flockHeadings = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<float3> _flockCenters = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<float3> _averageAvoidances = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);

            int entityIndex = 0;
            foreach (GameEntity e in _boidsGroup)
            {
                _boidPositions[entityIndex] = e.position.value;
                _boidRotations[entityIndex] = e.rotation.value;

                entityIndex++;
            }

            CalculateFlockmateParameterJob flockmateParameterJob = new()
            {
                perceptionRadius = _persistentDataService.BoidSettings.PerceptionRadius,
                avoidanceRadius = _persistentDataService.BoidSettings.AvoidanceRadius,
                boidPositions = _boidPositions,
                boidRotations = _boidRotations,
                flockHeadings = _flockHeadings,
                flockmateNumbers = _flockmateNumbers,
                flockCenters = _flockCenters,
                averageAvoidances = _averageAvoidances
            };

            JobHandle jobHandle = flockmateParameterJob.Schedule(_boidsGroup.count, 32);
            jobHandle.Complete();

            entityIndex = 0;
            foreach (GameEntity e in _boidsGroup)
            {
                e.ReplaceFlockmateNumber(_flockmateNumbers[entityIndex]);
                e.ReplaceFlockCenter(_flockCenters[entityIndex]);
                e.ReplaceAverageAvoidance(_averageAvoidances[entityIndex]);
                e.ReplaceAverageFlockDirection(_flockHeadings[entityIndex]);

                entityIndex++;
            }

            _boidPositions.Dispose();
            _boidRotations.Dispose();
            _flockHeadings.Dispose();
            _flockmateNumbers.Dispose();
            _flockCenters.Dispose();
            _averageAvoidances.Dispose();

            /*foreach (GameEntity e in _boidsGroup)
            {
                e.flockmateNumber.flockmates = 0;
                
                foreach (GameEntity otherE in _boidsGroup)
                {
                    if (otherE != e)
                    {
                        float sqrDst = (otherE.position.x - e.position.x) * (otherE.position.x - e.position.x) + 
                                       (otherE.position.y - e.position.y) * (otherE.position.y - e.position.y) + 
                                       (otherE.position.z - e.position.z) * (otherE.position.z - e.position.z);
    
                        if (sqrDst < _avoidanceRadius)
                        {
                           e.flockmateNumber.flockmates++;
                        }
                    }
                }
            }*/
        }
    }
}