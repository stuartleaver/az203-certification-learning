using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;

namespace KeyVault.Secrets
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, builder) =>
            {
                //Build the config from sources we have
                var config = builder.Build();
                
                //Create Managed Service Identity token provider
                var tokenProvider = new AzureServiceTokenProvider();
                
                //Create the Key Vault client
                var keyVaultClient = new KeyVaultClient(
                           new KeyVaultClient.AuthenticationCallback(
                               tokenProvider.KeyVaultTokenCallback));
                
                //Add Key Vault to configuration pipeline
                builder.AddAzureKeyVault(config["KeyVault:BaseUrl"], keyVaultClient, new DefaultKeyVaultSecretManager());
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            }
        );
    }
}
