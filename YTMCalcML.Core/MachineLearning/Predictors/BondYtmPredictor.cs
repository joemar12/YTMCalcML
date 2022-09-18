using YTMCalcML.Core.MachineLearning.Common;
using YTMCalcML.Core.MachineLearning.DataStructures;

namespace YTMCalcML.Core.MachineLearning.Predictors
{
    public class BondYtmPredictor : PredictorBase<Bond, YtmPrediction>
    {
        public BondYtmPredictor(string modelPath) : base(modelPath)
        {
        }
    }
}