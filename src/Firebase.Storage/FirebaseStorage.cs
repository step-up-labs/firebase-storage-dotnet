namespace Firebase.Storage
{
    public class FirebaseStorage
    {
        public FirebaseStorage(string storageBucket, FirebaseStorageOptions options = null)
        {
            this.StorageBucket = storageBucket;
            this.Options = options ?? new FirebaseStorageOptions();
        }

        public FirebaseStorageOptions Options
        {
            get;
            private set;
        }

        public string StorageBucket
        {
            get;
            private set;
        }

        public FirebaseStorageReference Child(string childRoot)
        {
            return new FirebaseStorageReference(this, childRoot);
        }
    }
}
