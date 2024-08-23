using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SkiaSharpOpenGLBenchmark
{
    public struct Fixed
    {
        // 22:10 fixed point math
        const int CSS_RADIX_POINT = 10;

        public int RawValue;

        // Useful values
        public static readonly Fixed F_PI_2 = new Fixed(0x00000648, true);   /* 1.5708 (PI/2) */
        public static readonly Fixed F_PI = new Fixed(0x00000c91, true);     /* 3.1415 (PI) */
        public static readonly Fixed F_3PI_2 = new Fixed(0x000012d9, true);  /* 4.7124 (3PI/2) */
        public static readonly Fixed F_2PI = new Fixed(0x00001922, true);    /* 6.2831 (2 PI) */

        public static readonly Fixed F_90 = new Fixed(0x00016800, true);     /*  90 */
        public static readonly Fixed F_180 = new Fixed(0x0002d000, true);    /* 180 */
        public static readonly Fixed F_270 = new Fixed(0x00043800, true);    /* 270 */
        public static readonly Fixed F_360 = new Fixed(0x0005a000, true);    /* 360 */

        public static readonly Fixed F_0 = new Fixed(0x00000000, true);      /* 0 */
        public static readonly Fixed F_0_5 = new Fixed(0x00000200, true);    /* 0.5 */
        public static readonly Fixed F_1 = new Fixed(0x00000400, true);      /*   1 */
        public static readonly Fixed F_10 = new Fixed(0x00002800, true);     /*  10 */
        public static readonly Fixed F_72 = new Fixed(0x00012000, true);     /*  72 */
        public static readonly Fixed F_96 = new Fixed(0x00018000, true);     /*  96 */
        public static readonly Fixed F_100 = new Fixed(0x00019000, true);    /* 100 */
        public static readonly Fixed F_200 = new Fixed(0x00032000, true);    /* 200 */
        public static readonly Fixed F_255 = new Fixed(0x0003FC00, true);    /* 255 */
        public static readonly Fixed F_300 = new Fixed(0x0004b000, true);    /* 300 */
        public static readonly Fixed F_400 = new Fixed(0x00064000, true);    /* 400 */

        public Fixed(int a, bool fromRaw = false)
        {
            if (fromRaw)
                RawValue = a;
            else
                RawValue = FromInt(a);
        }


        public Fixed(double a)
        {
            double xx = a * (double)(1 << CSS_RADIX_POINT);

            if (xx < Int32.MinValue)
                xx = Int32.MinValue;

            if (xx > Int32.MaxValue)
                xx = Int32.MaxValue;

            RawValue = (int)xx;
        }

        static int FromInt(int a)
        {
            long xx = ((long)a) * (1 << CSS_RADIX_POINT);

            if (xx < Int32.MinValue)
                xx = Int32.MinValue;

            if (xx > Int32.MaxValue)
                xx = Int32.MaxValue;

            return (int)xx;
            //RawValue = (int)xx;
        }

        public int ToInt()
        {
            return (int)(RawValue >> CSS_RADIX_POINT);
        }

        public double ToDouble()
        {
            return ((double)(RawValue) / (double)(1 << CSS_RADIX_POINT));
        }

        public override string ToString()
        {
            int iPart = RawValue >> CSS_RADIX_POINT;
            long fPart = RawValue & ((1 << CSS_RADIX_POINT) - 1);

            fPart = (fPart * 1000000000) / ((long)1 << CSS_RADIX_POINT);

            return $"{iPart}.{fPart}";
        }

        public Fixed Truncate()
        {
            int a = RawValue;
            int raw = (a & ~((1 << CSS_RADIX_POINT) - 1));
            return new Fixed(raw, true);
        }

        // Fixed point percentage (a) of an integer (b), to an integer
        // FPCT_OF_INT_TOINT()
        public int PercentageToInt(int b)
        {
            Fixed r = (this * b) / F_100;
            return r.ToInt();
        }

        #region +
        public static Fixed operator +(Fixed one, Fixed other)
        {
            Fixed fInt;
            fInt.RawValue = one.RawValue + other.RawValue;
            return fInt;
        }

        public static Fixed operator +(Fixed one, int other)
        {
            return one + (Fixed)other;
        }

        public static Fixed operator +(int other, Fixed one)
        {
            return one + (Fixed)other;
        }
        #endregion

        #region -
        public static Fixed operator -(Fixed one)
        {
            Fixed fint;
            fint.RawValue = -one.RawValue;
            return fint;
        }
        #endregion

        #region *
        public static Fixed operator *(Fixed one, Fixed other)
        {
            long xx = ((long)one.RawValue * (long)other.RawValue) >> CSS_RADIX_POINT;

            if (xx < Int32.MinValue)
                xx = Int32.MinValue;

            if (xx > Int32.MaxValue)
                xx = Int32.MaxValue;

            Fixed fInt;
            fInt.RawValue = (int)xx;
            return fInt;
        }
        public static Fixed operator *(Fixed one, int other)
        {
            long orv = ((long)other) * (1 << CSS_RADIX_POINT);
            long xx = ((long)one.RawValue * orv) >> CSS_RADIX_POINT;

            if (xx < Int32.MinValue)
                xx = Int32.MinValue;

            if (xx > Int32.MaxValue)
                xx = Int32.MaxValue;

            Fixed fInt;
            fInt.RawValue = (int)xx;
            return fInt;
        }

        #endregion

        #region /
        public static Fixed operator /(Fixed x, Fixed y)
        {
            long xx = ((long)x.RawValue * (1 << CSS_RADIX_POINT)) / y.RawValue;

            if (xx < Int32.MinValue)
                xx = Int32.MinValue;

            if (xx > Int32.MaxValue)
                xx = Int32.MaxValue;

            Fixed fInt;
            fInt.RawValue = (int)xx;
            return fInt;
        }
        #endregion

        #region <>
        public static bool operator <(Fixed one, int other)
        {
            Fixed o = (Fixed)other;
            return one.RawValue < o.RawValue;
        }
        public static bool operator >(Fixed one, int other)
        {
            Fixed o = (Fixed)other;
            return one.RawValue > o.RawValue;
        }
        public static bool operator <(Fixed one, Fixed o)
        {
            return one.RawValue < o.RawValue;
        }
        public static bool operator >(Fixed one, Fixed o)
        {
            return one.RawValue > o.RawValue;
        }
        #endregion

        public static explicit operator int(Fixed src)
        {
            return src.ToInt();
        }

        public static explicit operator Fixed(int src)
        {
            // ?!?1
            return new Fixed(src);
        }

        #region ==
        public static bool operator ==(Fixed b1, Fixed b2)
        {
            if ((object)b1 == null)
                return (object)b2 == null;

            return b1.Equals(b2);
        }
        public static bool operator !=(Fixed b1, Fixed b2)
        {
            return !(b1 == b2);
        }
        public override bool Equals(object obj)
        {
            if (obj is Fixed)
                return ((Fixed)obj).RawValue == this.RawValue;
            else
                return false;
        }
        #endregion
    }
}
