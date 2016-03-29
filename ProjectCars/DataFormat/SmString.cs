using System.Runtime.InteropServices;

namespace ProjectCars.DataFormat
{
    public struct SmString
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = (int) EStringLenMax.StringLengthMax)] public string Value;
    }
}