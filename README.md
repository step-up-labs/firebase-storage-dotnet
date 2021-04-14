# FirebaseStorage.net
[![AppVeyor Build status](https://ci.appveyor.com/api/projects/status/o8hpwxrgfyhu527b?svg=true)](https://ci.appveyor.com/project/bezysoftware/firebase-storage-dotnet)

Easily upload files and other content to Firebase Storage. More info in a [blog post](https://medium.com/step-up-labs/firebase-storage-c-library-d1656cc8b3c3)

For Authenticating with Firebase checkout the [Firebase Authentication library](https://github.com/step-up-labs/firebase-authentication-dotnet) and related [blog post](https://medium.com/step-up-labs/firebase-authentication-c-library-8e5e1c30acc2)

## Installation
```csharp
// Install release version
Install-Package FirebaseStorage.net -pre
```

## Supported frameworks
.NET Standard 1.1 - see https://github.com/dotnet/standard/blob/master/docs/versions.md for compatibility matrix

## Usage

```csharp
// Get any Stream - it can be FileStream, MemoryStream or any other type of Stream
var stream = File.Open(@"C:\Users\you\file.png", FileMode.Open);

//authentication
var auth = new FirebaseAuthProvider(new FirebaseConfig("api_key"));
var a = await auth.SignInWithEmailAndPasswordAsync("email", "password");

// Constructr FirebaseStorage, path to where you want to upload the file and Put it there
var task = new FirebaseStorage(
    "your-bucket.appspot.com"
     new FirebaseStorageOptions
     {
         AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
         ThrowOnCancel = true,
     })
    .Child("data")
    .Child("random")
    .Child("file.png")
    .PutAsync(stream);

// Track progress of the upload
task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");

// await the task to wait until upload completes and get the download url
var downloadUrl = await task;
```
