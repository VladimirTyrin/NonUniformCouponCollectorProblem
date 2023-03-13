public class IndexPicker
{
    private readonly Random _random;
    private readonly List<double> _cumulativeNormalizedWeights;

    public IndexPicker(IReadOnlyCollection<double> weights, Random random)
    {
        _random = random;
        _cumulativeNormalizedWeights = new List<double>(weights.Count);
        var partialWeightSum = 0.0;
        
        foreach (var weight in weights)
        {
            partialWeightSum += weight;
            
            _cumulativeNormalizedWeights.Add(partialWeightSum);
        }

        var reverseWeightSum = 1.0 / partialWeightSum;
        for (var i = 0; i < _cumulativeNormalizedWeights.Count; ++i)
        {
            _cumulativeNormalizedWeights[i] *= reverseWeightSum;
        }
    }

    public int Pick()
    {
        var value = _random.NextDouble();

        var result = _cumulativeNormalizedWeights.BinarySearch(value);

        if (result < 0)
        {
            result = ~result;
        }

        if (result >= _cumulativeNormalizedWeights.Count)
        {
            return _cumulativeNormalizedWeights.Count - 1;
        }

        return result;
    }
}