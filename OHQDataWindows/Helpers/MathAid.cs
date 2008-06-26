using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace OHQData.Helpers
{
    public static class MathAid
    {
        #region Clamp (int, float and double overloads)

        /// <summary>
        /// Restricts a value to be within a specified range.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int Clamp(int value, int min, int max)
        {
            if (min > max)
                throw new ArgumentException("min cannot be greater than max.");
            if (value > max)
                return max;
            if (value < min)
                return min;
            return value;
        }

        /// <summary>
        /// Restricts a value to be within a specified range.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float Clamp(float value, float min, float max)
        {
            if (min > max)
                throw new ArgumentException("min cannot be greater than max.");
            if (value > max)
                return max;
            if (value < min)
                return min;
            return value;
        }

        /// <summary>
        /// Restricts a value to be within a specified range.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static double Clamp(double value, double min, double max)
        {
            if (min > max)
                throw new ArgumentException("min cannot be greater than max.");
            if (value > max)
                return max;
            if (value < min)
                return min;
            return value;
        }

        #endregion

        #region Wrap (int, float and double overloads)

        /// <summary>
        /// Restricts an int to the range [min, max] by wrapping it around.
        /// </summary>
        public static int Wrap(int value, int min, int max)
        {
            if (min > max)
                throw new ArgumentException("min cannot be greater than max.");
            value -= min;
            max -= min;
            max++;
            value %= max;
            value += min;
            if (value < min)
                value += max;
            return value;
        }

        /// <summary>
        /// Restricts a float to the range [min, max) by wrapping it around.
        /// </summary>
        public static float Wrap(float value, float min, float max)
        {
            if (min > max)
                throw new ArgumentException("min cannot be greater than max.");
            value -= min;
            max -= min;
            value %= max;
            value += min;
            if (value < min)
                value += max;
            return value;
        }

        /// <summary>
        /// Restricts a double to the range [min, max) by wrapping it around.
        /// </summary>
        public static double Wrap(double value, double min, double max)
        {
            if (min > max)
                throw new ArgumentException("min cannot be greater than max.");
            value -= min;
            max -= min;
            value %= max;
            value += min;
            if (value < min)
                value += max;
            return value;
        }

        #endregion

        #region Sum (int, float and double overloads)

        /// <summary>
        /// Determines the sum of the values in an array.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static int Sum(IEnumerable<int> values)
        {
            if (values == null)
                throw new ArgumentNullException("values");
            int sum = 0;
            foreach (int i in values)
            {
                sum += i;
            }
            return sum;
        }

        /// <summary>
        /// Determines the sum of the values in an array.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static float Sum(IEnumerable<float> values)
        {
            if (values == null)
                throw new ArgumentNullException("values");
            float sum = 0;
            foreach (float i in values)
            {
                sum += i;
            }
            return sum;
        }

        /// <summary>
        /// Determines the sum of the values in an array.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static double Sum(IEnumerable<double> values)
        {
            if (values == null)
                throw new ArgumentNullException("values");
            double sum = 0;
            foreach (double i in values)
            {
                sum += i;
            }
            return sum;
        }

        #endregion

        #region Lerp (float and double overloads)

        public static float Lerp(float value1, float value2, float amount)
        {
            return (value1 + ((value2 - value1) * amount));
        }

        public static double Lerp(double value1, double value2, double amount)
        {
            return (value1 + ((value2 - value1) * amount));
        }

        #endregion

        /// <summary>
        /// This method is not guaranteed to be complete or work correctly.
        /// </summary>
        public static float[] ArrayOverFloat(int[] dividends, float divisor)
        {
            if (dividends == null)
                throw new ArgumentNullException("dividends");
            float[] quotients = new float[dividends.Length];
            for (int i = 0; i < dividends.Length; i++)
            {
                quotients[i] = dividends[i] / divisor;
            }
            return quotients;
        }

        /// <summary>
        /// This method is not guaranteed to be complete or work correctly.
        /// </summary>
        public static float[] ArrayOverArray(float[] dividends, float[] divisors)
        {
            if (dividends == null)
                throw new ArgumentNullException("dividends");
            if (divisors == null)
                throw new ArgumentNullException("divisors");
            if (dividends.Length != divisors.Length)
                throw new ArgumentException("dividends.Length must equal divisors.Length");
            float[] quotients = new float[dividends.Length];
            for (int i = 0; i < dividends.Length; i++)
            {
                quotients[i] = dividends[i] / divisors[i];
            }
            return quotients;
        }
        
        /// <summary>
        /// This method is not guaranteed to be complete or work correctly.
        /// </summary>
        /// <param name="values"></param>
        public static void SwitchFractionsToNegatives(float[] values)
        {
            if (values == null)
                throw new ArgumentNullException("values");
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] >= 1)
                    values[i]--;
                else if (values[i] < 1)
                    values[i] = (-1 / values[i]) + 1;
            }
        }

        public static double Max(IList<double> values)
        {
            if (values == null)
                throw new ArgumentNullException("values");
            if (values.Count == 0)
                throw new Exception("values must contain at least one item.");
            double max = values[0];
            for (int i = 1; i < values.Count; i++)
            {
                max = Math.Max(values[i], max);
            }
            return max;
        }

        public static double Max<TValues>(IList<TValues> values, Converter<TValues, double> converter)
        {
            if (values == null)
                throw new ArgumentNullException("values");
            if (values.Count == 0)
                throw new Exception("values must contain at least one item.");

            double max = converter(values[0]);
            for (int i = 1; i < values.Count; i++)
            {
                max = Math.Max(converter(values[i]), max);
            }
            return max;
        }

        /// <summary>
        /// Gets an array of numbers counting from start to finish.
        /// Examples:  (start = 0, finish = 5) => (return = [1, 2, 3, 4, 5])
        ///                    (start = 7, finish = 4) => (return = [6, 5, 4])
        ///                    (start = 3, finish = 3) => (return = null)
        /// </summary>
        public static int[] GetCountingNumbers(int start, int finish)
        {
            int change = finish - start;
            int[] result = null;
            if (change != 0)
            {
                result = new int[Math.Abs(change)];
                if (change > 0)
                {
                    for (int i = 0; i < result.Length; i++)
                        result[i] = start + i + 1;
                }
                else
                {
                    for (int i = 0; i < result.Length; i++)
                        result[i] = start - i - 1;
                }
            }
            return result;
        }

        public static Vector2 Vector3XY(Vector3 v)
        {
            return new Vector2(v.X, v.Y);
        }

        public static float AngleOf(Vector2 v)
        {
            return (float)Math.Atan2(v.Y, v.X);
        }
    }
}
