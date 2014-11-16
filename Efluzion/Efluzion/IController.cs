using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Efluzion
{
    public interface IController
    {
        //Gets the current registered pointer position.
        Point CurrentPosition { get; }

        //Gets the prev registered pointer position.
        Point PrevPosition { get; }

        //Gets the position of the last time the pointer was activated
        Point LastPointerActivate { get; }

        //Gets the position of the last time the pointer was released.
        Point LastPointerInactivate { get; }

        //Returns true if pointer was just clicked
        bool FreshRelease { get; }

        //Returns true if pointer was just released
        bool FreshActivate { get; }

        //Returns true if the pointer is down and being dragged.
        bool PointerDragging { get; }

        //Returns true if the pointer is being held down.
        bool PointerHolding { get; }

        //Update the state of the controls.
        void Update(GameTime gt);
    }
}
