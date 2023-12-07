# AppLauncherUpdater

## Overview
AppLauncherUpdater is a C# application designed to automatically update a Windows application. It checks for the latest version of the app, downloads it if available, and extracts the update for seamless user experience.

## Features
- Checks for the latest version of an application from a specified API.
- Downloads and extracts the update if a new version is available.
- Progress bar indicating the download progress.
- Error handling for scenarios such as failed downloads or updates.

## Prerequisites
- .NET Framework
- Newtonsoft.Json package for JSON handling

## Setup and Installation
1. Clone the repository or download the source code.
2. Ensure .NET Framework is installed on your system.
3. Open the solution in an IDE (e.g., Visual Studio) and restore any missing NuGet packages.
4. Build and run the application.

## Usage
1. Run the `AppLauncherUpdater.exe`.
2. The application will automatically check for updates.
3. If an update is available, it will be downloaded and applied. The application will then restart.
4. If no update is found, the application will start normally.

## Configuration
- Update the `apiPath` variable in the `LauncherForm` class to point to your application's update API.
- Place the local version of your application in `./App/version.txt`.
- Adjust the `extractionPath` to specify where the updates should be extracted.

## Troubleshooting
- Ensure the application has permissions to access the internet and write to the local file system.
- Check the API endpoint for availability and correct response format.

## Contributing
Feel free to fork the project and submit pull requests for any improvements or bug fixes.

## License
[Specify your License here or indicate if it's proprietary]
