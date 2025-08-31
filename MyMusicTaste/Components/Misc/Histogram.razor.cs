using System.Numerics;
using Microsoft.AspNetCore.Components;
using MyMusicTaste.Utils;

namespace MyMusicTaste.Components.Misc;

public partial class Histogram<TLabel> : ComponentBase
{
    [Parameter] public double[] Values { get; set; } = null!;
    [Parameter] public TLabel[] Labels { get; set; } = null!;
    [Parameter] public CssSize MaxHeight { get; set; }

    private double _maxValue;
    private CssSize[] _heights;

    protected override void OnInitialized()
    {
        _maxValue = Values.Max()!;
        _heights = Values.Select(val => val / _maxValue * MaxHeight)
                         .ToArray();
    }
}