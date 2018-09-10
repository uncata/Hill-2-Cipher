using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hill_2_Cipher
{
    class Program
    {
        static void Main(string[] args)
        {
            // Example 1
            //Console.WriteLine(Hill2Cipher(1, 2, 0, 3, "I AM HIDING"));

            // Example 2
            //Console.WriteLine(Residue(7,5));
            //Console.WriteLine(Residue(19, 2));
            //Console.WriteLine(Residue(-1, 26));
            //Console.WriteLine(Residue(12, 4));

            // Example 3
            //Console.WriteLine(Residue(87, 26));
            //Console.WriteLine(Residue(-38, 26));
            //Console.WriteLine(Residue(-26, 26));

            // Example 4
            //Console.WriteLine(Reciprocal(3, 26));

            // Example 5
            //Console.WriteLine(Reciprocal(4, 26));

            // Example 6

            // Example 7
            //Console.WriteLine(DecodeHill2Cipher(5, 6, 2, 3, "GTNKGKDUSK"));

            // Tests
            //Console.WriteLine(IsInvertibleModuloM(12, 2, 0, 3, 26));

            List<int> matrix = GenerateInvertible2x2Matrix();
            int a = matrix[0];
            int b = matrix[1];
            int c = matrix[2];
            int d = matrix[3];

            Console.WriteLine("Enciphering Matrix: " + a.ToString() + ", " + b.ToString() + ", " + c.ToString() + ", " + d.ToString());
            Console.WriteLine("Invertible Modulo 26? " + IsInvertibleModuloM(a, b, c, d, 26));
            Console.Write("Enter a Message you want to Hill 2-Cipher: ");
            string plaintext = Console.ReadLine();
            Console.WriteLine("\nPlaintext: " + plaintext);
            Console.WriteLine("\nHill 2-Cipher: " + Hill2Cipher(a, b, c, d, plaintext));
            Console.WriteLine("\nDecoded Hill 2-Cipher: " + DecodeHill2Cipher(a, b, c, d, Hill2Cipher(a, b, c, d, plaintext)));
            Console.Write("\nPress any key to continue . . . ");
            Console.ReadKey();

            //Console.WriteLine(Hill2Cipher(5, 6, 2, 3, "strike now"));
            //Console.WriteLine(DecodeHill2Cipher(5, 6, 2, 3, Hill2Cipher(5, 6, 2, 3, "strike now")));
        }

        public static long Residue(long a, int m)
        {
            long R = Math.Abs(a) % m;

            if (a >= 0)
            {
                return R;
            }

            if (a < 0 && R != 0)
            {
                return (m - R);
            }

            if (a < 0 && R == 0)
            {
                return 0;
            }

            return 0;
        }

        public static bool IsPrime(int m)
        {
            if (m <= 1)
            {
                return false;
            }

            if (m > 1 && m <= 3)
            {
                return true;
            }

            if (m % 2 == 0 || m % 3 == 0)
            {
                return false;
            }

            int i = 5;

            while (i * i <= m)
            {
                if (m % i == 0 || m % (i + 2) == 0)
                {
                    return false;
                }
                i += 6;
            }

            return true;
        }

        public static long Totient(int m)
        {
            long x = 0;

            for (int i = 1; i < m; i++)
            {
                if (GCD(i, m) == 1)
                {
                    x += 1;
                }
            }
            return x;
        }

        public static long Reciprocal(long a, int m)
        {
            if (IsReciprocable(a, m) == true)
            {
                return Residue(Convert.ToInt64(Math.Pow(a, Totient(m) - 1)), m);
            }
            throw new System.ArgumentException("The number " + a.ToString() + " has no reciprocal modulo " + m.ToString());
        }

        public static int GCD(int a, int m)
        {
            if (m == 0)
            {
                return a;
            }

            else
            {
                return GCD(m, a % m);
            }
        }

        public static List<long> Factors(long a)
        {
            List<long> factors = new List<long>();

            for (int i = 1; i < a + 1; i++)
            {
                if (a % i == 0)
                {
                    factors.Add(i);
                }
            }
            return factors;
        }

        public static List<long> PrimeFactors(long a)
        {
            List<long> primefactors = new List<long>();

            for (int i = 1; i < a + 1; i++)
            {
                if (IsPrime(i) == true && a % i == 0)
                {
                    primefactors.Add(i);
                }
            }
            return primefactors;
        }

        public static List<long> CF(long a, int m)
        {
            List<long> afactors = Factors(a);
            List<long> mfactors = Factors(m);

            return afactors.Intersect(mfactors).ToList();
        }

        public static List<long> CPF(long a, long m)
        {
            List<long> aprimefactors = PrimeFactors(a);
            List<long> mprimefactors = PrimeFactors(m);

            return aprimefactors.Intersect(mprimefactors).ToList();
        }

        public static long LCPF(long a, int m)
        {
            return CPF(a, m)[0];
        }

        public static long det(long a, long b, long c, long d)
        {
            return (a * d - b * c);
        }

        public static int LetterToNumber(char letter)
        {
            return (letter - 64);
        }

        public static string NumberToLetter(int number)
        {
            return Convert.ToChar(number + 64).ToString();
        }

        public static string Hill2Cipher(int a, int b, int c, int d, string plaintext)
        {
            plaintext = plaintext.ToUpper().Replace(" ", "");

            if (plaintext.Length % 2 != 0)
            {
                string dummyletter = plaintext[plaintext.Length - 1].ToString();
                plaintext += dummyletter;
            }

            string hill2cipher = "";

            for (int i = 0; i < plaintext.Length; i = i + 2)
            {
                char p1t = plaintext[i];
                char p2t = plaintext[i + 1];

                int p1 = LetterToNumber(p1t);
                int p2 = LetterToNumber(p2t);

                int Ap1 = Convert.ToInt32(Residue((a * p1 + b * p2), 26));
                if (Residue((a * p1 + b * p2), 26) == 0)
                {
                    Ap1 = 26;
                }

                int Ap2 = Convert.ToInt32(Residue((c * p1 + d * p2), 26));
                if (Residue((c * p1 + d * p2), 26) == 0)
                {
                    Ap2 = 26;
                }

                hill2cipher = hill2cipher + NumberToLetter(Ap1) + NumberToLetter(Ap2);
            }
            return hill2cipher;
        }

        public static bool IsReciprocable(long a, int m)
        {
            if (CPF(a, m).Count == 0)
            {
                return true;
            }

            if (a % 2 != 0)
            {
                return true;
            }
            return false;
        }
        public static bool IsInvertibleModuloM(long a, long b, long c, long d, int m)
        {
            long atemp = a;
            long detA = det(a, b, c, d);

            if (IsReciprocable(Residue(detA, m), m) == true)
            {
                if (CPF(m, Residue(detA, 26)).Count == 0)
                {
                    foreach (long primefactor in PrimeFactors(m))
                    {
                        if (Residue(detA, 26) % primefactor != 0)
                        {
                            return true;
                        }
                    }
                }

            }
            return false;
        }

        public static List<int> Generate2x2Matrix()
        {
            List<int> matrix = new List<int>();
            Random r = new Random();

            for (int i = 0; i < 4; i++)
            {
                matrix.Add(r.Next(-9, 9));
            }
            return matrix;
        }

        public static List<int> GenerateInvertible2x2Matrix()
        {
            List<int> invertiblematrix = Generate2x2Matrix();

            if (IsInvertibleModuloM(invertiblematrix[0], invertiblematrix[1], invertiblematrix[2], invertiblematrix[3], 26) == true)
            {
                return invertiblematrix;
            }

            Console.WriteLine("");
            Console.Clear();
            return GenerateInvertible2x2Matrix();
        }

        public static string DecodeHill2Cipher(long a, long b, long c, long d, string hill2cipher)
        {
            long atemp = a;
            long detA = det(a, b, c, d);

            a = Residue(d * Reciprocal(Residue(detA, 26), 26), 26);
            b = Residue(-b * Reciprocal(Residue(detA, 26), 26), 26);
            c = Residue(-c * Reciprocal(Residue(detA, 26), 26), 26);
            d = Residue(atemp * Reciprocal(Residue(detA, 26), 26), 26);


            string decodedhill2cipher = "";

            for (int i = 0; i < hill2cipher.Length; i = i + 2)
            {
                //int Ape1 = Convert.ToChar(hill2cipher[i] - 64);
                //int Ape2 = Convert.ToChar(hill2cipher[i + 1] - 64);
                int Ape1 = LetterToNumber(hill2cipher[i]);
                int Ape2 = LetterToNumber(hill2cipher[i + 1]);

                long pe1 = Residue((a * Ape1 + b * Ape2), 26);
                if (Residue((a * Ape1 + b * Ape2), 26) == 0)
                {
                    pe1 = 26;
                }

                long pe2 = Residue((c * Ape1 + d * Ape2), 26);
                if (Residue((c * Ape1 + d * Ape2), 26) == 0)
                {
                    pe2 = 26;
                }

                //decodedhill2cipher = decodedhill2cipher + Convert.ToChar(pe1 + 64) + Convert.ToChar(pe2 + 64);
                decodedhill2cipher = decodedhill2cipher + NumberToLetter(Convert.ToInt32(pe1)) + NumberToLetter(Convert.ToInt32(pe2));
            }
            return decodedhill2cipher;
        }
    }
}