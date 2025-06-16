using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace VanillaCookingExpandedVariety
{
    public class PawnDiet: IExposable
    {

        public List<ThingDef> last10Meals;
        public List<ThingDef> last10Ingredients;

        public PawnDiet()
        {
            last10Meals = new List<ThingDef>();
            last10Ingredients = new List<ThingDef>();

        }


        public void ExposeData()
        {
            Scribe_Collections.Look(ref last10Meals, "last10Meals", LookMode.Def);
            Scribe_Collections.Look(ref last10Ingredients, "last10Ingredients", LookMode.Def);

        }


    }
}
