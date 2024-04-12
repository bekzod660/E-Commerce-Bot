
namespace E_Commerce_Bot.Helpers
{
    public static class GetSmsCode
    {
        private static readonly char[] chars = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        public static string Get()
        {
            return SixLaborsCaptcha.Core.Extensions.GetUniqueKey(4, chars);
        }
    }
}
