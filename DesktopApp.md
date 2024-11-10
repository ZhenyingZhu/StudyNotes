# Windows Desktop Apps

Register app: <https://apps.dev.microsoft.com/>

windows Dektop App: request a token from AAD v2 endpoint and use HTTP GET and the token to call GRAPH API.

Microsoft Authentication Library (MSAL) handles token acquisition and renewal.

## Create a Windows Desktop .NET application (XAML)

Create a WPF App, in Package Manager Console, type

```powershell
Install-Package Microsoft.Identity.Client -Pre # install MSAL
```

Update App.xaml.cs with client id.

Update MainWindow.xaml with the UI.

b25e5911-9a82-4f06-8f59-b40805ef1bb7

## [One Drive](https://docs.microsoft.com/en-us/onedrive/developer/)

Reset OneDrive: <https://support.microsoft.com/en-us/office/reset-onedrive-34701e00-bf7b-42db-b960-84905399050c>

### [Sample code](https://docs.microsoft.com/en-us/onedrive/developer/sample-code)

### [OneDrive Explorer](https://github.com/OneDrive/onedrive-sample-apibrowser-dotnet)

`OneDriveApiBrowser\FormBrowser.cs` simpleUploadToolStripMenuItem_Click contains the logic to upload a file.
