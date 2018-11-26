using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Firebase.Storage
{
    /// <summary>
    /// Full list of meta data available here: https://firebase.google.com/docs/storage/web/file-metadata
    /// </summary>
    public class FirebaseMetaData
    {
        [JsonProperty("bucket")]
        public string Bucket { get; set; }

        [JsonProperty("generation")]
        public string Generation { get; set; }

        [JsonProperty("metageneration")]
        public string MetaGeneration { get; set; }

        [JsonProperty("fullPath")]
        public string FullPath { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("contentType")]
        public string ContentType { get; set; }

        [JsonProperty("timeCreated")]
        public DateTime TimeCreated { get; set; }

        [JsonProperty("updated")]
        public DateTime Updated { get; set; }

        [JsonProperty("md5Hash")]
        public string Md5Hash { get; set; }

        [JsonProperty("contentEncoding")]
        public string ContentEncoding { get; set; }

        [JsonProperty("contentDisposition")]
        public string ContentDisposition { get; set; }
    }
}
