using System;
using CognitiveServicesDotNetFrameworkTest.Models;
using CognitiveServicesDotNetFrameworkTest.Services;

namespace CognitiveServicesDotNetFrameworkTest
{
    class Program
    {
        /// <summary>
        /// [Entry point]
        /// </summary>
        /// <param name="args">The arguments.</param>
        static void Main(string[] args)
        {
            //0. Set-Up file to translate
            //TODO: Don't forget to change to your own
            string audioFile = @"C:\Users\boriszn\Downloads\b001.wav";


            //1. Authenticate (get and set SubscriptinkKey -> get authentication token)
            var authenticationService = new AuthenticationService();

            //2. Create SpeechRecognitionService -> pass token
            var speechRecognitionService = new SpeechRecognitionService(authenticationService);

            try
            {

                //3. Send/read audio file and receive result object with text
                RecognitionResult recognitionResult = speechRecognitionService.TranslateAudioFileToText(audioFile);

                Console.WriteLine($"Translation text: {recognitionResult.DisplayText}");

                Console.Read();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
    }

    
}
