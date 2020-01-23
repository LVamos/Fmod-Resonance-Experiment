using FmodAudio;
using FmodAudio.Dsp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace FmodResonanceExperiment
{
  public  class Sound
    {




        public bool IsPlaying { get => _currentChannel.IsPlaying; }
        public  readonly  string  Name;

        private readonly FmodSystem _fmod;
        private readonly string  _fileName;
        private readonly FmodAudio.Sound _sound;

        public bool Loop
        {
            get => _loop;
            set
            {
                _sound.Mode = (value) ? Mode.Loop_Normal : Mode.Default;
                _sound.LoopCount = (value) ? -1 : 1;
                _loop = value;
            }/*s*/
        }/*prp*/


        public uint  PlaybackPosition   
        {
            get => _currentChannel.GetPosition(TimeUnit.MS);
            set => _currentChannel.SetPosition(TimeUnit.MS, value);
        }/*prp*/


        private Channel _currentChannel;
        private Plugin _sourcePlugin;
        private Plugin _soundFieldPlugin;
     public    ResonanceAudioSourceDSP    SourceDSP;
        private DSP _soundFieldDSP;
        private ChannelGroup _channelGroup;
        private bool _loop;
        private ChannelGroup _parentChannelGroup;

        /// <summary>
        /// Konstruktor načte sampl ze souboru.
        /// </summary>
        /// <param name="fileName">Název souboru</param>
        /// <param name="loop">Přehrávat ve smyčce?</param>
        /// <param name="sourcePlugin">Handle DSP pluginu Source</param>
        /// <param name="soundFieldPlugin">Handle DSP pluginu Near field</param>
        /// <param name="fmod">Odkaz na inicializovaný FMOD systém</param>
        /// <param name="parentChannelGroup">Odkaz na nadřazenou skupinu kanálů</param>
        public Sound(string fileName, bool loop, Plugin sourcePlugin, Plugin soundFieldPlugin, FmodSystem fmod,  ChannelGroup parentChannelGroup)
        {
            _fmod = fmod ?? throw new ArgumentException(nameof(fmod));
            _parentChannelGroup = parentChannelGroup ?? throw new ArgumentException(nameof(parentChannelGroup));
            _fileName = fileName ?? throw new ArgumentException(nameof(fileName));
            Name = Path.GetFileNameWithoutExtension(fileName);

            _sound = _fmod.CreateSound(fileName, Mode.CreateSample);

            _sourcePlugin = sourcePlugin;
            _soundFieldPlugin = soundFieldPlugin;
            _soundFieldDSP = _fmod.CreateDSPByPlugin(soundFieldPlugin);

            _channelGroup = _fmod.CreateChannelGroup(Name);
            parentChannelGroup.AddGroup(_channelGroup, false);

            SourceDSP = new ResonanceAudioSourceDSP(fmod, _channelGroup, sourcePlugin);

            Loop = loop;
        }/*mtd*/

        public void Play()
        {
          _currentChannel = _fmod.PlaySound(_sound, _channelGroup);
        }/*mtd*/
    } /*cls*/
}/*nspc*/   
