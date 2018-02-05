namespace CognitiveServicesDotNetFrameworkTest.Models
{
    /// <summary>
    /// Model for text regonition service response
    /// (generated from JSON via: https://jsonutils.com/)
    /// </summary>
    public class RecognitionResult
    {
        /// <summary>
        /// Gets or sets the recognition status.
        /// </summary>
        /// <value>
        /// The recognition status.
        /// </value>
        public string RecognitionStatus { get; set; }

        /// <summary>
        /// Gets or sets the display text.
        /// </summary>
        /// <value>
        /// The display text.
        /// </value>
        public string DisplayText { get; set; }

        /// <summary>
        /// Gets or sets the offset.
        /// </summary>
        /// <value>
        /// The offset.
        /// </value>
        public int Offset { get; set; }

        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        /// <value>
        /// The duration.
        /// </value>
        public int Duration { get; set; }
    }
}
