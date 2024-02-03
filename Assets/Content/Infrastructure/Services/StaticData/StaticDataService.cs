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
        private BoidSettingsStaticData _boidSettingsStaticData;

        public event Action Initialized;

        public void Initialize()
        {
            LoadStagesData();
            LoadBoidSettingsData();

            Initialized?.Invoke();
        }

        public List<StageStaticData> GetAllStages() =>
            _stages.Values.ToList();
        public BoidSettingsStaticData GetSettingsStaticData() =>
            _boidSettingsStaticData;

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

        private void LoadBoidSettingsData()
        {
            _boidSettingsStaticData = GetBoidSettings();
        }


        #region Population Methods

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

        private BoidSettingsStaticData GetBoidSettings() => new()
        {
            BoidCountValues = new Vector2Int(0, 2000),
            BoidMinSpeedValues = new Vector2(1f, 10f),
            BoidMaxSpeedValues = new Vector2(2f, 20f),
            BoidAlignmentWeightValues = new Vector2(0f, 10f),
            BoidCohesionWeightValues = new Vector2(0f, 10f),
            BoidSeparationWeightValues = new Vector2(0f, 10f),
            BoidTargetWeightValues = new Vector2(0f, 10f),
            BoidCollisionAvoidanceValues = new Vector2(0f, 30f)
        };

        #endregion
    }
}