using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Efluzion
{
    public interface IDrawable
    {
        Rectangle Rectangle { get; }

        //The texture of the sprite.
        Texture2D Texture { get; }

        //Draw method for the object.
        void Draw(SpriteBatch spriteBatch);
    }
}
