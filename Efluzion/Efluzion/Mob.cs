using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Efluzion
{
    public enum Mobs
    {
        Target,
        FlazSprite,
        EronSprite,
        LuinSprite,
    };
    public enum DamageTypes
    {
        Heat,
        Arc,
        Ion,
        Force,
    };
    public struct MobToken
    {
        public MobToken(Mobs mob, int col)
        {
            _mobName = mob;
            _col = col;
        }

        public Mobs _mobName;
        public int _col;
    }
    public struct DamageToken
    {
        public DamageTypes _type;
        public int _damage;
        public DamageToken(DamageTypes Type, int Damage)
        {
            _type = Type;
            _damage = Damage;
        }
    }
    public class ResistanceProfile
    {
        float _heatResist, _arcResist, _ionResist, _forceResist;
        public ResistanceProfile()
        {
            _heatResist = _arcResist = _ionResist = _forceResist = 1;
        }
        public void SetResist(DamageTypes Type, float Resist)
        {
            switch (Type)
            {
                case DamageTypes.Heat: _heatResist = Resist; break;
                case DamageTypes.Arc: _arcResist = Resist; break;
                case DamageTypes.Ion: _ionResist = Resist; break;
                case DamageTypes.Force: _forceResist = Resist; break;
            }
        }
        public float GetResist(DamageTypes Type)
        {
            switch (Type)
            {
                case DamageTypes.Heat: return _heatResist;
                case DamageTypes.Arc: return _arcResist;
                case DamageTypes.Ion: return _ionResist;
                case DamageTypes.Force: return _forceResist;
            }
            return 0f;
        }
        public int CalcDamage(DamageToken Token)
        {
            return (int)(Token._damage / GetResist(Token._type));
        }
    }

    public abstract class Mob : IDrawable, IPhysical, IInteractive
    {
        //The hitpoints of the mob
        public ResistanceProfile _resistances;
        public Mobs Type { get; protected set; }
        public int HitPoints { get; private set; }
        TimeSpan _bleedTime;
        public bool Active { get; private set; }
        Vector2 Speed { get; set; }
        Vector2 _loc;
        Rectangle _area;
        Dictionary<EventType, EventDelegate> _events;
        
        public Mob(Rectangle Rectangle, Texture2D Texture, Mobs Type, int HP, bool Active, float Speed)
        {
            //Set events
            _events = new Dictionary<EventType, EventDelegate>();
            _events.Add(EventType.OnTap, OnTap);

            this.Texture = Texture;
            this.Type = Type;
            _loc = new Vector2(Rectangle.X, Rectangle.Y);
            _area = Rectangle;
            _resistances = new ResistanceProfile();
            this.HitPoints = HP;
            this._bleedTime = TimeSpan.Zero;
            this.Active = Active;
            this.Speed = new Vector2(0, Speed);
        }
        public void OnTap()
        {
            this.TakeDamage(new DamageToken(DamageTypes.Force, 1));
        }
        public void Update(GameTime gt)
        {
            _loc += Speed;
            if (_bleedTime > TimeSpan.Zero)
                _bleedTime -= gt.ElapsedGameTime;
            else
                _bleedTime = TimeSpan.Zero;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Color _drawClr = (_bleedTime > TimeSpan.Zero) ? Color.Red : Color.White;
            spriteBatch.Draw(Texture, Rectangle, _drawClr);
        }
        public void TakeDamage(DamageToken Token)
        {
            int dmg = _resistances.CalcDamage(Token);
            this.HitPoints -= dmg;
            _bleedTime += new TimeSpan(0,0,0,0,dmg*10);
        }
        public void Kill() { this.HitPoints = 0; }
        public Rectangle Rectangle { get { return new Rectangle((int)_loc.X, (int)_loc.Y, _area.Width, _area.Height); } }
        public Texture2D Texture { get; protected set; }
        ColliderType IPhysical.Type { get { return ColliderType.Mob; } }
        bool IPhysical.Active { get { return this.Active; } }
        Rectangle IPhysical.Area { get { return this.Rectangle; } }
        bool IPhysical.Dead { get { return (HitPoints <= 0) ? true : false; } }
        void IPhysical.Update(GameTime gt) { this.Update(gt); }
        Rectangle IInteractive.Area { get { return this.Rectangle; } }
        Dictionary<EventType, EventDelegate> IInteractive.RegisteredEvents
        {
            get { throw new NotImplementedException(); }
        }
        bool IInteractive.IsActive { get { return this.Active; } }
    }

    public class TargetMob : Mob
    {
        public TargetMob(Rectangle SpawnRect) 
            : base(SpawnRect, GameConfig.DrawTextures[GameAssets.MobTarget],
            Mobs.Target, 30, true, .15f)
        {
        }
    }

    public class SpriteMob : Mob
    {
        public SpriteMob(Rectangle SpawnRect, RuneType Type)
            : base(SpawnRect, GameConfig.DrawTextures[GameAssets.MobTarget],
            Mobs.Target, 30, true, .15f)
        {
            switch (Type)
            {
                case RuneType.Eron:
                    _resistances.SetResist(DamageTypes.Ion, 2f);
                    Texture = GameConfig.DrawTextures[GameAssets.MobEronSprite];
                    this.Type = Mobs.EronSprite;
                    break;
                case RuneType.Flaz:
                    _resistances.SetResist(DamageTypes.Heat, 2f);
                    Texture = GameConfig.DrawTextures[GameAssets.MobFlazSprite];
                    this.Type = Mobs.FlazSprite;
                    break;
                case RuneType.Luin:
                    _resistances.SetResist(DamageTypes.Arc, 2f);
                    Texture = GameConfig.DrawTextures[GameAssets.MobLuinSprite];
                    this.Type = Mobs.LuinSprite;
                    break;
            }
        }
    }
}
