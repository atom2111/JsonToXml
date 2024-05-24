using System;
using System.Text.Json;
using System.Xml.Linq;

class Program
{
    static void Main(string[] args)
    {
        // Пример JSON
        string json = @"
        {
            ""name"": ""John Smith"",
            ""age"": 30,
            ""address"": {
                ""street"": ""123 Main St"",
                ""city"": ""Anytown"",
                ""zipcode"": ""12345""
            },
            ""phones"": [
                ""123-456-7890"",
                ""098-765-4321""
            ]
        }";

        // Преобразование JSON в XML
        string xml = ConvertJsonToXml(json);

        Console.WriteLine(xml);
    }

    static string ConvertJsonToXml(string json)
    {
        using (JsonDocument doc = JsonDocument.Parse(json))
        {
            XElement root = new XElement("root");
            ConvertJsonToXml(doc.RootElement, root);
            return root.ToString();
        }
    }

    static void ConvertJsonToXml(JsonElement jsonElement, XElement parent)
    {
        switch (jsonElement.ValueKind)
        {
            case JsonValueKind.Object:
                foreach (var property in jsonElement.EnumerateObject())
                {
                    XElement element = new XElement(property.Name);
                    ConvertJsonToXml(property.Value, element);
                    parent.Add(element);
                }
                break;
            case JsonValueKind.Array:
                foreach (var item in jsonElement.EnumerateArray())
                {
                    XElement element = new XElement("item");
                    ConvertJsonToXml(item, element);
                    parent.Add(element);
                }
                break;
            case JsonValueKind.Number:
                parent.Add(new XElement("value", jsonElement.GetDouble()));
                break;
            case JsonValueKind.String:
                parent.Add(new XElement("value", jsonElement.GetString()));
                break;
            case JsonValueKind.True:
                parent.Add(new XElement("value", true));
                break;
            case JsonValueKind.False:
                parent.Add(new XElement("value", false));
                break;
            case JsonValueKind.Null:
                parent.Add(new XElement("value"));
                break;
            default:
                throw new NotSupportedException($"Неподдерживаемый тип значения JSON: {jsonElement.ValueKind}");
        }
    }

}
