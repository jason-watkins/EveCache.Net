#region License
/* EveCache.Net - EVE Cache File Reader Library
 * Copyright (C) 2011 Jason Watkins <jason@blacksunsystems.net>
 *
 * Based on libevecache
 * Copyright (C) 2009-2010  StackFoundry LLC and Yann Ramin
 * http: * dev.eve-central.com/libevecache/
 * http: * gitorious.org/libevecache
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public
 * License as published by the Free Software Foundation; either
 * version 2 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * General Public License for more details.
 *
 * You should have received a copy of the GNU General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
 */
#endregion

namespace EveCache
{
	using System.Collections.Generic;

	public static class ColumnLookup
	{
		#region Fields
		private static Dictionary<string, byte> _IDLookup;
		private static Dictionary<byte, string> _NameLookup;
		#endregion Fields

		#region Properties
		private static Dictionary<string, byte> IDLookup { get { return _IDLookup; } set { _IDLookup = value; } }
		private static Dictionary<byte, string> NameLookup { get { return _NameLookup; } set { _NameLookup = value; } }
		#endregion Properties

		#region Constructors
		static ColumnLookup()
		{
			IDLookup = new Dictionary<string, byte>();
			NameLookup = new Dictionary<byte, string>();

			Insert("*corpid", 1);
			Insert("*locationid", 2);
			Insert("age", 3);
			Insert("Asteroid", 4);
			Insert("authentication", 5);
			Insert("ballID", 6);
			Insert("beyonce", 7);
			Insert("bloodlineID", 8);
			Insert("capacity", 9);
			Insert("categoryID", 10);
			Insert("character", 11);
			Insert("characterID", 12);
			Insert("characterName", 13);
			Insert("characterType", 14);
			Insert("charID", 15);
			Insert("chatx", 16);
			Insert("clientID", 17);
			Insert("config", 18);
			Insert("contraband", 19);
			Insert("corporationDateTime", 20);
			Insert("corporationID", 21);
			Insert("createDateTime", 22);
			Insert("customInfo", 23);
			Insert("description", 24);
			Insert("divisionID", 25);
			Insert("DoDestinyUpdate", 26);
			Insert("dogmaIM", 27);
			Insert("EVE System", 28);
			Insert("flag", 29);
			Insert("foo.SlimItem", 30);
			Insert("gangID", 31);
			Insert("Gemini", 32);
			Insert("gender", 33);
			Insert("graphicID", 34);
			Insert("groupID", 35);
			Insert("header", 36);
			Insert("idName", 37);
			Insert("invbroker", 38);
			Insert("itemID", 39);
			Insert("items", 40);
			Insert("jumps", 41);
			Insert("line", 42);
			Insert("lines", 43);
			Insert("locationID", 44);
			Insert("locationName", 45);
			Insert("macho.CallReq", 46);
			Insert("macho.CallRsp", 47);
			Insert("macho.MachoAddress", 48);
			Insert("macho.Notification", 49);
			Insert("macho.SessionChangeNotification", 50);
			Insert("modules", 51);
			Insert("name", 52);
			Insert("objectCaching", 53);
			Insert("objectCaching.CachedObject", 54);
			Insert("OnChatJoin", 55);
			Insert("OnChatLeave", 56);
			Insert("OnChatSpeak", 57);
			Insert("OnGodmaShipEffect", 58);
			Insert("OnItemChange", 59);
			Insert("OnModuleAttributeChange", 60);
			Insert("OnMultiEvent", 61);
			Insert("orbitID", 62);
			Insert("ownerID", 63);
			Insert("ownerName", 64);
			Insert("quantity", 65);
			Insert("raceID", 66);
			Insert("RowClass", 67);
			Insert("securityStatus", 68);
			Insert("Sentry Gun", 69);
			Insert("sessionchange", 70);
			Insert("singleton", 71);
			Insert("skillEffect", 72);
			Insert("squadronID", 73);
			Insert("typeID", 74);
			Insert("used", 75);
			Insert("userID", 76);
			Insert("util.CachedObject", 77);
			Insert("util.IndexRowset", 78);
			Insert("util.Moniker", 79);
			Insert("util.Row", 80);
			Insert("util.Rowset", 81);
			Insert("*multicastID", 82);
			Insert("AddBalls", 83);
			Insert("AttackHit3", 84);
			Insert("AttackHit3R", 85);
			Insert("AttackHit4R", 86);
			Insert("DoDestinyUpdates", 87);
			Insert("GetLocationsEx", 88);
			Insert("InvalidateCachedObjects", 89);
			Insert("JoinChannel", 90);
			Insert("LSC", 91);
			Insert("LaunchMissile", 92);
			Insert("LeaveChannel", 93);
			Insert("OID+", 94);
			Insert("OID-", 95);
			Insert("OnAggressionChange", 96);
			Insert("OnCharGangChange", 97);
			Insert("OnCharNoLongerInStation", 98);
			Insert("OnCharNowInStation", 99);
			Insert("OnDamageMessage", 100);
			Insert("OnDamageStateChange", 101);
			Insert("OnEffectHit", 102);
			Insert("OnGangDamageStateChange", 103);
			Insert("OnLSC", 104);
			Insert("OnSpecialFX", 105);
			Insert("OnTarget", 106);
			Insert("RemoveBalls", 107);
			Insert("SendMessage", 108);
			Insert("SetMaxSpeed", 109);
			Insert("SetSpeedFraction", 110);
			Insert("TerminalExplosion", 111);
			Insert("address", 112);
			Insert("alert", 113);
			Insert("allianceID", 114);
			Insert("allianceid", 115);
			Insert("bid", 116);
			Insert("bookmark", 117);
			Insert("bounty", 118);
			Insert("channel", 119);
			Insert("charid", 120);
			Insert("constellationid", 121);
			Insert("corpID", 122);
			Insert("corpid", 123);
			Insert("corprole", 124);
			Insert("damage", 125);
			Insert("duration", 126);
			Insert("effects.Laser", 127);
			Insert("gangid", 128);
			Insert("gangrole", 129);
			Insert("hqID", 130);
			Insert("issued", 131);
			Insert("jit", 132);
			Insert("languageID", 133);
			Insert("locationid", 134);
			Insert("machoVersion", 135);
			Insert("marketProxy", 136);
			Insert("minVolume", 137);
			Insert("orderID", 138);
			Insert("price", 139);
			Insert("range", 140);
			Insert("regionID", 141);
			Insert("regionid", 142);
			Insert("role", 143);
			Insert("rolesAtAll", 144);
			Insert("rolesAtBase", 145);
			Insert("rolesAtHQ", 146);
			Insert("rolesAtOther", 147);
			Insert("shipid", 148);
			Insert("sn", 149);
			Insert("solarSystemID", 150);
			Insert("solarsystemid", 151);
			Insert("solarsystemid2", 152);
			Insert("source", 153);
			Insert("splash", 154);
			Insert("stationID", 155);
			Insert("stationid", 156);
			Insert("target", 157);
			Insert("userType", 158);
			Insert("userid", 159);
			Insert("volEntered", 160);
			Insert("volRemaining", 161);
			Insert("weapon", 162);
			Insert("agent.missionTemplatizedContent_BasicKillMission", 163);
			Insert("agent.missionTemplatizedContent_ResearchKillMission", 164);
			Insert("agent.missionTemplatizedContent_StorylineKillMission", 165);
			Insert("agent.missionTemplatizedContent_GenericStorylineKillMission", 166);
			Insert("agent.missionTemplatizedContent_BasicCourierMission", 167);
			Insert("agent.missionTemplatizedContent_ResearchCourierMission", 168);
			Insert("agent.missionTemplatizedContent_StorylineCourierMission", 169);
			Insert("agent.missionTemplatizedContent_GenericStorylineCourierMission", 170);
			Insert("agent.missionTemplatizedContent_BasicTradeMission", 171);
			Insert("agent.missionTemplatizedContent_ResearchTradeMission", 172);
			Insert("agent.missionTemplatizedContent_StorylineTradeMission", 173);
			Insert("agent.missionTemplatizedContent_GenericStorylineTradeMission", 174);
			Insert("agent.offerTemplatizedContent_BasicExchangeOffer", 175);
			Insert("agent.offerTemplatizedContent_BasicExchangeOffer_ContrabandDemand", 176);
			Insert("agent.offerTemplatizedContent_BasicExchangeOffer_Crafting", 177);
			Insert("agent.LoyaltyPoints", 178);
			Insert("agent.ResearchPoints", 179);
			Insert("agent.Credits", 180);
			Insert("agent.Item", 181);
			Insert("agent.Entity", 182);
			Insert("agent.Objective", 183);
			Insert("agent.FetchObjective", 184);
			Insert("agent.EncounterObjective", 185);
			Insert("agent.DungeonObjective", 186);
			Insert("agent.TransportObjective", 187);
			Insert("agent.Reward", 188);
			Insert("agent.TimeBonusReward", 189);
			Insert("agent.MissionReferral", 190);
			Insert("agent.Location", 191);
			Insert("agent.StandardMissionDetails", 192);
			Insert("agent.OfferDetails", 193);
			Insert("agent.ResearchMissionDetails", 194);
			Insert("agent.StorylineMissionDetails", 195);
			Insert("bad allocation", 196);
		}
		#endregion Constructors

		#region Methods
		private static void Insert(string name, byte id)
		{
			IDLookup[name] = id;
			NameLookup[id] = name;
		}

		public static byte LookupId(string name)
		{
			return IDLookup[name];
		}

		public static string LookupName(byte id)
		{
			return NameLookup[id];
		}
		#endregion Methods
	}

	enum DbType
	{
		Int64 = 20,
		Time64 = 64,
		Currency64 = 6,
		Int32 = 3,
		Int16 = 2,
		Int8 = 17,
		Double = 5,
		Bool = 11,
		String = 129,
		String2 = 130,
	}
}