using System;
using System.Diagnostics.CodeAnalysis;

namespace PinkRain.Utility
{
    internal static class Requires
    {
        internal static void NotNull<T>([NotNull] T? x, string name) where T : class
        {
            if (x is null)
            {
                throw new NullReferenceException($"{name} is null.");
            }
        }
    }
}