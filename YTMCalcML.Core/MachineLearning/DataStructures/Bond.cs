using Microsoft.ML.Data;

namespace YTMCalcML.Core.MachineLearning.DataStructures
{
    public class Bond
    {
        [LoadColumn(0)]
        public float Price { get; set; }

        [LoadColumn(1)]
        public float CouponRate { get; set; }

        [LoadColumn(2)]
        public float Redemption { get; set; }

        [LoadColumn(3)]
        public float Term { get; set; }

        [LoadColumn(4)]
        public float Frequency { get; set; }

        [LoadColumn(5)]
        public float YTM { get; set; }
    }
}