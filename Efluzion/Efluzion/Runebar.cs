using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Efluzion
{
    public enum RuneType
    {
        Eron,
        Flaz,
        Luin,
    };

    public class Runebar : IDrawable
    {
        class Rune
        {
            Texture2D _texture, _litTexture;
            bool _lit;
            float _scale;
            bool _scaleUp;

            public RuneType RuneType { get; private set; }

            public Rune(Texture2D Texture, Texture2D LitTexture, RuneType RuneType)
            {
                _texture = Texture;
                _litTexture = LitTexture;
                _lit = false;
                this.RuneType = RuneType;

                //Animation
                _scale = 1f;
                _scaleUp = true;
            }

            public void Update(GameTime gt)
            {
            }

            public void Draw(SpriteBatch Batch, Rectangle Destination)
            {
                if (_lit) Batch.Draw(_litTexture, Destination, Color.White);
                else
                    Batch.Draw(_texture, Destination, Color.White);
            }

            public void Light() { _lit = true; }
            public void UnLight() { _lit = false; }
        }
        class RuneBag
        {
            LinkedList<Rune> _bag;
            Random _rand;

            public RuneBag()
            {
                _bag = new LinkedList<Rune>();
                _rand = new Random();

                while (_bag.Count < 27)
                {
                    _bag.AddLast(new Rune(GameConfig.DrawTextures[GameAssets.EronRune],
                        GameConfig.DrawTextures[GameAssets.EronRuneLit], RuneType.Eron));
                    _bag.AddLast(new Rune(GameConfig.DrawTextures[GameAssets.FlazRune],
                        GameConfig.DrawTextures[GameAssets.FlazRuneLit], RuneType.Flaz));
                    _bag.AddLast(new Rune(GameConfig.DrawTextures[GameAssets.LuinRune],
                        GameConfig.DrawTextures[GameAssets.LuinRuneLit], RuneType.Luin));
                }
            }

            public Rune DrawRune()
            {
                int index = _rand.Next(_bag.Count);
                LinkedListNode<Rune> node = _bag.First;
                while (node.Next != null)
                {
                    if (index == 0) break;
                    node = node.Next;
                    index--;
                }
                Rune r = node.Value;
                _bag.Remove(node);
                return r;
            }

            public void AddRune(Rune r)
            {
                r.UnLight();
                _bag.AddLast(r);
            }
        }
        class RuneSlot : IInteractive
        {
            Rectangle _area;
            bool _active;
            Runebar _bar;
            public Rune SlotRune { get; set; }
            public bool Selected { get; private set; }
            Dictionary<EventType, EventDelegate> _events;

            //Events
            public void OnTap() 
            { 
                Selected ^= true;
                if (Selected) SlotRune.Light();
                else SlotRune.UnLight();
            }

            public RuneSlot(Rectangle Area, Runebar Bar)
            {
                //Set events
                _events = new Dictionary<EventType, EventDelegate>();
                _events.Add(EventType.OnTap, OnTap);

                _area = Area;
                _active = true;
                Selected = false; 
                _bar = Bar;
                this.SlotRune = null;
                _bar._board.RegisterObject(this);
            }

            public void Draw(SpriteBatch sb)
            {
                SlotRune.Draw(sb, _area);
            }

            public void Unselect() { Selected = false; }

            Rectangle IInteractive.Area { get { return _area; } }
            Dictionary<EventType, EventDelegate> IInteractive.RegisteredEvents { get { return _events; } }
            bool IInteractive.IsActive { get { return _active; } }
        }

        public bool Active { get; set; }
        RuneSlot[] _bar;
        RuneBag _bag;
        List<int> _selections;
        protected IBoard _board;
        SpellToken _readySpell;
        public SpellToken ReadySpell { get { return _readySpell; } }

        public Runebar(IBoard Board)
        {
            Active = false;
            _board = Board;
            _selections = new List<int>();
            _bar = new RuneSlot[9];
            _bag = new RuneBag();
            _readySpell = new SpellToken(Spells.None, 0);
            InitializeRunebar();
        }

        void InitializeRunebar()
        {
            _bar[0] = new RuneSlot(GameConfig.DrawLocations[GameAssets.RuneSlot1], this);
            _bar[1] = new RuneSlot(GameConfig.DrawLocations[GameAssets.RuneSlot2], this);
            _bar[2] = new RuneSlot(GameConfig.DrawLocations[GameAssets.RuneSlot3], this);
            _bar[3] = new RuneSlot(GameConfig.DrawLocations[GameAssets.RuneSlot4], this);
            _bar[4] = new RuneSlot(GameConfig.DrawLocations[GameAssets.RuneSlot5], this);
            _bar[5] = new RuneSlot(GameConfig.DrawLocations[GameAssets.RuneSlot6], this);
            _bar[6] = new RuneSlot(GameConfig.DrawLocations[GameAssets.RuneSlot7], this);
            _bar[7] = new RuneSlot(GameConfig.DrawLocations[GameAssets.RuneSlot8], this);
            _bar[8] = new RuneSlot(GameConfig.DrawLocations[GameAssets.RuneSlot9], this);

            foreach (RuneSlot s in _bar)
                s.SlotRune = _bag.DrawRune();
        }

        public void Update(GameTime gt)
        {
            _readySpell._spellName = Spells.None;
            _selections.Clear();
            for (int i = 0; i < _bar.Length; i++)
                if (_bar[i].Selected)
                    _selections.Add(i);
            if (_selections.Count == 3)
            {
                ResolveSpell();
                CycleRunes();
            }
        }

        void CycleRunes()
        {
            foreach (RuneSlot s in _bar)
            {
                if (s.Selected)
                {
                    Rune r = s.SlotRune;
                    s.SlotRune = _bag.DrawRune();
                    _bag.AddRune(r);
                    s.Unselect();
                }
            }
        }

        void ResolveSpell()
        {
            List<Rune>[] cols = new List<Rune>[3] { new List<Rune>(), new List<Rune>(), new List<Rune>() };
            Random rand = new Random();
            bool sameType = false, sameCol = false, targeted = false;
            int targetCol = -1, targetType;
            SortedSet<RuneType> types = new SortedSet<RuneType>();

            //Sort selected Runes into columns.
            foreach (int c in _selections)
            {
                int i = c / 3;
                cols[i].Add(_bar[c].SlotRune);
            }

            //Run flag checks
            for (int i = 0; i < 3; i++)
            {
                if (cols[i].Count > 1)
                {
                    targetCol = i;
                    targeted = true;
                    //Check for same typing
                    foreach (Rune r in cols[i])
                        types.Add(r.RuneType);
                    if (types.Count == 1)
                        sameType = true;
                    if (cols[i].Count == 3)
                        sameCol = true;
                }
            }

            //Resolve type conflicts
            if (!targeted)
                foreach (List<Rune> l in cols)
                    types.Add(l[0].RuneType);
            targetType = rand.Next(types.Count);

            //Choose spell
            if (sameCol && sameType) { UltSpell(types.ElementAt(targetType), targetCol); }
            else if (sameCol && !sameType) { LaneSpell(types.ElementAt(targetType), targetCol); }
            else if (targeted && !sameCol) { LaneSpell(types.ElementAt(targetType), targetCol); }
            else GlobalSpell(types.ElementAt(targetType));
        }

        void UltSpell(RuneType Type, int TargetCol)
        {
            Spells SpellName = Spells.None;
            switch (Type)
            {
                case RuneType.Eron: SpellName = Spells.FinalLight; break;
                case RuneType.Flaz: SpellName = Spells.Pyroblast; break;
                case RuneType.Luin: SpellName = Spells.Arcstream; break;
            }
            _readySpell._spellName = SpellName;
            _readySpell._col = TargetCol;
        }
        void LaneSpell(RuneType Type, int TargetCol)
        {
            Spells SpellName = Spells.None;
            switch (Type)
            {
                case RuneType.Eron: SpellName = Spells.Luxbolt; break;
                case RuneType.Flaz: SpellName = Spells.Pyrebolt; break;
                case RuneType.Luin: SpellName = Spells.Arcbolt; break;
            }
            _readySpell._spellName = SpellName;
            _readySpell._col = TargetCol;
        }
        void GlobalSpell(RuneType Type)
        {
            Spells SpellName = Spells.None;
            switch (Type)
            {
                case RuneType.Eron: SpellName = Spells.GlobalLux; break;
                case RuneType.Flaz: SpellName = Spells.GlobalPyre; break;
                case RuneType.Luin: SpellName = Spells.GlobalArc; break;
            }
            _readySpell._spellName = SpellName;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(this.Texture, this.Rectangle, Color.White);
            foreach (RuneSlot s in _bar)
                s.Draw(sb);
        }

        public Rectangle Rectangle { get { return GameConfig.DrawLocations[GameAssets.Runebar]; } }

        public Texture2D Texture { get { return GameConfig.DrawTextures[GameAssets.Runebar]; } }
    }
}
