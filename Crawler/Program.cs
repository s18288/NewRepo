using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Crawler
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Regex urlRx = new Regex(@"^(http|ftp|https|www)://([\w+?\.\w+])+([a-zA-Z0-9\~\!\@\#\$\%\^\&\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*)?$", RegexOptions.IgnoreCase);


            if (args.Length == 0 || string.IsNullOrEmpty(args[0]))
            {
                throw new ArgumentNullException("Web site URL received a null argument!");
            }
            if (urlRx.Match(args[0]).Success == false)
            {
                throw new ArgumentException("URL is incorrect");
            }
            string websiteUrl = args[0];

            using var httpClient = new HttpClient();
            string content;
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(websiteUrl);
                content = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Błąd w czasie pobierania strony");

            }

            Regex regex = new Regex(@"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])");
            MatchCollection matchCollection = regex.Matches(content);
            if (matchCollection.Count == 0)
            {
                throw new Exception("Nie znaleziono adresów email");
            }

            foreach (var match in matchCollection.Select(m => m.ToString()).Distinct())
            {
                Console.WriteLine(match);
            }


        }
    }
}
