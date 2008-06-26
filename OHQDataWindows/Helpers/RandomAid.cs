using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace OHQData.Helpers
{
    public static class RandomAid
    {
        private static Random rand = new Random();

        public static int NextIntExclusive(int maxValue)
        {
            return rand.Next(maxValue);
        }

        /// <summary>
        /// Gets a random int in range [min, max] (inclusive).
        /// </summary>
        /// <param name="min">Lower bound.</param>
        /// <param name="max">Upper bound.</param>
        /// <returns>Random number.</returns>
        public static int NextIntInRange(int min, int max)
        {
            if (max < min)
                throw new ArgumentException("min must be less than or equal to max");
            return rand.Next(min, max + 1);
        }

        public static double NextDouble()
        {
            return rand.NextDouble();
        }

        /// <summary>
        /// Gets a random float in range [min, max] (inclusive).
        /// </summary>
        /// <param name="min">Lower bound.</param>
        /// <param name="max">Upper bound.</param>
        /// <returns>Random number.</returns>
        public static float NextFloatInRange(float min, float max)
        {
            float range = max - min;
            if (range < 0)
                throw new ArgumentException("min must be less than or equal to max");
            return range * NextFloat() + min;
        }

        /// <summary>
        /// Gets a random Vector3 with components X, Y, and Z in ranges
        /// [min.X, max.X], [min.Y, max.Y], and [min.Z, max.Z] respectively.
        /// </summary>
        /// <param name="min">Lower bound.</param>
        /// <param name="max">Upper bound.</param>
        /// <returns>Random vector.</returns>
        public static Vector3 NextVector3InRange(Vector3 min, Vector3 max)
        {
            Vector3 range = max - min;
            if (range.X < 0 || range.Y < 0 || range.Z < 0)
                throw new ArgumentException("min must be less than or equal to max (in x, y, and z)");
            return new Vector3(
                NextFloatInRange(min.X, max.X), 
                NextFloatInRange(min.Y, max.Y), 
                NextFloatInRange(min.Z, max.Z));
        }

        /// <summary>
        /// Gets a random Vector3 with all components in the range [0, 1].
        /// </summary>
        /// <returns>Random vector.</returns>
        public static Vector3 NextVector3()
        {
            return new Vector3(NextFloat(), NextFloat(), NextFloat());
        }

        /// <summary>
        /// Gets a random float in the range [0, 1].
        /// </summary>
        /// <returns>Random number.</returns>
        public static float NextFloat()
        {
            return (float)rand.NextDouble();
        }

        /// <summary>
        /// Gets a random boolean with a T:F ratio of (0.5):(0.5).
        /// </summary>
        public static bool NextBoolean()
        {
            return rand.Next(2) == 0;
        }

        /// <summary>
        /// Gets a random boolean with a T:F ratio of (p):(1 - p).
        /// </summary>
        /// <param name="probability"></param>
        public static bool NextBoolean(double p)
        {
            return rand.NextDouble() <= p;
        }

        /// <summary>
        /// Gets a random boolean with a T:F ratio of (p):(1 - p).
        /// </summary>
        /// <param name="probability"></param>
        public static bool NextBoolean(float p)
        {
            return (float)rand.NextDouble() <= p;
        }
    }
}
