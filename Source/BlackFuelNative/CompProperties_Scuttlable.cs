using System;
using Verse;

namespace BlackFuelNative
{
    class CompProperties_Scuttlable : CompProperties
    {
        public int scuttlePollInTicks = 100;

        public CompProperties_Scuttlable()
        {
            this.compClass = typeof(CompScuttlable);
        }
    }
}
