namespace Win2Mqtt
{
    static public class Helpers
    {

        public static bool IsEmptyOrWhitespaced(this string text)
        {
            return string.IsNullOrWhiteSpace(text);
        }

    }
}
