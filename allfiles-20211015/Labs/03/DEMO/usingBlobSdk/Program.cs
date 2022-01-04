using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace usingBlobSdk
{
    class Program
    {
        static void Main(string[] args)
        {
             ProcessAsync().GetAwaiter().GetResult();
             Console.WriteLine("İşlem tamamlandı");

        }

        private static async Task ProcessAsync(){
            string connectionString ="DefaultEndpointsProtocol=https;AccountName=storagedemobilgeadam;AccountKey=Byn5648mBb5KgbUSDOY4qGmTVg/1Co8A/A3SV2XvQisGYA0zdaHMG3fCSSJ377FPcou/1wEbTYIpA+qIM9bAEw==;EndpointSuffix=core.windows.net";

            BlobServiceClient serviceClient = new BlobServiceClient(connectionString);
            await createContainer(serviceClient);
        }
        static string containerName; 
        static string localFile;

        static BlobClient blobClient;
        private static async Task createContainer(BlobServiceClient serviceClient)
        {
            containerName = "demo"+Guid.NewGuid().ToString();
            BlobContainerClient containerClient = await serviceClient.CreateBlobContainerAsync(containerName);
            Console.WriteLine($"{containerName} isimli container başarıyla oluşturuldu. Git portaldan bak");
            Console.ReadLine();

            await uploadFile(containerClient);
        }

        private static async Task uploadFile(BlobContainerClient containerClient)
        {
           string localPath = "./data/";
           string fileName = $"demo{Guid.NewGuid()}.txt";
           localFile = Path.Combine(localPath,fileName);

           await File.WriteAllTextAsync(localFile,"Bu bir demodur");
           blobClient =  containerClient.GetBlobClient(fileName); 
           using FileStream fs = File.OpenRead(localFile);
           await blobClient.UploadAsync(fs,true);
           fs.Close();

           await listAllFiles(containerClient);

        }

        private static async Task listAllFiles(BlobContainerClient containerClient)
        {
            Console.WriteLine("dosyalar listeleniyor!");
            await foreach (BlobItem item in containerClient.GetBlobsAsync())
            {
                Console.WriteLine(item.Name);
            }

            await downloadBlob();

        }

        private async static Task downloadBlob()
        {
            string downloadFilePath = localFile.Replace(".txt","DOWNLOADED.txt");
            BlobDownloadInfo   download = await blobClient.DownloadAsync();

            using (FileStream downloadStream = File.OpenWrite(downloadFilePath))
            {
                 await download.Content.CopyToAsync(downloadStream);
                 downloadStream.Close();
            }

            Console.WriteLine("İşlem OK ");
        
        }
    }


}
