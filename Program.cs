using System.Diagnostics;
using NonUniformCouponCollectorProblem;

const int runCount = 50000;
const int n = 1000;
var threadCount = Environment.ProcessorCount;

var weights = PrepareWeights();
var sw = Stopwatch.StartNew();
var expectation = Runner.GetExpectation(n, runCount, threadCount, weights);
Console.WriteLine(sw.Elapsed);

Console.WriteLine(expectation);

static double[] PrepareWeights()
{
    var weights = new double[n];
    weights[0] = 1;
    for (var i = 1; i < weights.Length; ++i)
    {
        weights[i] = 1;
    }
    return weights;
}