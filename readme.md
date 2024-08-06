# AppLauncherUpdater

AppLauncherUpdater is a .NET application designed to manage and update client-side applications by downloading and extracting the latest ZIP files from a server. It works in conjunction with the [Zip-Uploader-Downloader](https://github.com/yugich/Zip-Uploader-Downloader) server tool to ensure your application is always up-to-date.

## Features

- Check for the latest application version available on the server.
- Download and extract application updates automatically.
- Launch the updated application seamlessly.

## Prerequisites

- [.NET 6.0](https://dotnet.microsoft.com/download/dotnet/6.0) (or higher)
- [Zip-Uploader-Downloader](https://github.com/yugich/Zip-Uploader-Downloader) server running to provide updates.

## Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/yourusername/AppLauncherUpdater.git
   cd AppLauncherUpdater
   ```

2. Restore dependencies:

   ```bash
   dotnet restore
   ```

3. Build the application:

   ```bash
   dotnet build
   ```

## Configuration

1. Set the API path in `LauncherForm.Designer.cs` by replacing `"Set API Path Here"` with the URL of your `Zip-Uploader-Downloader` instance.

   ```csharp
   string apiPath = "http://your-server-address";
   ```

2. Ensure the `Zip-Uploader-Downloader` server is running and accessible from the launcher.

## Running the Launcher

- Run the launcher from the command line:

  ```bash
  dotnet run --project AppLauncherUpdater
  ```

- Alternatively, build the project and execute the compiled `.exe` file located in the `bin/Debug/net6.0-windows` directory.

## How It Works

1. **Version Checking**: The launcher checks the current version of the application against the latest version available on the server using the `/latest-version` endpoint.

2. **Download and Extract**: If a new version is available, the launcher downloads the ZIP file from the server and extracts it to the application directory.

3. **Launch Application**: After updating, the launcher starts the updated application.

## File Structure

- `LauncherForm.cs`: Main form logic for checking updates and launching applications.
- `LauncherForm.Designer.cs`: Contains UI elements and update logic.
- `Program.cs`: Entry point for the application.
- `AppLauncherUpdater.csproj`: Project file for .NET build configuration.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Contributing

Contributions are welcome! Please feel free to submit a pull request or open an issue for any bug reports or feature requests.
