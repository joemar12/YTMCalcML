
using System.Diagnostics;
using YTMCalcML.Core.MachineLearning.DataStructures;
using YTMCalcML.Core.MachineLearning.Predictors;
using YTMCalcML.Core.MachineLearning.Trainers;
using YTMCalcML.Core.Utils;
using YTMCalcML.TestClient;

var watch = new Stopwatch();
var sdcaModelPath = "sdcaModel.mdl";
var ogdModelPath = "ogdModel.mdl";
var trainingDataPath = "TrainingData//bond-prices.csv";

Console.Write("Do you want to generate new training data? (Y/N): ");
var answer = Console.ReadKey();
if (answer.Key == ConsoleKey.Y)
{
    Console.WriteLine();
    Console.Write($"Training data size: ");
    int.TryParse(Console.ReadLine(), out var size);
    var generator = new DataHelper();

    Console.WriteLine($"Generating training data...");
    watch.Start();
    var records = generator.GenerateBonds(size);
    FileHelper.WriteToFile(trainingDataPath, records);
    watch.Stop();
    Console.WriteLine($"Training data written to {trainingDataPath}");
    Console.WriteLine($"Elapsed secs: {watch.Elapsed.Seconds}...");
    Console.WriteLine();
}

Console.WriteLine();
Console.Write("Do you want to retrain the model with SDCA? (Y/N): ");
answer = Console.ReadKey();
if (answer.Key == ConsoleKey.Y)
{
    Console.WriteLine();

    var trainer = new BondYtmSdcaRegressionTrainer();

    Console.WriteLine($"Model training starts...");
    Console.WriteLine($"Fitting data from {trainingDataPath}");
    watch.Restart();
    trainer.Fit(trainingDataPath);
    watch.Stop();
    Console.WriteLine("Model training finished...");
    Console.WriteLine($"Elapsed secs: {watch.Elapsed.Seconds}...");
    Console.WriteLine();

    Console.WriteLine($"Evaluating model...");
    watch.Restart();
    var metrics = trainer.Evaluate();
    watch.Stop();
    ConsoleHelper.PrintRegressionMetrics(trainer.Name, metrics);
    Console.WriteLine($"Elapsed secs: {watch.Elapsed.Seconds}...");
    Console.WriteLine();

    Console.WriteLine($"Persisting model...");
    watch.Restart();
    trainer.Save(sdcaModelPath);
    watch.Stop();
    Console.WriteLine($"Model saved to {sdcaModelPath}");
    Console.WriteLine($"Elapsed secs: {watch.Elapsed.Seconds}...");
}

Console.WriteLine();
Console.Write("Do you want to retrain the model with OGD? (Y/N): ");
answer = Console.ReadKey();
if (answer.Key == ConsoleKey.Y)
{
    Console.WriteLine();

    var trainer = new BondYtmOgdRegressionTrainer();

    Console.WriteLine($"Model training starts...");
    Console.WriteLine($"Fitting data from {trainingDataPath}");
    watch.Restart();
    trainer.Fit(trainingDataPath);
    watch.Stop();
    Console.WriteLine("Model training finished...");
    Console.WriteLine($"Elapsed secs: {watch.Elapsed.Seconds}...");
    Console.WriteLine();

    Console.WriteLine($"Evaluating model...");
    watch.Restart();
    var metrics = trainer.Evaluate();
    watch.Stop();
    ConsoleHelper.PrintRegressionMetrics(trainer.Name, metrics);
    Console.WriteLine($"Elapsed secs: {watch.Elapsed.Seconds}...");
    Console.WriteLine();

    Console.WriteLine($"Persisting model...");
    watch.Restart();
    trainer.Save(ogdModelPath);
    watch.Stop();
    Console.WriteLine($"Model saved to {sdcaModelPath}");
    Console.WriteLine($"Elapsed secs: {watch.Elapsed.Seconds}...");
}

do
{
    Console.WriteLine();
    Console.WriteLine($"Enter new sample bond data for prediction: ");
    Console.Write($"Face Value: ");
    float.TryParse(Console.ReadLine(), out var redemption);
    Console.Write($"Current Price: ");
    float.TryParse(Console.ReadLine(), out var price);
    Console.Write($"Coupon Rate (%): ");
    float.TryParse(Console.ReadLine(), out var couponRate);
    Console.Write($"Term (Years): ");
    float.TryParse(Console.ReadLine(), out var term);
    Console.Write($"Coupon Payment Frequency: ");
    float.TryParse(Console.ReadLine(), out var frequency);
    //Console.Write($"Actual YTM: ");
    //float.TryParse(Console.ReadLine(), out var ytm);
    var sampleBond = new Bond()
    {
        Price = price,
        CouponRate = couponRate,
        FaceValue = redemption,
        Term = term,
        Frequency = frequency,
    };

    var sdcaPredictor = new BondYtmPredictor(sdcaModelPath);
    var ogdPredictor = new BondYtmPredictor(ogdModelPath);
    watch.Restart();
    var sdcaResult = sdcaPredictor.Predict(sampleBond);
    var ogdResult = ogdPredictor.Predict(sampleBond);
    watch.Stop();
    var actualYtm = BondCalc.ComputeYtm(sampleBond);
    sampleBond.YieldToMaturity = actualYtm;
    ConsoleHelper.PrintPredictionVsActual(sdcaResult.YTM.ToString("0.####"), actualYtm.ToString("0.####"), "YTM with SDCA", "YTM");
    ConsoleHelper.PrintPredictionVsActual(ogdResult.YTM.ToString("0.####"), actualYtm.ToString("0.####"),"YTM with OGD", "YTM");
    Console.WriteLine($"Elapsed secs: {watch.Elapsed.Seconds}...");
    Console.WriteLine();

    Console.WriteLine($"Computing price using actual YTM...");
    watch.Restart();
    var priceFromActualYtm = BondCalc.ComputePrice(sampleBond);
    watch.Stop();
    Console.WriteLine($"Computed Price: {priceFromActualYtm.ToString("0.####")}");
    Console.WriteLine($"Elapsed secs: {watch.Elapsed.Seconds}...");
    Console.WriteLine();

    Console.Write("Test another? (Y/N)");
} while (Console.ReadKey().Key == ConsoleKey.Y);

