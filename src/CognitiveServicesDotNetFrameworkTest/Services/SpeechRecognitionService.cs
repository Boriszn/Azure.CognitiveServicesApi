using System;
using System.IO;
using System.Net;
using CognitiveServicesDotNetFrameworkTest.Models;
using Newtonsoft.Json;

namespace CognitiveServicesDotNetFrameworkTest.Services
{
    /// <summary>
    /// Contains logic to read and send audio file and receive recognition text content response
    /// (also includes logic to separate and send file by chanks, build http request, and parse response etc)
    /// </summary>
    public class SpeechRecognitionService : ISpeechRecognitionService
    {
        // TODO: Move to configuration file
        private const string SpeechRecognitionServiceUrl =
            @"https://speech.platform.bing.com/speech/recognition/interactive/cognitiveservices/v1?language=en-US";

        private const string RequestHost = @"speech.platform.bing.com";
        private const string RequestContentType = @"audio/wav; codec=""audio/pcm""; samplerate=16000";
        private const string WebRequestAccept = @"application/json;text/xml";

        private readonly IAuthenticationService authentication;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpeechRecognitionService"/> class.
        /// TODO: [IMPORTANT: Remove dirrect dependencies use interfaces and DI / IOC container ]
        /// </summary>
        /// <param name="authentication">The authentication.</param>
        public SpeechRecognitionService(IAuthenticationService authentication)
        {
            this.authentication = authentication;
        }

        /// <summary>
        /// Translates the audio file to text.
        /// </summary>
        /// <param name="audioFile">The audio file.</param>
        /// <param name="authentication">The authentication.</param>
        /// <returns></returns>
        public RecognitionResult TranslateAudioFileToText(string audioFile)
        {
            string responseString;

            // Create reuest set http headers, mime types, authentication token etc
            var request = CreateRequestWithAuthenticationToken(SpeechRecognitionServiceUrl, this.authentication.AccessToken);

            using (var fileStream = new FileStream(audioFile, FileMode.Open, FileAccess.Read))
            {
                ReadFileAndWriteToRequestStream(request, fileStream);

                responseString = GetTranslationFromService(request);
            }

            // Deserialize JSON string to object model RecognitionResult
            return JsonConvert.DeserializeObject<RecognitionResult>(responseString);
        }

        /// <summary>
        /// Creates the request with authentication token.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        private static HttpWebRequest CreateRequestWithAuthenticationToken(string requestUri, string token)
        {
            var request = (HttpWebRequest) WebRequest.Create(requestUri);
            request.SendChunked = true;
            request.Accept = WebRequestAccept;
            request.Method = WebRequestMethods.Http.Post;
            request.ProtocolVersion = HttpVersion.Version11;
            request.Host = RequestHost;
            request.ContentType = RequestContentType;

            // Attach received authentication (bearer) token
            request.Headers["Authorization"] = $"Bearer {token}";

            return request;
        }

        /// <summary>
        /// Gets the translation from the cognitive service.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Translation response string</returns>
        private static string GetTranslationFromService(HttpWebRequest request)
        {
            string responseString = string.Empty;

            using (WebResponse response = request.GetResponse())
            {
                if (((HttpWebResponse) response).StatusCode == HttpStatusCode.OK)
                {
                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        responseString = streamReader.ReadToEnd();
                    }
                }
            }

            return responseString;
        }

        /// <summary>
        /// Open a request stream and write 1024 byte chunks in the stream one at a time.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="fileStream"></param>
        private static void ReadFileAndWriteToRequestStream(HttpWebRequest request, FileStream fileStream)
        {
            using (var requestStream = request.GetRequestStream())
            {
                var buffer = CreateFileBuffer(fileStream);

                int bytesRead;
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    requestStream.Write(buffer, 0, bytesRead);
                }

                requestStream.Flush();
            }
        }

        /// <summary>
        /// Read 1024 raw bytes from the input audio file.
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns>Buffer byte array</returns>
        private static byte[] CreateFileBuffer(FileStream fileStream)
        {
            return new Byte[checked((uint) Math.Min(1024, (int) fileStream.Length))];
        }
    }
}