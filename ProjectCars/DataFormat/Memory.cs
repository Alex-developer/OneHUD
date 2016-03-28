using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace ProjectCars.DataFormat
{
    public enum eSessionState
    {
        [Description("No Session")]
        SESSION_INVALID = 0,
        [Description("Practise")]
        SESSION_PRACTICE,
        [Description("Testing")]
        SESSION_TEST,
        [Description("Qualifying")]
        SESSION_QUALIFY,
        [Description("Formation Lap")]
        SESSION_FORMATIONLAP,
        [Description("Racing")]
        SESSION_RACE,
        [Description("Time Trial")]
        SESSION_TIME_ATTACK,
        //-------------
        SESSION_MAX
    }

    public enum eGameState
    {
        [Description("Waiting for game to start...")]
        GAME_EXITED = 0,
        [Description("In Menus")]
        GAME_FRONT_END,
        [Description("In Session")]
        GAME_INGAME_PLAYING,
        [Description("Game Paused")]
        GAME_INGAME_PAUSED,
        //-------------
        GAME_MAX
    }

    public struct SmString
    {
        [MarshalAs(23, SizeConst = 64)]
        public string Value;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PlayerInfo
    {
        [MarshalAs(3)]
        public Boolean mIsActive;
        public SmString mName;
        public SmPosition mWorldPosition;
        public Single mCurrentLapDistance;
        public UInt32 mRacePosition;
        public UInt32 mLapsCompleted;
        public UInt32 mCurrentLap;
        public UInt32 mCurrentSector;
    }

    public struct SmPosition
    {
        public float X;
        public float Y;
        public float Z;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SharedMemory
    {
        public uint mVersion;
        public uint mBuildVersionNumber;
        public uint mGameState;
        public uint mSessionState;
        public uint mRaceState;
        public int mViewedParticipantIndex;
        public int mNumParticipants;
        [MarshalAs(30, SizeConst = 64)]
        public PlayerInfo[] mPlayerInfo;
        public float mUnfilteredThrottle;
        public float mUnfilteredBrake;
        public float mUnfilteredSteering;
        public float mUnfilteredClutch;
        public SmString mCarName;
        public SmString mCarClassName;
        public uint mLapsInEvent;
        public SmString mTrackLocation;
        public SmString mTrackVariation;
        public float mTrackLength;
        [MarshalAs(3)]
        public bool mLapInvalidated;
        public float mBestLapTime;
        public float mLastLapTime;
        public float mCurrentTime;
        public float mSplitTimeAhead;
        public float mSplitTimeBehind;
        public float mSplitTime;
        public float mEventTimeRemaining;
        public float mPersonalFastestLapTime;
        public float mWorldFastestLapTime;
        public float mCurrentSector1Time;
        public float mCurrentSector2Time;
        public float mCurrentSector3Time;
        public float mFastestSector1Time;
        public float mFastestSector2Time;
        public float mFastestSector3Time;
        public float mPersonalFastestSector1Time;
        public float mPersonalFastestSector2Time;
        public float mPersonalFastestSector3Time;
        public float mWorldFastestSector1Time;
        public float mWorldFastestSector2Time;
        public float mWorldFastestSector3Time;
        public uint mHighestFlagColour;
        public uint mHighestFlagReason;
        public uint mPitMode;
        public uint mPitSchedule;
        public uint mCarFlags;
        public float mOilTempCelsius;
        public float mOilPressureKPa;
        public float mWaterTempCelsius;
        public float mWaterPressureKPa;
        public float mFuelPressureKPa;
        public float mFuelLevel;
        public float mFuelCapacity;
        public float mSpeed;
        public float mRpm;
        public float mMaxRPM;
        public float mBrake;
        public float mThrottle;
        public float mClutch;
        public float mSteering;
        public int mGear;
        public int mNumGears;
        public float mOdometerKM;
        [MarshalAs(3)]
        public bool mAntiLockActive;
        public int mLastOpponentCollisionIndex;
        public float mLastOpponentCollisionMagnitude;
        [MarshalAs(3)]
        public bool mBoostActive;
        public float mBoostAmount;
        [MarshalAs(30, SizeConst = 3)]
        public float[] mOrientation;
        [MarshalAs(30, SizeConst = 3)]
        public float[] mLocalVelocity;
        [MarshalAs(30, SizeConst = 3)]
        public float[] mWorldVelocity;
        [MarshalAs(30, SizeConst = 3)]
        public float[] mAngularVelocity;
        [MarshalAs(30, SizeConst = 3)]
        public float[] mLocalAcceleration;
        [MarshalAs(30, SizeConst = 3)]
        public float[] mWorldAcceleration;
        [MarshalAs(30, SizeConst = 3)]
        public float[] mExtentsCentre;
        [MarshalAs(30, SizeConst = 4)]
        public uint[] mTyreFlags;
        [MarshalAs(30, SizeConst = 4)]
        public uint[] mTerrain;
        [MarshalAs(30, SizeConst = 4)]
        public float[] mTyreY;
        [MarshalAs(30, SizeConst = 4)]
        public float[] mTyreRPS;
        [MarshalAs(30, SizeConst = 4)]
        public float[] mTyreSlipSpeed;
        [MarshalAs(30, SizeConst = 4)]
        public float[] mTyreTemp;
        [MarshalAs(30, SizeConst = 4)]
        public float[] mTyreGrip;
        [MarshalAs(30, SizeConst = 4)]
        public float[] mTyreHeightAboveGround;
        [MarshalAs(30, SizeConst = 4)]
        public float[] mTyreLateralStiffness;
        [MarshalAs(30, SizeConst = 4)]
        public float[] mTyreWear;
        [MarshalAs(30, SizeConst = 4)]
        public float[] mBrakeDamage;
        [MarshalAs(30, SizeConst = 4)]
        public float[] mSuspensionDamage;
        [MarshalAs(30, SizeConst = 4)]
        public float[] mBrakeTempCelsius;
        [MarshalAs(30, SizeConst = 4)]
        public float[] mTyreTreadTemp;
        [MarshalAs(30, SizeConst = 4)]
        public float[] mTyreLayerTemp;
        [MarshalAs(30, SizeConst = 4)]
        public float[] mTyreCarcassTemp;
        [MarshalAs(30, SizeConst = 4)]
        public float[] mTyreRimTemp;
        [MarshalAs(30, SizeConst = 4)]
        public float[] mTyreInternalAirTemp;
        public uint mCrashState;
        public float mAeroDamage;
        public float mEngineDamage;
        public float mAmbientTemperature;
        public float mTrackTemperature;
        public float mRainDensity;
        public float mWindSpeed;
        public float mWindDirectionX;
        public float mWindDirectionY;
        public float mCloudBrightness;
    }
}