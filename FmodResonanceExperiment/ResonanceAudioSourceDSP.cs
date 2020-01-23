using System;
using System.Numerics;
using System.Runtime.InteropServices;

using FmodAudio;
using FmodAudio.Dsp;

namespace FmodResonanceExperiment
{
  public   class ResonanceAudioSourceDSP : DSPBase
    {
        public const bool DefBypassRoom = false;

        public const float DefGain = 0f;

        public const float DefMaxDistance = 500f;

        public const float DefMinDistance = 1f;

        public const bool DefNearFieldFX = false;

        public const float DefNearFieldGain = 9f;

        public const float DefSpread = 0.00001f;

        public const float MaxGain = 24f;

        public const float MaxMaxDistance = 10000f;

        public const float MaxMinDistance = 10000f;

        public const float MaxNearFieldGain = 9f;

        public const float MaxSpread = 360f;

        public const float MinGain = -80f; // Db

        public const float MinMaxDistance = 0f; // m

        public const float MinMinDistance = 0f; // m

        public const float MinNearFieldGain = 1.00001f; // Db

        public const float MinSpread = 0f; // deg

        public const float DefDirectivity = 0.00001f;
        public const float DefDirSharpness = 1.00001f;
        public const int DefDistRolloff = 2;
        public const float DefOcclusion = 0.00001f;
        public const float MaxDirectivity = 1f;
        public const float MaxDirSharpness = 10f;
        public const int MaxDistRolloff = 4;
        public const float MaxOcclusion = 10f;
        public const float MinDirectivity = 0f;
        public const float MinDirSharpness = 1f;
        public const int MinDistRolloff = 0;
        public const float MinOcclusion = 0f;
        private SpatialParameters   _spatialAttributes;
        private bool _byPassRoom;
        private float _directivity;
        private float _dirSharpness;
        private int _distRolloff;
        private float _gain;

        private float _maxDistance;
        private float _minDistance;
        private bool _nearFieldFX;
        private float _nearFieldGain;
        private float _occlusion;
        private float _spread;
        /// <summary>
        /// Konstruktor inicializuje instanci třídy pro obsluhu DSP efektu Resonance Audio Source.
        /// </summary>
        /// <param name="fmod">Instance systému FMOD</param>
        /// <param name="channelGroup">Skupina kanálů</param>
        /// <param name="plugin">Instance DSP pluginu</param>
        public ResonanceAudioSourceDSP(FmodSystem fmod, ChannelGroup channelGroup, Plugin plugin) : base(fmod, plugin, channelGroup)
        {

            Gain = DefGain;
            Spread = DefSpread;
            MinDistance = DefMinDistance;
            MaxDistance = DefMaxDistance;
            Occlusion = DefOcclusion;
            Directivity = DefDirectivity;
            DirSharpness = DefDirSharpness;
            _spatialAttributes = DefAttributes3d();
            ByPassRoom = DefBypassRoom;
            NearFieldGain = DefNearFieldGain;
            NearFieldFX = DefNearFieldFX;
        }/*mtd*/


        public bool ByPassRoom { get => _byPassRoom; set => _dsp.SetParameterBool(9, value); }
        public float Directivity { get => _directivity; set => _setParam(6, value, MinDirectivity, MaxDirectivity); }
        public float DirSharpness { get => _dirSharpness; set => _setParam(7, value, MinDirSharpness, MaxDirSharpness); }
        public int DistRolloff { get => _distRolloff; set => _setParam(4, value, MinDistRolloff, MaxDistRolloff); }
        public float Gain { get => _gain; set => _setParam(0, value, MinGain, MaxGain); }
        public float MaxDistance { get => _maxDistance; set => _setParam(3, value, MinMaxDistance, MaxMaxDistance); }
        public float MinDistance { get => _minDistance; set => _setParam(2, value, MinMinDistance, MaxMinDistance); }
        public bool NearFieldFX { get => _nearFieldFX; set => _dsp.SetParameterBool(10, value); }
        public float NearFieldGain { get => _nearFieldGain; set => _setParam(11, value, MinNearFieldGain, MaxNearFieldGain); }
        public float Occlusion { get => _occlusion; set => _setParam(5, value, MinOcclusion, MaxOcclusion); }
        public SpatialParameters SpatialAttributes { get => _spatialAttributes; set => _setAttributes3d(value); }

        private void _setAttributes3d(SpatialParameters data)
        {
            int size = Marshal.SizeOf(typeof(Parameter3DAttributes));
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(data, ptr, false);
            _dsp.SetParameterData(8, ptr, (uint)size);
            Marshal.FreeHGlobal(ptr);


        }/*mtd*/

        public float Spread { get => _spread; set => _setParam(1, value, MinSpread, MaxSpread); }
        public  SpatialParameters  DefAttributes3d()
        {
            return new  SpatialParameters()
            {
                Relative = new SequentialAttributes3d()
                {
                    Position = new SequentialVector3f(0f),
                    Velocity = new SequentialVector3f(0f),
                    Forward = new SequentialVector3f(0f),
                    Up = new SequentialVector3f(0f)
                },
                Absolute = new  SequentialAttributes3d()
                {
                    Position = new SequentialVector3f(0f),
                    Velocity = new SequentialVector3f(0f),
                    Forward = new SequentialVector3f(0f),
                    Up = new SequentialVector3f(0f)
                }
            };
        }/*mtd*/
/*mtd*/
    } /*cls*/
}/*nspc*/