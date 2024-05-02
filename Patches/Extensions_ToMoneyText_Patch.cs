using HarmonyLib;
using System;

namespace CurrencyChanger2.Patches
{
    [HarmonyPatch(typeof(Extensions), "ToMoneyText")]
    public static class Extensions_ToMoneyText_Patch
    {
        public static void Postfix(ref string __result, ref float money, ref float fontSize)
        {
            string p = Plugin.CurrencyPrefix.Value;
            string s = Plugin.CurrencySuffix.Value;
            string d = Plugin.CurrencyDecimalSeperator.Value;
            string text;
            if (money < 0f)
            {
                text = "-$" + Math.Abs((float)Math.Round((double)money, 2)).ToString("0.00");
            }
            else
            {
                text = "$" + ((float)Math.Round((double)money, 2)).ToString("0.00");
            }
            text = text.Replace(',', '.');
            text = text.Replace(".", d);
            text = text.Replace("$", p);
            text = text + s;
            int num = text.IndexOf(d);
            if (num != -1)
            {
                string text2 = "<size=" + (fontSize * 20f / 25f).ToString() + ">";
                text2 = text2.Replace(',', '.');
                text = text.Insert(num + 1, text2);
            }
            __result = text;
        }
    }
}
