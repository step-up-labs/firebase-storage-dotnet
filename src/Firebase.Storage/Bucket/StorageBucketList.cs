using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Firebase.Storage.Bucket
{

    public class StorageBucketList
    {

        internal StorageBucketList()
        {

        }

        [JsonProperty("items")]
        internal List<StorageBucketListItem> items { get; set; }

        [JsonIgnore()]
        public IReadOnlyList<StorageBucketListItem> Items
        {
            get
            {
                return new ReadOnlyCollection<StorageBucketListItem>(items);
            }
        }

        [JsonProperty("prefixes")]
        public List<string> Prefixes { get; internal set; }

        [JsonProperty("nextPageToken")]
        public string NextPageToken { get; internal set; }

    }

}
