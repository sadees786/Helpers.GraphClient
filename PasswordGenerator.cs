using System;

namespace Cqc.Helpers.GraphClient
{
    public static class PasswordGenerator
    {
        const string LowerCases = "abcdefghijklmnopqursuvwxyz";
        private const string UpperCases = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string Numbers = "123456789";
        private const string Specials = @"!@£$%^&*()#€";


        public static string RandomPasswordGenerator()
        {
            char[] password = new char[16];
            string charSet = ""; 
            var random = new Random();
            int counter;
            charSet += LowerCases;
            charSet += UpperCases;
            charSet += Numbers;
            charSet += Specials;
            for (counter = 0; counter < 16; counter++)
            {
                password[counter] = charSet[random.Next(charSet.Length - 1)];
            }
            return string.Join(null, password);
        }
    }
}



