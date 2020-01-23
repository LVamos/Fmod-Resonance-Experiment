using System.Runtime.InteropServices;


namespace FmodResonanceExperiment
{
    [StructLayout(LayoutKind.Sequential)]
  public  struct RoomProperties
    {
        // Center position of the room in world space.
        public float positionX;
        public float positionY;
        public float positionZ;

        // Rotation (quaternion) of the room in world space.
        public float rotationX;
        public float rotationY;
        public float rotationZ;
        public float rotationW;

        // Size of the shoebox room in world space.
        public float dimensionsX;
        public float dimensionsY;
        public float dimensionsZ;

        // Material name of each surface of the shoebox room.
        public SurfaceMaterial materialLeft;
        public SurfaceMaterial materialRight;
        public SurfaceMaterial materialBottom;
        public SurfaceMaterial materialTop;
        public SurfaceMaterial materialFront;
        public SurfaceMaterial materialBack;

        // User defined uniform scaling factor for reflectivity. This parameter has no effect when set
        // to 1.0f.
        public float reflectivity;

        // User defined reverb tail gain multiplier. This parameter has no effect when set to 0.0f.
        public float reverbGainDb;

        // Adjusts the reverberation time across all frequency bands. RT60 values are multiplied by this
        // factor. Has no effect when set to 1.0f.
        public float reverbTime;

        // Controls the slope of a line from the lowest to the highest RT60 values (increases high
        // frequency RT60s when positive, decreases when negative). Has no effect when set to 0.0f.
        public float reverbBrightness;
    };

}/*nsp*/