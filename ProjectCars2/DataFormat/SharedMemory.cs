using System.Runtime.InteropServices;

namespace ProjectCars2.DataFormat
{

    public struct SharedMemory
    {
        public int MVersion;
        public int MBuildVersionNumber;

        public int MGameState;
        public int MSessionState;
        public int MRaceState;

        public int MViewedParticipantIndex;
        public int MNumParticipants;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EMaxPart.StoredParticipantsMax)]
        public ParticipantInfo[] MParticipantInfo;

        public float MUnfilteredThrottle;
        public float MUnfilteredBrake;
        public float MUnfilteredSteering;
        public float MUnfilteredClutch;

        public SmString MCarName;
        public SmString MCarClassName;

        public int MLapsInEvent;
        public SmString MTrackLocation;
        public SmString MTrackVariation;
        public float MTrackLength;

        public float unknown; // New in Pcars 2

        [MarshalAs(UnmanagedType.Bool)]
        public bool MLapInvalidated;

        public float MBestLapTime;
        public float MLastLapTime;
        public float MCurrentTime;
        public float MSplitTimeAhead;
        public float MSplitTimeBehind;
        public float MSplitTime;
        public float MEventTimeRemaining;
        public float MPersonalFastestLapTime;
        public float MWorldFastestLapTime;
        public float MCurrentSector1Time;
        public float MCurrentSector2Time;
        public float MCurrentSector3Time;
        public float MFastestSector1Time;
        public float MFastestSector2Time;
        public float MFastestSector3Time;
        public float MPersonalFastestSector1Time;
        public float MPersonalFastestSector2Time;
        public float MPersonalFastestSector3Time;
        public float MWorldFastestSector1Time;
        public float MWorldFastestSector2Time;
        public float MWorldFastestSector3Time;

        public int MHighestFlagColour;
        public int MHighestFlagReason;

        public int MPitMode;
        public int MPitSchedule;

        public int MCarFlags;
        public float MOilTempCelsius;
        public float MOilPressureKPa;
        public float MWaterTempCelsius;
        public float MWaterPressureKPa;
        public float MFuelPressureKPa;
        public float MFuelLevel;
        public float MFuelCapacity;
        public float MSpeed;
        public float MRpm;
        public float MMaxRpm;
        public float MBrake;
        public float MThrottle;
        public float MClutch;
        public float MSteering;
        public int MGear;
        public int MNumGears;
        public float MOdometerKm;

        [MarshalAs(UnmanagedType.I1)]
        public bool MAntiLockActive;

        public int MLastOpponentCollisionIndex;
        public float MLastOpponentCollisionMagnitude;
        public bool MBoostActive;
        public float MBoostAmount;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EVector.VecMax)]
        public float[] MOrientation;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EVector.VecMax)]
        public float[] MLocalVelocity;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EVector.VecMax)]
        public float[] MWorldVelocity;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EVector.VecMax)]
        public float[] MAngularVelocity;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EVector.VecMax)]
        public float[] MLocalAcceleration;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EVector.VecMax)]
        public float[] MWorldAcceleration;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EVector.VecMax)]
        public float[] MExtentsCentre;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)ETyres.TyreMax)]
        public int[] MTyreFlags;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)ETyres.TyreMax)]
        public int[] MTerrain;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)ETyres.TyreMax)]
        public float[] MTyreY;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)ETyres.TyreMax)]
        public float[] MTyreRps;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)ETyres.TyreMax)]
        public float[] MTyreSlipSpeed;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)ETyres.TyreMax)]
        public float[] MTyreTemp;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)ETyres.TyreMax)]
        public float[] MTyreGrip;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)ETyres.TyreMax)]
        public float[] MTyreHeightAboveGround;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)ETyres.TyreMax)]
        public float[] MTyreLateralStiffness;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)ETyres.TyreMax)]
        public float[] MTyreWear;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)ETyres.TyreMax)]
        public float[] MBrakeDamage;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)ETyres.TyreMax)]
        public float[] MSuspensionDamage;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)ETyres.TyreMax)]
        public float[] MBrakeTempCelsius;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)ETyres.TyreMax)]
        public float[] MTyreTreadTemp;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)ETyres.TyreMax)]
        public float[] MTyreLayerTemp;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)ETyres.TyreMax)]
        public float[] MTyreCarcassTemp;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)ETyres.TyreMax)]
        public float[] MTyreRimTemp;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)ETyres.TyreMax)]
        public float[] MTyreInternalAirTemp;

        public int MCrashState;
        public float MAeroDamage;
        public float MEngineDamage;

        public float MAmbientTemperature;
        public float MTrackTemperature;
        public float MRainDensity;
        public float MWindSpeed;
        public float MWindDirectionX;
        public float MWindDirectionY;
        public float MCloudBrightness;
    }
}