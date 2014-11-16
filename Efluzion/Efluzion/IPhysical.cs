using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Efluzion
{
    public enum ColliderType
    {
        Spell,
        Mob,
    };

    public interface IPhysical
    {
        //Physical layer of collision
        ColliderType Type { get; }

        //Object is active or not?
        bool Active { get; }

        //Object is dead or not?
        bool Dead { get; }
        
        //Area of collision
        Rectangle Area { get; }

        //Update
        void Update(GameTime gt);
    }
}
