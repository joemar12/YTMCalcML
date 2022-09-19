using YTMCalcML.Core.MachineLearning.DataStructures;

namespace YTMCalcML.Core.Utils
{
    public static class BondCalc
    {
        public static float ComputePrice(Bond bond)
        {
            var c = bond.CouponRate / 100;
            var par = bond.FaceValue;
            var coupon = par * c;
            var freq = bond.Frequency;
            var inputYtm = bond.YieldToMaturity / 100;
            var ytm = inputYtm / freq;
            var periods = bond.Term * freq;
            var cashFlow = coupon / freq;

            var price = cashFlow * ((1 - Math.Pow(1 + ytm, periods * -1)) / ytm) + par / Math.Pow(1 + ytm, periods);

            return (float)price;
        }
        public static float ComputeYtm(Bond bond, float inputInitYtm = 3f)
        {
            //using Newton's Method of Optimization to find the YTM value
            //that will yield the least difference of price within a certain number of iterations
            float epsilon = 0.0001f;
            var maxIterations = 100;
            float initYtm = inputInitYtm; //starts at 3% when no value is specified

            var c = bond.CouponRate / 100;
            var par = bond.FaceValue;
            var coupon = par * c;
            var freq = bond.Frequency;
            var periods = bond.Term * freq;
            var cashFlow = coupon / freq;

            float calcPriceDiff(float inputYtm)
            {
                var ytm = inputYtm / freq / 100;
                //the bond price formula
                var computedPrice = (float)(cashFlow * ((1 - Math.Pow(1 + ytm, periods * -1)) / ytm) + par / Math.Pow(1 + ytm, periods));
                var diff = computedPrice - bond.Price;
                return diff;
            }
            //we're optimizing the price diff here until it reaches a value less than the set threshold, the epsilon.
            return OptimizationHelper.NewtonMethod(calcPriceDiff, initYtm, epsilon, maxIterations);
        }
    }
}