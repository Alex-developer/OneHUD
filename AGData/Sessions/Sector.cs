using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneHUDData.Sessions
{
    public class Sector
    {
        private int _sectorNumber;
        private float _sectorStart;

        public Sector()
        {

        }
        public int SectorNumber
        {
            get
            {
                return _sectorNumber;
            }
            set
            {
                _sectorNumber = value;
            }
        }

        public float SectorStart
        {
            get
            {
                return _sectorStart;
            }
            set
            {
                _sectorStart = value;
            }
        }
    }
}
