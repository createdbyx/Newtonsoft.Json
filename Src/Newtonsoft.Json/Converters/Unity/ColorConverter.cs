#if UNITY_5
namespace Newtonsoft.Json.Converters.Unity
{
    using System;

    using Newtonsoft.Json.Serialization;
    using Newtonsoft.Json.Utilities;

    using UnityEngine;

    /// <summary>
    /// Converts a <see cref="Color"/> to and from JSON.
    /// </summary>
    public class ColorConverter : JsonConverter
    {
        private const string RName = "r";
        private const string GName = "g";
        private const string BName = "b";
        private const string AName = "a";

        private static readonly ThreadSafeStore<Type, ReflectionObject> ReflectionObjectPerType = new ThreadSafeStore<Type, ReflectionObject>(InitializeReflectionObject);

        private static ReflectionObject InitializeReflectionObject(Type t)
        {
            return ReflectionObject.Create(t, t.GetConstructor(new[] { typeof(float), typeof(float), typeof(float), typeof(float) }), RName, GName, BName, AName);
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
            writer.WritePropertyName((resolver != null) ? resolver.GetResolvedPropertyName(RName) : RName);
            serializer.Serialize(writer, reflectionObject.GetValue(value, RName), reflectionObject.GetType(RName));
            writer.WritePropertyName((resolver != null) ? resolver.GetResolvedPropertyName(GName) : GName);
            serializer.Serialize(writer, reflectionObject.GetValue(value, GName), reflectionObject.GetType(GName));
            writer.WritePropertyName((resolver != null) ? resolver.GetResolvedPropertyName(BName) : BName);
            serializer.Serialize(writer, reflectionObject.GetValue(value, BName), reflectionObject.GetType(BName));
            writer.WritePropertyName((resolver != null) ? resolver.GetResolvedPropertyName(AName) : AName);
            serializer.Serialize(writer, reflectionObject.GetValue(value, AName), reflectionObject.GetType(AName));
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
                    throw JsonSerializationException.Create(reader, "Cannot convert null value to Color.");

                return null;
            }

            object rValue = null;
            object gValue = null;
            object bValue = null;
            object aValue = null;

            ReadAndAssert(reader);

            while (reader.TokenType == JsonToken.PropertyName)
            {
                string propertyName = reader.Value.ToString();
                if (string.Equals(propertyName, RName, StringComparison.OrdinalIgnoreCase))
                {
                    ReadAndAssert(reader);
                    rValue = serializer.Deserialize(reader, reflectionObject.GetType(RName));
                }
                else if (string.Equals(propertyName, GName, StringComparison.OrdinalIgnoreCase))
                {
                    ReadAndAssert(reader);
                    gValue = serializer.Deserialize(reader, reflectionObject.GetType(GName));
                }
                else if (string.Equals(propertyName, BName, StringComparison.OrdinalIgnoreCase))
                {
                    ReadAndAssert(reader);
                    bValue = serializer.Deserialize(reader, reflectionObject.GetType(BName));
                }
                else if (string.Equals(propertyName, AName, StringComparison.OrdinalIgnoreCase))
                {
                    ReadAndAssert(reader);
                    aValue = serializer.Deserialize(reader, reflectionObject.GetType(AName));
                }
                else
                {
                    reader.Skip();
                }

                ReadAndAssert(reader);
            }

            return reflectionObject.Creator(rValue, gValue, bValue, aValue);
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
                return (t == typeof(Color));

            return false;
        }

        private static void ReadAndAssert(JsonReader reader)
        {
            if (!reader.Read())
                throw JsonSerializationException.Create(reader, "Unexpected end when reading Color.");
        }
    }
} 
#endif