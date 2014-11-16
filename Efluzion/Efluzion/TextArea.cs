using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Efluzion
{
    public enum TextAlignment
    {
        Center,
        Left,
        Right,
    };

    public class TextArea
    {
        string _text;
        public string Text 
        {
            get { return _text; }
            set
            {
                _textLength = Font.MeasureString(value);
                _text = value;
                _textOrigin = CalculateOrigin(Align, _textLength.X, _textLength.Y);
            }
        }
        public Color DrawColor { get; set; }
        SpriteFont Font { get; set; }
        TextAlignment Align { get; set; }
        Vector2 _loc, _lockPoint, _textOrigin, _textLength;

        public TextArea(SpriteFont Font, TextAlignment Align, Rectangle Area)
        {
            this.Font = Font;
            this.Align = Align;
            this.Text = String.Empty;
            _textLength = _textOrigin = Vector2.Zero;
            _loc = new Vector2(Area.X, Area.Y);
            _lockPoint = CalculateOrigin(Align, Area.Width, Area.Height);
            DrawColor = Color.Red;
        }

        private Vector2 CalculateOrigin(TextAlignment Align, float Width, float Height)
        {
            float vx = 0, vy = 0;
            switch (Align)
            {
                case TextAlignment.Center:
                    vx = Width / 2; vy = Height/2; break;
                case TextAlignment.Left:
                    vx = 0; vy = Height/2; break;
                case TextAlignment.Right:
                    vx = Width; vy = Height / 2; break;
            }
            return new Vector2(vx, vy);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Font, Text, _loc + _lockPoint, DrawColor, 0f, _textOrigin, 1f, SpriteEffects.None, 0f);
        }
    }
}
