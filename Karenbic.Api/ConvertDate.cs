using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Karenbic.Api
{
    public class ConvertDate
    {
        public static DateTime PersianTOJulian(string PersianDate)
        {
            try
            {
                string[] JulianDate = PersianDate.Split('/');
                System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
                return pc.ToDateTime(
                    Convert.ToInt32(JulianDate[0]),
                    Convert.ToInt32(JulianDate[1]),
                    Convert.ToInt32(JulianDate[2]),
                    0, 0, 0, 0);
            }
            catch
            {
                return DateTime.Now;
            }
        }
        
        public static string JulainToPersian(DateTime JulianDate)
        {
            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();

            try
            {
                int year = pc.GetYear(JulianDate);
                int month = pc.GetMonth(JulianDate);
                int day = pc.GetDayOfMonth(JulianDate);

                return string.Format("{0}/{1}/{2}", year, month, day);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string JulainToLongPersian(DateTime JulianDate)
        {
            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();

            try
            {
                int year = pc.GetYear(JulianDate);
                int month = pc.GetMonth(JulianDate);
                int day = pc.GetDayOfMonth(JulianDate);

                string MonthName = string.Empty;
                switch (month)
                {
                    case 1:
                        MonthName = "فروردین";
                        break;
                    case 2:
                        MonthName = "ادریبهشت";
                        break;
                    case 3:
                        MonthName = "خرداد";
                        break;
                    case 4:
                        MonthName = "تیر";
                        break;
                    case 5:
                        MonthName = "مرداد";
                        break;
                    case 6:
                        MonthName = "شهریور";
                        break;
                    case 7:
                        MonthName = "مهر";
                        break;
                    case 8:
                        MonthName = "آبان";
                        break;
                    case 9:
                        MonthName = "آذر";
                        break;
                    case 10:
                        MonthName = "دی";
                        break;
                    case 11:
                        MonthName = "بهمن";
                        break;
                    case 12:
                        MonthName = "اسفند";
                        break;

                }

                string DayName = string.Empty;
                switch (pc.GetDayOfWeek(DateTime.Now))
                {
                    case DayOfWeek.Saturday:
                        DayName = "شنبه";
                        break;
                    case DayOfWeek.Sunday:
                        DayName = "یکشنبه";
                        break;
                    case DayOfWeek.Monday:
                        DayName = "دوشنبه";
                        break;
                    case DayOfWeek.Tuesday:
                        DayName = "سه شنبه";
                        break;
                    case DayOfWeek.Wednesday:
                        DayName = "چهارشنبه";
                        break;
                    case DayOfWeek.Thursday:
                        DayName = "پنجشنبه";
                        break;
                    case DayOfWeek.Friday:
                        DayName = "جمعه";
                        break;
                }

                return string.Format("{1} {0} {2} {3}", day, DayName, MonthName, year);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
