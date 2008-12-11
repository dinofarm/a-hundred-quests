using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using OHQData.Quests;

namespace OHQData.Actors
{
    public class Avatar : Hero
    {
        Point mapCoordinates;
        public List<Quests.Quest> CompQuests = new List<Quests.Quest>();
        public List<Quests.Quest> CurrentQuests = new List<Quests.Quest>();


        public Avatar(String name, Races race, Genders gender, Point mapCoordinates)
            : base(name, race, gender)
        {
            this.mapCoordinates = mapCoordinates;
            this.CompQuests = null;
        }
    }
}
