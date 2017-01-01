namespace Firebase.Storage
{
    using System;

    public class FirebaseStorageException : Exception
    {
        public FirebaseStorageException(string url, string responseData, Exception innerException) : base(GenerateExceptionMessage(url, responseData), innerException)
        {
        }

        private static string GenerateExceptionMessage(string requestUrl, string responseData)
        {
            return $"Exception occured while processing the request.\nUrl: {requestUrl}\nResponse: {responseData}";
        }
    }
}
