namespace Firebase.Storage.UnitTests.Models
{
    using System.Threading.Tasks;

    using Firebase.Auth;

    public sealed class FirebaseAuthenticationModel
    {
        public string ApiKey { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public FirebaseAuthProvider CreateAuthProvider()
        {
            return new FirebaseAuthProvider(new FirebaseConfig(this.ApiKey));
        }

        public async Task<string> GetAuthenticationTokenAsync(FirebaseAuthProvider authProvider)
        {
            return (await authProvider.SignInWithEmailAndPasswordAsync(this.Email, this.Password)).FirebaseToken;
        }
    }
}