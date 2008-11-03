using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using OHQData;

namespace UnitTests
{
    [TestFixture]
    public class TerrainTest
    {
        Terrain terrain;

        [SetUp]
        protected void SetUp()
        {
            terrain = new Terrain();
        }

        [Test]
        public void Count()
        {
            Assert.That(terrain.Count, Is.EqualTo(1));
        }

        [Test]
        public void Name()
        {
            Assert.IsNotNull(terrain.Name);
            Assert.IsNotEmpty(terrain.Name);
            Assert.That(terrain.Name, Text.DoesNotContain("\\"));
        }
    }
}
