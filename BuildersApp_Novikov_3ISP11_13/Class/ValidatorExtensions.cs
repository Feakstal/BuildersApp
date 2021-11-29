using System;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace BuildersApp_Novikov_3ISP11_13.Class
{
    public static class ValidatorExtensions
    {
        public static bool IsValidEmailAddress(this string s)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return regex.IsMatch(s);
        }

        public static void PreviewTextInputControlLetters(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }

        public static void PreviewTextInputControlForDate(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0) && !(e.Text == ".") && !(e.Text == ":")) e.Handled = true;
        }

        public static void PreviewTextInputControlForPrice(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0) && !(e.Text == ",")) e.Handled = true;
        }

        public static void PreviewTextInputControlNumbers(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex(@"[абвгдеёжзийклмнопрстуфхцчшщьыъэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЬЫЪЭЮЯabcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ]");
            if (!regex.IsMatch(e.Text)) e.Handled = true;
        }

        public static void PreviewTextInputControlSpec(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex(@"[,.1234567890абвгдеёжзийклмнопрстуфхцчшщьыъэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЬЫЪЭЮЯabcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ]");
            if (!regex.IsMatch(e.Text)) e.Handled = true;
        }
    }
}
