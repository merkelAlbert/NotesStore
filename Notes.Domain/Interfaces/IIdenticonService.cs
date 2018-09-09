namespace Notes.Domain.Interfaces
{
    public interface IIdenticonService
    {
        byte[] GetIdenticon(string value);
    }
}