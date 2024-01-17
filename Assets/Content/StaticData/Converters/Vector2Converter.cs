using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Content.StaticData.Converters
{
    public class Vector2Converter : JsonConverter<Vector2>
    {
        public override Vector2 ReadJson(JsonReader reader, Type objectType, Vector2 existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return Vector2.zero;
            
            var jt = JToken.Load(reader);
            var parsedResult = JsonConvert.DeserializeObject<float[]>(jt.ToString());
            
            if (parsedResult != null) 
                return new Vector2(parsedResult[0], parsedResult[1]);
            
            Debug.LogError($"JSON Extensions: Failed to create Vector3: parsedResult is NULL");
            return Vector2.zero;
        }

        public override void WriteJson(JsonWriter writer, Vector2 value, JsonSerializer serializer)
        {
            var tmp = new float[2]
            {
                value.x,
                value.y
            };
            serializer.Serialize(writer, tmp);
        }
    }
}