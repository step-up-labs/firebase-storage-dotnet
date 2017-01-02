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
    }
}
