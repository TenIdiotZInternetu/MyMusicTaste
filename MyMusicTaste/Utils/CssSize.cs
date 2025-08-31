using System.Diagnostics;

namespace MyMusicTaste.Utils;

// TODO: Make these into static types and make compile-time checks
public enum CssUnit {Zero, Px, Em, Rem, Vh, Vw, Perc}

public record struct CssSize(double Value, CssUnit Unit)
{
    public override string ToString()
    {
        string unit = Unit switch
        {
            CssUnit.Zero => "",
            CssUnit.Perc => "%",
            _ => Unit.ToString().ToLower()
        };

        return Value + unit;
    }
    
    public static CssSize operator+(CssSize size1, CssSize size2)
    {
        if (size1.Unit != size2.Unit)
        {
            throw new ArgumentException($"Units of the operands must match!");
        }
        return size1 with { Value = size1.Value + size2.Value };
    }

    public static CssSize operator *(CssSize size1, CssSize size2)
    {
        if (size1.Unit != size2.Unit)
        {
            throw new ArgumentException($"Units of the operands must match!");
        }
        return size1 with { Value = size1.Value * size2.Value };
    }
}