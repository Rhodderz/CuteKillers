using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using GameStateManagement;
using System.Threading;

namespace CuteKillers
{
    class character
    {
        string cName;
        string cRace;
        double cHealth;
        double cMana;
        double cStrength;
        string starting_melee_wep;
        double cArmour;
        double cSpeed;
        double cJump;
        string cClass;
        Texture2D spr_head;
        Texture2D spr_body;
        Texture2D spr_arms;
        Texture2D spr_legs;
        string description;

        public character(
            string name, string race, double health, double mana, double strength,
            string starting_wep, double armour, double speed, double jump, string chClass,
            Texture2D head, Texture2D body, Texture2D arms, Texture2D legs, string descrip)
        {
            cName = name;
            cRace = race;
            cHealth = health;
            cMana = mana;
            cStrength = strength;
            starting_melee_wep = starting_wep;
            cArmour = armour;
            cSpeed = speed;
            cJump = jump;
            cClass = chClass;
            spr_head = head;
            spr_body = body;
            spr_arms = arms;
            spr_legs = legs;
            description = descrip;
        }
    }
}
