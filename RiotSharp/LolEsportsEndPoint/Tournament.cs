using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RiotSharp.LolEsportsEndPoint
{
    public class Contestant
    {
        public string id { get; set; }
        public string name { get; set; }
        public string acronym { get; set; }
    }

    [JsonObject]
    public class Contestants
    {
        public List<Contestant> ContestantsList { get; set; }
    }

    public class Tourney
    {
        public string id { get; set; }
        public string tournamentName { get; set; }
        public string namePublic { get; set; }
        public Contestants contestants { get; set; }
        public bool isFinished { get; set; }
        public string dateBegin { get; set; }
        public string dateEnd { get; set; }
        public int noVods { get; set; }
        public string season { get; set; }
        public bool published { get; set; }
        public string winner { get; set; }
    }

    public class Tourneys
    {
        public List<Tourney> TourneysList { get;set;}
    }

    public class TourneyConverter : JsonCreationConverter<Tourneys>
    {
        protected override Tourneys Create(Type objectType, JObject jObject)
        {
            JsonSerializer s = new JsonSerializer();
            s.Converters.Add(new ContestantsConverter());
            Tourneys t = new Tourneys();
            List<JToken> L = jObject.Children<JToken>().ToList<JToken>();
            t.TourneysList = new List<Tourney>();
            foreach (JProperty l in L)
            {
                Tourney to = l.Value.ToObject<Tourney>();
                to.id = l.Name.ToLower().Replace("tourney", "");
                t.TourneysList.Add(to);

            }
            return t;
        }


    }
    public class ContestantsConverter : JsonCreationConverter<Contestants>
    {
        protected override Contestants Create(Type objectType, JObject jObject)
        {
            Contestants t = new Contestants();
            List<JToken> L = jObject.Children<JToken>().ToList<JToken>();
            t.ContestantsList = new List<Contestant>();
            foreach (JProperty l in L)
                t.ContestantsList.Add(l.Value.ToObject<Contestant>());

            return t;
        }


    }
    /// <summary>Base Generic JSON Converter that can help quickly define converters for specific types by automatically
    /// generating the CanConvert, ReadJson, and WriteJson methods, requiring the implementer only to define a strongly typed Create method.</summary>
    public abstract class JsonCreationConverter<T> : JsonConverter
    {
        /// <summary>Create an instance of objectType, based properties in the JSON object</summary>
        /// <param name="objectType">type of object expected</param>
        /// <param name="jObject">contents of JSON object that will be deserialized</param>
        protected abstract T Create(Type objectType, JObject jObject);

        /// <summary>Determines if this converted is designed to deserialization to objects of the specified type.</summary>
        /// <param name="objectType">The target type for deserialization.</param>
        /// <returns>True if the type is supported.</returns>
        public override bool CanConvert(Type objectType)
        {
            // FrameWork 4.5
            // return typeof(T).GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());
            // Otherwise
            return typeof(T).IsAssignableFrom(objectType);
        }

        /// <summary>Parses the json to the specified type.</summary>
        /// <param name="reader">Newtonsoft.Json.JsonReader</param>
        /// <param name="objectType">Target type.</param>
        /// <param name="existingValue">Ignored</param>
        /// <param name="serializer">Newtonsoft.Json.JsonSerializer to use.</param>
        /// <returns>Deserialized Object</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            // Load JObject from stream
            JObject jObject = JObject.Load(reader);

            // Create target object based on JObject
            T target = Create(objectType, jObject);

            //Create a new reader for this jObject, and set all properties to match the original reader.
            JsonReader jObjectReader = jObject.CreateReader();
            jObjectReader.Culture = reader.Culture;
            jObjectReader.DateParseHandling = reader.DateParseHandling;
            jObjectReader.DateTimeZoneHandling = reader.DateTimeZoneHandling;
            jObjectReader.FloatParseHandling = reader.FloatParseHandling;

            // Populate the object properties
            serializer.Populate(jObjectReader, target);

            return target;
        }

        /// <summary>Serializes to the specified type</summary>
        /// <param name="writer">Newtonsoft.Json.JsonWriter</param>
        /// <param name="value">Object to serialize.</param>
        /// <param name="serializer">Newtonsoft.Json.JsonSerializer to use.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
