// Copyright (c) ktsu.dev
// All rights reserved.
// Licensed under the MIT license.

namespace ktsu.Physics.Atmospherics;

using ktsu.PhysicalQuantity.Density;
using ktsu.PhysicalQuantity.Length;
using ktsu.PhysicalQuantity.MolarMass;
using ktsu.PhysicalQuantity.Pressure;
using ktsu.PhysicalQuantity.Temperature;
using ktsu.Physics.Constants;
using ktsu.Physics.Earth;
using ktsu.Physics.Thermodynamics;
using ktsu.SignificantNumber;

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

		var p = PressureAtSeaLevel;
		var t = TemperatureAtSeaLevel;
		var g = Earth.GravityAtSeaLevel;
		var l = TemperatureLapse;
		var r = Constants.MolarGasConstantJoulesPerMoleKelvin;
		var m = MolarMassOfDryAir;
		var h = geometricAltitude;

		var lOnT = l / t;
		double gm = g * m;
		double rl = r * l;
		var gmOnRl = gm / rl;
		double lOnTH = lOnT * h;
		var oneMinusLOnTH = 1 - lOnTH;

		return p * Math.Pow(oneMinusLOnTH, gmOnRl);
	}

	public static Density DensityAtAltitude(Length geometricAltitude)
	{
		ArgumentNullException.ThrowIfNull(geometricAltitude, nameof(geometricAltitude));
		var pressure = PressureAtAltitude(geometricAltitude);
		var temperature = TemperatureAtAltitude(geometricAltitude);
		return (pressure / (SpecificGasConstantOfDryAir * temperature)).KilogramsPerCubicMeter();
	}
}
