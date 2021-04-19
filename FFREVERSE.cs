using System;
using Exiled.API.Features;
using Exiled.API.Enums;
using Player = Exiled.Events.Handlers.Player;
using Server = Exiled.Events.Handlers.Server;

namespace FFREVERSE
{
    public class FFREVERSE : Plugin<Config>
    {
        public override PluginPriority Priority { get; } = PluginPriority.Low;
        private static readonly Lazy<FFREVERSE> LazyInstance = new Lazy<FFREVERSE>(() => new FFREVERSE());
        public static FFREVERSE Instance => LazyInstance.Value;
        private Handlers.Player player;
        public FFREVERSE()
        {

        }
        public override void OnEnabled()
        {
            RegisterEvents();
        }
        public override void OnDisabled()
        {
            UnRegisterEvents();
        }
        public void RegisterEvents()
        {
            player = new Handlers.Player();
            Server.RoundStarted += player.OnRoundStart;
            Player.Dying += player.OnKills;
            Player.Hurting += player.OnDamage;
            Player.Verified += player.OnJoin;
            Player.Spawning += player.OnSpawning;
        }
        public void UnRegisterEvents()
        {
            player = new Handlers.Player();
            Server.RoundStarted -= player.OnRoundStart;
            Player.Dying -= player.OnKills;
            Player.Hurting -= player.OnDamage;
            Player.Verified -= player.OnJoin;
            Player.Spawning -= player.OnSpawning;
            player = null;
        }
    }
}
