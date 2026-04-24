using System;
using System.Text;

namespace Festival.Network.Utils
{
    /**
     * Utility class for password security.
     * In a production environment, use a real Hashing algorithm (like SHA256).
     * This implementation uses Base64 as requested for consistency.
     */
    public static class SecurityUtils
    {
        /// <summary>
        /// Encodes a plain-text string into a Base64 format.
        /// </summary>
        public static string Encode(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            byte[] textBytes = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(textBytes);
        }

        /// <summary>
        /// Decodes a Base64 encoded string back to plain-text.
        /// </summary>
        public static string Decode(string encodedText)
        {
            if (string.IsNullOrEmpty(encodedText)) return null;
            byte[] decodedBytes = Convert.FromBase64String(encodedText);
            return Encoding.UTF8.GetString(decodedBytes);
        }
    }
}