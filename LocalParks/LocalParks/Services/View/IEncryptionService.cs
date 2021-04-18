namespace LocalParks.Services.View
{
    public interface IEncryptionService
    {
        string Encrypt(int input, bool appendParam = false);
        string Encrypt(string input, bool appendParam = false);
        string Encrypt(string param, int value, bool appendParam = false);
        string Decrypt(string param, bool removeQuestionMark = true);
    }
}