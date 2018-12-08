using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BBS
{
    class Program
    {
        static void Main(string[] args)
        {
                BBS bbs = new BBS();
                BitArray ba = bbs.GenerateBits(20000);

                foreach (bool bit in ba)
                {
                    if (bit == false)
                        Console.Write(0);
                    else
                        Console.Write(1);
                    //Console.WriteLine(bit);
                }
            Console.WriteLine();
            Console.WriteLine("Monobit test: {0}", bbs.MonobitTest(ba));
            Console.WriteLine("Series test: {0}", bbs.SeriesTest(ba));
            Console.WriteLine("Long Series test: {0}", bbs.LongSeriesTest(ba));
            Console.WriteLine("Poker test: {0}", bbs.PokerTest(ba));
        }
        class BBS
        {
            long N;
            int p;
            int q;
            long a;
            int[] tabOfMod4;
            int[] tabOfSeeds;
            public BBS()
            {
                tabOfMod4 = ArrayOfMod4(1, 1000000);
                p = /*GenerateMod4();*/ TakeMod4(); //11;
                tabOfSeeds = ArrayOfMod4(1000, 100000);
                q = /*GenerateMod4();*/ TakeMod4(); //19;
                N = (UInt32)p * (UInt32)q;
                a = /*GenerateSeed(N);*/ TakeSeed(); //3;
            }
            int NWD(int a, int b)
            {
                while (a != b)
                {
                    if (a < b)
                        b -= a;
                    else
                        a -= b;
                }
                return a;
            }
            /*int GenerateSeed(int n)
            {
                bool tn = true;
                int l = 0;
                Random rand = new Random();
                while(tn)
                {
                    l = rand.Next();
                    if (NWD(l, n) == 1)
                        tn = false;
                }
                return l;
            }*/
            /*int GenerateMod4()
            {
                bool tn = true;
                int l = 0;
                Random rand = new Random();
                while (tn)
                {
                    l = rand.Next();
                    if (l%4==3)
                        tn = false;
                }
                return l;
            }*/
            int TakeMod4()
            {
                Random rand = new Random();
                int l = rand.Next(0, tabOfMod4.Length - 1);
                return tabOfMod4[l];
            }
            int TakeSeed()
            {
                Random rand = new Random();
                return tabOfSeeds[rand.Next(0, tabOfSeeds.Length - 1)];
            }
            public bool GenerateBit()
            {
                long x = (a * a) % N;
                BitArray bits = new BitArray(BitConverter.GetBytes(x));
                return bits[0];
            }
            public BitArray GenerateBits(int r)
            {
                BitArray bits = new BitArray(r);
                long x=0;
                for(int i=0;i<r;i++)
                {
                    if(i==0)
                    {
                        x = (a * a) % N;
                        BitArray bit = new BitArray(BitConverter.GetBytes(x));
                        bits[i] = bit[0];
                    }
                    else
                    {
                        x = (x * x) % N;
                        BitArray bit = new BitArray(BitConverter.GetBytes(x));
                        bits[i] = bit[0];
                    }
                }
                return bits;
            }
            private int[] ArrayOfMod4(int p, int k)
            {
                List<int> tab = new List<int>();
                for(int j = p; j < k; j++)
                {
                    if(j % 4 == 3)
                    {
                        tab.Add(j);
                    }
                }
                return tab.ToArray();
            }
            private int[] ArrayOfSeed(int p, int k)
            {
                List<int> tab = new List<int>();
                for (int j = p; j < k; j++)
                {
                    if (NWD((int)N,j)==1)
                    {
                        tab.Add(j);
                    }
                }
                return tab.ToArray();
            }
            public bool MonobitTest(BitArray ba)
            {
                int count = 0;
                for(int i=0;i<ba.Length;i++)
                {
                    if(ba[i]==true)
                    {
                        count++;
                    }
                }
                if (9654 < count && count < 10346)
                    return true;
                else
                    return false;
            }
            public bool SeriesTest(BitArray ba)
            {
                int seriesLength = 0;
                int one = 0, two = 0, three = 0, four = 0, five = 0, six = 0;
                for(int i=0;i<ba.Length;i++)
                {
                    if(ba[i]==true)
                    {
                        seriesLength++;
                    }
                    else
                    {
                        if(seriesLength==1)
                        {
                            one++;
                        }
                        if (seriesLength == 2)
                        {
                            two++;
                        }
                        if (seriesLength == 3)
                        {
                            three++;
                        }
                        if (seriesLength == 4)
                        {
                            four++;
                        }
                        if (seriesLength == 5)
                        {
                            five++;
                        }
                        if (seriesLength >= 6)
                        {
                            six++;
                        }
                        seriesLength = 0;
                    }
                }
                if (one > 2267 && one < 2733 &&
                    two > 1079 && two < 1421 &&
                    three > 502 && three < 748 &&
                    four > 223 && four < 402 &&
                    five > 90 && five < 223 &&
                    six > 90 && six < 223)
                    return true;
                else
                    return false;
            }
            public bool LongSeriesTest(BitArray ba)
            {
                int seriesLength = 0;
                bool ok = true;
                for(int i=0;i<ba.Length;i++)
                {
                    if(ba[i]==true)
                    {
                        seriesLength++;
                    }
                    else
                    {
                        if (seriesLength > 34)
                        {
                            ok = false;
                            break;
                        }
                        seriesLength = 0;
                    }
                }
                return ok;
            }
            public bool PokerTest(BitArray ba)
            {
                int[] tab = new int[16];
                for(int i=0; i<ba.Length;i+=4)
                {
                    BitArray fourBits = new BitArray(4);
                    fourBits[0] = ba[i];
                    fourBits[1] = ba[i+1];
                    fourBits[2] = ba[i+2];
                    fourBits[3] = ba[i+3];

                    string s = BitToString(fourBits);

                    if (s == "0000")
                        tab[0]++;
                    if (s == "0001")
                        tab[1]++;
                    if (s == "0010")
                        tab[2]++;
                    if (s == "0011")
                        tab[3]++;
                    if (s == "0100")
                        tab[4]++;
                    if (s == "0101")
                        tab[5]++;
                    if (s == "0110")
                        tab[6]++;
                    if (s == "0111")
                        tab[7]++;
                    if (s == "1000")
                        tab[8]++;
                    if (s == "1001")
                        tab[9]++;
                    if (s == "1010")
                        tab[10]++;
                    if (s == "1011")
                        tab[11]++;
                    if (s == "1100")
                        tab[12]++;
                    if (s == "1101")
                        tab[13]++;
                    if (s == "1110")
                        tab[14]++;
                    if (s == "1111")
                        tab[15]++;
                }
                double sum = 0;
                double sk = 0.0032;
                for (int i=0;i<tab.Length;i++)
                {
                    sum += pow(tab[i]) - (double)5000;
                }
                double x = sk * sum;
                if (x > 2.16 && x < 46.17)
                    return true;
                else
                    return false;
            }
            public string BitToString(BitArray ba)
            {
                string str = "";
                for(int i = 0; i<ba.Length; i++)
                {
                    if (ba[i] == true)
                        str += "1";
                    else
                        str += "0";
                }
                return str;
            }
            double pow(int l)
            {
                double l1 = (double)l;
                double wyn = l1 * l1;
                return wyn;
            }
        }
    }
}
