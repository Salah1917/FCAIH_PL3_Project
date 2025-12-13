using System.Text.Json.Serialization;

namespace DictionaryUI.DTO
{
    public class WordsDTO
    {
        public int? Id { get; set; }

        public string? Word { get; set; }

        public string? Definition { get; set; }

        [JsonPropertyName("wordType")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public WordType Type { get; set; }

        public string? Example { get; set; }
    }
}
