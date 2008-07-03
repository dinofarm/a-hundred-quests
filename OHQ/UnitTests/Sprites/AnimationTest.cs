using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using OHQData.Sprites;

namespace OHQData
{
    [TestFixture]
    public class AnimationTest
    {
        private Animation animation;

        [SetUp]
        protected void SetUp()
        {
            animation = new Animation("grass", 5, 10, true);
        }

        [Test]
        public void Construction()
        {
            Assert.That(animation.TextureName, Is.EqualTo("grass"));
            Assert.That(animation.FrameCount, Is.EqualTo(5));
            Assert.That(animation.FramesPerSecond, Is.EqualTo(10));
            Assert.That(animation.Looping, Is.True);
            Assert.That(animation.CurrentFrame, Is.EqualTo(1));
            Assert.That(animation.State, Is.EqualTo(AnimationState.Stopped));
            Assert.That(animation.TimeElapsed, Is.EqualTo(0));
        }

        [Test]
        public void Pause()
        {
            animation.pause();
            Assert.That(animation.State, Is.EqualTo(AnimationState.Stopped));
            animation.unpause();
            Assert.That(animation.State, Is.EqualTo(AnimationState.Stopped));

            animation.play();
            animation.pause();
            Assert.That(animation.State, Is.EqualTo(AnimationState.Paused));
            animation.unpause();
            Assert.That(animation.State, Is.EqualTo(AnimationState.Playing));
        }

        [Test]
        public void Play()
        {
            animation.stop();
            animation.play();
            Assert.That(animation.State, Is.EqualTo(AnimationState.Playing));
        }

        [Test]
        public void Stop()
        {
            animation.stop();
            Assert.That(animation.State, Is.EqualTo(AnimationState.Stopped));
        }
    }
}
