namespace Gurtle
{
    #region Imports

    using System;
    using System.IO;
    using System.Linq;
    using System.Text;

    #endregion

    class FilenameUtils
    {
        // based on http://stackoverflow.com/questions/1077935/will-urlencode-fix-this-problem-with-illegal-characters-in-file-names-c
        public static string EscapeFilename(string input)
        {
            StringBuilder builder = new StringBuilder(input.Length);
            for (int i = 0; i < input.Length; i++)
            {
                if (Path.GetInvalidPathChars().Contains(input[i]) || Path.GetInvalidFileNameChars().Contains(input[i]) || input[i] == '%')
                {
                    builder.Append(Uri.HexEscape(input[i]));
                }
                else
                {
                    builder.Append(input[i]);
                }
            }
            return builder.ToString();
        }
    }
}
