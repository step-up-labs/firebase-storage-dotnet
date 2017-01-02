namespace Firebase.Storage
{
    using System;

    public class FirebaseStorageException : Exception
    {
        public FirebaseStorageException(string url, string responseData, Exception innerException) : base(GenerateExceptionMessage(url, responseData), innerException)
        {
            this.RequestUrl = url;
            this.ResponseData = responseData;
        }

        /// <summary>
        /// Gets the original request url.
        /// </summary>
        public string RequestUrl
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the response data returned by the firebase service.
        /// </summary>
        public string ResponseData
        {
            get;
            private set;
        }

        private static string GenerateExceptionMessage(string requestUrl, string responseData)
        {
            return $"Exception occured while processing the request.\nUrl: {requestUrl}\nResponse: {responseData}";
        }
    }
}
