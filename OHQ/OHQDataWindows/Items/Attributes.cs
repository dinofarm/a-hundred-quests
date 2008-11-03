using System.Collections.Generic;
namespace OHQData.Items
{
    // An Attribute is a collection of modifiers for some Item
    public class Attribute
    {
        public List<Modifier> modifiers;
    }
    // A Modifier is a chance (%) that a Statistic will be increased/decreased by some percentage
    // TODO:  - make sure weapons with 100% chance to do something reflect it as a core property in-game
    public class Modifier
    {
        public int chance;
        public Statistic statistic;
        public int percent;

        public Modifier(Statistic statistic, int percent)
        {
            this.statistic = statistic;
            this.percent = percent;
            this.chance = 100;
        }
        public Modifier(Statistic statistic, int percent, int chance)
            : this(statistic, percent)
        {
            this.chance = chance;
        }
    }
    public enum Statistic { Damage, Cost, ChanceToMiss, RequiredStr, RequiredDex, ToHit }
}