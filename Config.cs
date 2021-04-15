using System;
using Exiled.API.Interfaces;
using System.ComponentModel;

namespace FFREVERSE
{
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("How much damage a player deals before they receive reverse friendly fire")]
        public float FFDamage = 100;

        [Description("How much times a player teamkills before they receive reverse friendly fire")]
        public int FFKills = 1;
    }
}
