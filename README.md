[![](https://img.shields.io/badge/License-GPLv3-blue.svg?style=for-the-badge)](LICENSE.txt)
[![.NET](https://img.shields.io/badge/.NET-8.0-blueviolet?style=for-the-badge)](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
[![GitHub commit activity](https://img.shields.io/github/commit-activity/w/sergiokml/Sii.RegistroCompraVenta?style=for-the-badge)](https://github.com/sergiokml/Sii.RegistroCompraVenta)
[![GitHub contributors](https://img.shields.io/github/contributors/sergiokml/Sii.RegistroCompraVenta?style=for-the-badge)](https://github.com/sergiokml/Sii.RegistroCompraVenta/graphs/contributors/)
[![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/sergiokml/Sii.RegistroCompraVenta?style=for-the-badge)](https://github.com/sergiokml/Sii.RegistroCompraVenta)

# Registro de Compras y Ventas (Chile) desde el SII

This solution allows you to query the **Registro de Compras y Ventas** from Chile's [Servicio de Impuestos Internos (SII)](https://www.sii.cl/) using scraping. It authenticates using a digital certificate stored in Azure Blob Storage and retrieves DTE summaries by document state.

---

### 📦 Details

| Package Reference                   | Version |
|------------------------------------|:-------:|
| Azure.Storage.Blobs                | 12.26.0 |
| Microsoft.Extensions.Azure         | 1.13.1  |
| Swashbuckle.AspNetCore             | 10.1.0  |
| Swashbuckle.AspNetCore.Annotations | 10.1.0  |

---

### 📋 Requirements

This project requires the following to run successfully:

* A **valid digital certificate (.pfx)** issued for SII services
* An **Azure Storage Account** or a local Blob Storage emulator like **Azurite** to store the certificate file.

---

### 🚀 Usage

Once the app is running, you can query purchases or sales:

```bash
curl -X GET "http://localhost:5225/api/RegistroCompraVenta/resumen?rut=71231117-1&year=2025&mes=12&operacion=compra" \
  -H "Accept: application/json"
```

- `rut`: RUT of the issuer (e.g., `71231117-1`)
- `year`: Year between 2023 and current year
- `mes`: Month between 1 and 12
- `operation`: `compra` or `centa`

The result is grouped by document state: `REGISTRO`, `RECLAMADO`, `PENDIENTE`.

---

### ⚙️ Configuration

Use `appsettings.json` or environment variables to configure the certificate source:

```json
{
  "StorageConnection": "UseDevelopmentStorage=true",
  "StorageConnection:ContainerName": "certificados",
  "StorageConnection:BlobName": "certificado1.pfx",
  "StorageConnection:CertPassword": "<your-cert-password>"
}
```

For local development, you can use a local `.pfx` file:

```json
{
  "Certificates": {
    "UseLocal": true,
    "Path": "cert/certificado.pfx",
    "Password": ""
  }
}
```

If `Path` is omitted, it defaults to `cert/certificado.pfx`. If the `.pfx` has no password, leave `Password` empty or omit it.

You may also define these as [Azure App Settings](https://learn.microsoft.com/en-us/azure/app-service/configure-common) if you're deploying the API to the cloud.

---

### 📢 Have a question? Found a Bug?

Feel free to **file a new issue** with a respective title and description in the [Sii.RegistroCompraVenta/issues](https://github.com/sergiokml/Sii.RegistroCompraVenta/issues) section.

---

### 💖 Community and Contributions

If this tool is useful, feel free to contribute ideas or improvements.

Improvements runnning properly updated and tested for Net core 9 on Macos 
<p align="center">
    <a href="https://www.paypal.com/donate/?hosted_button_id=EH2SYAXPWLRYG "target="_blank">
        <img width="12%" src="https://img.shields.io/badge/PayPal-00457C?style=for-the-badge&logo=paypal&logoColor=white" alt="PayPal">
    </a>
</p>


Author:
<p align="center">
    <a href="https://www.paypal.com/donate/?hosted_button_id=PTKX9BNY96SNJ" target="_blank">
        <img width="12%" src="https://img.shields.io/badge/PayPal-00457C?style=for-the-badge&logo=paypal&logoColor=white" alt="PayPal">
    </a>
</p>

---

### 📘 License

This repository is released under the [GNU General Public License v3.0](LICENSE.txt).

---

### 🛠️ Instalación y actualización (Consola)

Requiere **.NET SDK 9** instalado.

```bash
dotnet --version
dotnet restore
```

Para actualizar paquetes NuGet con herramienta de consola:

```bash
dotnet tool install --global dotnet-outdated-tool
dotnet outdated -u Sii.RegistroCompraVenta/Sii.RegistroCompraVenta.csproj
```
