using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace AzureQueueApp
{
    class Program
    {
        // { "connectionString": "DefaultEndpointsProtocol=https;EndpointSuffix=core.windows.net;AccountName=[unique-name];AccountKey=[account-key]" }
        private const string ConnectionString = "<STORAGE_ACCOUNT_CONNECTION_STRING>";
        private const string QueueName = "newsqueue";

        static async Task Main(string[] args)
        {
          if (args.Length > 0)
          {
            string value = String.Join(" ", args);

            await SendArticleAsync(value);

            Console.WriteLine($"Sent: {value}");
          }
          else
          {
            string value = await ReceiveArticleAsync();

            Console.WriteLine($"Received {value}");
          }
        }

        static async Task SendArticleAsync(string newsMessage)
        {
          CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);

          CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

          CloudQueue queue = queueClient.GetQueueReference(QueueName);
          bool createdQueue = await queue.CreateIfNotExistsAsync();
          if (createdQueue)
          {
            Console.WriteLine("The queue of news articles was created.");
          }

          CloudQueueMessage articleMessage = new CloudQueueMessage(newsMessage);
          await queue.AddMessageAsync(articleMessage);
        }

        static CloudQueue GetQueue()
        {
          CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);

          CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
          return queueClient.GetQueueReference(QueueName);
        }

        static async Task<string> ReceiveArticleAsync()
        {
          CloudQueue queue = GetQueue();
          bool exists = await queue.ExistsAsync();
          if (exists)
          {
            CloudQueueMessage retrievedArticle = await queue.GetMessageAsync();
            if (retrievedArticle != null)
            {
              string newsMessage = retrievedArticle.AsString;
              await queue.DeleteMessageAsync(retrievedArticle);
              return newsMessage;
            }
          }

          return "<queue empty or not created>";
        }
    }
}
