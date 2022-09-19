using Microsoft.ML.Data;

namespace YTMCalcML.TestClient
{
    public static class ConsoleHelper
    {
        public static void PrintPredictionVsActual(string prediction, string actual, string predictedValueLabel = "", string actualValueLabel = "")
        {
            Console.WriteLine($"*************************************************");
            Console.WriteLine($"Predicted {predictedValueLabel}: {prediction}");
            Console.WriteLine($"Actual {actualValueLabel}: {actual}");
            Console.WriteLine($"*************************************************");
        }

        public static void PrintRegressionMetrics(string name, RegressionMetrics metrics)
        {
            Console.WriteLine($"*************************************************");
            Console.WriteLine($"*       Metrics for {name} regression model      ");
            Console.WriteLine($"*------------------------------------------------");
            Console.WriteLine($"*       LossFn:        {metrics.LossFunction:0.##}");
            Console.WriteLine($"*       R2 Score:      {metrics.RSquared:0.##}");
            Console.WriteLine($"*       Absolute loss: {metrics.MeanAbsoluteError:#.##}");
            Console.WriteLine($"*       Squared loss:  {metrics.MeanSquaredError:#.##}");
            Console.WriteLine($"*       RMS loss:      {metrics.RootMeanSquaredError:#.##}");
            Console.WriteLine($"*************************************************");
        }
    }
}