using System;
using Harmony;
using Verse;
using System.Reflection;

namespace BlackFuelNative
{
    public class Main : Mod
    {
        public Main(ModContentPack content) : base(content)
        {
            var harmony = HarmonyInstance.Create("com.cosmicdan.rimworld.blackfuelnative");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Log.Message("BlackFuel mod native has loaded!");
        }
    }
}
