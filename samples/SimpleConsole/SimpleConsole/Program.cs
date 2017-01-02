namespace Firebase.Storage.SimpleConsole
{
    using Firebase.Auth;

    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    internal class Program
    {
        private static string ApiKey = "YOUR_API_KEY";
        private static string Bucket = "your-bucket.appspot.com";
        private static string AuthEmail = "your@email.com";
        private static string AuthPassword = "yourpasword";

        private static void Main(string[] args)
        {
            Run().Wait();
        }

        private static async Task Run()
        {
            // FirebaseStorage.Put method accepts any type of stream.
            var stream = new MemoryStream(Encoding.ASCII.GetBytes("Hello world!"));
            //var stream = File.Open(@"C:\someFile.png", FileMode.Open);

            // of course you can login using other method, not just email+password
            var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);

            var task = new FirebaseStorage(
                Bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken)
                })
                .Child("receipts")
                .Child("test")
                .Child("someFile.png")
                .Put(stream);

            task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e} %");

            try
            {
                // error during upload will be thrown when you await the task
                await task;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception was thrown: {0}", ex);
            }
        }
    }
}