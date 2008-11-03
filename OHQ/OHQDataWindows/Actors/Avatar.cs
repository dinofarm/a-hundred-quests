using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace OHQData.Actors
{
    public class Avatar : Hero
    {
        Point mapCoordinates;

        public Avatar(String name, Race race, Gender gender, Point mapCoordinates)
            : base(name, race, gender)
        {
            this.mapCoordinates = mapCoordinates;
        }
    }
}
