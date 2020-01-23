using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;


namespace FmodResonanceExperiment
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SequentialVector3f
    {
        public float X;
        public float Y;
        public float Z;


        public SequentialVector3f(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z        = z;
        }

        public SequentialVector3f(float x):this(x, x, x)
        {

        }

    }/*str*/

    [StructLayout(LayoutKind.Sequential)]
    public struct SequentialAttributes3d
    {
        public SequentialVector3f Position;
        public SequentialVector3f Velocity;
        public SequentialVector3f Forward;
        public SequentialVector3f Up;
    }/*str*/

    [StructLayout(LayoutKind.Sequential)]
    public struct SpatialParameters
    {
        public SequentialAttributes3d Relative;
        public SequentialAttributes3d Absolute;
    }/*str*/
}/*nspc*/
