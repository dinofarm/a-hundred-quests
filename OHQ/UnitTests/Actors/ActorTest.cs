using System;
using System.Collections.Generic;
using System.Text;
using OHQData.Actors;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Microsoft.Xna.Framework;
using OHQData.Items;

namespace UnitTests.Actors
{
    [TestFixture]
    public class ActorTest
    {
        Avatar actor;

        [TestFixtureSetUp]
        protected void SetUp()
        {
        }

        [SetUp]
        public void Initialize()
        {
            // actor
            Point mapCoordinates = new Point(5, 5);
            actor = new Avatar("Bob", Actor.Races.Dinoman, Actor.Genders.Male, mapCoordinates);

            // stats
            Actor.StatsDatum stats = new Actor.StatsDatum();
            stats.Cha = 5;
            stats.Con = 6;
            stats.Dex = 8;
            stats.Int = 6;
            stats.Str = 4;
            actor.Stats = stats;

            // equipment
            actor.equipment.body = new Armor("Cloth Armor", 5, 10);
            actor.equipment.leftHand = new Weapon("Awesome Sword", 3, 960, 1);
            //actor.equipment.rightHand = null;
            actor.equipment.accessory = new Accessory("Crystal Amulet");
        }

        [Test]
        public void Construction()
        {
            Assert.That(actor.Name, Is.EqualTo("Bob"));
            Assert.That(actor.Race, Is.EqualTo(Actor.Races.Dinoman));
            Assert.That(actor.Gender, Is.EqualTo(Actor.Genders.Male));
            Assert.IsTrue(actor.Hp == 0);
            Assert.IsTrue(actor.Mp == 0);
            
            Assert.IsTrue(actor.BattleCoordinates == new Point(0,0));
            Assert.That(actor.Experience, Is.EqualTo(0));
            Assert.IsNull(actor.Sprite);
            Assert.That(actor.StatusEffects.Count == 0);
            Assert.That(actor.Skills.Count == 0);
        }

        [Test]
        public void Stats()
        {
            Assert.IsTrue(actor.Stats.Cha == 5);
            Assert.IsTrue(actor.Stats.Con == 6);
            Assert.IsTrue(actor.Stats.Dex == 8);
            Assert.IsTrue(actor.Stats.Int == 6);
            Assert.IsTrue(actor.Stats.Str == 4);
        }

        [Test]
        public void SecondaryStats()
        {
            Assert.That(actor.Stats.Con, Is.EqualTo(6));
            Assert.That(actor.MaxHP, Is.EqualTo(18)); // MaxHP is 3 times the Con stat

            Assert.That(actor.MaxMP, Is.EqualTo(36)); // MaxMP is Con * Int

            Assert.That(actor.MaxMovePoints, Is.EqualTo(2));  // MaxMovePoints is the floor of Dex * .1 + 2
            Assert.That(actor.ArmorClass, Is.EqualTo(13));   //Dex + AC
            Assert.That(actor.Weapon.range, Is.EqualTo(1)); 
            Assert.AreEqual(actor.HpRegenPerTurn, 0.6); // Con / 10.0
            Assert.AreEqual(actor.MpRegenPerTurn, 0.6); // Int / 10.0

        }


        [Test]
        public void AddExperience()
        {
            actor.addExperience(5);
            Assert.That(actor.Experience, Is.EqualTo(5));
        }

        [Test]
        public void Level()
        {

        }

        [Test]
        public void Equipment()
        {
            Assert.IsNotNull(actor.equipment.leftHand);
            Assert.IsNotNull(actor.equipment.accessory);
            Assert.IsNotNull(actor.equipment.body);
            Assert.IsNull(actor.equipment.rightHand);
        }    


        //[Test]
        //public void Pause()
        //{
        //    animation.pause();
        //    Assert.That(animation.State, Is.EqualTo(AnimationState.Stopped));
        //    animation.unpause();
        //    Assert.That(animation.State, Is.EqualTo(AnimationState.Stopped));

        //    animation.play();
        //    animation.pause();
        //    Assert.That(animation.State, Is.EqualTo(AnimationState.Paused));
        //    animation.unpause();
        //    Assert.That(animation.State, Is.EqualTo(AnimationState.Playing));
        //}

        //[Test]
        //public void Play()
        //{
        //    animation.stop();
        //    animation.play();
        //    Assert.That(animation.State, Is.EqualTo(AnimationState.Playing));
        //}

        //[Test]
        //public void Stop()
        //{
        //    animation.stop();
        //    Assert.That(animation.State, Is.EqualTo(AnimationState.Stopped));
        //}
    }
}
