using System.Runtime.InteropServices;

namespace ProjectCars2.DataFormat
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ParticipantInfo
    {
        [MarshalAs(UnmanagedType.I1)] public bool mIsActive;
        public SmString mName;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int) EVector.VecMax)] public float[] mWorldPosition;

        public float mCurrentLapDistance;
        public int mRacePosition;
        public int mLapsCompleted;
        public int mCurrentLap;
        public int mCurrentSector;
    };
}