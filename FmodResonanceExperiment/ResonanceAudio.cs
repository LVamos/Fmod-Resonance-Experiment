    using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

using FmodAudio;
using FmodAudio.Dsp;

namespace FmodResonanceExperiment
{
    public static class ResonanceAudio
    {
        public static readonly ReadOnlyDictionary<string, Sound> Sounds;

        public static RoomProperties Room { get; private set; }

public static void UpdateRoomProperties()
        {
            int size = Marshal.SizeOf(typeof(RoomProperties));
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(Room, ptr, false);
            _listenerDSP.SetParameterData(1, ptr,                                                                           (uint)size);
            Marshal.FreeHGlobal(ptr);

        }/*mtd*/


        public static void TestPlayback()
        {
            ResonanceAudio.AddSound("sound.wav", true);
            Sound snd = ResonanceAudio.Sounds["sound"];
            snd.Play();

            TestSetRoomProperties();
            TestSourceDSP(snd);
        }/*mtd*/

       public  static void TestSourceDSP(Sound snd)
        {
            ResonanceAudioSourceDSP source = snd.SourceDSP;

            
            //source.Gain = 1f;
            
            //source.Spread = ResonanceAudioSourceDSP.MaxSpread;
            //source.Directivity = ResonanceAudioSourceDSP.MaxDirectivity;
            //source.DirSharpness = ResonanceAudioSourceDSP.MaxDirSharpness;
            //source.DistRolloff = ResonanceAudioSourceDSP.MaxDistRolloff;
            //source.NearFieldFX = true;
            //source.NearFieldGain = ResonanceAudioSourceDSP.MaxNearFieldGain;
            //source.Occlusion = ResonanceAudioSourceDSP.MaxOcclusion;
            //source.MaxDistance = 13;
            

            source.SpatialAttributes = new SpatialParameters()
            {
                Relative=new SequentialAttributes3d()
                {
                    Position=new  SequentialVector3f (10f, 10f, 0f),
                    Velocity=new SequentialVector3f (0f, 0f, 0f),
                    Forward=new  SequentialVector3f (0f, 0f, -1f),
                    Up=new  SequentialVector3f     (0f, 1f, 0f)
                },
                Absolute=new SequentialAttributes3d()
                {
                    Position=new   SequentialVector3f(0f, 0f, 0f),
                    Velocity = new SequentialVector3f(0f, 0f, 0f),
                    Forward = new SequentialVector3f(0f, 0f, -1f),
                    Up = new SequentialVector3f(0f, 1f, 0f)
                }
            };
        }/*mtd*/

        private static void TestSetRoomProperties()
        {
            RoomProperties room = new RoomProperties()
            {
                // listener position
                positionX = 0f,
                positionY = 0f,
                positionZ = 0f,

                // room dimensions
                dimensionsX = 2f,
                dimensionsY = 9f,
                dimensionsZ = 4f,

                // listener orientation
                rotationX = 0f,
                rotationY = 0f,
                rotationZ = -1f,
                rotationW = 1f,

                // surface materials of all sides of the room
                materialLeft = SurfaceMaterial.CurtainHeavy,
                materialRight = SurfaceMaterial.GlassThin,
                materialBottom = SurfaceMaterial.LinoleumOnConcrete,
                materialTop = SurfaceMaterial.AcousticCeilingTiles,
                materialFront = SurfaceMaterial.BrickPainted,
                materialBack = SurfaceMaterial.Transparent,

                // reverb parameters
                reflectivity = 1f,
                reverbGainDb = .5f,
                reverbBrightness = .5f,
                reverbTime = 1f
            };

            UpdateRoomProperties(room);
        }/*mtd*/

        public static void UpdateRoomProperties(RoomProperties room)
        {
            Room = room;
            UpdateRoomProperties();
    }/*mtd*/

      private static byte[] GetBytes(IntPtr ptr, int length)
        {
            if (ptr != IntPtr.Zero)
            {
                byte[] byteArray = new byte[length];
                Marshal.Copy(ptr, byteArray, 0, length);
                return byteArray;
            }
            return new byte[1];
        }/*mtd*/






        private static readonly Dictionary<string, Sound> _sounds;

        private static Plugin _basePlugin;

        private static FmodSystem _fmodSystem;

        private static DSP _listenerDSP;

        private static Plugin _listenerPlugin;

        private static ChannelGroup _masterChannelGroup;

        private static Plugin _soundFieldPlugin;

        private static Plugin _sourcePlugin;

        private static ChannelGroup _spatialChannelGroup;

        static ResonanceAudio()
        {
            _sounds = new Dictionary<string, Sound>();
            Sounds = new ReadOnlyDictionary<string, Sound>(_sounds);
        }/*mtd*/

        public static void AddSound(string fileName, bool loop = false)
        {
            Sound sound = new Sound(fileName, loop, _sourcePlugin, _soundFieldPlugin, _fmodSystem, _masterChannelGroup);
            _sounds[sound.Name] = sound;
        }

        public static void Free()
            => _fmodSystem.Release();

        public static void Init()
        {
            _fmodSystem = Fmod.CreateSystem();
            _fmodSystem.Init(32);

            _basePlugin = _fmodSystem.LoadPlugin("resonanceaudio.dll");

            _listenerPlugin = _fmodSystem.GetNestedPlugin(_basePlugin, 0);
            _listenerDSP = _fmodSystem.CreateDSPByPlugin(_listenerPlugin);
            _soundFieldPlugin = _fmodSystem.GetNestedPlugin(_basePlugin, 1);
            _sourcePlugin = _fmodSystem.GetNestedPlugin(_basePlugin, 2);

            _masterChannelGroup = _fmodSystem.MasterChannelGroup;
            _spatialChannelGroup = _fmodSystem.CreateChannelGroup("spatial");
            _masterChannelGroup.AddGroup(_spatialChannelGroup, false);

            _masterChannelGroup.AddDSP(ChannelControlDSPIndex.DspHead, _listenerDSP);
        }

        public static void ShowPluginsInfo()
        {
            void WriteParameters(StringBuilder sBuilder, string name, string dataType, string label, string defaultValue, string minValue, string maxValue) // Lokální funkce
            {
                sBuilder.AppendLine($"Název: {name}")
                    .AppendLine($"Datový typ: {dataType}")
                .AppendLine($"Jednotky: {label}")
                .AppendLine($"Výchozí hodnota: {defaultValue}")
                .AppendLine($"Minimální hodnota: {minValue}")
                .AppendLine($"Maximální hodnota: {maxValue}").AppendLine();
            }/*fnc*/

            void SaveParamDetails(ParameterDescription pDescription, StringBuilder pInfo) // Lokální funkce
            {
                switch (pDescription)
                {
                    case FloatParameterDescription fDesc: 
                        WriteParameters(pInfo, fDesc.Name, "float", fDesc.Label, fDesc.DefaultValue.ToString("N6"), fDesc.Min.ToString("N6"), fDesc.Max.ToString("N6")); break;
                    case IntParameterDescription iDesc:
                        WriteParameters(pInfo, iDesc.Name, "int", iDesc.Label, iDesc.DefaultValue.ToString(), iDesc.Min.ToString(), iDesc.Max.ToString()); break;
                    case BoolParameterDescription bDesc:
                        WriteParameters(pInfo, bDesc.Name, "bool", bDesc.Label, (bDesc.DefaultValue)?"true":"false", "", ""); break;
                    case DataParameterDescription dDesc:
                        WriteParameters(pInfo, dDesc.Name, "data", dDesc.Label, "", "", ""); break;
                    default:
                        throw new InvalidOperationException();
                }/*swt*/
            }/*fnc*/


      StringBuilder    GetPluginDetails(DSP plugin) // Lokální funkce
            {
                Span<char> pluginName = new char[256];
                plugin.GetInfo(pluginName, out FmodVersion a, out int b, out int c, out int d);
                string pluginNameSafe = pluginName.ToString();
                pluginNameSafe = pluginNameSafe.Substring(0, pluginNameSafe.IndexOf('\0'));

                StringBuilder msg = new StringBuilder($"#Vypisuji parametry pluginu {pluginNameSafe}")
                .AppendLine($"Počet parametrů: {plugin.ParameterCount.ToString()}");

                for (int paramIndex = 0; paramIndex < plugin.ParameterCount; paramIndex++)
                {
                    msg.AppendLine ( $"{paramIndex.ToString()}");
                    SaveParamDetails(plugin.GetParameterInfo(paramIndex), msg);
                    msg.AppendLine();
                }/*for*/
                msg.AppendLine();
                return msg;
            }/*fnc*/

            string listener = GetPluginDetails(_fmodSystem.CreateDSPByPlugin(_listenerPlugin)).ToString();
            string soundField = GetPluginDetails(_fmodSystem.CreateDSPByPlugin(_soundFieldPlugin)).ToString();
            string source =   GetPluginDetails  (_fmodSystem.CreateDSPByPlugin(_sourcePlugin)).ToString();

            Clipboard.SetText(listener +soundField +source);
            MessageBox.Show(listener);
            MessageBox.Show(soundField);
            MessageBox.Show(source);
            Application.Current.Shutdown();
        }/*mtd*/
        /*mtd*/
        /*mtd*/
    } /*cls*/
}/*nspc*/