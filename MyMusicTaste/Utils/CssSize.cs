namespace MyMusicTaste.Utils;

// TODO: Make these into static types and make compile-time checks
/// <summary>
/// Represents supported CSS size units.
/// </summary>
public enum CssUnit {Zero, Px, Em, Rem, Vh, Vw, Perc}

/// <summary>
/// Represents a CSS size consisting of a numeric value and a unit.
/// /// </summary>
/// <param name="Value">The numeric value of the size.</param>
/// <param name="Unit">The CSS unit of the size.</param>
public readonly record struct CssSize(double Value, CssUnit Unit)
{
    /// <summary>
    /// Converts the size to a CSS value (e.g. "10px", "50%").
    /// </summary>
    /// <returns>A string representation of the CSS value.</returns>
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

    /// <summary>
    /// Adds a number to the size value.
    /// </summary>
    public static CssSize operator+(CssSize size, double summator)
    {
        return size with { Value = size.Value + summator };
    }

    /// <summary>
    /// Adds a number to the size value.
    /// </summary>
    public static CssSize operator+(double summator, CssSize size) => size + summator;
    
    /// <summary>
    /// Adds two sizes with the same unit.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when units do not match.</exception>
    public static CssSize operator+(CssSize size1, CssSize size2)
    {
        if (size1.Unit != size2.Unit)
        {
            throw new ArgumentException($"Units of the operands must match!");
        }
        return size1 with { Value = size1.Value + size2.Value };
    }

    /// <summary>
    /// Multiplies the size value by a number.
    /// </summary>
    public static CssSize operator*(CssSize size, double factor)
    {
        return size with { Value = size.Value * factor };
    }

    /// <summary>
    /// Multiplies the size value by a number.
    /// </summary>
    public static CssSize operator*(double factor, CssSize size) => size * factor;

    /// <summary>
    /// Multiplies two sizes with the same unit.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when units do not match.</exception>
    public static CssSize operator*(CssSize size1, CssSize size2)
    {
        if (size1.Unit != size2.Unit)
        {
            throw new ArgumentException($"Units of the operands must match!");
        }
        return size1 with { Value = size1.Value * size2.Value };
    }
}