using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Firebase.Storage.Bucket
{

    public class StorageBucketList
    {

        internal StorageBucketList()
        {

        }

        [JsonProperty("prefixes")]
        public List<string> Prefixes { get; internal set; }

        [JsonProperty("items")]
        public List<StorageBucketListItem> Items { get; internal set; }

        [JsonProperty("nextPageToken")]
        public string NextPageToken { get; internal set; }

    }

}
