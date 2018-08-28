using System.Net.Http;

namespace Notes.Domain.Services
{
    public class IdenticonService
    {
        private const string Url = "https://avatars.dicebear.com/v2/identicon";

        public byte[] GetIdenticon(string value)
        {
            using (var client = new HttpClient())
            {
                var response = client.GetByteArrayAsync($"{Url}/{value}.svg").Result;
                return response;
            }
        }
    }
}