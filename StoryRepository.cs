using Proiect.POO; // Referință către modele
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Proiect_POO
{
    public static class StoryRepository
    {
        private const string StoryEntryName = "story.json";

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };

        public static StoryDefinition Load(string zipPath)
        {
            if (!File.Exists(zipPath)) throw new FileNotFoundException("Arhiva nu există.");

            using var zip = ZipFile.OpenRead(zipPath);
            var storyEntry = zip.GetEntry(StoryEntryName) ?? throw new InvalidDataException("Lipsă story.json");

            using var stream = storyEntry.Open();
            using var reader = new StreamReader(stream, Encoding.UTF8);
            return JsonSerializer.Deserialize<StoryDefinition>(reader.ReadToEnd(), _jsonOptions)
                   ?? throw new InvalidDataException("Eroare deserializare.");
        }
    }
}
