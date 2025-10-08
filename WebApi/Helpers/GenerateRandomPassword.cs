using System.Security.Cryptography;

namespace WebApi.Helpers
{
    public static class HelperMethods
    {
        public static string GenerateRandomPassword(int length = 6)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var password = new char[length];
            for (int i = 0; i < length; i++)
            {
                int idx = RandomNumberGenerator.GetInt32(chars.Length);
                password[i] = chars[idx];
            }
            return new string(password);
        }
    }
}