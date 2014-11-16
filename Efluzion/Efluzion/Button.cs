using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Efluzion
{
    class Button : IInteractive, IDrawable
    {
        String _buttonText;
        Texture2D _texture;
        Rectangle _area;

        public Button(Texture2D Texture, Rectangle Area, String Text)
        {
            _area = Area;
            _texture = Texture;
            _buttonText = Text;
        }

        public void OnClick()
        {
        }

        Rectangle IInteractive.Area { get { return _area; } }
        Dictionary<EventType, EventDelegate> IInteractive.RegisteredEvents
        {
            get { throw new NotImplementedException(); }
        }

        bool IInteractive.IsActive
        {
            get { throw new NotImplementedException(); }
        }

        public Rectangle Rectangle
        {
            get { throw new NotImplementedException(); }
        }

        public Texture2D Texture
        {
            get { throw new NotImplementedException(); }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
    }
}
