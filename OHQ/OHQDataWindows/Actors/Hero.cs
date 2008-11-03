using System;
using System.Collections.Generic;
using System.Text;
using OHQData;
using OHQData.Items;

namespace OHQData.Actors
{
    public class Hero : Actor
    {
        
        
        Player owner;  // TODO: review this var

        public Hero(String name, Races race, Genders gender)
            : base(name, race, gender) { }
        public Hero(String name, Races race, Genders gender, Player owner)
            : base(name, race, gender)
        {
            this.owner = owner;
        }

        // leftHand == true <-- equip to left hand
        // leftHand == false <-- equip to right hand
        public void equip(Weapon weapon, Item.Slot slot)
        {
            switch (slot)
            {
                case Item.Slot.LeftHand:
                    equipment.leftHand = weapon;
                    break;
                case Item.Slot.RightHand:
                    equipment.rightHand = weapon;
                    break;
                default:
                    throw new Exception("Weapons can only be equipped to the left or right hand.");
                    break;
            }
        }
        public void equip(Armor armor)
        {
            equipment.body = armor;
        }
        public void equip(Accessory accessory)
        {
            equipment.accessory = accessory;
        }
    }
}
