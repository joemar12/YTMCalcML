using YTMCalcML.Core.MachineLearning.DataStructures;

namespace YTMCalcML.Core.Utils
{
    public class DataHelper
    {
        public IEnumerable<Bond> GenerateBonds(int length)
        {
            var rng = new Random();
            for (int i = 1; i <= length; i++)
            {
                var bond = new Bond()
                {
                    CouponRate = rng.Next(3, 7) + rng.NextSingle(),
                    Redemption = rng.Next(1, 50) * 100f,
                    Term = rng.Next(5, 30),
                    Frequency = 2,
                    YTM = rng.Next(2, 10) + rng.NextSingle()
                };
                bond.Price = BondCalc.ComputePrice(bond);
                yield return bond;
            }
        }
    }
}