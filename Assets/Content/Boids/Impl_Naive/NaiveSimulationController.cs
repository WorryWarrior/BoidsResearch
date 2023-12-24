using System;
using System.Collections.Generic;
using Content.Boids.Interfaces;
using Content.Infrastructure.Factories.Interfaces;
using Content.Infrastructure.Services.PersistentData;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Content.Boids.Impl_Naive
{
    public class NaiveSimulationController : MonoBehaviour, IBoidsSimulationController
    {
        public Action Initialized { get; set; }

        private IPersistentDataService _persistentDataService;
        private IBoidFactory _boidFactory;
        
        private readonly List<Boid_Naive> _boids = new();
        private GameObject _boidTarget;
        
        [Inject]
        private void Construct(
            IPersistentDataService persistentDataService,
            IBoidFactory boidFactory)
        {
            _persistentDataService = persistentDataService;
            _boidFactory = boidFactory;
        }
        
        public async void InitializeBoids()
        {
            int safeBoidCount = Mathf.Clamp(_persistentDataService.BoidsSettings.BoidCount, 0, 1000);
            GameObject parentGO = new GameObject("Boid_Parent");
            _boidTarget = new GameObject("Boid_Target");
            
            for (int i = 0; i < safeBoidCount; i++) 
            {
                Vector3 pos = Random.insideUnitSphere * _persistentDataService.BoidsSettings.SpawnRadius;
                GameObject boidGO = await _boidFactory.Create(pos);
                boidGO.transform.parent = parentGO.transform;
                boidGO.transform.forward = Random.insideUnitSphere;
                
                Boid_Naive boidNaiveComponent = boidGO.AddComponent<Boid_Naive>();
                boidNaiveComponent.Initialize(_persistentDataService.BoidsSettings, _boidTarget.transform);
                _boids.Add(boidNaiveComponent);
            }
            
            Initialized?.Invoke();
        }
        
        private void Update ()
        {
            for (int i = 0; i < _boids.Count; i++)
            {
                Vector3 flockHeading = Vector3.zero;
                Vector3 flockCenter = Vector3.zero;
                Vector3 avoidanceHeading = Vector3.zero;
                int numFlockmates = 0;
            
                for (int j = 0; j < _boids.Count; j++)
                {
                    if (i != j)
                    {
                        Vector3 dist = _boids[j].boidPosition - _boids[i].boidPosition;

                        if (dist.magnitude < _persistentDataService.BoidsSettings.PerceptionRadius)
                        {
                            numFlockmates++;
                            flockHeading += _boids[j].forwardDir;
                            flockCenter += _boids[j].boidPosition;

                            if (dist.magnitude < _persistentDataService.BoidsSettings.AvoidanceRadius)
                            {
                                avoidanceHeading -= dist / dist.sqrMagnitude;
                            }
                        }
                    }
                }
            
                _boids[i].avgFlockHeading = flockHeading;
                _boids[i].centreOfFlockmates = flockCenter;
                _boids[i].avgAvoidanceHeading = avoidanceHeading;
                _boids[i].numPerceivedFlockmates = numFlockmates;

                _boids[i].UpdateBoid();
            }
        
            /*int numBoids = boids.Length;
        BoidData[] boidData = new BoidData[numBoids];

        for (int i = 0; i < boids.Length; i++) 
        {
            boidData[i].position = boids[i].boidPosition;
            boidData[i].direction = boids[i].forward;
        }
        
        ComputeBuffer boidBuffer = new ComputeBuffer (numBoids, BoidData.Size);
        boidBuffer.SetData (boidData);

        compute.SetBuffer (0, "boids", boidBuffer);
        compute.SetInt ("numBoids", boids.Length);
        compute.SetFloat ("viewRadius", settings.perceptionRadius);
        compute.SetFloat ("avoidRadius", settings.avoidanceRadius);

        int threadGroups = Mathf.CeilToInt (numBoids / (float) 1024);
        compute.Dispatch (0, threadGroups, 1, 1);

        boidBuffer.GetData (boidData);

        
        for (int i = 0; i < boids.Length; i++) 
        {
            boids[i].avgFlockHeading = boidData[i].flockHeading;
            boids[i].centreOfFlockmates = boidData[i].flockCentre;
            boids[i].avgAvoidanceHeading = boidData[i].avoidanceHeading;
            boids[i].numPerceivedFlockmates = boidData[i].numFlockmates;

            boids[i].UpdateBoid ();
        }
        
        boidBuffer.Release();*/
        }
        
        
        /*public struct BoidData
         {
            public Vector3 position;
            public Vector3 direction;

            public Vector3 flockHeading;
            public Vector3 flockCentre;
            public Vector3 avoidanceHeading;
            public int numFlockmates;

            public static int Size {
                get {
                    return sizeof (float) * 3 * 5 + sizeof (int);
                }
            }
        }*/
    }
}