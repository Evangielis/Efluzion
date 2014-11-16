using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Efluzion
{
    public class Stage : IDrawable
    {
        //A struct used to handle collision binaries
        public struct Collision
        {
            Spell _mySpell;
            public Spell SpellCollider { get { return _mySpell; } }
            Mob _myMob;
            public Mob MobCollider { get { return _myMob; } }

            public Collision(IPhysical spell, IPhysical mob)
            {
                _mySpell = (Spell)spell;
                _myMob = (Mob)mob;
            }
        }

        public bool Active { get; set; }
        IBoard _board;
        List<IPhysical> _mobObjs, _spellObjs;
        Stack<Collision> _collisionsPending;
        Stack<Mobs> _finishLineMobs, _killedMobs;
        public Stack<Mobs> FinishLineMobs { get { return _finishLineMobs; } }
        public Stack<Mobs> KilledMobs { get { return _killedMobs; } }

        public Stage(IBoard Board)
        {
            Active = false;
            _board = Board;
            _spellObjs = new List<IPhysical>();
            _mobObjs = new List<IPhysical>();
            _collisionsPending = new Stack<Collision>();
            _finishLineMobs = new Stack<Mobs>();
            _killedMobs = new Stack<Mobs>();
        }

        public void SpawnSpell(SpellToken Token)
        {
            switch (Token._spellName)
            {
                case Spells.Luxbolt:
                    _spellObjs.Add(new BoltSpell(GetSpellSpawnArea(Token._col), RuneType.Eron));
                    break;
                case Spells.Arcbolt:
                    _spellObjs.Add(new BoltSpell(GetSpellSpawnArea(Token._col), RuneType.Luin));
                    break;
                case Spells.Pyrebolt:
                    _spellObjs.Add(new BoltSpell(GetSpellSpawnArea(Token._col), RuneType.Flaz));
                    break;
                case Spells.Luxball:
                    _spellObjs.Add(new BallSpell(GetSpellSpawnArea(Token._col), RuneType.Eron));
                    break;
                case Spells.Arcball:
                    _spellObjs.Add(new BallSpell(GetSpellSpawnArea(Token._col), RuneType.Luin));
                    break;
                case Spells.Pyreball:
                    _spellObjs.Add(new BallSpell(GetSpellSpawnArea(Token._col), RuneType.Flaz));
                    break;
                case Spells.GlobalLux:
                    SpawnSpell(new SpellToken(Spells.Luxball,0));
                    SpawnSpell(new SpellToken(Spells.Luxball,1));
                    SpawnSpell(new SpellToken(Spells.Luxball,2));
                    break;
                case Spells.GlobalPyre:
                    SpawnSpell(new SpellToken(Spells.Pyreball, 0));
                    SpawnSpell(new SpellToken(Spells.Pyreball, 1));
                    SpawnSpell(new SpellToken(Spells.Pyreball, 2));
                    break;
                case Spells.GlobalArc:
                    SpawnSpell(new SpellToken(Spells.Arcball, 0));
                    SpawnSpell(new SpellToken(Spells.Arcball, 1));
                    SpawnSpell(new SpellToken(Spells.Arcball, 2));
                    break;
                case Spells.FinalLight:
                    _spellObjs.Add(new UltSpell(GetSpellSpawnArea(Token._col, true), RuneType.Eron));
                    break;
                case Spells.Arcstream:
                    _spellObjs.Add(new UltSpell(GetSpellSpawnArea(Token._col, true), RuneType.Luin));
                    break;
                case Spells.Pyroblast:
                    _spellObjs.Add(new UltSpell(GetSpellSpawnArea(Token._col, true), RuneType.Flaz));
                    break;
                default:
                    break;
            }
        }
        public Rectangle GetSpellSpawnArea(int col, bool ult=false)
        {
            if (ult)
                switch (col)
                {
                    case 0: return GameConfig.DrawLocations[GameAssets.SpawnUltSpell1];
                    case 1: return GameConfig.DrawLocations[GameAssets.SpawnUltSpell2];
                    case 2: return GameConfig.DrawLocations[GameAssets.SpawnUltSpell3];
                    default: return Rectangle.Empty;
                }
            else
                switch (col)
                {
                    case 0: return GameConfig.DrawLocations[GameAssets.SpawnSpell1];
                    case 1: return GameConfig.DrawLocations[GameAssets.SpawnSpell2];
                    case 2: return GameConfig.DrawLocations[GameAssets.SpawnSpell3];
                    default: return Rectangle.Empty;
                }
        }
        public void SpawnMob(MobToken Token)
        {
            Rectangle SpawnArea;
            Mob SpawnMob = null;
            switch (Token._col)
            {
                case 0:
                    SpawnArea = GameConfig.DrawLocations[GameAssets.SpawnMob1];
                    break;
                case 1:
                    SpawnArea = GameConfig.DrawLocations[GameAssets.SpawnMob2];
                    break;
                case 2:
                    SpawnArea = GameConfig.DrawLocations[GameAssets.SpawnMob3];
                    break;
                default:
                    SpawnArea = GameConfig.DrawLocations[GameAssets.SpawnMob3];
                    break;
            }

            switch (Token._mobName)
            {
                case Mobs.Target:
                    SpawnMob = new TargetMob(SpawnArea);
                    break;
                case Mobs.EronSprite:
                    SpawnMob = new SpriteMob(SpawnArea, RuneType.Eron);
                    break;
                case Mobs.FlazSprite:
                    SpawnMob = new SpriteMob(SpawnArea, RuneType.Flaz);
                    break;
                case Mobs.LuinSprite:
                    SpawnMob = new SpriteMob(SpawnArea, RuneType.Luin);
                    break;
                default:
                    break;
            }
            if (SpawnMob != null)
            {
                _mobObjs.Add(SpawnMob);
            }
        }
        public void Update(GameTime gt)
        {
            _finishLineMobs.Clear();
            _killedMobs.Clear();

            CheckSpellOutOfBounds();
            CheckSpellCollision();
            ResolveCollisions();
            ScoreDeadMobs();
            CheckMobOutOfBounds();
            CleanUpLists();

            foreach (IPhysical o in _spellObjs)
                o.Update(gt);
            foreach (IPhysical o in _mobObjs)
                o.Update(gt);
        }
        private void ScoreDeadMobs()
        {
            _mobObjs.FindAll(s => s.Dead).ForEach(m => _killedMobs.Push((m as Mob).Type));
        }
        private void CleanUpLists()
        {
            _spellObjs.RemoveAll(s => s.Dead);
            _mobObjs.RemoveAll(s => s.Dead);
        }
        public Rectangle Rectangle
        {
            get { return GameConfig.DrawLocations[GameAssets.Stage]; }
        }
        public Texture2D Texture
        {
            get { return GameConfig.DrawTextures[GameAssets.Stage]; }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Texture, this.Rectangle, Color.White);
            foreach (Spell s in _spellObjs)
                s.Draw(spriteBatch);
            foreach (Mob m in _mobObjs)
                m.Draw(spriteBatch);
        }
        
        #region Collision Methods
        private void OnFinishLine(Mob mob)
        {
            mob.Kill();
            _finishLineMobs.Push(mob.Type);
        }
        private void CheckMobOutOfBounds()
        {
            foreach (IPhysical o in _mobObjs)
            {
                if (o.Area.Y + o.Area.Height >= this.Rectangle.Y + this.Rectangle.Height)
                    OnFinishLine((Mob)o);
            }
        }
        private void CheckSpellOutOfBounds()
        {
            foreach (IPhysical o in _spellObjs)
            {
                if (o.Area.Y <= this.Rectangle.Y)
                    (o as Spell).Kill();
            }
        }
        private void CheckSpellCollision()
        {
            foreach (IPhysical spell in _spellObjs)
                if (spell.Active)
                    foreach (IPhysical mob in _mobObjs)
                        if (spell.Area.Intersects(mob.Area))
                            _collisionsPending.Push(new Collision(spell, mob));
        }
        private void ResolveCollisions()
        {
            Collision c;
            while (true)
            {
                try { c = _collisionsPending.Pop(); }
                catch (InvalidOperationException e) { break; }
                c.SpellCollider.OnCollision(c.MobCollider);
            }
        }
        #endregion
    }
}
