using Microsoft.ML;

namespace YTMCalcML.Core.MachineLearning.Common
{
    public interface IPredictorBase<TSrc, TDst>
        where TSrc : class
        where TDst : class, new()
    {
        ITransformer LoadModelFromFile(string modelPath);

        TDst Predict(TSrc input);
    }
}