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
        String name;
        int level;
        List<Requirement> requirements;

        public Skill(String name, List<Requirement> requirements)
        {
            this.name = name;
            this.requirements = requirements;
        }

        public struct Requirement
        {
            public Skill skill;
            public int level;
        }
    }

    // StatSkill - Skills which add a point to STATS
    public class StatSkill : Skill   
    {
        public Actor.Stat stat; // character stat to modify
        public int bonus; // points to modify the stat by

        public StatSkill(String name, List<Requirement> requirements,
                         Actor.Stat stat, int bonus) : base(name, requirements)
        {
            this.stat = stat;
            this.bonus = bonus;
        }
    }

    // ActiveSkill - Skills which are actively used in combat
    public class ActiveSkill : Skill
    {
        int mpCost;

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
