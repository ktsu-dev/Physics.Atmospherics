// Ignore Spelling: geopotential

namespace ktsu.io.Physics.Atmospherics;

using ktsu.io.PhysicalQuantity.Density;
using ktsu.io.PhysicalQuantity.Length;
using ktsu.io.PhysicalQuantity.MolarMass;
using ktsu.io.PhysicalQuantity.Pressure;
using ktsu.io.PhysicalQuantity.Temperature;
using ktsu.io.Physics.Earth;
using ktsu.io.Physics.Thermodynamics;
using ktsu.io.SignificantNumber;

public static class Atmospherics
{
	// Nominal values for International Standard Atmosphere
	// https://www.iso.org/standard/7472.html - ISO 2533:1975
	public static Pressure PressureAtSeaLevel { get; } = 101325.Pascals();
	public static Temperature TemperatureAtSeaLevel { get; } = 15.Celsius();
	public static Density DensityAtSeaLevel { get; } = 1.225.KilogramsPerCubicMeter();
	public static RelativeHumidity HumidityAtSeaLevel { get; } = 0.PercentRelativeHumidity();
	public static TemperatureLapse TemperatureLapse { get; } = 0.0065.KelvinsPerMeter();
	public static MolarMass MolarMassOfDryAir { get; } = 0.02896968.KilogramsPerMole();
	public static MolarMass MolarMassOfWaterVapor { get; } = 18.01528.GramsPerMole();
	public static SpecificHeatCapacity SpecificGasConstantOfDryAir { get; } = 287.0528.JoulesPerKelvinPerKilogram();
	public static SpecificHeatCapacity SpecificGasConstantOfHumidAir { get; } = 461.495.JoulesPerKelvinPerKilogram();

	public static Pressure SaturatedVaporPressure(Temperature temperature)
	{
		ArgumentNullException.ThrowIfNull(temperature);
		var temperatureC = temperature.Celsius<SignificantNumber>();
		return (611.21.ToSignificantNumber() * SignificantNumber.Exp((18.678.ToSignificantNumber() - (temperatureC / 234.5.ToSignificantNumber())) * (temperatureC / (257.14.ToSignificantNumber() + temperatureC)))).Pascals();
	}

	public static Temperature TemperatureAtAltitude(Length geometricAltitude)
	{
		ArgumentNullException.ThrowIfNull(geometricAltitude);
		var geopotentialAltitude = Earth.GeopotentialAltitude(geometricAltitude);
		return TemperatureAtSeaLevel - (TemperatureLapse * geopotentialAltitude);

	}

	public static Pressure PressureAtAltitude(Length geometricAltitude)
	{
		ArgumentNullException.ThrowIfNull(geometricAltitude);
		return BarometricFormula(geometricAltitude);
	}

	public static Pressure BarometricFormula(Length geometricAltitude)
	{
		ArgumentNullException.ThrowIfNull(geometricAltitude);

		double p = PressureAtSeaLevel;
		double t = TemperatureAtSeaLevel;
		double g = Earth.GravityAtSeaLevel.ToMetersPerSecondSquared();
		double l = TemperatureLapse;
		double r = Constants.MolarGasConstantJoulesPerMoleKelvin.ToDouble();
		double m = MolarMassOfDryAir.ToKilogramsPerMole();
		double h = geometricAltitude.ToMeters();

		var lOnT = TemperatureLapse / TemperatureAtSeaLevel;
		double gm = Earth.GravityAtSeaLevel * MolarMassOfDryAir;
		double rl = r * l;
		double gmOnRl = gm / rl;
		double lOnTH = lOnT * h;
		double oneMinusLOnTH = 1 - lOnTH;

		return PressureAtSeaLevel * Math.Pow(oneMinusLOnTH, gmOnRl);
	}

	public static Density DensityAtAltitude(Length geometricAltitude)
	{
		ArgumentNullException.ThrowIfNull(geometricAltitude, nameof(geometricAltitude));
		var pressure = PressureAtAltitude(geometricAltitude);
		var temperature = TemperatureAtAltitude(geometricAltitude);
		return Density.FromKilogramsPerCubicMeter(pressure.ToPascals() / (SpecificGasConstantOfDryAir.ToDouble() * temperature.ToKelvin()));
	}
}
