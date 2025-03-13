using NetFwTypeLib;

namespace ValveServerPicker
{
	internal static class Firewall
	{
		private const string SDR_RULE_GROUP = "ValveServerPicker";
		private const string SDR_RULE_NAME = "ValveServerPicker_SDR_";
		private static readonly List<INetFwRule> managedRules = [];

		private static INetFwPolicy2? _fwPolicy2;
		private static INetFwPolicy2 FirewallPolicy
		{
			get
			{
				if (_fwPolicy2 == null)
				{
					Type tNetFwPolicy2 = Type.GetTypeFromProgID("HNetCfg.FwPolicy2") ?? throw new Exception("Failed to get type for HNetCfg.FwPolicy2");
					_fwPolicy2 = Activator.CreateInstance(tNetFwPolicy2) as INetFwPolicy2 ?? throw new Exception("Failed to create INetFwPolicy2 instance");
				}

				return _fwPolicy2;
			}
		}

		public static bool UpdateSDRRuleList()
		{
			INetFwRules rules = FirewallPolicy.Rules;

			managedRules.Clear();

			managedRules.Capacity = rules.Count;

			foreach (INetFwRule rule in rules)
			{
				if (rule.Grouping == SDR_RULE_GROUP)
				{
					managedRules.Add(rule);
				}
			}

			managedRules.TrimExcess();

			return true;
		}

		public static bool TryGetSDRRule(SteamSDR sdr, out INetFwRule retRule)
		{
			string[] aliases = sdr.Aliases;

			retRule = default!;

			foreach (INetFwRule rule in managedRules)
			{
				foreach (string alias in aliases)
				{
					if (rule.Name == $"{SDR_RULE_NAME}{alias}")
					{
						retRule = rule;
						return true;
					}
				}
			}

			return false;
		}

		private static bool DeleteSDRRule(SteamSDR sdr)
		{
			if (TryGetSDRRule(sdr, out INetFwRule rule))
			{
				FirewallPolicy.Rules.Remove(rule.Name);
				return true;
			}
			return false;
		}

		public static bool DeleteAllSDRRules()
		{
			foreach (INetFwRule rule in FirewallPolicy.Rules)
			{
				if (rule.Grouping == SDR_RULE_GROUP)
				{
					FirewallPolicy.Rules.Remove(rule.Name);
				}
			}

			managedRules.Clear();
			return true;
		}

		public static void UpdateSDRRule(SteamSDR sdr, bool shouldBeBlocked)
		{
			// Delete the rule
			DeleteSDRRule(sdr);

			// No need to add a rule if there are no relays or if it shouldn't be blocked.
			if (!shouldBeBlocked || sdr.Relays == null || sdr.Relays.Count == 0)
			{
				return;
			}

			INetFwRule rule;
			Type ruleType = Type.GetTypeFromProgID("HNetCfg.FWRule") ?? throw new Exception("Failed to get type for HNetCfg.FWRule");
			rule = Activator.CreateInstance(ruleType) as INetFwRule ?? throw new Exception("Failed to create INetFwRule instance");

			rule.Grouping = SDR_RULE_GROUP;

			rule.Name = $"{SDR_RULE_NAME}{sdr.Aliases[0]}";

			rule.Description = $"ValveServerPicker rule for '{sdr.Desc}' ({sdr.AliasesString})";

			rule.Protocol = (int)NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_UDP;

			List<string> addresses = [];
			List<string> ports = [];
			foreach (SteamRelay relay in sdr.Relays!)
			{
				addresses.Add(relay.IPv4);

				string portStr = $"{relay.PortRange.Low}-{relay.PortRange.High}";
				if (!ports.Contains(portStr))
				{
					ports.Add(portStr);
				}
			}

			rule.RemoteAddresses = string.Join(",", addresses);

			rule.RemotePorts = string.Join(",", ports);

			rule.Action = NET_FW_ACTION_.NET_FW_ACTION_BLOCK;

			rule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT;

			rule.Profiles = FirewallPolicy.CurrentProfileTypes;

			rule.Enabled = shouldBeBlocked;

			FirewallPolicy.Rules.Add(rule);
			managedRules.Add(rule);
		}
	}
}
