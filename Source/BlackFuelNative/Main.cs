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

    [StaticConstructorOnStartup]
    public class BlackFuelUtil
    {

        private static ThingCategoryDef carbonThingCategory;

        static BlackFuelUtil()
        {
            carbonThingCategory = DefDatabase<ThingCategoryDef>.GetNamed("Carbon", false);

            if (carbonThingCategory == null)
            {
                Log.Error("No thingCategory 'Carbon' was found! BlackFuel mod will NOT function correctly.");
            }
        }

        public static bool CarbonThingsExist
        {
            get
            {
                return carbonThingCategory != null;
            }
        }

        public static bool IsCarbonThing(Thing thing)
        {
            if (CarbonThingsExist)
                return thing.def.thingCategories.Contains(carbonThingCategory);

            return false;
        }
    }
}
