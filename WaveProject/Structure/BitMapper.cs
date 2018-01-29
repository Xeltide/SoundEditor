using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveProject.Structure
{
    class BitMapper
    {

        public static Func<int, int> GetMapper(int from, int to)
        {
            switch (from)
            {
                case 8:
                    switch (to)
                    {
                        case 16:
                            return From8To16Bit;
                        case 24:
                            return From8To24Bit;
                        case 32:
                            return From8To32Bit;
                    }
                    break;
                case 16:
                    switch (to)
                    {
                        case 8:
                            return From16To8Bit;
                        case 24:
                            return From16To24Bit;
                        case 32:
                            return From16To32Bit;
                    }
                    break;
                case 24:
                    switch (to)
                    {
                        case 8:
                            return From24To8Bit;
                        case 16:
                            return From24To16Bit;
                        case 32:
                            return From24To32Bit;
                    }
                    break;
                case 32:
                    switch (to)
                    {
                        case 8:
                            return From32To8Bit;
                        case 16:
                            return From32To16Bit;
                        case 24:
                            return From32To24Bit;
                    }
                    break;
            }
            return NoChange;
        }

        // 8 bit conversions

        public static int From8To16Bit(int sample)
        {
            double normalized = sample / (double)Byte.MaxValue;
            if (normalized >= 0.5)
            {
                return (int)((normalized - 0.5) * short.MaxValue);
            }
            else
            {
                return (int)(-1 * ((1 - (normalized * 2)) * short.MaxValue));
            }
        }

        public static int From8To24Bit(int sample)
        {
            double normalized = sample / (double)Byte.MaxValue;
            if (normalized >= 0.5)
            {
                return (int)((normalized - 0.5) * Math.Pow(2, 23));
            }
            else
            {
                return (int)(-1 * ((1 - (normalized * 2)) * Math.Pow(2, 23)));
            }
        }

        public static int From8To32Bit(int sample)
        {
            double normalized = sample / (double)Byte.MaxValue;
            if (normalized >= 0.5)
            {
                return (int)((normalized - 0.5) * Int32.MaxValue);
            }
            else
            {
                return (int)(-1 * ((1 - (normalized * 2)) * Int32.MaxValue));
            }
        }

        // 16 bit conversions

        public static int From16To8Bit(int sample)
        {
            return (int)(((sample / (double)short.MaxValue + 1) / 2.0) * Byte.MaxValue);
        }

        public static int From16To24Bit(int sample)
        {
            return (int)((sample / (double)short.MaxValue) * Math.Pow(2, 23));
        }

        public static int From16To32Bit(int sample)
        {
            return (int)((sample / (double)short.MaxValue) * Int32.MaxValue);
        }

        // 24 bit conversions

        public static int From24To8Bit(int sample)
        {
            return (int)(((sample / Math.Pow(2, 23) + 1) / 2.0) * Byte.MaxValue);
        }

        public static int From24To16Bit(int sample)
        {
            return (int)((sample / Math.Pow(2, 23)) * short.MaxValue);
        }

        public static int From24To32Bit(int sample)
        {
            return (int)((sample / Math.Pow(2, 23)) * Int32.MaxValue);
        }

        // 32 bit conversions

        public static int From32To8Bit(int sample)
        {
            return (int)(((sample / Int32.MaxValue + 1) / 2.0) * Byte.MaxValue);
        }

        public static int From32To16Bit(int sample)
        {
            return (int)((sample / (double)Int32.MaxValue) * short.MaxValue);
        }

        public static int From32To24Bit(int sample)
        {
            return (int)((sample / (double)Int32.MaxValue) * Math.Pow(2, 23));
        }

        // Default and no conversion

        public static int NoChange(int sample)
        {
            return sample;
        }
    }
}
