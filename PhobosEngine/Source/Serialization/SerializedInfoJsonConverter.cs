using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PhobosEngine.Serialization
{
    public class SerializedInfoJsonConverter : JsonConverter<SerializedInfo>
    {
        public override void Write(Utf8JsonWriter writer, SerializedInfo value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            WriteSerializedInfo(writer, value, options);
            writer.WriteEndObject();
        }

        private void WriteSerializedInfo(Utf8JsonWriter writer, SerializedInfo value, JsonSerializerOptions options)
        {
            // Loop through and write everything out of the data store (4 possible data types)
            foreach(var pair in value.Store)
            {
                switch(pair.Value)
                {
                    case string s:
                        writer.WriteString(pair.Key, s);
                        break;
                    case string[] ss:
                        WriteStringArray(writer, pair.Key, ss, options);
                        break;
                    case SerializedInfo info:
                        WriteRecursive(writer, pair.Key, info, options);
                        break;
                    case SerializedInfo[] infos:
                        WriteRecursiveArray(writer, pair.Key, infos, options);
                        break;
                }
            }
        }

        private void WriteStringArray(Utf8JsonWriter writer, string key, string[] values, JsonSerializerOptions options)
        {
            // Construct an array with a given key and values
            writer.WriteStartArray(key);
            foreach(string str in values)
            {
                writer.WriteStringValue(str);
            }
            writer.WriteEndArray();
        }

        private void WriteRecursive(Utf8JsonWriter writer, string key, SerializedInfo childInfo, JsonSerializerOptions options)
        {
            // Recursively write the object under the given key
            writer.WriteStartObject(key);
            WriteSerializedInfo(writer, childInfo, options);
            writer.WriteEndObject();
        }

        private void WriteRecursiveArray(Utf8JsonWriter writer, string key, SerializedInfo[] childInfos, JsonSerializerOptions options)
        {
            // Open array, write children, close array
            writer.WriteStartArray(key);
            foreach(SerializedInfo info in childInfos)
            {
                writer.WriteStartObject();
                Write(writer, info, options);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
        }

        public override SerializedInfo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Start with an empty SerializedInfo
            SerializedInfo info = new SerializedInfo();

            string activeKey = "";

            // Recursively reconstruct SerializedInfo objects
            while(reader.Read())
            {
                switch(reader.TokenType)
                {
                    case JsonTokenType.PropertyName:
                        // Set key value for use in next read
                        activeKey = reader.GetString();
                        break;
                    case JsonTokenType.String:
                        // Extract primitive string
                        info.Write(activeKey, reader.GetString());
                        break;
                    case JsonTokenType.StartObject:
                        // Recursive call to recover child SerializedInfo
                        info.Write(activeKey, Read(ref reader, typeToConvert, options));
                        break;
                    case JsonTokenType.StartArray:
                        // Arrays can be of two types: string[] or SerializedInfo[] (or empty)
                        reader.Read();
                        switch(reader.TokenType) {
                            case JsonTokenType.String:
                                info.Write(activeKey, ReadStringArray(ref reader, options));
                                break;
                            case JsonTokenType.StartObject:
                                info.Write(activeKey, ReadInfoArray(ref reader, typeToConvert, options));
                                break;
                            case JsonTokenType.EndArray:
                                info.WriteEmptyArray(activeKey);
                                break;
                        }
                        break;
                    case JsonTokenType.EndObject:
                        // Indicates that the SerializedInfo has been fully read
                        return info;
                }
            }
            throw new JsonException("no end object token");
        }

        private string[] ReadStringArray(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            List<string> strs = new List<string>();
            // Needed to "peak" one ahead, so add that value now
            strs.Add(reader.GetString());
            while(reader.Read())
            {
                switch(reader.TokenType)
                {
                    case JsonTokenType.String:
                        strs.Add(reader.GetString());
                        break;
                    case JsonTokenType.EndArray:
                        return strs.ToArray();
                }
            }
            throw new JsonException("did not receive end array after start array");
        }


        private SerializedInfo[] ReadInfoArray(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            List<object> objs = new List<object>();
            // Read one immediately due to peak
            objs.Add(Read(ref reader, typeToConvert, options));
            while(reader.Read())
            {
                switch(reader.TokenType)
                {
                    case JsonTokenType.StartObject:
                        objs.Add(Read(ref reader, typeToConvert, options));
                        break;
                    case JsonTokenType.EndArray:
                        return objs.ToArray() as SerializedInfo[];
                }
            }
            throw new JsonException("did not receive end array after start array");
        }
    }
}