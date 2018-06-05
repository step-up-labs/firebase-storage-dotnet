namespace Firebase.Storage.UnitTests
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    using Firebase.Storage.UnitTests.Models;

    using FluentAssertions;

    using Newtonsoft.Json;

    using Xunit;

    public sealed class UploadWithMimeTypeTest
    {
        [Theory]
        [InlineData("secrets.json", "test.jpg", "image/jpeg")]
        public async Task ShouldBeSetWhenDownloadingAsync(string secretsFilePath, string fileToUpload, string mimeType)
        {
            File.Exists(secretsFilePath).Should().BeTrue("You need to create your own secrets.json (check out the example file for details)");

            var secrets = JsonConvert.DeserializeObject<Secrets>(File.ReadAllText(secretsFilePath));
            var options = secrets.Authentication.CreateAuthProvider().GetStorageAuthOptions(secrets.Authentication);
            var storage = new FirebaseStorage(secrets.BucketName, options);

            long actualStreamSize;
            string downloadUrl;
            using (var contentStream = new MemoryStream(File.ReadAllBytes(fileToUpload)))
            {
                actualStreamSize = contentStream.Length;
                downloadUrl = await storage.Child("test.jpg").PutAsync(
                                      contentStream,
                                      CancellationToken.None,
                                      mimeType);
            }

            // now download the file and check the mime-type
            var http = await storage.Options.CreateHttpClientAsync();
            using (var response = await http.GetAsync(downloadUrl))
            {
                response.Content.Headers.ContentType.MediaType.Should().Be(mimeType);
                response.Content.Headers.ContentLength.HasValue.Should().BeTrue();
                // ReSharper disable once PossibleInvalidOperationException
                response.Content.Headers.ContentLength.Value.Should().Be(actualStreamSize);
                (await response.Content.ReadAsByteArrayAsync()).LongLength.Should().Be(actualStreamSize);
            }
        }
    }
}
