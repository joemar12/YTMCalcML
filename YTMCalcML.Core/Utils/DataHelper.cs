using YTMCalcML.Core.MachineLearning.DataStructures;

namespace YTMCalcML.Core.Utils
{
    public class DataHelper
    {
        private static Random _rng = new Random();

        public IEnumerable<Bond> GenerateBonds(int length)
        {
            for (int i = 1; i <= length; i++)
            {
                var bond = new Bond()
                {
                    FaceValue = _rng.Next(1, 50) * 100f,
                    CouponRate = (float)Math.Round(((_rng.NextDouble() * 7) + 3), 4), //random value between 3 and 10
                    Term = _rng.Next(5, 30),
                    Frequency = 2,
                };
                //limit the difference of face value and current price to 20%
                var priceVariance = bond.FaceValue * (_rng.Next(-20, 20) / 100f);
                bond.Price = bond.FaceValue + priceVariance;
                bond.YieldToMaturity = BondCalc.ComputeYtm(bond);
                yield return bond;
            }
        }
    }
}