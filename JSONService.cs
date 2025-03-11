using System;
using System.Text.Json;
using System.Collections;

namespace BlazorApp1
{
    public class MyData
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class JSONService
    {

        public static void WriteToJsonFile(string filePath, List<MyData> data)
        {
            var existingData = ReadListFromJsonFile(filePath);
            existingData.AddRange(data);

            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(existingData, options);
            File.WriteAllText(filePath, jsonString);
        }

        public static List<MyData> ReadListFromJsonFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new List<MyData>(); // Return an empty list if the file doesn't exist
            }
            string jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<MyData>>(jsonString);
        }

        public static void CreateJsonFile(int Id, string Name)
        {
            MyData data = new MyData
            {
                Id = Id,
                Name = Name
            };
            var list = new List<MyData> { data };
            WriteToJsonFile("Info/data.json", list);
        }
    }
}
