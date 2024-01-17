using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Content.StaticData.Converters
{
    public class Vector2IntConverter : JsonConverter<Vector2Int>
    {
        public override Vector2Int ReadJson(JsonReader reader, Type objectType, Vector2Int existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return Vector2Int.zero;
            
            var jt = JToken.Load(reader);
            var parsedResult = JsonConvert.DeserializeObject<int[]>(jt.ToString());
            
            if (parsedResult != null) 
                return new Vector2Int(parsedResult[0], parsedResult[1]);
            
            Debug.LogError($"JSON Extensions: Failed to create Vector3: parsedResult is NULL");
            return Vector2Int.zero;
        }

        public override void WriteJson(JsonWriter writer, Vector2Int value, JsonSerializer serializer)
        {
            var tmp = new int[2]
            {
                value.x,
                value.y
            };
            serializer.Serialize(writer, tmp);
        }
    }
}