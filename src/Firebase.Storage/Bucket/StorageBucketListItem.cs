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

        /// <summary>
        /// Returns only the file name / directory name of the current item.
        /// </summary>
        /// <returns></returns>
        public string GetFilename()
        {
            if (string.IsNullOrEmpty(Name)) return "";

            var i = Name.LastIndexOf("/");
            return Name.Substring(i + 1);
        }

        /// <summary>
        /// Retusn the directory path of the current item, excluding the item name.
        /// </summary>
        /// <returns></returns>
        public string GetDirectory()
        {
            if (string.IsNullOrEmpty(Name)) return "";
            int i = Name.LastIndexOf("/");

            if (i == -1)
            {
                return Name;
            }
            else
            {
                return Name.Substring(0, i);
            }
        }

        /// <summary>
        /// Create a <see cref="FirebaseStorageReference" /> object from this item.
        /// </summary>
        /// <returns></returns>
        public FirebaseStorageReference GetReference()
        {
            return new FirebaseStorageReference(storage, Name);
        }

        public override string ToString()
        {
            return Name;
        }

    }

}
