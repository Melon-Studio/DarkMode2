using System;

namespace DarkMode_2.Models;

public static class DarkModeException
{
    public class InitColorMouleException : Exception
    {
        public InitColorMouleException(string message) : base(message) { }
    }

}

