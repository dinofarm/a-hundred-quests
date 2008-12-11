using System;
using System.Collections.Generic;
using System.Text;
using OHQData.Actors;

namespace OHQData
{
    public class Player
    {
        static public Avatar instance;
        public Avatar getAvatar()
        {
            return instance;
        }
    }
}
