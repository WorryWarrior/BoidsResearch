using System;
using System.Collections.Generic;
using Content.StaticData;
using Zenject;

namespace Content.Infrastructure.Services.StaticData
{
    public interface IStaticDataService : IInitializable
    {
        Action Initialized { get; set; }
        List<StageStaticData> GetAllStages();
    }
}