namespace Firebase.Storage
{
    using Newtonsoft.Json;

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    public class FirebaseStorageReference
    {
        private const string FirebaseStorageEndpoint = "https://firebasestorage.googleapis.com/v0/b/";

        private readonly FirebaseStorage storage;
        private readonly List<string> children;

        internal FirebaseStorageReference(FirebaseStorage storage, string childRoot)
        {
            this.children = new List<string>();

            this.storage = storage;
            this.children.Add(childRoot);
        }

        /// <summary>
        /// Starts uploading given stream to target location.
        /// </summary>
        /// <param name="stream"> Stream to upload. </param>
        /// <param name="cancellationToken"> Cancellation token which can be used to cancel the operation. </param>
        /// <returns> <see cref="FirebaseStorageTask"/> which can be used to track the progress of the upload. </returns>
        public FirebaseStorageTask Put(Stream stream, CancellationToken cancellationToken)
        {
            return new FirebaseStorageTask(this.storage.Options, this.GetTargetUrl(), this.GetFullDownloadUrl(), stream, cancellationToken);
        }

        /// <summary>
        /// Starts uploading given stream to target location.
        /// </summary>
        /// <param name="stream"> Stream to upload. </param>
        /// <returns> <see cref="FirebaseStorageTask"/> which can be used to track the progress of the upload. </returns>
        public FirebaseStorageTask Put(Stream fileStream)
        {
            return this.Put(fileStream, CancellationToken.None);
        }

        /// <summary>
        /// Gets the url to download given file.
        /// </summary>
        public async Task<string> GetDownloadUrlAsync()
        {
            var url = this.GetDownloadUrl();

            using (var http = await this.storage.Options.CreateHttpClientAsync())
            {
                var result = await http.GetStringAsync(url);
                var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
                
                return this.GetFullDownloadUrl() + data["downloadTokens"];
            }
        }

        /// <summary>
        /// Constructs firebase path to the file.
        /// </summary>
        /// <param name="name"> Name of the entity. This can be folder or a file name or full path.</param>
        /// <example>
        ///     [<![CDATA[
        ///     storage
        ///         .Child("some")
        ///         .Child("path")
        ///         .Child("to/file.png");
        ///     ]]>
        /// </example>
        /// <returns> <see cref="FirebaseStorageReference"/> for fluid syntax. </returns>
        public FirebaseStorageReference Child(string name)
        {
            this.children.Add(name);
            return this;
        }

        private string GetTargetUrl()
        {
            return $"{FirebaseStorageEndpoint}{this.storage.StorageBucket}/o?name={this.GetEscapedPath()}";
        }

        private string GetDownloadUrl()
        {
            return $"{FirebaseStorageEndpoint}{this.storage.StorageBucket}/o/{this.GetEscapedPath()}";
        }

        private string GetFullDownloadUrl()
        {
            return this.GetDownloadUrl() + "?alt=media&token=";
        }

        private string GetEscapedPath()
        {
            return Uri.EscapeDataString(string.Join("/", this.children));
        }
    }
}
