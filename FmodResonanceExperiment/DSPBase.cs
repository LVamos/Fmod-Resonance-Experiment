using FmodAudio;
using FmodAudio.Dsp;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;


namespace FmodResonanceExperiment
{
 public  abstract  class DSPBase
    {
        protected FmodSystem _fmod;
        protected Plugin _plugin;
        private ChannelGroup _channelGroup;
        protected DSP _dsp;        

     protected  void _setParam(int index, float value, float minValue, float maxValue)
        {
            if (value < minValue || value >maxValue )
                throw new ArgumentException($"Nepovolená hodnota {nameof(value)}.{Environment.NewLine}Hodnota musí být v rozmezí {minValue.ToString("N6")}-{maxValue.ToString("N6")}.");
            else _dsp.SetParameterFloat(0, value);
        }/*mtd*/

       protected  void _setParam(int index, int value, int minValue, int maxValue)
        {
            if (value < minValue || value > maxValue)
                throw new ArgumentException($"Nepovolená hodnota {nameof(value)}.{Environment.NewLine}Hodnota musí být v rozmezí {minValue.ToString()}-{maxValue.ToString()}.");
            else _dsp.SetParameterFloat(0, value);
        }/*mtd*/

        protected void _setParam(int index, object data, Type t)
        {
            int size = Marshal.SizeOf(t);
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(data, ptr, false);
            _dsp.SetParameterData(index, ptr, (uint)size);
            Marshal.FreeHGlobal(ptr);

        }/*mtd*/

        /// <summary>
        /// KOnstruktor inicializuje základní třídu, z níž jsou odvozeny třídy pro DSP efekty.
        /// </summary>
        /// <param name="fmod">Instance FMODSystem</param>
        /// <param name="plugin">Instance DSP pluginu</param>
        /// <param name="channelGroup">Instance skupiny kanálů</param>
        protected DSPBase(FmodSystem fmod, Plugin plugin, ChannelGroup channelGroup)
        {
            _fmod = fmod ?? throw new ArgumentNullException(nameof(fmod));
            _plugin = plugin;
            _dsp = _fmod.CreateDSPByPlugin(_plugin);
            _channelGroup = channelGroup ?? throw new InvalidOperationException(nameof(channelGroup));
            _channelGroup.AddDSP(ChannelControlDSPIndex.DSPTail, _dsp);
        }/*mtd*/


    } /*cls*/
}/*nspc*/
