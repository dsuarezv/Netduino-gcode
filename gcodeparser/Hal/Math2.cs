using System;
//using Microsoft.SPOT;

namespace gcodeparser
{

#if DESKTOP_MATH

    public class Math2
    {
        public static float Sqrt(float val)
        {
            float result = (float)Math.Sqrt(val);

            return result;
        }

        public static float Atan2(float y, float x)
        {
            float result = (float)Math.Atan2(y, x);

            return result;
        }

        public static float Cos(float rad)
        {
            float result = (float)Math.Cos(rad);

            return result;
        }

        public static float Sin(float rad)
        {
            float result = (float)Math.Sin(rad);

            return result;
        }

        public static float Abs(float rad)
        {
            float result = (float)Math.Abs(rad);

            return result;
        }
    }


#else

    public class Math2
    {
        public static readonly float PI = 3.14159265358979323846F;


        const float sq2p1 = 2.414213562373095048802e0F;
        const float sq2m1 = .414213562373095048802e0F;
        const float pio2 = 1.570796326794896619231e0F;
        const float pio4 = .785398163397448309615e0F;
        const float log2e = 1.4426950408889634073599247F;
        const float sqrt2 = 1.4142135623730950488016887F;
        const float ln2 = 6.93147180559945286227e-01F;
        const float atan_p4 = .161536412982230228262e2F;
        const float atan_p3 = .26842548195503973794141e3F;
        const float atan_p2 = .11530293515404850115428136e4F;
        const float atan_p1 = .178040631643319697105464587e4F;
        const float atan_p0 = .89678597403663861959987488e3F;
        const float atan_q4 = .5895697050844462222791e2F;
        const float atan_q3 = .536265374031215315104235e3F;
        const float atan_q2 = .16667838148816337184521798e4F;
        const float atan_q1 = .207933497444540981287275926e4F;
        const float atan_q0 = .89678597403663861962481162e3F;



        public static float Sqrt(float val)
        {
            // Taken from http://highfieldtales.wordpress.com/tag/netduino/

            //cut off any special case
            if (val <= 0.0f) return 0.0f;

            //here is a kind of base-10 logarithm
            //so that the argument will fall between
            //1 and 100, where the convergence is fast

            float exp = 1.0f;

            while (val < 1.0f)
            {
                val *= 100.0f;
                exp *= 0.1f;
            }

            while (val > 100.0f)
            {
                val *= 0.01f;
                exp *= 10.0f;
            }

            //choose the best starting point
            //upon the actual argument value
            float prev;

            if (val > 10f)
            {
                //decade (10..100)
                prev = 5.51f;
            }
            else if (val == 1.0f)
            {
                //avoid useless iterations
                return val * exp;
            }
            else
            {
                //decade (1..10)
                prev = 1.741f;
            }

            //apply the Newton-Rhapson method
            //just for three times
            // DAVE: apply the method once more to increase precission
            for (int pp = 0; pp < 4; ++pp)
            {
                prev = 0.5f * (prev + val / prev);
            }

            //adjust the result multiplying for
            //the base being cut off before
            return prev * exp;
        }

        public static float Atan2(float y, float x)
        {
            // DAVE: swapped y and x to get the same result as the .net math function
            if ((y + x) == y)
            {
                if ((y == 0F) & (x == 0F)) return 0F;

                if (y >= 0.0F)
                    return pio2;
                else
                    return (-pio2);
            }
            else if (x < 0.0F)
            {
                if (y >= 0.0F)
                    return ((pio2 * 2) - atans((-y) / x));
                else
                    return (((-pio2) * 2) + atans(y / x));

            }
            else if (y > 0.0F)
            {
                return (atans(y / x));
            }
            else
            {
                return (-atans((-y) / x));
            }
        }

        private static float atans(float x)
        {
            if (x < sq2m1)
                return (atanx(x));
            else if (x > sq2p1)
                return (pio2 - atanx(1.0F / x));
            else
                return (pio4 + atanx((x - 1.0F) / (x + 1.0F)));
        }

        private static float atanx(float x)
        {
            float argsq;
            float value;

            argsq = x * x;
            value = ((((atan_p4 * argsq + atan_p3) * argsq + atan_p2) * argsq + atan_p1) * argsq + atan_p0);
            value = value / (((((argsq + atan_q4) * argsq + atan_q3) * argsq + atan_q2) * argsq + atan_q1) * argsq + atan_q0);
            return (value * x);
        }



        public static float Cos(float x)
        {
            // This function is based on the work described in
            // http://www.ganssle.com/approx/approx.pdf

            x = x % (PI * 2.0f);

            // Make X positive if negative
            if (x < 0) { x = 0.0F - x; }

            // Get quadrand

            // Quadrand 0,  >-- Pi/2
            byte quadrand = 0;

            // Quadrand 1, Pi/2 -- Pi
            if ((x > (PI / 2F)) & (x < (PI)))
            {
                quadrand = 1;
                x = PI - x;
            }

            // Quadrand 2, Pi -- 3Pi/2
            if ((x > (PI)) & (x < ((3F * PI) / 2)))
            {
                quadrand = 2;
                x = PI - x;
            }

            // Quadrand 3 - 3Pi/2 -->
            if ((x > ((3F * PI) / 2)))
            {
                quadrand = 3;
                x = (2F * PI) - x;
            }

            // Constants used for approximation
            const float c1 = 0.99999999999925182f;
            const float c2 = -0.49999999997024012f;
            const float c3 = 0.041666666473384543f;
            const float c4 = -0.001388888418000423f;
            const float c5 = 0.0000248010406484558f;
            const float c6 = -0.0000002752469638432f;
            const float c7 = 0.0000000019907856854f;

            // X squared
            float x2 = x * x;

            // Check quadrand
            if ((quadrand == 0) | (quadrand == 3))
            {
                // Return positive for quadrand 0, 3
                return (c1 + x2 * (c2 + x2 * (c3 + x2 * (c4 + x2 * (c5 + x2 * (c6 + c7 * x2))))));
            }
            else
            {
                // Return negative for quadrand 1, 2
                return 0.0F - (c1 + x2 * (c2 + x2 * (c3 + x2 * (c4 + x2 * (c5 + x2 * (c6 + c7 * x2))))));
            }
        }

        public static float Sin(float x)
        {
            return Cos((PI / 2.0F) - x);
        }

        public static float Abs(float x)
        {
            if (x >= 0.0F)
                return x;
            else
                return (-x);
        }


        // This is the double.Parse implementation in the .NET MF 4.2. 
        // There is a bug in .NET MF 4.1 where negative numbers between 0 and -1 are
        // incorrectly parsed as positive. http://netmf.codeplex.com/workitem/788
        public static double StringToDouble(string s)
        {
            if (s == null)
                return 0;

            s = s.Trim(' ').ToLower();

            if (s.Length == 0) return 0;

            int decimalpoint = s.IndexOf('.');
            int exp = s.IndexOf('e');

            if (exp != -1 && decimalpoint > exp)
                throw new Exception();

            char[] chars = s.ToCharArray();
            int len = chars.Length;
            double power = 0;
            double rightDecimal = 0;
            int decLeadingZeros = 0;
            double leftDecimal = 0;
            int leftDecLen = 0;
            bool isNeg = chars[0] == '-';

            // convert the exponential portion to a number            
            if (exp != -1 && exp + 1 < len - 1)
            {
                int tmp;
                power = GetDoubleNumber(chars, exp + 1, len - (exp + 1), out tmp);
            }

            // convert the decimal portion to a number
            if (decimalpoint != -1)
            {
                double number;
                int decLen;

                if (exp == -1)
                {
                    decLen = len - (decimalpoint + 1);
                }
                else
                {
                    decLen = (exp - (decimalpoint + 1));
                }

                number = GetDoubleNumber(chars, decimalpoint + 1, decLen, out decLeadingZeros);

                rightDecimal = number * System.Math.Pow(10, -decLen);
            }

            // convert the integer portion to a number
            if (decimalpoint != 0)
            {
                int leadingZeros;

                if (decimalpoint == -1 && exp == -1) leftDecLen = len;
                else if (decimalpoint != -1) leftDecLen = decimalpoint;
                else leftDecLen = exp;

                leftDecimal = GetDoubleNumber(chars, 0, leftDecLen, out leadingZeros);
                // subtract leading zeros from integer length
                leftDecLen -= leadingZeros;

                if (chars[0] == '-' || chars[0] == '+') leftDecLen--;
            }

            double value = 0;
            if (leftDecimal < 0)
            {
                value = -leftDecimal + rightDecimal;
                value = -value;
            }
            else
            {
                value = leftDecimal + rightDecimal;
            }

            // lets normalize the integer portion first
            while (leftDecLen > 1)
            {
                switch (leftDecLen)
                {
                    case 2:
                        value /= 10.0;
                        power += 1;
                        leftDecLen -= 1;
                        break;
                    case 3:
                        value /= 100.0;
                        power += 2;
                        leftDecLen -= 2;
                        break;
                    case 4:
                        value /= 1000.0;
                        power += 3;
                        leftDecLen -= 3;
                        break;
                    default:
                        value /= 10000.0;
                        power += 4;
                        leftDecLen -= 4;
                        break;
                }
            }

            // now normalize the decimal portion
            if (value != 0.0 && value < 1.0 && value > -1.0)
            {
                // for normalization we want x.xxx instead of 0.xxx
                decLeadingZeros++;

                while (decLeadingZeros > 0)
                {
                    switch (decLeadingZeros)
                    {
                        case 1:
                            value *= 10.0;
                            power -= 1;
                            decLeadingZeros -= 1;
                            break;
                        case 2:
                            value *= 100.0;
                            power -= 2;
                            decLeadingZeros -= 2;
                            break;
                        case 3:
                            value *= 1000.0;
                            power -= 3;
                            decLeadingZeros -= 3;
                            break;
                        default:
                            value *= 10000.0;
                            power -= 4;
                            decLeadingZeros -= 4;
                            break;
                    }
                }
            }

            // special case for epsilon (the System.Math.Pow native method will return zero for -324)
            if (power == -324)
            {
                value = value * System.Math.Pow(10, power + 1);
                value /= 10.0;
            }
            else
            {
                value = value * System.Math.Pow(10, power);
            }

            if (value == double.PositiveInfinity || value == double.NegativeInfinity)
            {
                throw new Exception();
            }

            if (isNeg && value > 0)
            {
                value = -value;
            }

            return value;
        }

        private static double GetDoubleNumber(char[] chars, int start, int length, out int numLeadingZeros)
        {
            double number = 0;
            bool isNeg = false;
            int end = start + length;

            numLeadingZeros = 0;

            if (chars[start] == '-')
            {
                isNeg = true;
                start++;
            }
            else if (chars[start] == '+')
            {
                start++;
            }

            for (int i = start; i < end; i++)
            {
                int digit;
                char c = chars[i];

                // switch statement is faster than subtracting '0'                
                switch (c)
                {
                    case '0':
                        // update the number of leading zeros (used for normalizing)
                        if ((numLeadingZeros + start) == i)
                        {
                            numLeadingZeros++;
                        }
                        digit = 0;
                        break;
                    case '1':
                        digit = 1;
                        break;
                    case '2':
                        digit = 2;
                        break;
                    case '3':
                        digit = 3;
                        break;
                    case '4':
                        digit = 4;
                        break;
                    case '5':
                        digit = 5;
                        break;
                    case '6':
                        digit = 6;
                        break;
                    case '7':
                        digit = 7;
                        break;
                    case '8':
                        digit = 8;
                        break;
                    case '9':
                        digit = 9;
                        break;
                    default:
                        throw new Exception();
                }

                number *= 10;
                number += digit;
            }

            return isNeg ? -number : number;
        }



    }

#endif
}
