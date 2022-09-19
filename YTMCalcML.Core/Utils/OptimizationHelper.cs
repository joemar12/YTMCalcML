namespace YTMCalcML.Core.Utils
{
    public static class OptimizationHelper
    {
        /// <summary>
        /// Find the minima of the function using Newton's optimization method
        /// </summary>
        public static float NewtonMethod(Func<float, float> f, float initVal, float epsilon = 1e-10f, int maxIteration = 100)
        {
            var h = epsilon;
            var h2 = epsilon * 2f;
            
            var xn = initVal;
            float dFxn;
            float fxn;
            for (int i = 1; i <= maxIteration; i++)
            {
                fxn = f(xn);
                if (Math.Abs(fxn) < epsilon)
                {
                    Console.WriteLine($"iteration {i} - fxn: {fxn} - xn: {xn}");
                    return xn;
                }
                //a numeric differentiation for f(x)
                //[f(x-2h) - 8f(x-h) + 8f(x+h) - f(x+2h)] / 12h
                var f1 = f(xn - h2);
                var f2 = 8 * f(xn - h);
                var f3 = 8 * f(xn + h);
                var f4 = f(xn + h2);
                dFxn = (f1 - f2 + f3 - f4) / (6 * h2);
                if (dFxn == 0)
                {
                    //no solution found.
                    break;
                }
                Console.WriteLine($"iteration {i} - fxn: {fxn} - dfxn: {dFxn} - xn: {xn}");
                //newton's method formula
                xn -= (fxn / dFxn);
            }
            return xn;
        }
    }
}