using System;

namespace DarkMode_2.Models;

public static class DarkModeException
{
    public class PositionTimeoutException : Exception
    {
        public PositionTimeoutException(string message) : base(message) { }
    }

}

