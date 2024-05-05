using BepInEx.Configuration;
using System;

namespace CurrencyChanger2
{
    public class PriceRounder
    {
        public enum RoundingModeType { ROUND_UP, ROUND_DOWN, AUTOMATIC }
        public ConfigEntry<float> RoundingPoint { get;set; }
        public ConfigEntry<RoundingModeType> RoundingMode { get;set; }
        public PriceRounder(ConfigFile Config)
        {
            RoundingPoint = Config.Bind("Price Rounding", "Rounding Point", 0.01f, "Does your currency not have denominations for some small values?\nAdjust this to define the smallest possible denomination, and have all prices adjust to that.");
            RoundingMode = Config.Bind("Price Rounding", "Rounding Mode", RoundingModeType.AUTOMATIC, "What should happen if a price does not match the indicated rounding point?");
        }
        public float Round(float price)
        {
            float value = price / RoundingPoint.Value;
            switch(RoundingMode.Value)
            {
                default:
                case RoundingModeType.AUTOMATIC: value = (float)Math.Round(value); break;
                case RoundingModeType.ROUND_UP: value = (float)Math.Ceiling(value); break;
                case RoundingModeType.ROUND_DOWN: value = (float)Math.Floor(value); break;
            }
            value *= RoundingPoint.Value;
            return value;
        }
    }
}
