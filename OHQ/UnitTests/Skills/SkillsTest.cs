using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using OHQData.Skills;
using OHQData.Actors;


namespace UnitTests.Skills
{
    [TestFixture]
    public class SkillsTest
    {
        StatSkill skill;

        [SetUp]
        public void Initialize()
        {
            int bonus = 5;
            List<Skill.Requirement> requirements = null;
            skill = new StatSkill("Charisma up", requirements, Actor.StatTypes.Cha, bonus);
        }

        [Test]
        public void Constructor()
        {
            Assert.That(skill.Name, Is.EqualTo("Charisma up"));
            Assert.IsNull(skill.Requirements);
        }

        [Test]
        public void Requirements()
        {
            // TODO: test a skill with requirements of other skills at certain levels
            List<Skill.Requirement> requirements = new List<Skill.Requirement>();
            Skill.Requirement req = new Skill.Requirement(skill, 1);
            Assert.That(req.level, Is.EqualTo(1));
            Assert.That(req.skill, Is.EqualTo(skill));
            requirements.Add(req);

            int bonus = 3;
            StatSkill requirementSkill = new StatSkill("Constitution up", requirements, Actor.StatTypes.Con, bonus);

            Assert.That(requirementSkill.Requirements.Count, Is.EqualTo(1));
        }
    }

    [TestFixture]
    public class ActiveSkillTest
    {
        ActiveSkill activeSkill;

        [SetUp]
        public void Initialize()
        {
            int mpCost = 5;
            List<Skill.Requirement> requirements = null;
            activeSkill = new ActiveSkill("War Howl", null, mpCost);
        }

        [Test]
        public void Constructor()
        {
            Assert.That(activeSkill.MpCost, Is.EqualTo(5));
        }
    }
}
