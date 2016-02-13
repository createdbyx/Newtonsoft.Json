#if UNITY_5
namespace Newtonsoft.Json.Converters.Unity
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json.Serialization;
    using Newtonsoft.Json.Utilities;

    using UnityEngine;

    /// <summary>
    /// Converts a <see cref="Vector2"/> to and from JSON.
    /// </summary>
    public class Vector2Converter : JsonConverter
    {
        private const string XName = "x";
        private const string YName = "y";

        private static readonly ThreadSafeStore<Type, ReflectionObject> ReflectionObjectPerType = new ThreadSafeStore<Type, ReflectionObject>(InitializeReflectionObject);

        private static ReflectionObject InitializeReflectionObject(Type t)
        {
            return ReflectionObject.Create(t, t.GetConstructor(new[] { typeof(float), typeof(float) }), XName, YName);
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
            writer.WritePropertyName((resolver != null) ? resolver.GetResolvedPropertyName(XName) : XName);
            serializer.Serialize(writer, reflectionObject.GetValue(value, XName), reflectionObject.GetType(XName));
            writer.WritePropertyName((resolver != null) ? resolver.GetResolvedPropertyName(YName) : YName);
            serializer.Serialize(writer, reflectionObject.GetValue(value, YName), reflectionObject.GetType(YName));
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
                    throw JsonSerializationException.Create(reader, "Cannot convert null value to Vector2.");

                return null;
            }

            object key = null;
            object value = null;

            ReadAndAssert(reader);

            while (reader.TokenType == JsonToken.PropertyName)
            {
                string propertyName = reader.Value.ToString();
                if (string.Equals(propertyName, XName, StringComparison.OrdinalIgnoreCase))
                {
                    ReadAndAssert(reader);
                    key = serializer.Deserialize(reader, reflectionObject.GetType(XName));
                }
                else if (string.Equals(propertyName, YName, StringComparison.OrdinalIgnoreCase))
                {
                    ReadAndAssert(reader);
                    value = serializer.Deserialize(reader, reflectionObject.GetType(YName));
                }
                else
                {
                    reader.Skip();
                }

                ReadAndAssert(reader);
            }

            return reflectionObject.Creator(key, value);
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

            if (t.IsValueType() && !t.IsGenericType())
                return (t == typeof(Vector2));

            return false;
        }

        private static void ReadAndAssert(JsonReader reader)
        {
            if (!reader.Read())
                throw JsonSerializationException.Create(reader, "Unexpected end when reading Vector2.");
        }
    }
}
#endif