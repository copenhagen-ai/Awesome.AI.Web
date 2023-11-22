namespace Awesome.AI.Web.Extensions
{
    public static class Extensions
    {
        public static string LineBreakToBreak(this string s)
        {
            return s.Replace("\r\n", "<br />");
        }

        public static string TapToSpan(this string s)
        {
            return s.Replace("\t", "<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>");
        }
    }
}
