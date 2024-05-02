using HarmonyLib;
using TMPro;

namespace CurrencyChanger2.Patches
{
    [HarmonyPatch(typeof(PosTerminal), "Start")]
    public static class PosTerminal_Start_Patch
    {
        public static void Postfix(PosTerminal __instance)
        {
            string p = Plugin.TerminalSymbol.Value;
            var tmp = __instance.gameObject.transform.GetChild(2).GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();
            tmp.text = p;
            tmp.enableWordWrapping = false;
        }
    }
}
