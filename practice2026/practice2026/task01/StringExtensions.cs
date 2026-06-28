using System;
namespace task01
{
    public static class StringExtensions
    {
        public static bool IsPalindrome(this string input)
        {
            string lowerword = input.ToLowerInvariant();
            string newword = "";
            foreach (char c in lowerword)
            {
                if (!char.IsPunctuation(c) && !char.IsWhiteSpace(c))
                {
                    newword += c;
                }
            }
            if (newword == new string(newword.Reverse().ToArray()) && newword.Length != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
