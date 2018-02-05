namespace CognitiveServicesDotNetFrameworkTest.Services
{
    /// <summary>
    /// The class contains logic to obtain/manage auhtentication token/registration
    /// based on OAuth 2.0 Authorization "https://tools.ietf.org/html/rfc6750"
    /// </summary>>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <value>
        /// The access token.
        /// </value>
        string AccessToken { get; }
    }
}