using System;
using Content.StaticData.Converters;
using Newtonsoft.Json;
using UnityEngine;

namespace Content.Data
{
    [Serializable]
    public class PlayerStateData
    {
        [JsonConverter(typeof(Vector3Converter))]
        public Vector3 PlayerPosition { get; set; }
    }
}