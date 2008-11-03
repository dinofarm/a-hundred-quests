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
        #region Primary stats
        private int movePoints;
        public int MovePoints
        {
            get { return movePoints; }
        }
        private int hp;
        public int Hp
        {
            get { return hp;}
        }
        private int mp;
        public int Mp
        {
            get { return mp; }
        }
             
        private Point battleCoordinates;
        public Point BattleCoordinates
        {
            get { return battleCoordinates; }
        }

        private String name;
        public String Name
        {
            get { return name; }
        }

        private Races race;
        public Races Race
        {
            get { return race; }
        }

        private Genders gender;
        public Genders Gender
        {
            get { return gender; }
        }

        private int experience;
        public int Experience
        {
            get { return experience; }
        }

        private StatsDatum stats;
        public StatsDatum Stats
        {
            get { return stats + bonuses; }
            set { stats = value; }
        }
        private StatsDatum bonuses;

        #endregion

        #region Secondary stats
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
                return (int)(stats.Dex * 0.1 + 2);
            }
        }

        //Armor Class, Damage Reduction, Damage, Range, Mana Regen Rate, HP regen Rate, 
        //To-Hit%, Evade%, Attack of Opportunity%, Spell Success

        public int Damage
        {
            get { return stats.Str + Weapon.Damage; }            
        }

        public int ArmorClass
        {
            get
            {
                int bonusFromArmor = (equipment.body == null) ? 0 : equipment.body.armorClass;
                return stats.Dex + bonusFromArmor;
            }
        }

        public int DamageReduction
        {
            get
            { 
                int bonusFromArmor = (equipment.body == null) ? 0 : equipment.body.damageReduction;
                return bonusFromArmor;
            }
        }

        public int Range
        {
            get { return Weapon.range; }
        }

        //TODO:  Address Evade % ||| Evade! Armor Class Stuff!  Sort me out!

        public int AttackOfOpportunityPercent
        {
            //TODO: Check if player has a shortsword!
            get { return (stats.Int * (stats.Dex / 2)) + 10; }
        }

        public int SpellSuccessPercent
        {
            get { return stats.Int * 4; }
        }

        //TODO:  Apply every turn, save the remaining fractions, like 1.5, we'll save the .5
        public double MpRegenPerTurn
        {
            get { return stats.Int / 10.0; }
        }

        public double HpRegenPerTurn
        {
            get { return stats.Con / 10.0; }
        }

        public int ToHitPercent(int targetDex)
        {
            //% to hit = (Weapon % (Melee are about 80, range are about 50) – (-Target Dex + Your Dex) 
            return Weapon.hitRate - (stats.Dex - targetDex);
        }

        #endregion

        public struct Equipment
        {

            public Item leftHand;
            public Item rightHand;
            public Armor body;
            public Item accessory;
        }

        private AnimatingSprite sprite;
        public AnimatingSprite Sprite
        {
            get { return sprite; }
        }

        public Weapon Weapon
        {
            get
            {
                Weapon weapon = null;
                if(equipment.leftHand != null)
                {
                    if(equipment.leftHand.GetType() == typeof(Weapon))
                    {
                        weapon = (Weapon)equipment.leftHand;
                    }
                }
                else if(equipment.rightHand != null)
                {
                    if(equipment.rightHand.GetType() == typeof(Weapon))
                    {
                        weapon = (Weapon)equipment.rightHand;
                    }
                }
                return weapon;
            } 
        }

        public Equipment equipment;
        private List<Skill> skills;
        public List<Skill> Skills
        {
            get { return skills; }
        }
        private List<StatusEffect> statusEffects;
        public List<StatusEffect> StatusEffects
        {
            get { return statusEffects; }
        }


        #region Constructors

        public Actor(String name, Races race, Genders gender)
        {
            stats = new StatsDatum();
            equipment = new Equipment();
            skills = new List<Skill>();
            statusEffects = new List<StatusEffect>();
            this.name = name;
            this.race = race;
            this.gender = gender;

        }
        public Actor(String name, Races race, Genders gender, Point battleCoordinates)
            : this(name, race, gender)
        {
            this.battleCoordinates = battleCoordinates;
        }

        #endregion

        public void addSkill(StatSkill skill)
        {
            //add
            skills.Add(skill);

            //apply
            switch (skill.statType)
            {
                case StatTypes.Str:
                    stats.Str += skill.bonus;
                    break;
                case StatTypes.Cha:
                    stats.Cha += skill.bonus;
                    break;
                case StatTypes.Con:
                    stats.Con += skill.bonus;
                    break;
                case StatTypes.Dex:
                    stats.Dex += skill.bonus;
                    break;
                case StatTypes.Int:
                    stats.Int += skill.bonus;
                    break;
                case StatTypes.Hp:
                    hp += skill.bonus;
                    break;
                case StatTypes.Mp:
                    mp += skill.bonus;
                    break;
                case StatTypes.MovePoints:
                    movePoints += skill.bonus;
                    break;
                default:
                    break;
            }
        }
        public void addSkill(ActiveSkill skill) { }
        public void addSkill(PassiveSkill skill) { }
        public void addSkill(MagicSkill skill) { }



        public void addExperience(int points)
        {
            experience += points;
        }

        #region Structs and Enums

        public struct StatsDatum
        {
            public int Str;
            public int Dex;
            public int Con;
            public int Int;
            public int Cha;

            public StatsDatum(int Str, int Dex, int Con, int Int, int Cha)
            {
                this.Str = Str;
                this.Dex = Dex;
                this.Con = Con;
                this.Int = Int;
                this.Cha = Cha;
            }

            public static StatsDatum operator +(StatsDatum stats1, StatsDatum stats2)
            {
                StatsDatum newStats;
                newStats.Str = stats1.Str + stats2.Str;
                newStats.Dex = stats1.Dex + stats2.Dex;
                newStats.Con = stats1.Con + stats2.Con;
                newStats.Int = stats1.Int + stats2.Int;
                newStats.Cha = stats1.Cha + stats2.Cha;

                return newStats;
            }
        }
        public enum StatTypes { Str, Dex, Con, Int, Cha, Hp, Mp, MovePoints }
        public enum Genders { Male, Female, Other };
        public enum Races { Human, Dinoman, Fairy, Alien, Undead, Mechanical };

        #endregion
    }
}
