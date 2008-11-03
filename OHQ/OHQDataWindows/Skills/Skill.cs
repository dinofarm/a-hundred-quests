using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework.SyntaxHelpers;
using OHQData.Actors;

namespace OHQData.Skills
{
    // Skill, represents the skill tree for the player, abstract class defining all skills
    public abstract class Skill
    {
        private String name;
        public String Name
        {
            get { return name; }
        }

        private int level;
        public int Level
        {
            get { return level; }
        }

        private List<Requirement> requirements;
        public List<Requirement> Requirements
        {
            get { return requirements; }
        }

        public Skill(String name, List<Requirement> requirements)
        {
            this.name = name;
            this.requirements = requirements;
        }

        public struct Requirement
        {
            public Skill skill;
            public int level;

            public Requirement(Skill skill, int level)
            {
                this.skill = skill;
                this.level = level;
            }
        }
    }

    // StatSkill - Skills which add a point to STATS
    public class StatSkill : Skill   
    {
        public Actor.StatTypes statType; // character stat to modify
        public int bonus; // points to modify the stat by

        public StatSkill(String name, List<Requirement> requirements,
                         Actor.StatTypes statType, int bonus) : base(name, requirements)
        {
            this.statType = statType;
            this.bonus = bonus;
        }
    }

    // ActiveSkill - Skills which are actively used in combat
    public class ActiveSkill : Skill
    {
        private int mpCost;
        public int MpCost
        {
            get { return mpCost; }
        }

        public ActiveSkill(String name, List<Requirement> requirements, int mpCost)
            : base(name, requirements)
        {
            this.mpCost = mpCost;
        }
    }

    // PassiveSkill - Skills which have a constant effect 
    public class PassiveSkill : Skill
    {

        public PassiveSkill(String name, List<Requirement> requirements)
            : base(name, requirements)
        {
        }

    }

    // MagicSkill - Adding points here teaches player new spells
    public class MagicSkill : Skill
    {
        public MagicSkill(String name, List<Requirement> requirements)
            : base(name, requirements)
        {
        }

    }
}
