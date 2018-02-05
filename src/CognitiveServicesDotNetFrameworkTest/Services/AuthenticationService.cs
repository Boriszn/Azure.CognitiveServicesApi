using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CognitiveServicesDotNetFrameworkTest.Services
{
    /// <summary>
    /// The class contains logic to obtain/manage/renew auhtentication token/registration
    /// based on OAuth 2.0 Authorization info: https://tools.ietf.org/html/rfc6750
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        // TODO: Move to configuration file
        public static readonly string FetchTokenUri = "https://api.cognitive.microsoft.com/sts/v1.0";

        // TODO: Move to configuration file
        /// <summary>
        /// The public subscription key
        /// (can be obtained during registration here: https://azure.microsoft.com/en-us/services/cognitive-services/speech/ )
        /// </summary>
        private const string SubscriptionKey = "1446ae70d6794c9eb47f75dfeee92016";

        private readonly Timer accessTokenRenewer;

        /// <summary>
        /// The refresh token duration. 
        /// Access token expires every 10 minutes. Renew it every 9 minutes only.
        /// </summary>
        private const int RefreshTokenDuration = 9;

        /// <summary>
        /// Initializes a new instance of the <see cref="Authentication"/> class.
        /// </summary>
        /// <param name="subscriptionKey"></param>
        public AuthenticationService()
        {
            this.AccessToken = FetchToken(FetchTokenUri).Result;

            // Renew the token every specfied minutes
            accessTokenRenewer = new Timer(OnTokenExpiredCallback,
                this,
                TimeSpan.FromMinutes(RefreshTokenDuration),
                TimeSpan.FromMilliseconds(-1));
        }

        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <value>
        /// The access token.
        /// </value>
        public string AccessToken { get; private set; }

        private void RenewAccessToken()
        {
            this.AccessToken = FetchToken(FetchTokenUri).Result;
            Console.WriteLine("Renewed token.");
        }

        private void OnTokenExpiredCallback(object stateInfo)
        {
            try
            {
                RenewAccessToken();
            }
            catch (Exception ex)
            {
                // TODO: Use some logger component
                // Console.WriteLine($"Failed renewing access token. Details: {ex.Message}");
            }
            finally
            {
                try
                {
                    accessTokenRenewer.Change(TimeSpan.FromMinutes(RefreshTokenDuration),
                        TimeSpan.FromMilliseconds(-1));
                }
                catch (Exception ex)
                {
                    // TODO: Use some logger component
                    // Console.WriteLine($"Failed to reschedule the timer to renew access token. Details: {ex.Message}");
                }
            }
        }

        private async Task<string> FetchToken(string fetchUri)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", SubscriptionKey);
                var uriBuilder = new UriBuilder(fetchUri);
                uriBuilder.Path += "/issueToken";

                var result = await client.PostAsync(uriBuilder.Uri.AbsoluteUri, null);
                return await result.Content.ReadAsStringAsync();
            }
        }
    }
}