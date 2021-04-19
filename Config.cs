using Exiled.API.Interfaces;
using System.ComponentModel;

namespace FFREVERSE
{
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("How much damage a player deals before they receive reverse friendly fire")]
        public float FFDamage { get; set; } = 180;

        [Description("How many times a player teamkills before they receive reverse friendly fire")]
        public int FFKills { get; set; } = 1;

        [Description("How many rounds reverse friendly fire lasts for")]
        public int FFRounds { get; set; } = 1;

        [Description("Count micro HID damage as friendly fire")]
        public bool FFMicro { get; set; } = false;

        [Description("Multiplier for FFdamage")]
        public double FFMultiplier { get; set; } = 0.8;
    }
}
