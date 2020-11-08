README.md

1. Create Storage Account

  ```bash
  az storage account create --name [unique-name] -g [sandbox resource group name] --kind StorageV2 --sku Standard_LRS
  ```

2. Create app

  ```bash
  dotnet new console -n AzureQueueApp
  ```

3. Get connection string

  ```bash
  az storage account show-connection-string --name [unique-name] --resource-group [sandbox resource group name]
  ```

  _output_:
  ```
  { 
    "connectionString": "DefaultEndpointsProtocol=https;EndpointSuffix=core.windows.net;AccountName=[unique-name];AccountKey=[account-key]"
  }
  ```
4. Add connection string constant in Program.cs, replace <AZURE_STORAGE_CONNECTION_STRING> with output above

  ```csharp
  ...
  private const string ConnectionString = "<AZURE_STORAGE_CONNECTION_STRING>";

  static void Main(string[] args)
  { ... 
  ```

5. Add Storage package

  ```bash
  dotnet add package WindowsAzure.Storage
  ```

6. Swtich to C# 7.1 to support async/await syntax

  ```xml

  <Project Sdk="Microsoft.NET.Sdk">

    <PropertyGroup>
      <OutputType>Exe</OutputType>
      <TargetFramework>netcoreapp3.1</TargetFramework>
      <LangVersion>7.1</LangVersion>
    </PropertyGroup>
  ...

  ```

7. Add send/receive methods in Program.cs

8. Build and run

  ```bash
  dotnet build
  ```

  # set env
  ```bash
  AZURE_STORAGE_CONNECTION_STRING="DefaultEndpointsProtocol=https;EndpointSuffix=core.windows.net;AccountName=[unique-name];AccountKey=[account-key]"
  ```

  # send message, queue name is hardcoded in Program.cs
  ```bash
  dotnet run "hi there"

  az storage message peek --queue-name newsqueue --connection-string "$AZURE_STORAGE_CONNECTION_STRING"
  ```

  # receive message
  ```bash
  dotnet run

  az storage message peek --queue-name newsqueue --connection-string "$AZURE_STORAGE_CONNECTION_STRING"
  ```

  # delete queue
  ```bash
  az storage queue delete --name newsqueue --connection-string "$AZURE_STORAGE_CONNECTION_STRING"
  ```
