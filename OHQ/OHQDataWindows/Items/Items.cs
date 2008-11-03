using OHQData.Actors;
using System.Collections.Generic;
using System;

namespace OHQData.Items
{

    public class Weapon : Item
    {
        private int minDamage;
        private int maxDamage;

        public int Damage
        {
            get
            {
                Random random = new Random();
                return random.Next(minDamage, maxDamage+1);
            }
        }
        
        public int range;
        public int hitRate;

        public Weapon(string name, int minDamage, int maxDamage, int range)
            : base(name)
        {
            this.minDamage = minDamage;
            this.maxDamage = maxDamage;
            this.range = range;
            this.hitRate = hitRate;
        }

        // TODO: different weapon behaviors
        // for example, arrows don't require a line of sight to the enemy - spears can attack 2 enemies deep (penetrate)
        // lasers DO require a line of sight

        public enum Type { Sword, Polearm, Range };
    }
    public class Armor : Item
    {
        public int damageReduction;
        public int armorClass;
               
        public Armor(string name, int armorClass, int damageReduction)
            : base(name)
        {
            this.armorClass = armorClass;
            this.damageReduction = damageReduction;
        }
    }
    public class Accessory : Item
    {
        public Accessory(string name) : base(name)
        {

        }
    }
    public class Shield : Accessory
    {
        public Shield(string name) : base(name)
        {

        }
    }
    public abstract class Item
    {
        public enum Slot { Body, LeftHand, RightHand, Accessory };

        public int numSlots; // takes up 1 or 2 slots;

        public Actor.Races race; // some items are only available in certain race's shops
        public bool soldInShops;
        public int cost;
        private string name;
        public string Name
        {
            get { return name; }
        }

        private Actor.StatsDatum requiredStats;
        public Actor.StatsDatum RequiredStats
        {
            get { return requiredStats + requiredStatsModifiers(); }
        }

        public List<Attribute> attributes;

        public Item(String name)
        {
            this.name = name;
        }

        private Actor.StatsDatum requiredStatsModifiers()
        {
            return new Actor.StatsDatum();
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
