namespace Firebase.Storage
{
    using System;
    using System.Threading.Tasks;

    public class FirebaseStorageOptions
    {
        /// <summary>
        /// Gets or sets the method for retrieving auth tokens. Default is null.
        /// </summary>
        public Func<Task<string>> AuthTokenAsyncFactory
        {
            get;
            set;
        }
    }
}
