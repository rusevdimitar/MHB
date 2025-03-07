using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MHB.BL
{
    public static class DateHelper
    {
        private const string TIME_OF_DAY_AM = "AM";
        private const string TIME_OF_DAY_PM = "PM";

        public static string ToISODate(this DateTime dt)
        {
            if (dt == null) throw new ArgumentNullException("MHB.BL.DateHelper.ToISODate(): DateTime is null!");

            string month = dt.Month <= 9 ? string.Format("0{0}", dt.Month) : dt.Month.ToString();
            string day = dt.Day <= 9 ? string.Format("0{0}", dt.Day) : dt.Day.ToString();

            string hour = dt.Hour <= 9 ? string.Format("0{0}", dt.Hour) : dt.Hour.ToString();
            string minute = dt.Minute <= 9 ? string.Format("0{0}", dt.Minute) : dt.Minute.ToString();
            string second = dt.Second <= 9 ? string.Format("0{0}", dt.Second) : dt.Second.ToString();

            return string.Format("{0}-{1}-{2} {3}:{4}:{5}", dt.Year, month, day, hour, minute, second);
        }

        public static string ToISODateFileName(this DateTime dt)
        {
            if (dt == null) throw new ArgumentNullException("MHB.BL.DateHelper.ToISODateFileName(): DateTime is null!");

            string month = dt.Month <= 9 ? string.Format("0{0}", dt.Month) : dt.Month.ToString();
            string day = dt.Day <= 9 ? string.Format("0{0}", dt.Day) : dt.Day.ToString();

            return string.Format("{0}-{1}-{2}", dt.Year, month, day);
        }

        [Obsolete("Very dangerous this - dt.ToString(tt)")]
        public static Enums.TimeOfDay DayTime(this DateTime dt)
        {
            switch (dt.ToString("tt").ToUpper())
            {
                case TIME_OF_DAY_AM:
                    return Enums.TimeOfDay.AM;

                case TIME_OF_DAY_PM:
                    return Enums.TimeOfDay.PM;

                default:
                    return Enums.TimeOfDay.Undetermined;
            }
        }

        public static bool IsWeekDay(this DateTime dt)
        {
            switch (dt.DayOfWeek)
            {
                case DayOfWeek.Monday:
                case DayOfWeek.Tuesday:
                case DayOfWeek.Wednesday:
                case DayOfWeek.Thursday:
                case DayOfWeek.Friday:
                    return true;

                case DayOfWeek.Saturday:
                case DayOfWeek.Sunday:
                    return false;

                default:
                    return false;
            }
        }

        public static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
    }

    public static class ComparerExtensions
    {
        public static bool IsBetween<T>(this T item, T start, T end)
        {
            return Comparer<T>.Default.Compare(item, start) >= 0
                && Comparer<T>.Default.Compare(item, end) <= 0;
        }

        public static void RemoveRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items.ToArray())
            {
                collection.Remove(item);
            }
        }

        public static void RemoveRange<T>(this ICollection<T> collection, Func<T, bool> predicate)
        {
            if (collection == null) throw new ArgumentNullException("collection");
            if (predicate == null) throw new ArgumentNullException("predicate");

            IEnumerable<T> itemsToRemove = collection.Where(predicate).ToArray();

            collection.RemoveRange(itemsToRemove);
        }
    }

    public static class SortingExtensions
    {
        public static IEnumerable<T> Sort<T>(this IEnumerable<T> source, string sortExpression)
        {
            string[] sortParts = sortExpression.Split(' ');
            var param = Expression.Parameter(typeof(T), string.Empty);
            try
            {
                var property = Expression.Property(param, sortParts[0]);
                var sortLambda = Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), param);

                if (sortParts.Length > 1 && (sortParts[1].Equals("desc", StringComparison.OrdinalIgnoreCase) || sortParts[1].Equals("descending", StringComparison.OrdinalIgnoreCase)))
                {
                    return source.AsQueryable<T>().OrderByDescending<T, object>(sortLambda);
                }

                return source.AsQueryable<T>().OrderBy<T, object>(sortLambda);
            }
            catch (ArgumentException)
            {
                return source;
            }
        }
    }

    public static class EqualityExtensions
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> knownKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (knownKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }

    public static class NumericExtensions
    {
        public static bool IsOdd(this int value)
        {
            return value % 2 != 0;
        }

        public static bool IsEven(this int value)
        {
            return value % 2 == 0;
        }
    }
}