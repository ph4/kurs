using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace kurs
{
    public class JsonSpecificationConverter :  Newtonsoft.Json.Converters.CustomCreationConverter<ISpecification>
    {
        public override ISpecification Create(Type objectType)
        {
            throw new NotImplementedException();
        }

        public ISpecification Create(Type objectType, JObject jObject)
        {
            var type = (string)jObject.Property("type");
            switch (type)
            {
                case "number":
                    return new Specification<float>();
                case "string":
                    return new Specification<string>();
                case "bool":
                    return new Specification<bool>();
                default:
                    throw new ApplicationException(String.Format("The given type {0} is not supported!", type));
            }

        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Load JObject from stream
            JObject jObject = JObject.Load(reader);

            // Create target object based on JObject
            var target = Create(objectType, jObject);

            // Populate the object properties
            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }
    }

    public interface ISpecification
    {
        string Name { get; set; }
        string Unit { get; set; }
        string ValueWithUnit { get; }
    }

    public class Specification<T> : ISpecification
    {
        public string Type { get; set; }

        public Type GetValueType()
        {
            return Value.GetType();
        }


        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("unit")]
        public string Unit { get; set; }

        public string ValueWithUnit => (Value.ToString() + " " + Unit).Trim();

        [JsonProperty("value")]
        public T Value { get; set; }

        public override string ToString() => ValueWithUnit;
    }

    partial class product
    {
        private JsonConverter jsonConverter = new JsonSpecificationConverter();
        public IList<ISpecification> Specifications =>
            specifications_json is null ? new List<ISpecification>()
                : JsonConvert.DeserializeObject<List<ISpecification>>(specifications_json, jsonConverter);
    }
}
