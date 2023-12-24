using System;
using System.Collections.Generic;
using System.Linq;
using Content.Boids.Interfaces;
using Content.StaticData;
using UnityEngine;

namespace Content.Infrastructure.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private Dictionary<string, StageStaticData> _stages;
        
        public Action Initialized { get; set; }
        
        public void Initialize()
        {
            LoadStagesData();
            Initialized?.Invoke();
        }

        public List<StageStaticData> GetAllStages() => _stages.Values.ToList();
        
        // At this point it could be loaded remotely, constructed locally etc...
        private void LoadStagesData()
        {
            _stages = new Dictionary<string, StageStaticData>();

            List<StageStaticData> stageList = GetStagesList();
            
            for (int i = 0; i < stageList.Count; i++)
            {
                _stages[stageList[i].StageKey] = stageList[i];
            }
        }

        private List<StageStaticData> GetStagesList() => new()
            {
                new StageStaticData
                {
                    StageKey = "Stage_BoidsEntitas",
                    StageDescription = "Entitas Implementation",
                    StageTitle = "Entitas",
                    CameraSpawnPoint = new Vector3(25f, 0f, 10f),
                    BoidsSimulationType = BoidsSimulationType.Entitas,
                },
                new StageStaticData
                {
                    StageKey = "Stage_BoidsLeoEcs",
                    StageDescription = "LeoEcs Implementation",
                    StageTitle = "LeoEcs",
                    CameraSpawnPoint = new Vector3(25f, 0f, 10f),
                    BoidsSimulationType = BoidsSimulationType.LeoEcs,
                },
                new StageStaticData
                {
                    StageKey = "Stage_BoidsNaive",
                    StageDescription = "Naive Implementation",
                    StageTitle = "Naive",
                    CameraSpawnPoint = new Vector3(25f, 0f, 10f),
                    BoidsSimulationType = BoidsSimulationType.Naive,
                }
            };
        }
    
}