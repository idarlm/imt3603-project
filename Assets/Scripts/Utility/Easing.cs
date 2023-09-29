using System;

namespace Utility
{
    // Easing provides basic easing methods for use with interpolation
    // see: https://easings.net/
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
        public static float OutElastic(float x) {
            var c4 = (2 * Math.PI) / 3;
            return (float)(Math.Pow(2, -10 * x) * Math.Sin((x * 10 - 0.75) * c4) + 1);
        }
        public static float InElastic(float x) {
            var c4 = (2 * Math.PI) / 3;
            return (float)(-Math.Pow(2, 10 * x - 10) * Math.Sin((x * 10 - 10.75) * c4));
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