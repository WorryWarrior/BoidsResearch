using System;
using System.Collections.Generic;
using Content.StaticData;
using Zenject;

namespace Content.Infrastructure.Services.StaticData
{
    public interface IStaticDataService : IInitializable
    {
        public event Action Initialized;
        List<StageStaticData> GetAllStages();
        BoidSettingsStaticData GetSettingsStaticData();
    }
}