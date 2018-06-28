using System;
using System.IO;
using System.Net.Http;
using System.Security.Policy;

namespace Notes.Services
{
    public class IdenticonService
    {
        private const string Url = "https://avatars.dicebear.com/v2/identicon";

        public byte[] GetIdenticon(string value)
        {
            using (var client = new HttpClient())
            {
                Console.WriteLine($"{Url}/{value}.svg");
                var response = client.GetByteArrayAsync($"{Url}/{value}.svg").Result;
                return response;
            }
        }
    }
}