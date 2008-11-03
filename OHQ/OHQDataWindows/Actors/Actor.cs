using System;
using System.Collections.Generic;
using System.Text;
using OHQData.Items;
using Microsoft.Xna.Framework;
using OHQData.Sprites;
using OHQData.Skills;



namespace OHQData.Actors
{
    public abstract class Actor
    {
        public int movePoints;
        public int hp;
        public int mp;   
     
        Point battleCoordinates;
        String name;
        Race race;
        Gender gender;
        int Experience;

        public int Level
        {
            get { return -1; }  //TODO write in functionality
        }
        public int MaxHP
        {
            get { return (stats.Con * 3); }
        }
        public int MaxMP
        {
            get { return (stats.Con * stats.Int); }
        }
        public int MaxMovePoints
        {
            get {
                return (int)((stats.Dex * .1) + 2);
            }
        }

        public struct Equipment
        {

            public Item leftHand;
            public Item rightHand;
            public Armor body;
            public Item accessory;
        }

        AnimatingSprite sprite;


        private Stats stats;
        public Stats Stats
        {
            get { return stats + bonuses.stats; }
        }

        //Armor Class, Damage Reduction, Damage, Range, Mana Regen Rate, HP regen Rate, 
        //To-Hit%, Evade%, Attack of Opportunity%, Spell Success

        public int Damage
        {
            get { return stats.Str + weapon.Damage; }

            
        }

        public int ArmorClass
        {
            get
            {
                bonusFromArmor = (bonusFromArmor == null) ? 0 : equipment.body.armorClass;
                return stats.Dex + bonusFromArmor;
            }
        }

        public int DamageReduction
        {
            get
            { 
                bonusFromArmor = (bonusFromArmor == null) ? 0 : equipment.body.damageReduction;
                return bonusFromArmor;
            }
        }

        public int Range
        {
            get { return Weapon.range; }
        }


        public Weapon Weapon
        {
            get
            {
                Weapon weapon;
                if(equipment.leftHand != null)
                {
                    if(equipment.leftHand.GetType() == typeof(Weapon))
                    {
                        weapon = equipment.leftHand;
                    }
                }
                else if(equipment.rightHand != null)
                {
                    if(equipment.rightHand.GetType() == typeof(Weapon))
                    {
                        weapon = equipment.rightHand;
                    }
                }
                return weapon;
            } 
        }

        public Equipment equipment;
        private List<OHQData.Skills.Skill> skills;
        List<StatusEffect> statusEffects;

        public Actor(String name, Race race, Gender gender)
        {
            stats = new Stats();
            equipment = new Equipment();
            skills = new List<OHQData.Skills.Skill>();
            statusEffects = new List<StatusEffect>();
            this.name = name;
            this.race = race;
            this.gender = gender;

        }
        public Actor(String name, Race race, Gender gender, Point battleCoordinates)
            : this(name, race, gender)
        {
            this.battleCoordinates = battleCoordinates;
        }

        public void addSkill(StatSkill skill)
        {
            //add
            skills.Add(skill);

            //apply
            switch (skill.stat)
            {
                case Stat.Str:
                    stats.Str += skill.bonus;
                    break;
                case Stat.Cha:
                    stats.Cha += skill.bonus;
                    break;
                case Stat.Con:
                    stats.Con += skill.bonus;
                    break;
                case Stat.Dex:
                    stats.Dex += skill.bonus;
                    break;
                case Stat.Int:
                    stats.Int += skill.bonus;
                    break;
                case Stat.Hp:
                    hp += skill.bonus;
                    break;
                case Stat.Mp:
                    mp += skill.bonus;
                    break;
                case Stat.MovePoints:
                    movePoints += skill.bonus;
                    break;
                default:
                    break;
            }
        }
        public void addSkill(ActiveSkill skill) { }
        public void addSkill(PassiveSkill skill) { }
        public void addSkill(MagicSkill skill) { }

        public struct Stats
        {
            public int Str;
            public int Dex;
            public int Con;
            public int Int;
            public int Cha;

            public static Stats operator +(Stats stats1, Stats stats2)
            {
                Stats newStats;
                newStats.Str = stats1.Str + stats2.Str;
                newStats.Dex = stats1.Dex + stats2.Dex;
                newStats.Con = stats1.Con + stats2.Con;
                newStats.Int = stats1.Int + stats2.Int;
                newStats.Cha = stats1.Cha + stats2.Cha;

                return newStats;
            }
        }
        public enum Stat { Str, Dex, Con, Int, Cha, Hp, Mp, MovePoints }
        public enum Gender { Male, Female, Other };
        public enum Race { Human, Dinoman, Fairy, Alien, Undead, Mechanical };
    }
}
