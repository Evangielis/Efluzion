using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Efluzion
{
    public class MenuBoard : IBoard
    {
        Button _newGame, _exitGame;

        public MenuBoard()
        {

        }

        List<IInteractive> IBoard.InteractiveObjects
        {
            get { throw new NotImplementedException(); }
        }
        bool IBoard.Active
        {
            get { throw new NotImplementedException(); }
        }
        void IBoard.Activate()
        {
            throw new NotImplementedException();
        }
        void IBoard.Update(GameTime gt)
        {
            throw new NotImplementedException();
        }
        void IBoard.RegisterObject(IInteractive obj)
        {
            throw new NotImplementedException();
        }
        Rectangle IDrawable.Rectangle
        {
            get { throw new NotImplementedException(); }
        }
        Texture2D IDrawable.Texture
        {
            get { throw new NotImplementedException(); }
        }
        void IDrawable.Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
    }
}
