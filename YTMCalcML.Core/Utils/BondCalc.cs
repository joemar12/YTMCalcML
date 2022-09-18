using YTMCalcML.Core.MachineLearning.DataStructures;

namespace YTMCalcML.Core.Utils
{
    public static class BondCalc
    {
        public static float ComputePrice(Bond bond)
        {
            var c = bond.CouponRate / 100;
            var par = bond.Redemption;
            var coupon = par * c;
            var freq = bond.Frequency;
            var ytm = bond.YTM / 100;
            var i = ytm / freq;
            var N = bond.Term * freq;
            var cashFlow = coupon / freq;

            var price = cashFlow * ((1 - Math.Pow(1 + i, N * -1)) / i) + par / Math.Pow(1 + i, N);

            return (float)price;
        }
    }
}