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
    class Program
    {
        private DocumentClient client;

        static void Main(string[] args)
        {
            try
            {
                Program p = new Program();
                p.BasicOperations().Wait();
            }
            catch (DocumentClientException de)
            {
                Exception baseException = de.GetBaseException();
                Console.WriteLine("{0} error occurred: {1}, Message: {2}", de.StatusCode, de.Message, baseException.Message);
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }
            finally
            {
                Console.WriteLine("End of demo, press any key to exit.");
                Console.ReadKey();
            }
        }

        private async Task BasicOperations()
        {
            this.client = new DocumentClient(new Uri(ConfigurationManager.AppSettings["accountEndpoint"]), ConfigurationManager.AppSettings["accountKey"]);

            await this.client.CreateDatabaseIfNotExistsAsync(new Database { Id = "Users" });

            await this.client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri("Users"), new DocumentCollection { Id = "WebCustomers" });

            Operations operations = new Operations(); 

            Console.WriteLine("Database and collection validation complete");

            User yanhe = new User
            {
                Id = "1",
                UserId = "yanhe",
                LastName = "He",
                FirstName = "Yan",
                Email = "yanhe@contoso.com",
                OrderHistory = new OrderHistory[]
            {
            new OrderHistory {
                OrderId = "1000",
                DateShipped = "08/17/2018",
                Total = "52.49"
            }
            },
                ShippingPreference = new ShippingPreference[]
            {
                new ShippingPreference {
                        Priority = 1,
                        AddressLine1 = "90 W 8th St",
                        City = "New York",
                        State = "NY",
                        ZipCode = "10001",
                        Country = "USA"
                }
                },
            };

            await operations.CreateUserDocumentIfNotExists("Users", "WebCustomers", yanhe, this.client);

            yanhe.LastName = "Suh";

            await operations.ReplaceUserDocument("Users", "WebCustomers", yanhe, this.client);

            User nelapin = new User
            {
                Id = "2",
                UserId = "nelapin",
                LastName = "Pindakova",
                FirstName = "Nela",
                Email = "nelapin@contoso.com",
                Dividend = "8.50",
                OrderHistory = new OrderHistory[]
                {
                    new OrderHistory {
                    OrderId = "1001",
                    DateShipped = "08/17/2018",
                    Total = "105.89"
                }
                },
                ShippingPreference = new ShippingPreference[]
                {
                    new ShippingPreference {
                    Priority = 1,
                    AddressLine1 = "505 NW 5th St",
                    City = "New York",
                    State = "NY",
                    ZipCode = "10001",
                    Country = "USA"
                    },
                new ShippingPreference {
                        Priority = 2,
                        AddressLine1 = "505 NW 5th St",
                        City = "New York",
                        State = "NY",
                        ZipCode = "10001",
                        Country = "USA"
                    }
                },
                Coupons = new CouponsUsed[]
                {
                new CouponsUsed{
                    CouponCode = "Fall2018"
                    }
                }
            };

            await operations.CreateUserDocumentIfNotExists("Users", "WebCustomers", nelapin, this.client);

            await operations.ReadUserDocument("Users", "WebCustomers", yanhe, this.client);

            await operations.DeleteUserDocument("Users", "WebCustomers", yanhe, this.client);
        }

        

    }
}
