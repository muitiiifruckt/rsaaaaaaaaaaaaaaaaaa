using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;
using System.Threading;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Security.Cryptography;



namespace RSA
{
    public partial class Form1 : Form
    {
        BigInteger p, q, N, e, d;
        public static RandomNumberGenerator rng = RandomNumberGenerator.Create();
        Random randomm = new Random();
        public static string alf_en = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890.,?!";
        string nums = "12234567890 ";
        string text = "";

        int n;
        public bool string_in(char a, string s) // функция, которая проверяет наличие того или иного элемента в алфавите 
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (a == s[i])
                    return true;
            }
            return false;
        }
        public Form1()
        {
            InitializeComponent();
        }

        private BigInteger rand_bits_len_odd(int n) // ПОЛУЧАЕМ РАНДОМНЫОЕ НЕЧЕТНОЕ ЧИСЛО
        {

            Random random = new Random();
            BigInteger rez = 0;
            for (int i = 1; i < n; i++)
            {
                if (random.Next(0, 2) == 1)
                    rez += BigInteger.Pow(2, i);
            }
            return rez + 1;
        }
        private BigInteger rand_bits_len_odd(Random random, int n) // ПОЛУЧАЕМ РАНДОМНЫОЕ НЕЧЕТНОЕ ЧИСЛО
        {


            BigInteger rez = 0;
            for (int i = 1; i < n; i++)
            {
                if (random.Next(0, 2) == 1)
                    rez += BigInteger.Pow(2, i);
            }
            return rez + 1;
        }
        private void rand_p() // ПОЛУЧАЕМ РАНДОМНЫОЕ НЕЧЕТНОЕ ЧИСЛО
        {


            BigInteger rez = 0;
            for (int i = 1; i < n; i++)
            {
                if (randomm.Next(0, 2) == 1)
                    rez += BigInteger.Pow(2, i);
            }
            rez += 1;
            p = rez;
        }
        private void rand_q() // ПОЛУЧАЕМ РАНДОМНЫОЕ НЕЧЕТНОЕ ЧИСЛО
        {

            Random randomm = new Random();
            BigInteger rez = 0;
            for (int i = 1; i < n; i++)
            {
                if (randomm.Next(0, 2) == 1)
                    rez += BigInteger.Pow(2, i);
            }
            rez += 1;
            q = rez;
        }
        private static BigInteger ExtendedEuclideanAlgorithm(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y)
        {
            if (a == 0)
            {
                x = 0;
                y = 1;
                return b;
            }

            BigInteger x1, y1;
            BigInteger gcd = ExtendedEuclideanAlgorithm(b % a, a, out x1, out y1);

            x = y1 - (b / a) * x1;
            y = x1;

            return gcd;
        }
        public static bool is_Prime_Miller(BigInteger R) // алг Миллера для проверки на простоту
        {
            BigInteger t;
            int s;
            s_t(R, out t, out s);
            int k = (int)BigInteger.Log(R, 2.0);
            for (int i = 0; i < k; i++)
            {
                BigInteger b = BigInteger.ModPow(GetRand_Miller(R), t, R);
                if (b == 1 || b == R - 1)
                    continue;
                for (int r = 1; r < s; r++)
                {
                    // x ← x^2 mod n
                    b = BigInteger.ModPow(b, 2, R);

                    // если x == 1, то вернуть "составное"
                    if (b == 1)
                        return false;

                    // если x == n − 1, то перейти на следующую итерацию внешнего цикла
                    if (b == R - 1)
                        break;

                }
                if (b != R - 1)
                    return false;
            }
            return true;
        }
        public BigInteger ModularExponentiation(BigInteger x, BigInteger y, BigInteger modulus)
        {
            if (modulus == 1)
            {
                return 0;
            }

            BigInteger result = 1;
            x = x % modulus;

            while (y > 0)
            {
                if (y % 2 == 1)
                {
                    result = (result * x) % modulus;
                }

                y = y / 2;
                x = (x * x) % modulus;
            }

            return result;
        }
        public static BigInteger GetRand_Miller(BigInteger R)   // получение рандомного числа для Миллера
        {
            byte[] _a = new byte[R.ToByteArray().LongLength];
            BigInteger b;
            do
            {
                rng.GetBytes(_a);
                b = new BigInteger(_a);
            }
            while (b < 2 || b >= R - 2);
            return b;
        }


        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {



        }
        private BigInteger phi(BigInteger n)
        {
            BigInteger result = n;
            for (int i = 2; i * i <= n; i++)
            {
                if (n % i == 0)
                {
                    while (n % i == 0)
                    {
                        n /= i;
                    }
                    result -= result / i;
                }
            }
            if (n > 1)
            {
                result -= result / n;
            }
            return result;
        }
        public delegate void ThreadStart();

        private void button_dec_Click(object sender, EventArgs e)
        {
            rsa_dec();
        }
        private void rsa_dec()
        {
            try
            {

                if (p == 0) throw new Exception("Ключи не сгенtрированы");

                //
                text = richTextBox_enc.Text;
                if (text.Length == 0) throw new Exception("Исходный текст пуст");
                //
                for (int i = 0; i < text.Length; i++)
                    if (!string_in(text[i], nums)) throw new Exception("В тексте присутствует лишние символы ( другой язык, пробелы и т.д.)");

                string res = "", help = "";
                BigInteger r;
                foreach (char c in text)
                {
                    if (c == ' ')
                    {
                        if (!BigInteger.TryParse(help, out r))
                            throw new Exception("Incorrect data");
                        res += alf_en[(int)(BigInteger.ModPow(r, d, N) - 2)];
                        help = "";
                    }
                    else
                        help += c;
                }
                richTextBox_decryort.Text = res;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Ошибка: {e.Message}");

            }
        }
        private void button_en_Click(object sender, EventArgs e)
        {
            rsa_encrypt();
        }

        private void button_run_Click(object sender, EventArgs e)
        {
            RSA_en();
        }

        private void RSA_en()
        {
            try
            {
                if (richTextBox1.Text.Length == 0) throw new Exception("incorrect num");

                if (!int.TryParse(richTextBox1.Text, out int n)) throw new Exception("incorrect num");
                if (n > 1024 || n < 2) throw new Exception("out of range 2-1024");

                Thread myThread1 = new Thread(() =>
                {

                    while (true)
                    {
                        p = rand_bits_len_odd(randomm, n);
                        if (is_Prime_Miller(p))
                            break;
                    }
                });
                Thread myThread2 = new Thread(() =>
                {

                    while (true)
                    {
                        q = rand_bits_len_odd(randomm, n);
                        if (is_Prime_Miller(q))
                            break;
                    }
                });

                myThread1.Start();
                myThread2.Start();

                myThread1.Join();
                myThread2.Join();



                N = BigInteger.Multiply(q, p);
                BigInteger qq = BigInteger.Multiply((p - 1), (q - 1));


                while (true)
                {
                    e = rand_bits_len_odd((n * 2) / 3);
                    if (is_Prime_Miller(e) && qq % e != 0)
                        break;
                }
                BigInteger x, y = 0;
                ExtendedEuclideanAlgorithm(qq, e, out x, out y);
                d = qq + y;
                ///

                ///
                richTextBox2.Text = Convert.ToString(q);
                richTextBox3.Text = Convert.ToString(p);
                richTextBox8.Text = Convert.ToString(N);
                richTextBox4.Text = Convert.ToString(qq);
                richTextBox6.Text = Convert.ToString(e);
                richTextBox7.Text = Convert.ToString(y);
                richTextBox9.Text = Convert.ToString(d);

            }
            catch (Exception e)
            {
                MessageBox.Show($"Ошибка: {e.Message}");

            }




        }
        private void rsa_encrypt()
        {
            try
            {

                if (p == 0) throw new Exception("Ключи не сгенtрированы");

                //
                text = richTextBox_text.Text;
                if (text.Length == 0) throw new Exception("Исходный текст пуст");
                //
                for (int i = 0; i < text.Length; i++)
                    if (!string_in(text[i], alf_en)) throw new Exception("В тексте присутствует лишние символы ( другой язык, пробелы и т.д.)");

                string res = "";

                foreach (char c in text)
                {
                    res += BigInteger.ModPow((BigInteger)(alf_en.IndexOf(c) + 2), e, N).ToString() + " ";
                }
                richTextBox_enc.Text = res;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Ошибка: {e.Message}");

            }
        }
        public static void s_t(BigInteger R, out BigInteger t, out int s)   // получение параметров с и т
        {
            t = R - 1;
            for (s = 0; t % 2 != 1; s++)
                t = t / 2;
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///

        private void button_run_factorizachiya_Click(object sender, EventArgs e)
        {
        }

        private void button_run_Click_1(object sender, EventArgs e)
        {

        }

        private BigInteger fucnk(BigInteger n)
        {
            n = n ^ 2 + 1;
            return n;
        }


    }
}
