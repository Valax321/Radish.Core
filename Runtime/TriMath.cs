using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

namespace Radish
{
    [PublicAPI]
    public static class TriMath
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (float, float) GetXYForRightAngle(float hypotenuseLength, float angle)
        {
            var b = Mathf.Cos(angle * Mathf.Deg2Rad) * Mathf.Rad2Deg * Mathf.Sqrt(hypotenuseLength);
            var a = Mathf.Sin(angle * Mathf.Deg2Rad) * Mathf.Rad2Deg * Mathf.Sqrt(hypotenuseLength);
            return (Mathf.Sqrt(a), Mathf.Sqrt(b));
        }
    }
}