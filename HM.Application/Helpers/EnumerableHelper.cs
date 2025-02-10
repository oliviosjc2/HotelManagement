namespace HM.Application.Helpers
{
    public static class EnumerableHelper
    {
        public static bool AnySafe<T>(this IEnumerable<T> source)
        {
            if (source != null)
                if (source.Any())
                    return true;

            return false;
        }

        public static bool AnySafe<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            if (source != null)
                if (source.Any(predicate))
                    return true;

            return false;
        }
    }
}
