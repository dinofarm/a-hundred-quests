
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace OHQData.Actors
{
    public class Monster : Actor
    {
        private List<String> quotes;
        private List<Terrain> areas;
        private List<Monster> friends;

        public Monster(String name, Race race, Gender gender, Point battleCoordinates,
                       List<String> quotes, List<Terrain> areas, List<Monster> friends)
            : base(name, race, gender, battleCoordinates)
        {
            this.quotes = quotes;
            this.areas = areas;
            this.friends = friends;
        }
    }
}
