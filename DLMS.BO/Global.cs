using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spell;
namespace DLMS.BO
{
    public class Global
    {
        public static User CurrentUser { get; set; }
        public static string NumWords(int Num)
        {
            #region Old Code
            //string[] numbersArr = new string[] { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
            //string[] tensArr = new string[] { "twenty", "thirty", "fourty", "fifty", "sixty", "seventy", "eighty", "ninty" };
            //string[] suffixesArr = new string[] { "thousand", "million", "billion", "trillion", "quadrillion", "quintillion", "sextillion", "septillion", "octillion", "nonillion", "decillion", "undecillion", "duodecillion", "tredecillion", "Quattuordecillion", "Quindecillion", "Sexdecillion", "Septdecillion", "Octodecillion", "Novemdecillion", "Vigintillion" };
            //string words = "";

            //bool tens = false;

            //if (n < 0)
            //{
            //    words += "negative ";
            //    n *= -1;
            //}

            //int power = (suffixesArr.Length + 1) * 3;

            //while (power > 3)
            //{
            //    double pow = Math.Pow(10, power);
            //    if (n > pow)
            //    {
            //        if (n % Math.Pow(10, power) > 0)
            //        {
            //            words += NumWords(Math.Floor(n / pow)) + " " + suffixesArr[(power / 3) - 1] + " and ";
            //        }
            //        else if (n % pow > 0)
            //        {
            //            words += NumWords(Math.Floor(n / pow)) + " " + suffixesArr[(power / 3) - 1];
            //        }
            //        n %= pow;
            //    }
            //    power -= 3;
            //}
            //if (n >= 1000)
            //{
            //    if (n % 1000 > 0) words += NumWords(Math.Floor(n / 1000)) + " thousand and ";
            //    else words += NumWords(Math.Floor(n / 1000)) + " thousand";
            //    n %= 1000;
            //}
            //if (0 <= n && n <= 999)
            //{
            //    if ((int)n / 100 > 0)
            //    {
            //        words += NumWords(Math.Floor(n / 100)) + " hundred";
            //        n %= 100;
            //    }
            //    if ((int)n / 10 > 1)
            //    {
            //        if (words != "")
            //            words += " ";
            //        words += tensArr[(int)n / 10 - 2];
            //        tens = true;
            //        n %= 10;
            //    }

            //    if (n < 20)
            //    {
            //        if (words != "" && tens == false)
            //            words += " ";
            //        words += (tens ? "-" + numbersArr[(int)n - 1] : numbersArr[(int)n - 1]);
            //        n -= Math.Floor(n);
            //    }
            //}

            //return words;
            #endregion
            string[] Below20 = { "", "One ", "Two ", "Three ", "Four ", 
      "Five ", "Six " , "Seven ", "Eight ", "Nine ", "Ten ", "Eleven ", 
    "Twelve " , "Thirteen ", "Fourteen ","Fifteen ", 
      "Sixteen " , "Seventeen ","Eighteen " , "Nineteen " };
            string[] Below100 = { "", "", "Twenty ", "Thirty ", 
      "Forty ", "Fifty ", "Sixty ", "Seventy ", "Eighty ", "Ninety " };
            string InWords = "";
            if (Num >= 1 && Num < 20)
                InWords += Below20[Num];
            if (Num >= 20 && Num <= 99)
                InWords += Below100[Num / 10] + Below20[Num % 10];
            if (Num >= 100 && Num <= 999)
                InWords += NumWords(Num / 100) + " Hundred " + NumWords(Num % 100);
            if (Num >= 1000 && Num <= 99999)
                InWords += NumWords(Num / 1000) + " Thousand " + NumWords(Num % 1000);
            if (Num >= 100000 && Num <= 9999999)
                InWords += NumWords(Num / 100000) + " Lac " + NumWords(Num % 100000);
            if (Num >= 10000000)
                InWords += NumWords(Num / 10000000) + " Crore " + NumWords(Num % 10000000);

            return InWords;
        }

        public static string TakaFormat(double TotalAmt)
        {

            string sInWords = string.Empty;

            string sPoisa = string.Empty;
            string[] words = TotalAmt.ToString().Split('.');
            string sTaka = words[0];
            if (words.Length == 1)
            {
                sPoisa = "00";
            }
            else
            {
                sPoisa = words[1];
                if (sPoisa.Length == 1)
                {
                    sPoisa = sPoisa + "0";
                }
            }

            int i = sTaka.Length;
            string sDH1 = string.Empty;

            if (i == 9)
            {
                sDH1 = Spell.SpellAmount.F_Crores(sTaka, sPoisa);
            }
            else if (i == 8)
            {
                sDH1 = Spell.SpellAmount.F_Crore(sTaka, sPoisa);
            }
            else if (i == 7)
            {
                sDH1 = Spell.SpellAmount.F_Lakhs(sTaka, sPoisa);
            }
            else if (i == 6)
            {
                sDH1 = Spell.SpellAmount.F_Lakh(sTaka, sPoisa);
            }
            else if (i == 5)
            {
                sDH1 = Spell.SpellAmount.F_Thousands(sTaka, sPoisa);
            }
            else if (i == 4)
            {
                sDH1 = Spell.SpellAmount.F_Thousand(sTaka, sPoisa);
            }
            else if (i == 3)
            {
                sDH1 = Spell.SpellAmount.F_Hundred(sTaka, sPoisa);
            }
            else if (i == 2)
            {
                sDH1 = Spell.SpellAmount.Tens(sTaka);
            }

            sInWords = "In word ," + sDH1 + ".";

            return sInWords;

        }
    }
}
