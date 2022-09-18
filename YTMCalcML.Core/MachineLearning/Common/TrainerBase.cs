using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;

namespace YTMCalcML.Core.MachineLearning.Common
{
    public abstract class TrainerBase<TModel> : ITrainerBase<TModel>
        where TModel : class
    {
        protected readonly MLContext MlContext;
        private DataOperationsCatalog.TrainTestData _dataSplit;
        private ITransformer? _trainedModel;

        public TrainerBase(string name)
        {
            Name = name;
            MlContext = new MLContext();
        }

        public string Name { get; private set; }

        public void Fit(string trainingDataFilename)
        {
            if (!File.Exists(trainingDataFilename))
            {
                throw new FileNotFoundException($"The file {trainingDataFilename} was not found.");
            }

            _dataSplit = LoadDataFromFile(trainingDataFilename);
            var dataProcessPipeline = BuildDataProcessingPipeline();
            var model = GetTrainer();

            if (dataProcessPipeline is null)
            {
                throw new InvalidOperationException($"Data processing pipeline is not set.");
            }
            if (model is null)
            {
                throw new InvalidOperationException($"Model trainer is not set.");
            }
            var trainingPipeline = dataProcessPipeline.Append(model);

            _trainedModel = trainingPipeline.Fit(_dataSplit.TrainSet);
        }

        public RegressionMetrics Evaluate()
        {
            if (_dataSplit.TestSet is null)
            {
                throw new InvalidOperationException($"Test dataset must be loaded before evaluation.");
            }
            if (_trainedModel is null)
            {
                throw new InvalidOperationException($"Model must be trained before evaluation.");
            }
            var testSetTransform = _trainedModel.Transform(_dataSplit.TestSet);
            return MlContext.Regression.Evaluate(testSetTransform);
        }

        public void Save(string modelPath)
        {
            if (_dataSplit.TrainSet is null)
            {
                throw new InvalidOperationException($"Training dataset must be loaded before saving.");
            }
            if (_trainedModel is null)
            {
                throw new InvalidOperationException($"Model must be trained before saving.");
            }
            MlContext.Model.Save(_trainedModel, _dataSplit.TrainSet.Schema, modelPath);
        }

        public virtual DataOperationsCatalog.TrainTestData LoadDataFromFile(string fileName)
        {
            var trainingDataView = MlContext.Data.LoadFromTextFile<TModel>
                      (fileName, hasHeader: true, separatorChar: ',');
            return MlContext.Data.TrainTestSplit(trainingDataView, 0.3);
        }

        protected abstract ITrainerEstimator<RegressionPredictionTransformer<LinearRegressionModelParameters>, LinearRegressionModelParameters> GetTrainer();

        protected abstract EstimatorChain<NormalizingTransformer> BuildDataProcessingPipeline();
    }
}