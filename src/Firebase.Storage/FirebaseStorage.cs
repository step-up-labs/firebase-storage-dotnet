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
            var p = childRoot;

            if (!string.IsNullOrEmpty(p) && p.LastIndexOf("/") == p.Length - 1)
            {
                p = p.Substring(0, p.Length - 1);

            }

            return new FirebaseStorageReference(this, p);
        }

        /// <summary>
        /// List all prefixes (folders) immediately descended from the root of the storage bucket.
        /// </summary>
        /// <param name="maxResults">Maximum results per page (absolute maximum is 1000)</param>
        /// <param name="pageToken">Next page token</param>
        /// <returns>A <see cref="StorageBucketList" /> object with the requested results.</returns>
        public async Task<StorageBucketList> ListRootPrefixes(int maxResults = 1000, string pageToken = null) 
            => await ListPrefixes(null, maxResults, pageToken);

        /// <summary>
        /// List all files in the storage bucket.
        /// </summary>
        /// <param name="maxResults">Maximum results per page (absolute maximum is 1000)</param>
        /// <param name="pageToken">Next page token</param>
        /// <returns>A <see cref="StorageBucketList" /> object with the requested results.</returns>
        public async Task<StorageBucketList> ListAllFiles(int maxResults = 1000, string pageToken = null)
            => await ListFiles(null, maxResults, pageToken);

        private async Task<StorageBucketList> InternalBucketRequest(string fullUrl)
        {
            string json = null;

            try
            {
                var cli = await Options.CreateHttpClientAsync();
                var resp = await cli.GetAsync(fullUrl);
                var statusCode = resp.StatusCode;

                resp.EnsureSuccessStatusCode();
                StorageBucketList bucket = null;

                json = await resp.Content.ReadAsStringAsync();

                var cfg = new JsonSerializerSettings()
                {
                    ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                };

                bucket = JsonConvert.DeserializeObject<StorageBucketList>(json, cfg);

                foreach (var item in bucket.Items)
                {
                    item.storage = this;
                }

                return bucket;

            }
            catch (Exception ex)
            {
                throw new FirebaseStorageException(fullUrl, json, ex);
            }
        }

        private string InternalGetListUrl(FirebaseStorageReference child, bool forPrefix, int maxResults = 1000, string pageToken = null)
        {
            if (maxResults > 1000 || maxResults < 1) throw new ArgumentOutOfRangeException(nameof(maxResults), "Must be a positive value between 1 and 1000 inclusive.");

            string reqUrl;
            string sb = StorageBucket.Replace(".appspot.com", "");

            reqUrl = $"{FirebaseStorageReference.FirebaseStorageEndpoint}{sb}/o?maxResults={maxResults}";

            if (child != null)
            {
                reqUrl += $"&prefix={child.GetEscapedPath()}{Uri.EscapeDataString("/")}";
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
            return await InternalBucketRequest(InternalGetListUrl(child, false, maxResults, pageToken));
        }

        internal async Task<StorageBucketList> ListPrefixes(FirebaseStorageReference child, int maxResults = 1000, string pageToken = null)
        {
            return await InternalBucketRequest(InternalGetListUrl(child, true, maxResults, pageToken));
        }

    }
}
