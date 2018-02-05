using CognitiveServicesDotNetFrameworkTest.Models;

namespace CognitiveServicesDotNetFrameworkTest.Services
{
    /// <summary>
    /// Contains logic to read and send audio file and receive recognition text content response
    /// (also includes logic to separate and send file by chanks, build http request, and parse response etc)
    /// </summary>
    public interface ISpeechRecognitionService
    {
        /// <summary>
        /// Translates the audio file to text.
        /// </summary>
        /// <param name="audioFile">The audio file.</param>
        /// <returns></returns>
        RecognitionResult TranslateAudioFileToText(string audioFile);
    }
}