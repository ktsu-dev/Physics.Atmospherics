namespace ktsu.Physics.Thermodynamics;

using System.Numerics;
using ktsu.PhysicalQuantity;
using ktsu.PhysicalQuantity.Generic;
using ktsu.SignificantNumber;

/// <summary>
/// Represents a relative humidity quantity.
/// </summary>
[SIUnit("", " relative humidity ratio", " relative humidity ratio")]
public sealed record RelativeHumidity
	: PhysicalQuantity<RelativeHumidity>
{
}

/// <summary>
/// Provides extension methods for converting values to and from <see cref="RelativeHumidity"/>.
/// </summary>
public static class RelativeHumidityConversions
{
	private const double PercentToRatio = 0.01;

	/// <summary>
	/// Converts a numeric value to <see cref="RelativeHumidity"/>.
	/// </summary>
	/// <typeparam name="TNumber">The numeric type of the value.</typeparam>
	/// <param name="value">The value to convert.</param>
	/// <returns>An <see cref="RelativeHumidity"/> instance representing the specified value.</returns>
	public static RelativeHumidity RatioRelativeHumidity<TNumber>(this TNumber value)
		where TNumber : INumber<TNumber>
		=> value.ConvertToQuantity<TNumber, RelativeHumidity>();

	/// <summary>
	/// Converts an <see cref="RelativeHumidity"/> value to a numeric value.
	/// </summary>
	/// <typeparam name="TNumber">The numeric type of the result.</typeparam>
	/// <param name="value">The <see cref="RelativeHumidity"/> value to convert.</param>
	/// <returns>The numeric value representing the relative humidity.</returns>
	public static TNumber RatioRelativeHumidity<TNumber>(this RelativeHumidity value)
		where TNumber : INumber<TNumber>
		=> value.ConvertToNumber().To<TNumber>();

	/// <summary>
	/// Converts a numeric value to <see cref="RelativeHumidity"/>.
	/// </summary>
	/// <typeparam name="TNumber">The numeric type of the value.</typeparam>
	/// <param name="value">The value to convert.</param>
	/// <returns>An <see cref="RelativeHumidity"/> instance representing the specified value.</returns>
	public static RelativeHumidity PercentRelativeHumidity<TNumber>(this TNumber value)
		where TNumber : INumber<TNumber>
		=> value.ConvertToQuantity<TNumber, RelativeHumidity>(PercentToRatio.ToSignificantNumber());

	/// <summary>
	/// Converts an <see cref="RelativeHumidity"/> value to a numeric value.
	/// </summary>
	/// <typeparam name="TNumber">The numeric type of the result.</typeparam>
	/// <param name="value">The <see cref="RelativeHumidity"/> value to convert.</param>
	/// <returns>The numeric value representing the relative humidity.</returns>
	public static TNumber PercentRelativeHumidity<TNumber>(this RelativeHumidity value)
		where TNumber : INumber<TNumber>
		=> value.ConvertToNumber(PercentToRatio.ToSignificantNumber()).To<TNumber>();
}
