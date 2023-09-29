using System;

namespace Utility
{
    public static class Easing
    {
        public static float InOutQuart(float x) {
            return x < 0.5 ? 8 * x * x * x * x : 1 - (float)Math.Pow(-2 * x + 2, 4) / 2;
        }
        public static float InOutCubic(float x) {
            return x < 0.5 ? 4 * x * x * x : 1 - (float)Math.Pow(-2 * x + 2, 3) / 2;
        }
        public static float InOutQuad(float x) {
            return x < 0.5 ? 4 * x * x : 1 - (float)Math.Pow(-2 * x + 2, 2) / 2;
        }

        public static float InQuad(float x)
        {
            return x * x;
        }

        public static float InCubic(float x)
        {
            return x * x * x;
        }

        public static float InQuart(float x)
        {
            return x * x * x * x;
        }
    }
}