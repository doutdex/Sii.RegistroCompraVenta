using System.Net;
using System.Security.Cryptography.X509Certificates;
using Azure.Storage.Blobs;

namespace Sii.RegistroCompraVenta.Helper;

public class DigitalCertLoader
{
    public static async Task<HttpClientHandler> LoadCertificateAsync(
        IConfiguration config,
        ILogger<DigitalCertLoader> logger
    )
    {
        bool useLocal = config.GetValue<bool>("Certificates:UseLocal");
        string? localPath = config["Certificates:Path"];
        string? localPassword = config["Certificates:Password"];
        try
        {
            X509Certificate2 cert;
            if (useLocal)
            {
                if (string.IsNullOrWhiteSpace(localPath))
                {
                    localPath = Path.Combine(Directory.GetCurrentDirectory(), "cert", "certificado.pfx");
                }

                logger.LogInformation(
                    "Certificado local habilitado. Ruta: '{LocalPath}'. Clave requerida: {RequiresPassword}",
                    localPath,
                    !string.IsNullOrWhiteSpace(localPassword)
                );

                if (!File.Exists(localPath))
                {
                    throw new FileNotFoundException($"No se encontro el certificado local en '{localPath}'.");
                }

                cert = X509CertificateLoader.LoadPkcs12FromFile(
                    localPath,
                    string.IsNullOrWhiteSpace(localPassword) ? null : localPassword,
                    X509KeyStorageFlags.Exportable
                );
            }
            else
            {
                string connectionString = config["StorageConnection"] ?? string.Empty;
                string containerName = config["StorageConnection:containerName"] ?? string.Empty;
                string blobName = config["StorageConnection:blobName"] ?? string.Empty;
                string password = config["StorageConnection:certPassword"] ?? string.Empty;

                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new InvalidOperationException(
                        "Falta StorageConnection o Certificates:Path para cargar el certificado."
                    );
                }

                BlobServiceClient blobClient = new(connectionString);
                BlobContainerClient containerClient = blobClient.GetBlobContainerClient(containerName);
                BlobClient blob = containerClient.GetBlobClient(blobName);

                using MemoryStream ms = new();
                await blob.DownloadToAsync(ms);

                cert = X509CertificateLoader.LoadPkcs12(
                    ms.ToArray(),
                    string.IsNullOrWhiteSpace(password) ? null : password,
                    X509KeyStorageFlags.Exportable
                );
            }

            CookieContainer cookieContainer = new();
            HttpClientHandler handler = new() { CookieContainer = cookieContainer };
            handler.ClientCertificates.Add(cert);
            return handler;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
