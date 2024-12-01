namespace ktsu.Physics.Thermodynamics;

using System.Numerics;
using ktsu.PhysicalQuantity;
using ktsu.PhysicalQuantity.Generic;
using ktsu.PhysicalQuantity.Length;
using ktsu.PhysicalQuantity.Temperature;

/// <summary>
/// Represents a temperature lapse quantity measured in kelvins per meter.
/// </summary>
[SIUnit("K/m", "kelvin per meter", "kelvins per meter")]
public sealed record TemperatureLapse
	: PhysicalQuantity<TemperatureLapse>
	, IIntegralOperators<TemperatureLapse, Length, Temperature>
{
	public static Temperature operator *(TemperatureLapse left, Length right) =>
		IIntegralOperators<TemperatureLapse, Length, Temperature>.Integrate(left, right);
}

/// <summary>
/// Provides extension methods for converting values to and from <see cref="TemperatureLapse"/>.
/// </summary>
public static class TemperatureLapseConversions
{
	/// <summary>
	/// Converts a numeric value to <see cref="TemperatureLapse"/> measured in kelvins per meter.
	/// </summary>
	/// <typeparam name="TNumber">The numeric type of the value.</typeparam>
	/// <param name="value">The value to convert.</param>
	/// <returns>An <see cref="TemperatureLapse"/> instance representing the specified value in kelvins per meter.</returns>
	public static TemperatureLapse KelvinsPerMeter<TNumber>(this TNumber value)
		where TNumber : INumber<TNumber>
		=> value.ConvertToQuantity<TNumber, TemperatureLapse>();

	/// <summary>
	/// Converts an <see cref="TemperatureLapse"/> value to a numeric value measured in kelvins per meter.
	/// </summary>
	/// <typeparam name="TNumber">The numeric type of the result.</typeparam>
	/// <param name="value">The <see cref="TemperatureLapse"/> value to convert.</param>
	/// <returns>The numeric value representing the temperature lapse in kelvins per meter.</returns>
	public static TNumber KelvinsPerMeter<TNumber>(this TemperatureLapse value)
		where TNumber : INumber<TNumber>
		=> value.ConvertToNumber().To<TNumber>();
}
