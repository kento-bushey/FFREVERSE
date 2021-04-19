using Exiled.API.Enums;

namespace FFREVERSE
{
    class PlayerInfo
    {
        public int teamKills;
        public float teamDamage;
        public Exiled.API.Enums.Side lastSide;
        public PlayerInfo()
        {
            teamKills = 0;
            teamDamage = 0;
        }
    }
}