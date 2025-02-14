using Microsoft.Extensions.Configuration;

namespace BaracataBot
{
    internal class Configuration
    {
        public Settings Settings { get; set; }

        public Configuration() {
            var environmentConfig = new ConfigurationBuilder()
               .AddEnvironmentVariables()
               .Build();
            var environment = environmentConfig["RUNTIME_ENVIRONMENT"];
            Console.WriteLine($"running on {environment}");

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, false)
                .AddJsonFile($"appsettings.{environment}.json", false, true)
                .Build();

            Settings = new Settings()
            {
                DiscordBotToken = configuration["DiscordBotToken"],
                ConnectionStrings = new ConnectionStrings() { defaultConnection = configuration.GetConnectionString("defaultConnection") }
            };
        }
    }
}
