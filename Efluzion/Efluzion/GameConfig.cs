using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Efluzion
{
    public enum GameAssets
    {
        Runebar,
        EronRune,
        EronRuneLit,
        FlazRune,
        FlazRuneLit,
        LuinRune,
        LuinRuneLit,
        FontStd,
        FontMini,
        MobTarget,
        MobLuinSprite,
        MobEronSprite,
        MobFlazSprite,
        RuneSlot1,
        RuneSlot2,
        RuneSlot3,
        RuneSlot4,
        RuneSlot5,
        RuneSlot6,
        RuneSlot7,
        RuneSlot8,
        RuneSlot9,
        Stage,
        SpawnMob1,
        SpawnMob2,
        SpawnMob3,
        SpawnSpell1,
        SpawnSpell2,
        SpawnSpell3,
        SpawnUltSpell1,
        SpawnUltSpell2,
        SpawnUltSpell3,
        SpellLuxball,
        SpellPyreball,
        SpellArcball,
        SpellLuxbolt,
        SpellPyrebolt,
        SpellArcbolt,
        SpellPyroblast,
        SpellFinalLight,
        SpellArcstream,
        TextStageScore,
        TextStagePlayerName,
        TextStageLives,
        TextStageGameOver,
    };

    public static class GameConfig
    {
        public static Dictionary<GameAssets, Rectangle> DrawLocations;
        public static Dictionary<GameAssets, Texture2D> DrawTextures;
        public static Dictionary<GameAssets, SpriteFont> DrawFonts;

        public static void LoadConfig(ContentManager Content, GraphicsDeviceManager Graphics)
        {
            //Set draw locations
            DrawTextures = new Dictionary<GameAssets, Texture2D>();
            DrawLocations = new Dictionary<GameAssets, Rectangle>();
            DrawFonts = new Dictionary<GameAssets, SpriteFont>();

            LoadDrawTextures(Content);
            LoadDrawFonts(Content);
            LoadDrawLocations(Graphics);
        }

        private static void LoadDrawTextures(ContentManager Content)
        {
            DrawTextures.Add(GameAssets.Runebar, Content.Load<Texture2D>("Runebar"));
            DrawTextures.Add(GameAssets.FlazRune, Content.Load<Texture2D>("FlazRune"));
            DrawTextures.Add(GameAssets.FlazRuneLit, Content.Load<Texture2D>("FlazRuneLight"));
            DrawTextures.Add(GameAssets.LuinRune, Content.Load<Texture2D>("LuinRune"));
            DrawTextures.Add(GameAssets.LuinRuneLit, Content.Load<Texture2D>("LuinRuneLight"));
            DrawTextures.Add(GameAssets.EronRune, Content.Load<Texture2D>("EronRune"));
            DrawTextures.Add(GameAssets.EronRuneLit, Content.Load<Texture2D>("EronRuneLight"));
            DrawTextures.Add(GameAssets.Stage, Content.Load<Texture2D>("Stage"));
            
            DrawTextures.Add(GameAssets.MobTarget, Content.Load<Texture2D>("TargetMob"));
            DrawTextures.Add(GameAssets.MobEronSprite, Content.Load<Texture2D>("EronSpriteMob"));
            DrawTextures.Add(GameAssets.MobFlazSprite, Content.Load<Texture2D>("FlazSpriteMob"));
            DrawTextures.Add(GameAssets.MobLuinSprite, Content.Load<Texture2D>("LuinSpriteMob"));

            //Spells
            DrawTextures.Add(GameAssets.SpellLuxbolt, Content.Load<Texture2D>("LuxboltSpell"));
            DrawTextures.Add(GameAssets.SpellArcbolt, Content.Load<Texture2D>("ArcboltSpell"));
            DrawTextures.Add(GameAssets.SpellPyrebolt, Content.Load<Texture2D>("PyreboltSpell"));
            DrawTextures.Add(GameAssets.SpellLuxball, Content.Load<Texture2D>("LuxballSpell"));
            DrawTextures.Add(GameAssets.SpellArcball, Content.Load<Texture2D>("ArcballSpell"));
            DrawTextures.Add(GameAssets.SpellPyreball, Content.Load<Texture2D>("PyreballSpell"));
            DrawTextures.Add(GameAssets.SpellFinalLight, Content.Load<Texture2D>("FinallightSpell"));
            DrawTextures.Add(GameAssets.SpellArcstream, Content.Load<Texture2D>("ArcstreamSpell"));
            DrawTextures.Add(GameAssets.SpellPyroblast, Content.Load<Texture2D>("PyroblastSpell"));
        }

        private static void LoadDrawFonts(ContentManager Content)
        {
            DrawFonts.Add(GameAssets.FontStd, Content.Load<SpriteFont>("StdFont"));
            DrawFonts.Add(GameAssets.FontMini, Content.Load<SpriteFont>("FontMini"));
        }

        private static void LoadDrawLocations(GraphicsDeviceManager Graphics)
        {
            const int BOX_W = 30,
                BOX_H = 30,
                COL_W = 90,
                COL_H = 300,
                BORDER_W = 2,
                STAGE_W = COL_W * 3 + BORDER_W * 5,
                STAGE_H = COL_H + BORDER_W * 2,
                STAGE_X = 10,
                STAGE_Y = 40,
                RUNEBAR_BUFFER = 10,
                TEXT_BUFFER = RUNEBAR_BUFFER,
                TEXT_H = 10,
                GAMEOVER_H = 20,
                GAMEOVER_W = 50,
                RUNEBAR_X = STAGE_X,
                RUNEBAR_Y = STAGE_Y + STAGE_H + RUNEBAR_BUFFER,
                COL_Y = STAGE_Y + BORDER_W,
                COL_X = STAGE_X + BORDER_W,
                COL_CENTER_X = COL_X + COL_W / 2,
                SPAWN_X = COL_CENTER_X - BOX_W / 2,
                SPAWN_Y = COL_Y,
                SPAWN_SPELL_Y = SPAWN_Y + COL_H - BOX_H;

            DrawLocations.Add(GameAssets.Runebar, CalcDrawRectangle(GameAssets.Runebar, RUNEBAR_X, RUNEBAR_Y));
            DrawLocations.Add(GameAssets.RuneSlot1, CalcDrawRectangle(GameAssets.EronRune, RUNEBAR_X + BORDER_W, RUNEBAR_Y + BORDER_W));
            DrawLocations.Add(GameAssets.RuneSlot2, CalcDrawRectangle(GameAssets.EronRune, RUNEBAR_X + BORDER_W + BOX_W, RUNEBAR_Y + BORDER_W));
            DrawLocations.Add(GameAssets.RuneSlot3, CalcDrawRectangle(GameAssets.EronRune, RUNEBAR_X + BORDER_W + BOX_W*2, RUNEBAR_Y + BORDER_W));
            DrawLocations.Add(GameAssets.RuneSlot4, CalcDrawRectangle(GameAssets.EronRune, RUNEBAR_X + BORDER_W*2 + BOX_W*3, RUNEBAR_Y + BORDER_W));
            DrawLocations.Add(GameAssets.RuneSlot5, CalcDrawRectangle(GameAssets.EronRune, RUNEBAR_X + BORDER_W*2 + BOX_W*4, RUNEBAR_Y + BORDER_W));
            DrawLocations.Add(GameAssets.RuneSlot6, CalcDrawRectangle(GameAssets.EronRune, RUNEBAR_X + BORDER_W*2 + BOX_W*5, RUNEBAR_Y + BORDER_W));
            DrawLocations.Add(GameAssets.RuneSlot7, CalcDrawRectangle(GameAssets.EronRune, RUNEBAR_X + BORDER_W*3 + BOX_W*6, RUNEBAR_Y + BORDER_W));
            DrawLocations.Add(GameAssets.RuneSlot8, CalcDrawRectangle(GameAssets.EronRune, RUNEBAR_X + BORDER_W*3 + BOX_W*7, RUNEBAR_Y + BORDER_W));
            DrawLocations.Add(GameAssets.RuneSlot9, CalcDrawRectangle(GameAssets.EronRune, RUNEBAR_X + BORDER_W*2 + BOX_W*8, RUNEBAR_Y + BORDER_W));
            DrawLocations.Add(GameAssets.Stage, CalcDrawRectangle(GameAssets.Stage, STAGE_X, STAGE_Y));
            DrawLocations.Add(GameAssets.SpawnMob1, new Rectangle(SPAWN_X, SPAWN_Y, BOX_W, BOX_H));
            DrawLocations.Add(GameAssets.SpawnMob2, new Rectangle(SPAWN_X + COL_W + BORDER_W, SPAWN_Y, BOX_W, BOX_H));
            DrawLocations.Add(GameAssets.SpawnMob3, new Rectangle(SPAWN_X + COL_W*2 + BORDER_W*2, SPAWN_Y, BOX_W, BOX_H));
            DrawLocations.Add(GameAssets.SpawnSpell1, new Rectangle(SPAWN_X, SPAWN_SPELL_Y, BOX_W, BOX_H));
            DrawLocations.Add(GameAssets.SpawnSpell2, new Rectangle(SPAWN_X + COL_W + BORDER_W, SPAWN_SPELL_Y, BOX_W, BOX_H));
            DrawLocations.Add(GameAssets.SpawnSpell3, new Rectangle(SPAWN_X + COL_W * 2 + BORDER_W * 2, SPAWN_SPELL_Y, BOX_W, BOX_H));
            DrawLocations.Add(GameAssets.SpawnUltSpell1, new Rectangle(COL_X, SPAWN_Y, COL_W, COL_H));
            DrawLocations.Add(GameAssets.SpawnUltSpell2, new Rectangle(COL_X + COL_W + BORDER_W, SPAWN_Y, COL_W, COL_H));
            DrawLocations.Add(GameAssets.SpawnUltSpell3, new Rectangle(COL_X + COL_W * 2 + BORDER_W * 2, SPAWN_Y, COL_W, COL_H));
            DrawLocations.Add(GameAssets.TextStagePlayerName, new Rectangle(STAGE_X + BORDER_W, STAGE_Y - TEXT_BUFFER - TEXT_H, COL_W, TEXT_H));
            DrawLocations.Add(GameAssets.TextStageScore, new Rectangle(STAGE_X + COL_W + BORDER_W*2, STAGE_Y - TEXT_BUFFER - TEXT_H, COL_W, TEXT_H));
            DrawLocations.Add(GameAssets.TextStageLives, new Rectangle(STAGE_X + COL_W*2 + BORDER_W*3, STAGE_Y - TEXT_BUFFER - TEXT_H, COL_W, TEXT_H));
            DrawLocations.Add(GameAssets.TextStageGameOver, new Rectangle(STAGE_X + STAGE_W/2 - GAMEOVER_W/2, STAGE_Y + STAGE_H/2 - GAMEOVER_H/2, GAMEOVER_W, GAMEOVER_H));
        }

        public static Rectangle CalcDrawRectangle(GameAssets Asset, int x, int y)
        {
            Texture2D myTexture = GameConfig.DrawTextures[Asset];
            return new Rectangle(x, y, myTexture.Bounds.Width, myTexture.Bounds.Height);
        }
    }
}
