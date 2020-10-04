using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Firebase.Storage.Bucket
{
    public class StorageBucketListItem
    {

        internal FirebaseStorage storage;

        internal StorageBucketListItem()
        {

        }

        [JsonProperty("name")]
        public string Name { get; internal set; }

        [JsonProperty("bucket")]
        public string Bucket { get; internal set; }

        public string GetFilename()
        {
            var i = Name.LastIndexOf("/");
            return Name.Substring(i + 1);
        }

        public FirebaseStorageReference GetReference()
        {
            return new FirebaseStorageReference(storage, Name);
        }

    }

}
