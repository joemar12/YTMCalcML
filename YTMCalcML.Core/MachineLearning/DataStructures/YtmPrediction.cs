using Microsoft.ML.Data;

namespace YTMCalcML.Core.MachineLearning.DataStructures
{
    public class YtmPrediction
    {
        [ColumnName("Score")]
        public float YTM { get; set; }
    }
}