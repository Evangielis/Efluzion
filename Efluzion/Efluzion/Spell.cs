using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Efluzion
{
    public enum Spells
    {
        GlobalLux,
        Luxball,
        Luxbolt,
        FinalLight,
        GlobalPyre,
        Pyreball,
        Pyrebolt,
        Pyroblast,
        GlobalArc,
        Arcball,
        Arcbolt,
        Arcstream,
        None,
    };

    public struct SpellToken
    {
        public SpellToken(Spells spell, int col)
        {
            _spellName = spell;
            _col = col;
        }

        public Spells _spellName;
        public int _col;
    }

    public abstract class Spell : IDrawable, IPhysical
    {   
        public bool Active { get; private set; }
        public DamageTypes DmgType { get; protected set; }
        public int Damage { get; private set; }
        public int Lifespan { get; private set; }
        public Vector2 Speed { get; private set; }
        Vector2 _loc;
        Rectangle _area;

        public Spell(Rectangle Rectangle, Texture2D Texture, DamageTypes DmgType, int Damage, bool Active, int Lifespan, float Speed)
        {
            this.Texture = Texture;
            _loc = new Vector2(Rectangle.X, Rectangle.Y);
            _area = Rectangle;
            this.DmgType = DmgType;
            this.Damage = Damage;
            this.Active = Active;
            this.Lifespan = Lifespan;
            this.Speed = new Vector2(0, -Speed);
        }

        public virtual void OnCollision(Mob Target) 
        {
            Target.TakeDamage(new DamageToken(this.DmgType, this.Damage));
            this.Active = false;
        }
        public void Kill()
        {
            this.Active = false;
            this.Lifespan = 0;
        }
        public Rectangle Rectangle 
        { 
            get { return new Rectangle((int)_loc.X, (int)_loc.Y, _area.Width, _area.Height); }
        }
        public Texture2D Texture { get; protected set; }
        public void Update(GameTime gt) 
        {
            if (!Active) Lifespan--;
            else
                _loc += Speed;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, _loc, Color.White);
        }

        ColliderType IPhysical.Type
        {
            get { return ColliderType.Spell; }
        }
        bool IPhysical.Active
        {
            get { return this.Active; }
        }
        bool IPhysical.Dead
        {
            get { return (!Active && Lifespan <= 0) ? true : false; }
        }
        Rectangle IPhysical.Area
        {
            get { return this.Rectangle; }
        }
    }

    public class BoltSpell : Spell
    {
        public BoltSpell(Rectangle SpawnRect, RuneType Type) 
            : base(SpawnRect, null, DamageTypes.Ion, 20, true, 1, 10f)
        {
            switch (Type)
            {
                case RuneType.Eron:
                    DmgType = DamageTypes.Ion;
                    Texture = GameConfig.DrawTextures[GameAssets.SpellLuxbolt];
                    break;
                case RuneType.Flaz:
                    DmgType = DamageTypes.Heat;
                    Texture = GameConfig.DrawTextures[GameAssets.SpellPyrebolt];
                    break;
                case RuneType.Luin:
                    DmgType = DamageTypes.Arc;
                    Texture = GameConfig.DrawTextures[GameAssets.SpellArcbolt];
                    break;
            }
        }
        public override void OnCollision(Mob Target)
        {
            base.OnCollision(Target);
        }
    }

    public class BallSpell : Spell
    {
        public BallSpell(Rectangle SpawnRect, RuneType Type)
            : base(SpawnRect, null, DamageTypes.Ion, 10, true, 1, 10f)
        {
            switch (Type)
            {
                case RuneType.Eron:
                    DmgType = DamageTypes.Ion;
                    Texture = GameConfig.DrawTextures[GameAssets.SpellLuxball];
                    break;
                case RuneType.Flaz:
                    DmgType = DamageTypes.Heat;
                    Texture = GameConfig.DrawTextures[GameAssets.SpellPyreball];
                    break;
                case RuneType.Luin:
                    DmgType = DamageTypes.Arc;
                    Texture = GameConfig.DrawTextures[GameAssets.SpellArcball];
                    break;
            }
        }
        public override void OnCollision(Mob Target)
        {
            base.OnCollision(Target);
        }
    }

    public class UltSpell : Spell
    {
        public UltSpell(Rectangle SpawnRect, RuneType Type)
            : base(SpawnRect, null, DamageTypes.Ion, 30, true, 50, 0f)
        {
            switch (Type)
            {
                case RuneType.Eron:
                    DmgType = DamageTypes.Ion;
                    Texture = GameConfig.DrawTextures[GameAssets.SpellFinalLight];
                    break;
                case RuneType.Flaz:
                    DmgType = DamageTypes.Heat;
                    Texture = GameConfig.DrawTextures[GameAssets.SpellPyroblast];
                    break;
                case RuneType.Luin:
                    DmgType = DamageTypes.Arc;
                    Texture = GameConfig.DrawTextures[GameAssets.SpellArcstream];
                    break;
            }
        }

        public override void OnCollision(Mob Target)
        {
            base.OnCollision(Target);
        }
    }
}
