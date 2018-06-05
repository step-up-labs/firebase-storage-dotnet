namespace Firebase.Storage.UnitTests.Models
{
    public sealed class Secrets
    {
        public string BucketName { get; set; }

        public FirebaseAuthenticationModel Authentication { get; set; }
    }
}