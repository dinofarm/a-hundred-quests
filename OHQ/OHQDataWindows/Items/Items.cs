using OHQData.Actors;
using System.Collections.Generic;

namespace OHQData.Items
{
    public class Weapon : Item
    {
        private int minDamage;
        private int maxDamage;
        public int Damage
        {
            get { RandomNumber.Next(minDamage, maxDamage+1); }
        }
        
        public int range;
        public int hitRate;
        public bool twoHanded;

        public Weapon(int minDamage, int maxDamage, int range, int twoHanded)
        {
            this.minDamage = minDamage;
            this.maxDamage = maxDamage;
            this.range = range;
            this.twoHanded = twoHanded;
            this.hitRate = hitRate;
        }

        // TODO: different weapon behaviors
        // for example, arrows don't require a line of sight to the enemy - spears can attack 2 enemies deep (penetrate)
        // lasers DO require a line of sight
    }
    public class Armor : Item
    {
        public int damageReduction;
        public int armorClass;
        public Armor(int armorClass, int damageReduction) 
        {
            this.armorClass = armorClass;
            this.damageReduction = damageReduction;
        }
    }
    public class Accessory : Item
    {
    }
    public class Shield : Accessory
    {
    }
    public abstract class Item
    {
        public enum Slot { Body, LeftHand, RightHand, Accessory };

        public int numSlots; // takes up 1 or 2 slots;

        public Actor.Race race; // some items are only available in certain race's shops
        public bool soldInShops;
        public int cost;

        private Actor.Stats requiredStats;
        public Actor.Stats RequiredStats
        {
            get { return requiredStats + requiredStatsModifiers(); }
        }

        public List<Attribute> attributes;

        private Actor.Stats requiredStatsModifiers()
        {
            return new Actor.Stats();
        }

        // TODO: deprecated by requiredStatsModifiers() ??
        private int modifiersFor(Statistic statistic)
        {
            int percent = 0;
            foreach (Attribute attr in attributes)
            {
                foreach (Modifier mod in attr.modifiers)
                {
                    if (mod.statistic == statistic) { percent += mod.percent; }
                } 
            }
            return percent;
        }
    } 
}
