using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.Events.EventArgs;
using Exiled.API.Features;
using Exiled.Events.Handlers;

namespace FFREVERSE.Handlers
{
    class Player
    {
		public static Dictionary<string, PlayerInfo> PlayerInfoDict = new Dictionary<string, PlayerInfo >();
		private void addPlayer(Exiled.API.Features.Player p)
        {
			if (!PlayerInfoDict.ContainsKey(p.UserId))
			{
				Exiled.API.Features.Log.Info("Player with userID:" + p.UserId + " added to dict.");
				PlayerInfoDict.Add(p.UserId, new PlayerInfo());
			}
		}
		private void updateDict()
        {
			IEnumerable<Exiled.API.Features.Player> PList = Exiled.API.Features.Player.List;
			foreach (Exiled.API.Features.Player p in PList)
			{
				addPlayer(p);
			}
		}
		public void OnRoundStart()
		{
			updateDict();
		}
		public void OnJoin(JoinedEventArgs ev)
        {
			addPlayer(ev.Player);
		}
		public void OnKills(DyingEventArgs ev)
		{
			if (ev.Killer != null)
            {
				if (PlayerInfoDict.ContainsKey(ev.Killer.UserId))
                {
					addPlayer(ev.Killer);
				}
				PlayerInfo pinfo = PlayerInfoDict[ev.Killer.UserId];
				if (ev.Killer.Role == ev.Target.Role)
                {
					pinfo.teamKills += 1;
                }
			}
			
		}
		public void OnDamage(HurtingEventArgs ev)
        {
			if (ev.Attacker != null)
            {
				if (PlayerInfoDict.ContainsKey(ev.Attacker.UserId))
				{
					addPlayer(ev.Attacker);
				}
				PlayerInfo pinfo = PlayerInfoDict[ev.Attacker.UserId];
				if (ev.Attacker.Role == ev.Target.Role)
				{
					if (pinfo.teamKills >= FFREVERSE.Instance.Config.FFKills || pinfo.teamDamage >= FFREVERSE.Instance.Config.FFDamage)
                    {
						ev.Attacker.Hurt(ev.Amount,ev.Attacker);
                    }
					pinfo.teamDamage += ev.Amount;
				}
			}
        }

	}
}
