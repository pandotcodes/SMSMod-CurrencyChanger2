using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace CurrencyChanger2.Patches
{
    [HarmonyPatch(typeof(CheckoutChangeManager), "AddOrRemoveMoney")]
    public static class CheckoutChangeManager_AddOrRemoveMoney_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var instruction in instructions)
            {
                // Check if the instruction loads the constant 1f onto the stack
                if (instruction.opcode == OpCodes.Ldc_R4 && (float)instruction.operand == 1f)
                {
                    // Replace the constant 1f with your desired value (e.g., 2f)
                    yield return new CodeInstruction(OpCodes.Ldc_R4, Plugin.LowestBill.Value);
                }
                else
                {
                    yield return instruction;
                }
            }
        }
    }
}
