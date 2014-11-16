using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Efluzion
{
    public static class GameControl
    {
        public static Player ActivePlayer { get; set; }

        #region Lists and Stuff
        private static IController _controller;
        private static List<IInteractive> _interactiveObjects;
        private static List<IInteractive> _tapObjs, _touchObjs, _dragObjs, _holdObjs;
        #endregion

        #region Registration Methods
        //private static void RegisterCollision(Collision c) { _collisionsPending.Push(c); }
        private static void RegisterObject(IInteractive obj) { _interactiveObjects.Add(obj); }
        //private static void RegisterObject(IPhysical obj) { _collisionObjects.Add(obj); }
        private static void RegisterList(List<IInteractive> olist)
        {
            foreach (IInteractive o in olist)
                _interactiveObjects.Add(o);
        }
        private static void ClearRegistrations() 
        { 
            _interactiveObjects.Clear();
            _tapObjs.Clear();
            _touchObjs.Clear();
            _dragObjs.Clear();
            _holdObjs.Clear();
        }
        #endregion

        public static void LoadBoard(IBoard board)
        {
            ClearRegistrations();
            RegisterList(board.InteractiveObjects);
            _tapObjs = _interactiveObjects.FindAll(o => o.RegisteredEvents.ContainsKey(EventType.OnTap));
            _touchObjs = _interactiveObjects.FindAll(o => o.RegisteredEvents.ContainsKey(EventType.OnTouch));
            _holdObjs = _interactiveObjects.FindAll(o => o.RegisteredEvents.ContainsKey(EventType.OnHold));
            _dragObjs = _interactiveObjects.FindAll(o => o.RegisteredEvents.ContainsKey(EventType.OnDrag));
        }
        public static void LoadController(IController Controls)
        {
            _controller = Controls;
            _interactiveObjects = new List<IInteractive>();
            _tapObjs = new List<IInteractive>();
            _touchObjs = new List<IInteractive>();
            _holdObjs = new List<IInteractive>();
            _dragObjs = new List<IInteractive>();
        }
        public static void Update(GameTime gt)
        {
            _controller.Update(gt);
            CheckTap();
            //CheckTouch();
            //CheckHold();
            //CheckDrag();
        }

        #region Interactivity Methods
        private static void CheckTap()
        {
            if (_controller.FreshRelease)
                foreach (IInteractive o in _tapObjs)
                    if (o.Area.Contains(_controller.LastPointerActivate)
                        && o.Area.Contains(_controller.LastPointerInactivate))
                        o.RegisteredEvents[EventType.OnTap].Invoke();
        }
        private static void CheckTouch()
        {
            throw new NotImplementedException();
        }
        private static void CheckHold()
        {
            throw new NotImplementedException();
        }
        private static void CheckDrag()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
