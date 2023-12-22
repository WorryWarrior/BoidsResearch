using System;

namespace Content.StaticData
{
    [Serializable]
    public record StageStaticData
    {
        public string StageKey { get; set; }
        public string StageTitle{ get; set; }
        public string StageDescription{ get; set; }
    }
}