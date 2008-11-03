using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Content;

namespace UnitTests
{
    //[TestFixture]
    //public class AnimatingSpriteTest
    //{
    //    AnimatingSprite sprite;
    //    ContentManager content;

    //    [SetUp]
    //    protected void SetUp()
    //    {
    //        // content manager
    //        Uri codeBaseUri = new Uri(System.Reflection.Assembly.GetEntryAssembly().CodeBase);
    //        string rootDirectory = System.IO.Directory.GetParent(codeBaseUri.AbsolutePath).FullName;
    //        content = new ContentManager(Services, rootDirectory);

    //        // sprite sheet
    //        string path = @"Textures\Map\Terrains\SpriteSheet";
    //        SpriteSheet spriteSheet = content.Load<SpriteSheet>(path);

    //        // sprite
    //        sprite = new AnimatingSprite("water", spriteSheet);
    //    }

    //    [Test]
    //    public void stopAnimation()
    //    {
    //        sprite.stopAnimation();
    //        Assert.That(sprite.CurrentAnimation.State, Is.EqualTo(AnimationState.Stopped));
    //    }

    //    [Test]
    //    public void startAnimation()
    //    {
    //        sprite.startAnimation();
    //        Assert.That(sprite.CurrentAnimation.State, Is.EqualTo(AnimationState.Playing));
    //    }
    //}
}
