using System;

namespace BuildersApp_Novikov_3ISP11_13.Class
{
    public static class Calculations
    {
        public static decimal CalculationComponent(decimal price, decimal count, decimal price2)
        {
            return (count * price) + price2;
        }

        public static bool AgeLessThan18(DateTime firstDate, DateTime secondDate)
        {
            int diff = (int)Math.Floor((firstDate - secondDate).TotalDays);
            if (diff < 6570) return true;
            else return false;
        }
    }
}
