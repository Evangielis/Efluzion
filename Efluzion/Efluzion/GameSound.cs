using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;

namespace Efluzion
{
    public enum Songs
    {
        SecundaFortuna,
        GameOver,
    };

    public enum SFX
    {
        None,
    };

    public static class GameSound
    {
        public static Dictionary<Songs, Song> SongLibrary { get; private set; }
        public static Dictionary<SFX, SoundEffect> SfxLibrary { get; private set; }

        public static void LoadSongs(ContentManager Content)
        {
            SongLibrary = new Dictionary<Songs,Song>();
            SongLibrary.Add(Songs.SecundaFortuna, Content.Load<Song>("SecundaFortuna"));
            SongLibrary.Add(Songs.GameOver, Content.Load<Song>("MegaraptorGameOver"));
        }

        public static void LoadSFX(ContentManager Content)
        {
            SfxLibrary = new Dictionary<SFX, SoundEffect>();
        }

        public static void PlaySong(Songs song, bool Repeat = true)
        {
            MediaPlayer.IsRepeating = Repeat;
            MediaPlayer.Play(SongLibrary[song]);
        }

        public static void StopSong()
        {
            MediaPlayer.Stop();
        }
    }
}
