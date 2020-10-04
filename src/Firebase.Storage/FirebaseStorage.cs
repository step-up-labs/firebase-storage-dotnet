using Firebase.Storage.Bucket;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Firebase.Storage
{
    /// <summary>
    /// Entry class into firebase storage. 
    /// </summary>
    public class FirebaseStorage
    {
        /// <summary>
        /// Creates an instance of <see cref="FirebaseStorage"/> class.
        /// </summary>
        /// <param name="storageBucket"> Google storage bucket. E.g. 'your-bucket.appspot.com'. </param>
        /// <param name="options"> Optional settings. </param>
        public FirebaseStorage(string storageBucket, FirebaseStorageOptions options = null)
        {
            this.StorageBucket = storageBucket;
            this.Options = options ?? new FirebaseStorageOptions();
        }

        /// <summary>
        /// Gets the <see cref="FirebaseStorageOptions"/>.
        /// </summary>
        public FirebaseStorageOptions Options
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the google storage bucket.
        /// </summary>
        public string StorageBucket
        {
            get;
            private set;
        }


        /// <summary>
        /// Constructs firebase path to the file.
        /// </summary>
        /// <param name="name"> Name of the entity. This can be folder or a file name or full path.</param>
        /// <example>
        ///     storage
        ///         .Child("some")
        ///         .Child("path")
        ///         .Child("to/file.png");
        /// </example>
        /// <returns> <see cref="FirebaseStorageReference"/> for fluid syntax. </returns>
        public FirebaseStorageReference Child(string childRoot)
        {
            return new FirebaseStorageReference(this, childRoot);
        }


        /// <summary>
        /// List all prefixes (folders) immediately descended from the root of the storage bucket.
        /// </summary>
        /// <param name="maxResults">Maximum results per page</param>
        /// <param name="pageToken">Next page token</param>
        /// <returns></returns>
        public async Task<StorageBucketList> ListRootPrefixes(int maxResults = 1000, string pageToken = null) 
            => await ListPrefixes(null, maxResults, pageToken);

        /// <summary>
        /// List all files in the storage bucket.
        /// </summary>
        /// <param name="maxResults">Maximum results per page</param>
        /// <param name="pageToken">Next page token</param>
        /// <returns></returns>
        public async Task<StorageBucketList> ListAllFiles(int maxResults = 1000, string pageToken = null)
            => await ListFiles(null, maxResults, pageToken);

        private async Task<StorageBucketList> InternalBucketRequest(string fullUrl)
        {
            var cli = await Options.CreateHttpClientAsync();
            var resp = await cli.GetAsync(fullUrl);

            StorageBucketList bucket = null;

            if (resp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var json = await resp.Content.ReadAsStringAsync();
                var cfg = new JsonSerializerSettings()
                {
                    ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                };

                bucket = JsonConvert.DeserializeObject<StorageBucketList>(json, cfg);

                foreach (var item in bucket.Items)
                {
                    item.storage = this;
                }
            }

            return bucket;
        }

        private string GetListUrl(FirebaseStorageReference child, bool forPrefix, int maxResults = 1000, string pageToken = null)
        {
            if (maxResults > 1000 || maxResults < 1) throw new ArgumentOutOfRangeException(nameof(maxResults));

            string path;
            string reqUrl;

            if (child != null)
            {
                path = child.GetEscapedPath();
                reqUrl = $"{FirebaseStorageReference.FirebaseStorageEndpoint}{StorageBucket}/o?maxResults={maxResults}&prefix={path}%2f";
            }
            else
            {
                reqUrl = $"{FirebaseStorageReference.FirebaseStorageEndpoint}{StorageBucket}/o?maxResults={maxResults}";
            }

            if (!string.IsNullOrEmpty(pageToken))
            {
                reqUrl += $"&pageToken={pageToken}";
            }

            if (forPrefix) reqUrl += "&delimiter=/";
            return reqUrl;
        }

        internal async Task<StorageBucketList> ListFiles(FirebaseStorageReference child, int maxResults = 1000, string pageToken = null)
        {
            return await InternalBucketRequest(GetListUrl(child, false, maxResults, pageToken));
        }

        internal async Task<StorageBucketList> ListPrefixes(FirebaseStorageReference child, int maxResults = 1000, string pageToken = null)
        {
            return await InternalBucketRequest(GetListUrl(child, true, maxResults, pageToken));
        }

    }
}
