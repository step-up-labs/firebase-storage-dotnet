using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Firebase.Storage
{
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

        public FirebaseStorageTask Put(Stream fileStream, CancellationToken cancellationToken)
        {
            return new FirebaseStorageTask(this.storage.Options, this.GetTargetUrl(), fileStream, cancellationToken);
        }

        public FirebaseStorageTask Put(Stream fileStream)
        {
            return this.Put(fileStream, CancellationToken.None);
        }

        public FirebaseStorageReference Child(string name)
        {
            this.children.Add(name);
            return this;
        }

        private string GetTargetUrl()
        {
            return $"{FirebaseStorageEndpoint}{this.storage.StorageBucket}/o?name={string.Join("/", this.children)}";
        }
    }
}
