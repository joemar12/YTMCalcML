using Microsoft.ML;

namespace YTMCalcML.Core.MachineLearning.Common
{
    public abstract class PredictorBase<TSrc, TDst> : IPredictorBase<TSrc, TDst>
        where TSrc : class
        where TDst : class, new()
    {
        protected readonly MLContext MlContext;
        private readonly string _modelPath;

        public PredictorBase(string modelPath)
        {
            MlContext = new MLContext();
            _modelPath = modelPath;
        }

        public TDst Predict(TSrc input)
        {
            var model = LoadModelFromFile(_modelPath);

            var predictionEngine = MlContext.Model.CreatePredictionEngine<TSrc, TDst>(model);

            return predictionEngine.Predict(input);
        }

        public virtual ITransformer LoadModelFromFile(string modelPath)
        {
            if (!File.Exists(modelPath))
            {
                throw new FileNotFoundException($"The file {modelPath} was not found.");
            }

            using (var stream = new FileStream(_modelPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return MlContext.Model.Load(stream, out _);
            }
        }
    }
}