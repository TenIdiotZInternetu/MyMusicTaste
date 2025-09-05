using System.Numerics;
using Microsoft.AspNetCore.Components;
using MyMusicTaste.Utils;

namespace MyMusicTaste.Components.Misc;

/// <summary>
/// A generic component that renders a histogram chart based on numeric values and associated labels.
/// </summary>
/// <typeparam name="TValue">The numeric type of the values to display.</typeparam>
/// <typeparam name="TLabel">The type of labels for each value.</typeparam>
public partial class Histogram<TValue, TLabel> : ComponentBase
    where TValue : INumber<TValue>
{
    /// <summary>
    /// Array of numeric values to display in the histogram.
    /// </summary>
    [Parameter] public TValue[] Values { get; set; } = null!;
    
    /// <summary>
    /// Array of labels corresponding to the values.
    /// </summary>
    [Parameter] public TLabel[] Labels { get; set; } = null!;
    
    /// <summary>
    /// Maximum height of the histogram bars.
    /// </summary>
    [Parameter] public CssSize MaxHeight { get; set; }

    private double[] _values = [];
    private double _maxValue;
    private CssSize[] _heights = [];

    protected override void OnInitialized()
    {
        _values = Values.Select(double.CreateChecked).ToArray();
        _maxValue = _values.Max();
        _heights = _values.Select(val => val / _maxValue * MaxHeight)
                          .ToArray();
    }
}