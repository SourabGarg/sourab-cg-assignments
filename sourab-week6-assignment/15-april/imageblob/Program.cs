using Azure;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Security.Cryptography;

namespace imageblob
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string storageAccountName = "capcuimgblob68";
            string containerName = "images";
            string localImagePath = "sample.jpg";
            string encryptedBlobName = "sample.encrypted";
            string decryptedOutputPath = "sample.decrypted.jpg";

            try
            {
                if (!File.Exists(localImagePath))
                {
                    Console.WriteLine($"Image not found at '{localImagePath}'. Put an image with this name in the project output folder and run again.");
                    return;
                }

                var serviceClient = new BlobServiceClient(
                    new Uri($"https://{storageAccountName}.blob.core.windows.net"),
                    new DefaultAzureCredential());

                var containerClient = serviceClient.GetBlobContainerClient(containerName);
                await containerClient.CreateIfNotExistsAsync(PublicAccessType.None);

                byte[] originalImageBytes = await File.ReadAllBytesAsync(localImagePath);

                using Aes aes = Aes.Create();
                aes.KeySize = 256;
                aes.GenerateKey();
                aes.GenerateIV();

                byte[] encryptedBytes = EncryptBytes(originalImageBytes, aes.Key, aes.IV);

                var blobClient = containerClient.GetBlobClient(encryptedBlobName);
                using var encryptedStream = new MemoryStream(encryptedBytes);
                await blobClient.UploadAsync(encryptedStream, overwrite: true);
                await blobClient.SetMetadataAsync(new Dictionary<string, string>
                {
                    ["iv"] = Convert.ToBase64String(aes.IV)
                });
                await blobClient.SetHttpHeadersAsync(new BlobHttpHeaders
                {
                    ContentType = "application/octet-stream"
                });

                Console.WriteLine($"Encrypted image uploaded to blob '{encryptedBlobName}'.");

                BlobDownloadResult downloadResult = await blobClient.DownloadContentAsync();
                byte[] downloadedEncryptedBytes = downloadResult.Content.ToArray();
                byte[] iv = Convert.FromBase64String(downloadResult.Details.Metadata["iv"]);

                byte[] decryptedBytes = DecryptBytes(downloadedEncryptedBytes, aes.Key, iv);
                await File.WriteAllBytesAsync(decryptedOutputPath, decryptedBytes);

                Console.WriteLine($"Decrypted image saved as '{decryptedOutputPath}'.");
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine($"Azure error ({ex.Status}): {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Runtime error: {ex.Message}");
            }
        }

        private static byte[] EncryptBytes(byte[] plainBytes, byte[] key, byte[] iv)
        {
            using Aes aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;

            using var ms = new MemoryStream();
            using (var cryptoStream = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cryptoStream.Write(plainBytes, 0, plainBytes.Length);
            }

            return ms.ToArray();
        }

        private static byte[] DecryptBytes(byte[] encryptedBytes, byte[] key, byte[] iv)
        {
            using Aes aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;

            using var inputMs = new MemoryStream(encryptedBytes);
            using var cryptoStream = new CryptoStream(inputMs, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using var outputMs = new MemoryStream();
            cryptoStream.CopyTo(outputMs);
            return outputMs.ToArray();
        }
    }
}
