using System.Collections.Generic;
using System;
using UnityEngine;

public class Translation
{
  public static Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>{
    // Menu
    {"menu", new string[]{"Menu", "Valikko" } },
    {"exit_game", new string[]{"Exit game", "Sulje peli" } },
    {"new_game", new string[]{"New game", "Uusi peli" } },
    {"confirm", new string[]{"Confirm", "Vahvista" } },
    {"continue", new string[]{"Continue", "Jatka" } },

    // Level
    {"gameover", new string[]{"Game Over", "Peli päättyi" } },
    {"restart", new string[]{"Restart", "Uudestaan" } },
    {"next_level", new string[]{"Next level", "Jatka" } },
    {"quit", new string[]{"Quit", "Lopeta" } },
    {"victory", new string[]{"Victory", "Voitto" } },
    {"defeated", new string[]{"Defeated", "Häviö" } },
    {"wave", new string[]{"Wave", "Aalto" } },
    {"buildTower", new string[]{"Build a tower", "Rakenna torni" } },
    {"upgrade", new string[]{"Upgrade", "Tehostus" } },
    {"sell", new string[]{"Sell", "Myy" } },

    // Rewards
    {"select_tower", new string[]{"Select a tower", "Valitse uusi torni" } },
    {"select_rune", new string[]{"Select a rune", "Valitse uusi riimu" } },

    // Attributes
    {"cost", new string[]{"Cost", "Hinta" } },
    {"life", new string[]{"Life", "Kesto" } },
    {"armor", new string[]{"Armor", "Panssari" } },
    {"speed", new string[]{"Speed", "Nopeus" } },
    {"value", new string[]{"Value", "Arvo" } },
    {"danger", new string[]{"Danger", "Vaara" } },

    {"attribute_damage", new string[]{"Damage %value%", "Vahinko %value%" } },
    {"attribute_range", new string[]{"Range %value%", "Kantama %value%" } },
    {"attribute_reloadspeed", new string[]{"Fire speed %value%", "Tulinopeus %value%" } },
    {"attribute_cost", new string[]{"Base cost %value%", "Perus hinta %value%" } },
    {"attribute_costincrease", new string[]{"Cost increase %value%", "Hinnan kasvu %value%" } },

    {"attribute_sellvalue", new string[]{"100% sell value", "100% myyntiarvo" } },
    {"attribute_buildingcooldown", new string[]{"Has cooldown instead of building cost.", "Korvaa rakentamishinnan rakentamisajalla." } },

    // Tower names
    {"tower_turret", new string[]{"Turret", "Torni" } },
    {"tower_cannon", new string[]{"Cannon", "Tykki" } },
    {"tower_bomb", new string[]{"Mortar", "Pommittaja" } },
    {"tower_generator", new string[]{"Generator", "Generaattori" } },
    {"tower_laserSlow", new string[]{"Gravity laser", "Hitaussäde" } },
    {"tower_laserBurn", new string[]{"Fire laser", "Tulisäde" } },
    {"tower_laserChain", new string[]{"Tesla tower", "Teslatorni" } },
    {"tower_auraValue", new string[]{"Collector", "Kerääjä" } },
    {"tower_auraSlow", new string[]{"Gravity field", "Hidastuttaja" } },
    {"tower_auraDamage", new string[]{"Thumber", "Tömistäjä" } },
    {"tower_tileTeleport", new string[]{"Teleporter", "Teleporttaaja" } },
    // Tower targeting
    {"targeting_closest", new string[]{"Closest", "Lähin" } },
    {"targeting_distance", new string[]{"Furthest", "Pisimmällä" } },
    {"targeting_life", new string[]{"Life", "Sydämet" } },
    {"targeting_lowLife", new string[]{"Lowest life", "Vähiten sydämiä" } },
    {"targeting_armor", new string[]{"Armor", "Panssari" } },
    // Tower info
    {"info_cooldown", new string[]{"Cooldown: %value%s", "Viive: %value%s" } },
    {"info_damage", new string[]{"Damage: %value%", "Vahinko: %value%" } },
    {"info_damageOverTime", new string[]{"Damage: %value% / s", "Vahinko: %value% / s" } },
    {"info_range", new string[]{"Range: %value%", "Kantama: %value%" } },
    {"info_explosionRadius", new string[]{"Explosion radius: %value%", "Räjähdys kantama: %value%" } },
    {"info_chain", new string[]{"Hits up to %value% enemies.", "Osuu %value% viholliseen." } },
    {"info_resourceGain", new string[]{"Generates %value% resources.", "Tuottaa +%value% resurssia." } },
    {"info_duration", new string[]{"Duration: %value%s", "Kesto: %value%s" } },
    {"info_burn", new string[]{"Burns %value% damage / s", "Polttaa %value% vahinkoa / s" } },
    {"info_slow", new string[]{"Slows target %value%%.", "Hidastaa kohdetta %value%%." } },
    {"info_teleport", new string[]{"Teleports enemy to beginning.", "Teleporttaa vihollisen alkuun." } },
    {"info_auraSlow", new string[]{"Slow enemies in range by %value%%.", "Hidastaa vihollisia kantamalla %value%%." } },
    {"info_auraValue", new string[]{"Enemies in range give +%value%% resources.", "Viholliset kantamalla antavat +%value%% resursseja." } },
    {"info_auraDamage", new string[]{"Enemies in range take %value% damage.", "Viholliset kantamalla kärsivät %value% vahinkoa." } },

    // Rune
    {"rune_speed", new string[]{"Speed", "Nopeus" } },
    {"rune_damage", new string[]{"Damage", "Vahinko" } },
    {"rune_range", new string[]{"Range", "Kantama" } },
    {"rune_cost", new string[]{"Cost", "Hinta" } },
    {"rune_costgrowth", new string[]{"Cost growth", "Hinnan kasvu" } },
    {"rune_sellvalue", new string[]{"Recycle", "Kierrätys" } },
    {"rune_elite", new string[]{"Elite", "Eliitti" } },
    {"rune_buildingcooldown", new string[]{"Auto assembly", "Itse rakentuva" } },

    // Enemy
    {"enemy_basic", new string[]{"Enemy", "Vihu" } },
    {"enemy_fast", new string[]{"Sprinter", "Kipittäjä" } },
    {"enemy_armor", new string[]{"Armor", "Panssari" } },
    {"enemy_boss", new string[]{"Boss", "Pomo" } },
  };

  public static string GetTranslation(string key, StringFormat format = StringFormat.normal, Dictionary<string, string> values = null)
  {
    if(key == ""){
      return "";
    }
    try{
      int language = (int)DataManager.instance.settings.language;
      string str = dictionary[key][language];
      str = InsertValues(str, values);
      str = FormatString(str, format);
      return str;
    } catch (KeyNotFoundException){
      string warning = $"Key not found '{key}'";
      Debug.LogWarning(warning);
      return warning;
    }
  }

  public static string GetTranslation(string key, string value){
    return GetTranslation(key, new Dictionary<string, string>{{"value", value}});
  }

  public static string GetTranslation(string key, object value){
    return GetTranslation(key, new Dictionary<string, string>{{"value", value.ToString()}});
  }

  public static string GetTranslation(string key, Dictionary<string, string> values)
  {
    return GetTranslation(key, StringFormat.normal, values);
  }

  private static string InsertValues(string str, Dictionary<string, string> values){
    if (values != null)
    {
      foreach(string k in values.Keys)
      {
        str = str.Replace("%" + k + "%", values[k]);
      }
    }
    return str;
  }

  private static string FormatString(string str, StringFormat format)
  {
    switch (format)
    {
      case StringFormat.normal:
        break;
      case StringFormat.capitalized:
        str = Capitalize(str);
        break;
      case StringFormat.upcase:
        str = str.ToUpper();
        break;
      case StringFormat.lowcase:
        str = str.ToLower();
        break;
    }
    return str;
  }

  private static string Capitalize(string str)
  {
    return str.Substring(0, 1).ToUpper() + str.Substring(1).ToLower();
  }
}