using Newtonsoft.Json.Linq;
using OnlineStore;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

string databaseServer = "localhost",
    databaseName = "BWP401F22",
    databaseUsername = "sa",
    databasePassword = "mfbzLMC+ubfhvEy/OWooPA==",
    serverUrl = "http://localhost/API/",
    clientUrl = "http://localhost/OnlineStore/",
    ImageDirectory = "Images";

try
{
    FileInfo configFile = new FileInfo("config.json");
    using (StreamReader stream = new StreamReader(configFile.OpenRead()))
    {
        string json = stream.ReadToEnd();
        JObject jsonObject = JObject.Parse(json);
        foreach (var property in jsonObject)
        {
            string key = property.Key;
            JToken value = property.Value??"";

            if (!string.IsNullOrEmpty(value.ToString()))
            {
                switch (key)
                {
                    case "databaseServer":
                        databaseServer = value.ToString();
                        break;
                    case "databaseName":
                        databaseName = value.ToString();
                        break;
                    case "databaseUsername":
                        databaseUsername = value.ToString();
                        break;
                    case "databasePassword":
                        databasePassword = value.ToString();
                        break;
                    case "serverUrl":
                        serverUrl = value.ToString();
                        break;
                    case "clientUrl":
                        clientUrl = value.ToString();
                        break;
                    case "ImageDirectory":
                        ImageDirectory = value.ToString();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}

DatabaseConnection.SetConnectionStringValue(databaseServer, databaseName, databaseUsername, Authentication.Decrypt(databasePassword));
Files.SetUploadImagePath(ImageDirectory);
Config.setApiUrl(serverUrl);

app.Run();
