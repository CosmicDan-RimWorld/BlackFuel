using RimWorld;
using System.Linq;
using System.Collections.Generic;
using Verse;
using UnityEngine;

namespace BlackFuelNative
{
    class CompScuttlable : ThingComp
    {
        private int ticker = 0;
        private List<IntVec3> cachedAdjCellsCardinal;
        private CompRefuelable compRefuelable;

        private List<IntVec3> AdjCellsCardinalInBounds
        {
            get
            {
                if (this.cachedAdjCellsCardinal == null)
                {
                    this.cachedAdjCellsCardinal = (from c in GenAdj.CellsAdjacentCardinal(parent)
                                                   where c.InBounds(parent.Map)
                                                   select c).ToList<IntVec3>();
                }
                return this.cachedAdjCellsCardinal;
            }
        }

        public override void CompTick()
        {
            base.CompTick();
            ticker++;
            if (ticker >= Props.scuttlePollInTicks)
            {
                ticker = 0;

                if (compRefuelable == null)
                    compRefuelable = parent.TryGetComp<CompRefuelable>();

                if (compRefuelable == null || !BlackFuelUtil.CarbonThingsExist || compRefuelable.IsFull)
                    return;
                if (parent.IsForbidden(parent.Faction))
                    return;

                CompFlickable compFlickable = parent.GetComp<CompFlickable>();
                if (compFlickable != null && !compFlickable.SwitchIsOn)
                    return;

                if (!compRefuelable.ShouldAutoRefuelNow)
                    return;

                Thing fuelThing = FindFuelInAnyScuttle();
                if (fuelThing != null)
                {
                    // should I check if the item is forbidden first? Core hopper logic doesn't seem to...
                    int fuelNeeded = compRefuelable.GetFuelCountToFullyRefuel();
                    int fuelThingValue = fuelThing.stackCount;

                    if (BlackFuelUtil.IsCarbonThing(fuelThing))
                    {
                        fuelNeeded /= 3;
                        fuelThingValue *= 3;
                    }

                    if (fuelThingValue <= fuelNeeded)
                    {
                        compRefuelable.Refuel(fuelThingValue);
                        fuelThing.Destroy();
                    } else
                    {
                        compRefuelable.Refuel(fuelThingValue);
                        fuelThing.stackCount -= fuelNeeded;
                    }
                }
            }
        }

        protected virtual Thing FindFuelInAnyScuttle()
        {
            for (int i = 0; i < this.AdjCellsCardinalInBounds.Count; i++)
            {
                Thing thing = null;
                Thing thing2 = null;
                List<Thing> thingList = this.AdjCellsCardinalInBounds[i].GetThingList(parent.Map);
                for (int j = 0; j < thingList.Count; j++)
                {
                    Thing thingInThisCell = thingList[j];
                    if (compRefuelable.Props.fuelFilter.Allows(thingInThisCell))
                    {
                        thing = thingInThisCell;
                    }
                    if (thingInThisCell.def.defName.Equals("CoalScuttle"))
                    {
                        thing2 = thingInThisCell;
                    }
                }
                if (thing != null && thing2 != null)
                {
                    return thing;
                }
            }
            return null;
        }

        public CompProperties_Scuttlable Props
        {
            get
            {
                return (CompProperties_Scuttlable)this.props;
            }
        }
    }
}
