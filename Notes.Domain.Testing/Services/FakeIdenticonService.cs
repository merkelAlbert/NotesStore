using System.Net.Http;
using Notes.Domain.Interfaces;

namespace Notes.Domain.Testing.Services
{
    public class FakeIdenticonService : IIdenticonService
    {
        public byte[] GetIdenticon(string value)
        {
            return new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9};
        }
    }
}