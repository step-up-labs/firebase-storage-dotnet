# FirebaseStorage.net
[![AppVeyor Build status](https://ci.appveyor.com/api/projects/status/o8hpwxrgfyhu527b?svg=true)](https://ci.appveyor.com/project/bezysoftware/firebase-storage-dotnet)

Easily upload files and other content to Firebase Storage. 

## Installation
```csharp
// Install release version
Install-Package FirebaseStorage.net -pre
```

## Supported frameworks
* .NET 4.5+
* Windows 8.x
* UWP
* Windows Phone 8.1
* CoreCLR

## Usage

```csharp
// Get any Stream - it can be FileStream, MemoryStream or any other type of Stream
var stream = File.Open(@"C:\Users\you\file.png", FileMode.Open);

// Constructr FirebaseStorage, path to where you want to upload the file and Put it there
var task = new FirebaseStorage("your-bucket.appspot.com")
    .Child("data")
    .Child("random")
    .Child("file.png")
    .PutAsync(stream);

// Track progress of the upload
task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");

// await the task to wait until upload completes and get the download url
var downloadUrl = await task;
```
