using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Efluzion
{
    public class WeightedTable<T>
    {
        Dictionary<T, int> _entries;
        Random _rand;
        int _totalOfWeights;

        public WeightedTable()
        {
            _entries = new Dictionary<T, int>();
            _rand = new Random();
            _totalOfWeights = 0;
        }

        public void MakeEntry(T TKey, int Weight)
        {
            _entries[TKey] = Weight;

            foreach (int i in _entries.Values)
                _totalOfWeights += i;
        }

        public T GetRandomEntry()
        {
            int n = _rand.Next(_totalOfWeights) + 1;

            foreach (T key in _entries.Keys)
                if (n <= _entries[key])
                    return key;
                else
                    n -= _entries[key];
            return _entries.Keys.ElementAt(0);
        }
    }

    public class GameBoard : IBoard
    {
        int _score, _lives;
        TextArea _nameArea, _scoreArea, _livesArea, _gameOver;
        Stage _gameStage;
        Runebar _runeBar;
        List<IInteractive> _interactiveObjects;
        List<IPhysical> _physicalObjects;
        public bool Active { get; private set; }
        public bool GameOver { get; private set; }
        TimeSpan _spawnDelay;
        TimeSpan _sinceLastSpawn;
        WeightedTable<Mobs> _mobTable;

        public GameBoard() 
        {
            _interactiveObjects = new List<IInteractive>();
            _physicalObjects = new List<IPhysical>();
            _runeBar = new Runebar(this);
            _gameStage = new Stage(this);
            this.Active = false;
            this.GameOver = false;
            _score = 0;
            _lives = 10;
            _spawnDelay = new TimeSpan(0, 0, 4);
            _sinceLastSpawn = TimeSpan.Zero;

            //Mob table
            _mobTable = new WeightedTable<Mobs>();
            _mobTable.MakeEntry(Mobs.EronSprite, 1);
            _mobTable.MakeEntry(Mobs.FlazSprite, 1);
            _mobTable.MakeEntry(Mobs.LuinSprite, 1);

            //Text areas
            _nameArea = new TextArea(GameConfig.DrawFonts[GameAssets.FontMini], TextAlignment.Center,
                GameConfig.DrawLocations[GameAssets.TextStagePlayerName]);
            _nameArea.Text = GameControl.ActivePlayer.Name;
            _scoreArea = new TextArea(GameConfig.DrawFonts[GameAssets.FontMini], TextAlignment.Center,
                GameConfig.DrawLocations[GameAssets.TextStageScore]);
            _scoreArea.Text = _score.ToString();
            _livesArea = new TextArea(GameConfig.DrawFonts[GameAssets.FontMini], TextAlignment.Center,
                GameConfig.DrawLocations[GameAssets.TextStageLives]);
            _livesArea.Text = _lives.ToString();
            _gameOver = new TextArea(GameConfig.DrawFonts[GameAssets.FontStd], TextAlignment.Center,
                GameConfig.DrawLocations[GameAssets.TextStageGameOver]);
            _gameOver.Text = "GAME OVER";
        }

        public void Activate()
        {
            this.Active = true;
            GameControl.LoadBoard(this);
            GameSound.PlaySong(Songs.SecundaFortuna);
        }
        public void AddPoints(int points)
        {
            _score += points;
        }
        public void LoseLives(int lives)
        {
            _lives -= lives;
        }
        private void OnGameOver()
        {
            GameOver = true;
            GameSound.StopSong();
            GameSound.PlaySong(Songs.GameOver);
        }
        public void Update(GameTime gt)
        {
            _livesArea.Text = "Lives: " + _lives.ToString();
            _scoreArea.Text = "Score: " + _score.ToString();

            if (_lives <= 0)
                OnGameOver();

            if (!GameOver && Active)
            {
                _gameStage.Update(gt);
                _runeBar.Update(gt);
                _sinceLastSpawn += gt.ElapsedGameTime;

                _score += (int)Math.Pow(2d, (double)_gameStage.KilledMobs.Count) - 1;
                _lives -= _gameStage.FinishLineMobs.Count;

                if (_sinceLastSpawn >= _spawnDelay)
                {
                    _sinceLastSpawn = TimeSpan.Zero;
                    _gameStage.SpawnMob(new MobToken(_mobTable.GetRandomEntry(), 0));
                    _gameStage.SpawnMob(new MobToken(_mobTable.GetRandomEntry(), 1));
                    _gameStage.SpawnMob(new MobToken(_mobTable.GetRandomEntry(), 2));
                }

                if (_runeBar.ReadySpell._spellName != Spells.None)
                    _gameStage.SpawnSpell(_runeBar.ReadySpell);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            _runeBar.Draw(spriteBatch);
            _gameStage.Draw(spriteBatch);
            _nameArea.Draw(spriteBatch);
            _scoreArea.Draw(spriteBatch);
            _livesArea.Draw(spriteBatch);

            if (GameOver)
                _gameOver.Draw(spriteBatch);
        }       
        Rectangle IDrawable.Rectangle
        {
            get { throw new NotImplementedException(); }
        }
        Texture2D IDrawable.Texture
        {
            get { throw new NotImplementedException(); }
        }
        void IDrawable.Draw(SpriteBatch spriteBatch)
        {
            this.Draw(spriteBatch);
        }
        List<IInteractive> IBoard.InteractiveObjects
        {
            get { return this._interactiveObjects; }
        }
        bool IBoard.Active
        {
            get { return this.Active; }
        }
        void IBoard.RegisterObject(IInteractive obj)
        {
            _interactiveObjects.Add(obj);
        }
    }
}
