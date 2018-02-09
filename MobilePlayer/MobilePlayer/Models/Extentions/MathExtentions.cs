namespace MobilePlayer.Models.Extentions
{
    public static class MathExtentions
    {
        public static int Mod(this int a, int n)
        {
            var result = a % n;
            if (a<0 && n>0 || a>0 && n<0)
                result += n;
            return result;
        }
    }
}