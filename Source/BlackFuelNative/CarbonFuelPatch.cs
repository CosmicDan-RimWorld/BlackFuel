using Harmony;
using RimWorld;
using System;
using Verse;
using Verse.AI;

namespace BlackFuelNative
{
    // hook the job for getting fuel - if carbon item is designated, divide the count (for delivery) by 3
    // sadly we can't hook the CompRefuelable itself since it isn't aware of the thing that the job is going to refuel with
    [HarmonyPatch(typeof(WorkGiver_Refuel))]
    [HarmonyPatch("JobOnThing")]
    [HarmonyPatch(new Type[] { typeof(Pawn), typeof(Thing), typeof(bool) })]
    class WorkGiver_Refuel__JobOnThing
    {
        static void Postfix(WorkGiver_Refuel __instance, ref Job __result)
        {
            if (BlackFuelUtil.IsCarbonThing(__result.targetB.Thing))
                __result.count /= 3;
        }

    }

    // hook the actual fuel insertion action - if carbon item is added, multiply it by 3
    [HarmonyPatch(typeof(CompRefuelable))]
    [HarmonyPatch("Refuel")]
    [HarmonyPatch(new Type[] { typeof(Thing) })]
    class PatchCompRefuelable__Refuel
    {
        static void Prefix(CompRefuelable __instance, ref Thing fuelThing)
        {
            if (BlackFuelUtil.IsCarbonThing(fuelThing))
                fuelThing.stackCount *= 3;
        }

    }
}
