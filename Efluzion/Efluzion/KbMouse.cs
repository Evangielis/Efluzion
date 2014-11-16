using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Efluzion
{
    public class KbMouse : IController
    {
        //KeyboardState _nowKeyboardState, _prevKeyboardState;
        MouseState _nowMouseState, _prevMouseState;
        Point _currentPos, _prevPos, _lastClick, _lastRelease;
        bool _justClicked, _justReleased, _isDragging, _isHolding;

        public KbMouse()
        {
            //_nowKeyboardState = _prevKeyboardState = Keyboard.GetState();
            _nowMouseState = _prevMouseState = Mouse.GetState();
            _currentPos = _prevPos = _lastClick = _lastRelease = Point.Zero;
            _justClicked = _justReleased = _isDragging = _isHolding = false;
        }

        public void Update(GameTime gt)
        {
            //Shuffle the states
            //UpdateKeyboard();
            UpdateMouse();

            CheckMouseJustReleased();
            CheckMouseJustPressed();
            CheckHolding();
            CheckDragging();
        }

        /*void UpdateKeyboard()
        {
            _prevKeyboardState = _nowKeyboardState;
            _nowKeyboardState = Keyboard.GetState();
        }*/

        void UpdateMouse()
        {
            _prevMouseState = _nowMouseState; 
            _nowMouseState = Mouse.GetState();
            _prevPos.X = _currentPos.X;
            _prevPos.Y = _currentPos.Y;
            _currentPos.X = _nowMouseState.X;
            _currentPos.Y = _nowMouseState.Y;
        }

        void CheckMouseJustReleased()
        {
            if (_prevMouseState.LeftButton.Equals(ButtonState.Pressed)
                && _nowMouseState.LeftButton.Equals(ButtonState.Released))
            {
                _justReleased = true;
                _lastRelease.X = _currentPos.X;
                _lastRelease.Y = _currentPos.Y;
            }
            else
                _justReleased = false;
        }
        void CheckMouseJustPressed()
        {
            if (_prevMouseState.LeftButton.Equals(ButtonState.Released)
                && _nowMouseState.LeftButton.Equals(ButtonState.Pressed))
            {
                _justClicked = true;
                _lastClick.X = _currentPos.X;
                _lastClick.Y = _currentPos.Y;
            }
            else
                _justClicked = false;
        }
        void CheckHolding()
        {
            if (_prevMouseState.LeftButton.Equals(ButtonState.Pressed)
                && _nowMouseState.LeftButton.Equals(ButtonState.Pressed))
                _isHolding = true;
            else
                _isHolding = false;
        }
        void CheckDragging()
        {
            if (_isHolding && _prevPos != _currentPos)
                _isDragging = true;
            else
                _isDragging = false;
        }

        #region IController Explicit Implementation
        void IController.Update(GameTime gt) { this.Update(gt); }
        Point IController.CurrentPosition { get { return _currentPos; } }
        Point IController.PrevPosition { get { return _prevPos; } }
        Point IController.LastPointerActivate { get { return _lastClick; } }
        Point IController.LastPointerInactivate { get { return _lastRelease; } }
        bool IController.FreshRelease { get { return _justReleased; } }
        bool IController.FreshActivate { get { return _justClicked; } }
        bool IController.PointerDragging { get { return _isDragging; } }
        bool IController.PointerHolding { get { return _isHolding; } }
        #endregion
    }
}
