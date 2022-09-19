# YTMCalcML

An attempt to use Machine Learning to predict Bond Yield to Maturity (YTM) instead of the approximation method. Currently the model still needs tweaking to achieve higher accuracy, though.

### Training data

To generate training data, a set of random parameters (par value, annual coupon rate, term in years, coupon payment frequency, and YTM) are fed to a price calculator function and the resulting price is set to the same input model. A list of such objects is then be dumped into a csv file for training. During training, the YTM will be the `Label` and the other properties of the Bond object will be the `Features`.

### Utilities

There are some utility classes provided to help generate training data under the namespace `YTMCalcML.Core.Utils`

### Test Console Client

There's a privided console client to tweak and test the ML model.
