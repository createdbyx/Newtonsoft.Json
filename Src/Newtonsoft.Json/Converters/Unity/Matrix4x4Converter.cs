#if UNITY_5
namespace Newtonsoft.Json.Converters.Unity
{
    using System;

    using Newtonsoft.Json.Serialization;
    using Newtonsoft.Json.Utilities;

    using UnityEngine;

    /// <summary>
    /// Converts a <see cref="Vector2"/> to and from JSON.
    /// </summary>
    public class Matrix4x4Converter : JsonConverter
    {
        private static string[] PropNames = { "m00", "m01", "m02", "m03", "m10", "m11", "m12", "m13", "m20", "m21", "m22", "m23", "m30", "m31", "m32", "m33" };

        private static readonly ThreadSafeStore<Type, ReflectionObject> ReflectionObjectPerType = new ThreadSafeStore<Type, ReflectionObject>(InitializeReflectionObject);

        private static ReflectionObject InitializeReflectionObject(Type t)
        {
            return ReflectionObject.Create(t, t.GetConstructor(Type.EmptyTypes), PropNames);
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
            foreach (var propName in PropNames)
            {
                writer.WritePropertyName((resolver != null) ? resolver.GetResolvedPropertyName(propName) : propName);
                serializer.Serialize(writer, reflectionObject.GetValue(value, propName), reflectionObject.GetType(propName));
            }
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
                    throw JsonSerializationException.Create(reader, "Cannot convert null value to Matrix4x4.");

                return null;
            }

            object[] values = new object[PropNames.Length];

            ReadAndAssert(reader);

            while (reader.TokenType == JsonToken.PropertyName)
            {
                string propertyName = reader.Value.ToString();

                var index = Array.IndexOf(PropNames, propertyName);
                if (index != -1)
                {
                    var name = PropNames[index];
                    ReadAndAssert(reader);
                    values[index] = serializer.Deserialize(reader, reflectionObject.GetType(name));
                }
                else
                {
                    reader.Skip();
                }

                ReadAndAssert(reader);
            }

            var matrix = (Matrix4x4)reflectionObject.Creator();
            matrix.m00 = (float)values[0];
            matrix.m01 = (float)values[1];
            matrix.m02 = (float)values[2];
            matrix.m03 = (float)values[3];
            matrix.m10 = (float)values[4];
            matrix.m11 = (float)values[5];
            matrix.m12 = (float)values[6];
            matrix.m13 = (float)values[7];
            matrix.m20 = (float)values[8];
            matrix.m21 = (float)values[9];
            matrix.m22 = (float)values[10];
            matrix.m23 = (float)values[11];
            matrix.m30 = (float)values[12];
            matrix.m31 = (float)values[13];
            matrix.m32 = (float)values[14];
            matrix.m33 = (float)values[15];
            return matrix;
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
                return (t == typeof(Matrix4x4));

            return false;
        }

        private static void ReadAndAssert(JsonReader reader)
        {
            if (!reader.Read())
                throw JsonSerializationException.Create(reader, "Unexpected end when reading Matrix4x4.");
        }
    }
}
#endif