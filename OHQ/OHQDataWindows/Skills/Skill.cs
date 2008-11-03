using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework.SyntaxHelpers;
using OHQData.Actors;

namespace OHQData.Skills
{
    public class Skill
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

    public class StatSkill : Skill
    {
        public Actor.Stat stat; // character stat to modify
        public int bonus; // points to modify the stat by

        public StatSkill(String name, List<Requirements> requirements,
                         Actor.Stat stat, int bonus) : base(name, requirements)
        {
            this.stat = stat;
            this.bonus = bonus;
        }
    }
    public class ActiveSkill : Skill
    {
        int mpCost;

        public ActiveSkill(String name, List<Requirement> requirements, int mpCost)
            : base(name, requirements)
        {
            this.mpCost = mpCost;
        }
    }
    public class PassiveSkill : Skill
    {

    }
    public class MagicSkill : Skill
    {

    }
}
