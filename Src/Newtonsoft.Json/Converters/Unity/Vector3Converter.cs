#if UNITY_5
namespace Newtonsoft.Json.Converters.Unity
{
    using System;

    using Newtonsoft.Json.Serialization;
    using Newtonsoft.Json.Utilities;

    using UnityEngine;

    /// <summary>
    /// Converts a <see cref="Vector3"/> to and from JSON.
    /// </summary>
    public class Vector3Converter : JsonConverter
    {
        private const string XName = "x";
        private const string YName = "y";
        private const string ZName = "z";

        private static readonly ThreadSafeStore<Type, ReflectionObject> ReflectionObjectPerType = new ThreadSafeStore<Type, ReflectionObject>(InitializeReflectionObject);

        private static ReflectionObject InitializeReflectionObject(Type t)
        {
            return ReflectionObject.Create(t, t.GetConstructor(new[] { typeof(float), typeof(float), typeof(float) }), XName, YName, ZName);
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
            writer.WritePropertyName((resolver != null) ? resolver.GetResolvedPropertyName(ZName) : ZName);
            serializer.Serialize(writer, reflectionObject.GetValue(value, ZName), reflectionObject.GetType(ZName));
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
                    throw JsonSerializationException.Create(reader, "Cannot convert null value to Vector3.");

                return null;
            }

            object xValue = null;
            object yValue = null;
            object zValue = null;

            ReadAndAssert(reader);

            while (reader.TokenType == JsonToken.PropertyName)
            {
                string propertyName = reader.Value.ToString();
                if (string.Equals(propertyName, XName, StringComparison.OrdinalIgnoreCase))
                {
                    ReadAndAssert(reader);
                    xValue = serializer.Deserialize(reader, reflectionObject.GetType(XName));
                }
                else if (string.Equals(propertyName, YName, StringComparison.OrdinalIgnoreCase))
                {
                    ReadAndAssert(reader);
                    yValue = serializer.Deserialize(reader, reflectionObject.GetType(YName));
                }
                else if (string.Equals(propertyName, ZName, StringComparison.OrdinalIgnoreCase))
                {
                    ReadAndAssert(reader);
                    zValue = serializer.Deserialize(reader, reflectionObject.GetType(ZName));
                }
                else
                {
                    reader.Skip();
                }

                ReadAndAssert(reader);
            }

            return reflectionObject.Creator(xValue, yValue, zValue);
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
                return (t == typeof(Vector3));

            return false;
        }

        private static void ReadAndAssert(JsonReader reader)
        {
            if (!reader.Read())
                throw JsonSerializationException.Create(reader, "Unexpected end when reading Vector3.");
        }
    }
} 
#endif