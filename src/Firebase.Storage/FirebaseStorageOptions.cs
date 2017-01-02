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

        /// <summary>
        /// Gets or sets whether <see cref="TaskCanceledException"/> should be thrown when cancelling a running <see cref="FirebaseStorageTask"/>.
        /// </summary>
        public bool ThrowOnCancel
        {
            get;
            set;
        }
    }
}
