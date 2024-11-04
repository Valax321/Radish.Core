using Unity.Collections;

namespace Radish
{
    public static class NativeArrayExtensions
    {
        public delegate bool NACounter<T>(in T v) where T : struct;
        
        public static int Count<T>(this in NativeArray<T> array, NACounter<T> predicate) where T : struct
        {
            var c = 0;
            for (var i = 0; i < array.Length; ++i)
            {
                if (predicate(array[i]))
                    c++;
            }

            return c;
        }
    }
}