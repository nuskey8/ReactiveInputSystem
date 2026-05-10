using System;
using System.Runtime.CompilerServices;

namespace ReactiveInputSystem
{
    internal static class Error
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ArgumentNullException<T>(T value)
        {
            if (value == null) throw new ArgumentNullException();
        }
    }
}