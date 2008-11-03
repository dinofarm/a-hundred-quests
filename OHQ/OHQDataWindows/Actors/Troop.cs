using System;
using System.Collections.Generic;
using System.Text;

namespace OHQData.Actors
{
    public class Troop : Actor
    {
        private int cost;

        public Troop(String name, Races race, Genders gender, int cost)
            : base(name, race, gender)
        {
            this.cost = cost;
        }
    }
}

