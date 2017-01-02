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
var stream = File.Open(@"C:\Users\you\file.png", FileMode.Open);

var task = new FirebaseStorage("your-bucket.appspot.com")
    .Child("data")
    .Child("random")
    .Child("file.png")
    .Put(stream);

task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e} %");

await task;
```
