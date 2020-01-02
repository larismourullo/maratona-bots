using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace DevtranslateBot.Services
{
    public class LanguageService
    {
        private readonly string _translateApiKey = ConfigurationManager.AppSettings["TranslateApiKey"];
        private readonly string _translateUri = ConfigurationManager.AppSettings["TranslateUri"];

        public async Task<string> EnglishTranslate(string text)
        {
            var cliente = new HttpClient();

            cliente.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _translateApiKey);

            var response = await cliente.GetAsync(_translateUri + "?to=en-us" + 
                "&text=" + System.Net.WebUtility.UrlEncode(text));

            var content = XElement.Parse(await response.Content.ReadAsStringAsync()).Value;

            return $"Sua tradução para o inglês é: { content }";
        }
    }
}