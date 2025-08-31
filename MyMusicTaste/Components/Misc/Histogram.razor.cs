using System.Numerics;
using Microsoft.AspNetCore.Components;
using MyMusicTaste.Utils;

namespace MyMusicTaste.Components.Misc;

public partial class Histogram<TValue, TLabel> : ComponentBase
    where TValue : INumber<TValue>
{
    [Parameter] public TValue[] Values { get; set; } = null!;
    [Parameter] public TLabel[] Labels { get; set; } = null!;
    [Parameter] public CssSize MaxHeight { get; set; }

    private double[] _values = [];
    private double _maxValue;
    private CssSize[] _heights = [];

    protected override void OnInitialized()
    {
        _values = Values.Select(double.CreateChecked).ToArray();
        _maxValue = _values.Max()!;
        _heights = _values.Select(val => val / _maxValue * MaxHeight)
                          .ToArray();
    }
}