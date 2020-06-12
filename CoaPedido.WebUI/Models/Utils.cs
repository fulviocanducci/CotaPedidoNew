using System.Text;

namespace System.Web.Mvc
{
    public static class Utils
    {
        public static string FriendlyURL(this string word)
        {
            var w = Encoding
                    .ASCII
                    .GetString(
                        Encoding.GetEncoding("Cyrillic")
                        .GetBytes(word.ToLowerInvariant())
                    );
            return w.Replace(" - ","").Replace(' ', '-');
        }
    }
}