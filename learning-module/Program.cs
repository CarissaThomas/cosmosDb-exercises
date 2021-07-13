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

                p.CheckDatabaseAndCollection().Wait();
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

        private static DocumentClient InitalizeDocumentClient()
        {
            return new DocumentClient(new Uri(ConfigurationManager.AppSettings["accountEndpoint"]), ConfigurationManager.AppSettings["accountKey"]);
        }

        private static async Task<ResourceResponse<Database>> CreateDatabaseIfItDoesntExist(DocumentClient client)
        {
            return await client.CreateDatabaseIfNotExistsAsync(new Database { Id = "Users" });
        }

        private static async Task<ResourceResponse<DocumentCollection>> CreateDocumentCollectionIfNotExistsAsync(DocumentClient client)
        {
            return await client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri("Users"), new DocumentCollection { Id = "WebCustomers" }); ;
        }

        private async Task CheckDatabaseAndCollection()
        {
            this.client = InitalizeDocumentClient();

            await CreateDatabaseIfItDoesntExist(client);

            await CreateDocumentCollectionIfNotExistsAsync(client);
        }

        private async Task<User> CreateUser(DocumentClient client)
        {
            Operations operations = new Operations();

            User yanhe = CreateYanhe();

            return await operations.CreateUserDocumentIfNotExists("Users", "WebCustomers", yanhe, client);
        }

         private async Task<User> ReadUser(DocumentClient client)
        {
            Operations operations = new Operations();

            User yanhe = CreateYanhe();

            //CREATE USER HERE IN TESTS

            return await operations.ReadUserDocument("Users", "WebCustomers", yanhe, client);
        }

        private async Task<User> UpdateUser(DocumentClient client)
        {
            Operations operations = new Operations();

            User yanhe = CreateYanhe();

            yanhe.LastName = "Suh";

            return await operations.ReplaceUserDocument("Users", "WebCustomers", yanhe, client);
        }

        private async Task<User> DeleteUser(DocumentClient client)
        {
            Operations operations = new Operations();

            User nelapin = CreateNelapin();

            await operations.CreateUserDocumentIfNotExists("Users", "WebCustomers", nelapin, client);

            await operations.ReadUserDocument("Users", "WebCustomers", nelapin, client);

            return await operations.DeleteUserDocument("Users", "WebCustomers", nelapin, this.client);
        }

        private static User CreateNelapin()
        {
            return new User
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
        }

        private static User CreateYanhe()
        {
            return new User
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
        }
    }
}
