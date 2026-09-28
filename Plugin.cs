using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using DiceKingdoms.Game;
using Unity.Mathematics;

namespace IslandConfig;

public static class Cfg {
    public const string SEC_HARD     = "HardLimit";
    public const string SEC_ALT      = "Altitude";
    public const string SEC_MOIST    = "Moisture";
    public const string SEC_SAND     = "Sand";
    public const string SEC_CLIFFS   = "InnerCliffs";
    public const string SEC_TREES    = "Nature/Trees";
    public const string SEC_ROCKS    = "Nature/Rocks";
    public const string SEC_FISH     = "Nature/Fish";
    public const string SEC_FLOWERS  = "Nature/Flowers";

    public static ConfigEntry<int> HardRadius;
    public static ConfigEntry<int> HardPower;

    public static ConfigEntry<float> AltNoiseHOffsetScale;
    public static ConfigEntry<float> AltNoiseHScale;
    public static ConfigEntry<float> AltDistancePower;
    public static ConfigEntry<float> AltNoiseVScale;
    public static ConfigEntry<float> AltDistanceVScale;
    public static ConfigEntry<float> AltVerticalOffset;
    public static ConfigEntry<int>   AltIslandMinSize;

    public static ConfigEntry<float> MoistHOffsetScale;
    public static ConfigEntry<float> MoistHScale;

    public static ConfigEntry<float> SandMargin;
    public static ConfigEntry<int>   SandMinSize;

    public static ConfigEntry<int> CliffMainsMin;
    public static ConfigEntry<int> CliffMainsMax;
    public static ConfigEntry<int> CliffExtMin;
    public static ConfigEntry<int> CliffExtMax;
    public static ConfigEntry<int> CliffMinDistance;
    public static ConfigEntry<int> CliffMinSpacing;

    public static ConfigEntry<float> TreeMoistMin;
    public static ConfigEntry<float> TreeMoistMax;
    public static ConfigEntry<float> TreeMinDistance;
    public static ConfigEntry<float> TreeCountAvg;
    public static ConfigEntry<float> TreeCountDev;
    public static ConfigEntry<float> TreeSizeAvg;
    public static ConfigEntry<float> TreeSizeDev;

    public static ConfigEntry<float> RockMoistMin;
    public static ConfigEntry<float> RockMoistMax;
    public static ConfigEntry<float> RockMinDistance;
    public static ConfigEntry<float> RockCountAvg;
    public static ConfigEntry<float> RockCountDev;
    public static ConfigEntry<float> RockSizeAvg;
    public static ConfigEntry<float> RockSizeDev;

    public static ConfigEntry<float> FishIdealMoisture;
    public static ConfigEntry<float> FishMaxMoistAmp;
    public static ConfigEntry<float> FishProbAtIdeal;
    public static ConfigEntry<float> FishProbAtMaxAmp;
    public static ConfigEntry<float> FishProbPower;
    public static ConfigEntry<float> FishMaxDistance;
    public static ConfigEntry<int>   FishValidMargin;
    public static ConfigEntry<int>   FishMaxCount;

    public static ConfigEntry<float> FlowerProbability;

	public static void Bind(ConfigFile cfg) {
        HardRadius = cfg.Bind(SEC_HARD, "radius", 22, "Radius of the island's hard circular boundary");
        HardPower  = cfg.Bind(SEC_HARD, "power", 3, "Steepness of the height falloff at the boundary");

        AltNoiseHOffsetScale = cfg.Bind(SEC_ALT, "noiseHorizontalOffsetScale", 5f, "Terrain noise horizontal offset");
        AltNoiseHScale       = cfg.Bind(SEC_ALT, "noiseHorizontalScale", 0.05f, "Terrain noise horizontal scale");
        AltDistancePower     = cfg.Bind(SEC_ALT, "distancePower", 3.5f, "Power of distance-from-center influence on height");
        AltNoiseVScale       = cfg.Bind(SEC_ALT, "noiseVerticalScale", 0.4f, "Terrain roughness amplitude");
        AltDistanceVScale    = cfg.Bind(SEC_ALT, "distanceVerticalScale", 1.4f, "Rate of height falloff toward the edge");
        AltVerticalOffset    = cfg.Bind(SEC_ALT, "verticalOffset", 0.61f, "Global sea level offset");
        AltIslandMinSize     = cfg.Bind(SEC_ALT, "islandMinSize", 6, "Minimum island size (in cells)");

		MoistHOffsetScale = cfg.Bind(SEC_MOIST, "horizontalOffsetScale", 1f, "Moisture noise offset");
        MoistHScale       = cfg.Bind(SEC_MOIST, "horizontalScale", 0.05f, "Moisture noise scale");

        SandMargin  = cfg.Bind(SEC_SAND, "margin", 2f, "Width of the sand strip along the shore");
        SandMinSize = cfg.Bind(SEC_SAND, "minSize", 8, "Minimum sand area size");

        CliffMainsMin    = cfg.Bind(SEC_CLIFFS, "numberMains.min", 2, "Min. number of main cliffs");
        CliffMainsMax    = cfg.Bind(SEC_CLIFFS, "numberMains.max", 3, "Max. number of main cliffs");
        CliffExtMin      = cfg.Bind(SEC_CLIFFS, "numberExtensions.min", 1, "Min. number of branches");
        CliffExtMax      = cfg.Bind(SEC_CLIFFS, "numberExtensions.max", 3, "Max. number of branches");
        CliffMinDistance = cfg.Bind(SEC_CLIFFS, "minDistance", 8, "Min. distance between cliffs");
        CliffMinSpacing  = cfg.Bind(SEC_CLIFFS, "minSpacing", 5, "Min. spacing between cliffs");

        TreeMoistMin    = cfg.Bind(SEC_TREES, "moistureRange.min", -0.5f, "Moisture range: min");
        TreeMoistMax    = cfg.Bind(SEC_TREES, "moistureRange.max",  1.0f, "Moisture range: max");
        TreeMinDistance = cfg.Bind(SEC_TREES, "minDistance", 4f, "Min. distance between trees");
        TreeCountAvg    = cfg.Bind(SEC_TREES, "count.average",  3.3f, "Number of patches: average");
        TreeCountDev    = cfg.Bind(SEC_TREES, "count.deviation", 0.4f, "Number of patches: deviation");
        TreeSizeAvg     = cfg.Bind(SEC_TREES, "size.average",   30f,  "Patch size: average");
        TreeSizeDev     = cfg.Bind(SEC_TREES, "size.deviation", 10f,  "Patch size: deviation");

        RockMoistMin    = cfg.Bind(SEC_ROCKS, "moistureRange.min", -1.0f, "Moisture range: min");
        RockMoistMax    = cfg.Bind(SEC_ROCKS, "moistureRange.max",  0.5f, "Moisture range: max");
        RockMinDistance = cfg.Bind(SEC_ROCKS, "minDistance", 5f, "Min. distance between rocks");
        RockCountAvg    = cfg.Bind(SEC_ROCKS, "count.average",  8.5f, "Number of patches: average");
        RockCountDev    = cfg.Bind(SEC_ROCKS, "count.deviation", 1.0f, "Number of patches: deviation");
        RockSizeAvg     = cfg.Bind(SEC_ROCKS, "size.average",   4f,  "Patch size: average");
        RockSizeDev     = cfg.Bind(SEC_ROCKS, "size.deviation", 0.8f, "Patch size: deviation");

        FishIdealMoisture = cfg.Bind(SEC_FISH, "idealMoisture", 0.4f, "Moisture for max. fish probability");
        FishMaxMoistAmp   = cfg.Bind(SEC_FISH, "maximumMoistureAmplitude", 0.3f, "Max. deviation from idealMoisture");
        FishProbAtIdeal   = cfg.Bind(SEC_FISH, "probabilityAtIdealMoisture", 0.6f, "Probability at idealMoisture (0..1)");
        FishProbAtMaxAmp  = cfg.Bind(SEC_FISH, "probabilityAtMaximumMoistureAmplitude", 0f, "Probability at max. deviation (0..1)");
        FishProbPower     = cfg.Bind(SEC_FISH, "probabilityPower", 1.5f, "Power of the probability curve");
        FishMaxDistance   = cfg.Bind(SEC_FISH, "maximumDistance", 22f, "Max. distance from shore for fish");
        FishValidMargin   = cfg.Bind(SEC_FISH, "validMarginDistance", 2, "Allowed margin from the water edge");
        FishMaxCount      = cfg.Bind(SEC_FISH, "maxCount", 20, "Maximum number of fish points");

        FlowerProbability = cfg.Bind(SEC_FLOWERS, "flowerProbability", 0.05f, "Probability of flowers on grassy tiles (0..1)");
    }

    public static void Apply(ref IslandGeneration.Parameters p) {
        p.hardLimit.radius = HardRadius.Value;
        p.hardLimit.power  = HardPower.Value;

        p.altitude.noiseHorizontalOffsetScale = AltNoiseHOffsetScale.Value;
        p.altitude.noiseHorizontalScale       = AltNoiseHScale.Value;
        p.altitude.distancePower              = AltDistancePower.Value;
        p.altitude.noiseVerticalScale         = AltNoiseVScale.Value;
        p.altitude.distanceVerticalScale      = AltDistanceVScale.Value;
        p.altitude.verticalOffset             = AltVerticalOffset.Value;
        p.altitude.islandMinSize              = AltIslandMinSize.Value;

        p.moisture.horizontalOffsetScale = MoistHOffsetScale.Value;
        p.moisture.horizontalScale       = MoistHScale.Value;

        p.sand.margin  = SandMargin.Value;
        p.sand.minSize = SandMinSize.Value;

        p.innerCliffs.numberMains      = new int2(CliffMainsMin.Value, CliffMainsMax.Value);
        p.innerCliffs.numberExtensions = new int2(CliffExtMin.Value,   CliffExtMax.Value);
        p.innerCliffs.minDistance      = CliffMinDistance.Value;
        p.innerCliffs.minSpacing       = CliffMinSpacing.Value;

        p.nature.trees.moistureRange = new float2(TreeMoistMin.Value, TreeMoistMax.Value);
        p.nature.trees.minDistance   = TreeMinDistance.Value;
        p.nature.trees.count.average   = TreeCountAvg.Value;
        p.nature.trees.count.deviation = TreeCountDev.Value;
        p.nature.trees.size.average    = TreeSizeAvg.Value;
        p.nature.trees.size.deviation  = TreeSizeDev.Value;

        p.nature.rocks.moistureRange = new float2(RockMoistMin.Value, RockMoistMax.Value);
        p.nature.rocks.minDistance   = RockMinDistance.Value;
        p.nature.rocks.count.average   = RockCountAvg.Value;
        p.nature.rocks.count.deviation = RockCountDev.Value;
        p.nature.rocks.size.average    = RockSizeAvg.Value;
        p.nature.rocks.size.deviation  = RockSizeDev.Value;

        p.nature.fish.idealMoisture                         = FishIdealMoisture.Value;
        p.nature.fish.maximumMoistureAmplitude              = FishMaxMoistAmp.Value;
        p.nature.fish.probabilityAtIdealMoisture            = FishProbAtIdeal.Value;
        p.nature.fish.probabilityAtMaximumMoistureAmplitude = FishProbAtMaxAmp.Value;
        p.nature.fish.probabilityPower                      = FishProbPower.Value;
        p.nature.fish.maximumDistance                       = FishMaxDistance.Value;
        p.nature.fish.validMarginDistance                   = FishValidMargin.Value;
        p.nature.fish.maxCount                              = FishMaxCount.Value;

        p.nature.flowerProbability = FlowerProbability.Value;
    }
}

[HarmonyPatch(typeof(IslandGrid), nameof(IslandGrid.GenerateJob))]
static class Patch_IslandGrid_GenerateJob {
    [HarmonyPrefix]
    static void Prefix(IslandGrid __instance, uint seed) {
        var p = __instance.generationParameters;
		Plugin.CfgFile.Reload();
        Cfg.Apply(ref p);
        __instance.generationParameters = p;
    }
}

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin {
    internal static new ManualLogSource Log;
	internal static ConfigFile CfgFile;

    public override void Load() {
        Log = base.Log;

		CfgFile = Config;
        Cfg.Bind(CfgFile);

        var harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        harmony.PatchAll(typeof(Patch_IslandGrid_GenerateJob));
    }
}
