using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Efluzion
{
    public interface IBoard : IDrawable
    {
        //List of IInteractive objects on the board
        List<IInteractive> InteractiveObjects { get; }

        //Returns true if the board is Active.
        bool Active { get; }

        //Activates this board.
        void Activate();

        //Update method
        void Update(GameTime gt);

        //Registers an object with this board
        void RegisterObject(IInteractive obj);
    }
}
