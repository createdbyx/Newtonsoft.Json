#if UNITY_5
namespace Newtonsoft.Json.Converters.Unity
{
    using System;

    using Newtonsoft.Json.Serialization;
    using Newtonsoft.Json.Utilities;

    using UnityEngine;

    /// <summary>
    /// Converts a <see cref="GameObject"/> to and from JSON.
    /// </summary>
    public class GameObjectConverter : JsonConverter
    {
        private const string Name = "name";
        private const string isStaticName = "isStatic";
        private const string layerName = "layer";
        private const string tagName = "tag";
        private const string flagsName = "hideFlags";

        private static readonly ThreadSafeStore<Type, ReflectionObject> ReflectionObjectPerType = new ThreadSafeStore<Type, ReflectionObject>(InitializeReflectionObject);

        private static ReflectionObject InitializeReflectionObject(Type t)
        {
            return ReflectionObject.Create(t, t.GetConstructor(Type.EmptyTypes), Name, isStaticName, layerName, tagName, flagsName);
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            ReflectionObject reflectionObject = ReflectionObjectPerType.Get(value.GetType());

            DefaultContractResolver resolver = serializer.ContractResolver as DefaultContractResolver;

            writer.WriteStartObject();
            writer.WritePropertyName((resolver != null) ? resolver.GetResolvedPropertyName(Name) : Name);
            serializer.Serialize(writer, reflectionObject.GetValue(value, Name), reflectionObject.GetType(Name));
            writer.WritePropertyName((resolver != null) ? resolver.GetResolvedPropertyName(isStaticName) : isStaticName);
            serializer.Serialize(writer, reflectionObject.GetValue(value, isStaticName), reflectionObject.GetType(isStaticName));
            writer.WritePropertyName((resolver != null) ? resolver.GetResolvedPropertyName(layerName) : layerName);
            serializer.Serialize(writer, reflectionObject.GetValue(value, layerName), reflectionObject.GetType(layerName));
            writer.WritePropertyName((resolver != null) ? resolver.GetResolvedPropertyName(tagName) : tagName);
            serializer.Serialize(writer, reflectionObject.GetValue(value, tagName), reflectionObject.GetType(tagName));
            writer.WritePropertyName((resolver != null) ? resolver.GetResolvedPropertyName(flagsName) : flagsName);
            serializer.Serialize(writer, reflectionObject.GetValue(value, flagsName), reflectionObject.GetType(flagsName));
            writer.WriteEndObject();
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            bool isNullable = ReflectionUtils.IsNullableType(objectType);

            Type t = (isNullable)
                         ? Nullable.GetUnderlyingType(objectType)
                         : objectType;

            ReflectionObject reflectionObject = ReflectionObjectPerType.Get(t);

            if (reader.TokenType == JsonToken.Null)
            {
                if (!isNullable)
                    throw JsonSerializationException.Create(reader, "Cannot convert null value to GameObject.");

                return null;
            }

            object name = null;
            object isStatic = null;
            object layer = null;
            object tag = null;
            object flags = null;

            ReadAndAssert(reader);

            while (reader.TokenType == JsonToken.PropertyName)
            {
                string propertyName = reader.Value.ToString();
                if (string.Equals(propertyName, Name, StringComparison.OrdinalIgnoreCase))
                {
                    ReadAndAssert(reader);
                    name = serializer.Deserialize(reader, reflectionObject.GetType(Name));
                }
                else if (string.Equals(propertyName, isStaticName, StringComparison.OrdinalIgnoreCase))
                {
                    ReadAndAssert(reader);
                    isStatic = serializer.Deserialize(reader, reflectionObject.GetType(isStaticName));
                }
                else if (string.Equals(propertyName, layerName, StringComparison.OrdinalIgnoreCase))
                {
                    ReadAndAssert(reader);
                    layer = serializer.Deserialize(reader, reflectionObject.GetType(layerName));
                }
                else if (string.Equals(propertyName, tagName, StringComparison.OrdinalIgnoreCase))
                {
                    ReadAndAssert(reader);
                    tag = serializer.Deserialize(reader, reflectionObject.GetType(tagName));
                }
                else if (string.Equals(propertyName, flagsName, StringComparison.OrdinalIgnoreCase))
                {
                    ReadAndAssert(reader);
                    flags = serializer.Deserialize(reader, reflectionObject.GetType(flagsName));
                }
                else
                {
                    reader.Skip();
                }

                ReadAndAssert(reader);
            }

            var obj = (GameObject)reflectionObject.Creator();
            obj.name = (string)name;
            obj.isStatic = (bool)isStatic;
            obj.layer = (int)layer;
            obj.tag = (string)tag;
            obj.hideFlags = (HideFlags)flags;
            return obj;
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// 	<c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            Type t = (ReflectionUtils.IsNullableType(objectType))
                         ? Nullable.GetUnderlyingType(objectType)
                         : objectType;

            if (!t.IsValueType() && !t.IsGenericType())
                return (t == typeof(GameObject));

            return false;
        }

        private static void ReadAndAssert(JsonReader reader)
        {
            if (!reader.Read())
                throw JsonSerializationException.Create(reader, "Unexpected end when reading GameObject.");
        }
    }
} 
#endif