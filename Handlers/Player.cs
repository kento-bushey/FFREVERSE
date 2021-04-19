using System.Collections.Generic;
using Exiled.Events.EventArgs;
using Exiled.API.Features;
using Exiled.API.Enums;
using System;
using MEC;

namespace FFREVERSE.Handlers
{
	class Player
	{
		public static Dictionary<string, PlayerInfo> PlayerInfoDict = new Dictionary<string, PlayerInfo>();
		int rounds = 0;
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
			rounds += 1;
			if (rounds > FFREVERSE.Instance.Config.FFRounds)
            {
				foreach (Exiled.API.Features.Player p in Exiled.API.Features.Player.List)
				{
					PlayerInfo pinfo = PlayerInfoDict[p.UserId];
					pinfo.teamDamage = 0;
					pinfo.teamKills = 0;
				}
			}
		}
		public void OnJoin(VerifiedEventArgs ev)
		{
			if (ev.Player != null)
            {
				addPlayer(ev.Player);
			}
		}
		public void OnKills(DyingEventArgs ev)
		{
			
			if (ev.HitInformation.GetDamageType() == DamageTypes.MicroHid && !FFREVERSE.Instance.Config.FFMicro)
			{
				return;
			}
			
			if (ev.Killer != null && ev.Killer.UserId != ev.Target.UserId && Round.IsStarted)
			{
				PlayerInfo pinfo = PlayerInfoDict[ev.Killer.UserId];
				Exiled.API.Enums.Side aTeam = pinfo.lastSide;
				Exiled.API.Enums.Side tTeam = PlayerInfoDict[ev.Target.UserId].lastSide;
				if (aTeam == tTeam)
				{
					pinfo.teamKills++;
				}
			}

		}
		public void OnSpawning(SpawningEventArgs ev)
        {
			PlayerInfo pinfo = PlayerInfoDict[ev.Player.UserId];
			pinfo.lastSide = ev.Player.Side;
		}
		public void OnDamage(HurtingEventArgs ev)
		{
			if (!PlayerInfoDict.ContainsKey(ev.Attacker.UserId))
			{
				return;
			}
			
			if (ev.DamageType == DamageTypes.MicroHid && !FFREVERSE.Instance.Config.FFMicro)
            {
				return;
            }
			PlayerInfo pinfo = PlayerInfoDict[ev.Attacker.UserId];
			Exiled.API.Enums.Side aTeam = pinfo.lastSide;
			Exiled.API.Enums.Side tTeam = PlayerInfoDict[ev.Target.UserId].lastSide;
			if (aTeam == tTeam && ev.Attacker.Id != ev.Target.Id && Round.IsStarted)
			{
				double dmg = ev.Amount;
				double multiplier = FFREVERSE.Instance.Config.FFMultiplier;
				if (pinfo.teamKills >= FFREVERSE.Instance.Config.FFKills || pinfo.teamDamage >= FFREVERSE.Instance.Config.FFDamage)
				{
					ev.Attacker.ShowHint("<size=35><b><color=#F52929>Инверсия \"огня по своим\" включена</color></b></size> \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n", 5);
					if (ev.DamageType == DamageTypes.Grenade)
                    {
						Timing.CallDelayed(0.4f, () => ev.Attacker.Hurt((float)(dmg * multiplier), ev.DamageType));
                    }
                    else
                    {
						ev.Attacker.Hurt((float)(dmg * multiplier), ev.DamageType/**ev.Attacker?.Nickname,ev.Attacker.Id**/);
					}
					ev.Attacker.EnableEffect("Bleeding", 1);
					ev.IsAllowed = false;
                }
                else
                {
					if (pinfo.teamDamage>50)
                    {
						ev.Attacker.ShowHint("<size=35><color=#fad106>Не наносите урон своим союзникам</color></size> \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n", 3);
					}
					pinfo.teamDamage += (float)(dmg * multiplier);
				}
			}
		}
	}
}
