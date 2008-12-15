using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace OHQ.Sound
{
    class SoundManager : GameComponent
    {
        private AudioEngine Engine;
        public List<SoundBank> SoundBankList { get; private set; }
        public List<WaveBank> WaveBankList { get; private set; }

       public SoundManager(Game game, string pathToSettings)
           : base(game)
       {
           Engine = new AudioEngine(pathToSettings);
           SoundBankList = new List<SoundBank>();
           WaveBankList = new List<WaveBank>();
       }

        /// <summary>
        /// Should be called as the game exits, it will call Dispose()
        /// on the engine and all of the wave and sound banks
        /// </summary>
        public void CleanUp()
        {
            Engine.Dispose();
            foreach (SoundBank soundBank in SoundBankList)
            {
                soundBank.Dispose();
            }
            foreach (WaveBank waveBank in WaveBankList)
            {
                waveBank.Dispose();
            }
            
        }
    }
}
