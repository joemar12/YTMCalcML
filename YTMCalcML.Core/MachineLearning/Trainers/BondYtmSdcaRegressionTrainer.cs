using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;
using YTMCalcML.Core.MachineLearning.Common;
using YTMCalcML.Core.MachineLearning.DataStructures;

namespace YTMCalcML.Core.MachineLearning.Trainers
{
    public class BondYtmSdcaRegressionTrainer : TrainerBase<Bond>
    {
        public BondYtmSdcaRegressionTrainer() : base("Bond YTM SDCA Regression Trainer")
        {
        }

        protected override EstimatorChain<NormalizingTransformer> BuildDataProcessingPipeline()
        {
            var dataProcessPipeline = MlContext.Transforms
              .CopyColumns("Label", nameof(Bond.YTM))
                .Append(MlContext.Transforms.Concatenate("Features",
                                                nameof(Bond.Price),
                                                nameof(Bond.CouponRate),
                                                nameof(Bond.Redemption),
                                                nameof(Bond.Term),
                                                nameof(Bond.Frequency)))
                .Append(MlContext.Transforms.NormalizeLogMeanVariance("Features", "Features"))
                .AppendCacheCheckpoint(MlContext);

            return dataProcessPipeline;
        }

        protected override ITrainerEstimator<RegressionPredictionTransformer<LinearRegressionModelParameters>, LinearRegressionModelParameters> GetTrainer()
        {
            return MlContext.Regression.Trainers.Sdca(labelColumnName: "Label", featureColumnName: "Features");
        }
    }
}