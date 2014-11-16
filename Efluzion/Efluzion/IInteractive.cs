using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Efluzion
{
    public enum EventType
    {
        OnTap,
        OnTouch,
        OnDrag,
        OnHold,
    };

    public delegate void EventDelegate();

    public interface IInteractive
    {
        //The area occupied by the object
        Rectangle Area { get; }

        //The events this object is registered for
        Dictionary<EventType, EventDelegate> RegisteredEvents { get; }

        //Determines if this object is active or not
        bool IsActive { get; }
    }
}
