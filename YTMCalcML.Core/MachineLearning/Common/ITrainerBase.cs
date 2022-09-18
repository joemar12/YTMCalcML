using Microsoft.ML;
using Microsoft.ML.Data;

namespace YTMCalcML.Core.MachineLearning.Common
{
    public interface ITrainerBase<TModel> where TModel : class
    {
        string Name { get; }

        RegressionMetrics Evaluate();

        void Fit(string trainingDataFilename);

        DataOperationsCatalog.TrainTestData LoadDataFromFile(string fileName);

        void Save(string modelPath);
    }
}