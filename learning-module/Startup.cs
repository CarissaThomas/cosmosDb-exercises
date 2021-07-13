using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;

namespace learning_module
{
    class Startup
    { 
        //private DocumentClient client;

        // private async Task BasicOperations()
        // {
        //     this.client = new DocumentClient(new Uri(ConfigurationManager.AppSettings["accountEndpoint"]), ConfigurationManager.AppSettings["accountKey"]);

        //     await this.client.CreateDatabaseIfNotExistsAsync(new Database { Id = "Users" });

        //     await this.client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri("Users"), new DocumentCollection { Id = "WebCustomers" });

        //     Console.WriteLine("Database and collection validation complete");

        //     return client;
        // }
    }
}