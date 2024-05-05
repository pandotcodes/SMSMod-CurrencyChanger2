using HarmonyLib;
using UnityEngine.XR;

namespace CurrencyChanger2.Patches
{
    [HarmonyPatch(typeof(IDManager), "SectionSO")]
    public static class IDManager_SectionSO_Patch
    {
        public static bool Done = false;
        public static void Prefix(IDManager __instance)
        {
            if (Done) return;
            Done = true;
            __instance.Sections.ForEach(x => x.Cost *= Plugin.CurrencyValueFactor.Value);
        }
    }
}
