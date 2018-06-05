namespace Firebase.Storage.UnitTests
{
    using Firebase.Auth;
    using Firebase.Storage.UnitTests.Models;

    internal static class AuthenticationExtensions
    {
        internal static FirebaseStorageOptions GetStorageAuthOptions(this FirebaseAuthProvider authProvider, FirebaseAuthenticationModel authSettings)
        {
            return new FirebaseStorageOptions { AuthTokenAsyncFactory = async () => await authSettings.GetAuthenticationTokenAsync(authProvider) };
        }
    }
}