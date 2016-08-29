using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneHUDData.Players
{
    public class Player
    {
        float _x;
        float _y;
        float _z;
        bool _isMe;

        #region Stuf
        public bool IsMe
        {
            get
            {
                return _isMe;
            }
            set
            {
                _isMe = value;
            }
        }        
        #endregion

        #region Car Position
        public float Z
        {
            get
            {
                return _z;
            }
            set
            {
                _z = value;
            }
        }

        public float Y
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
            }
        }

        public float X
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
            }
        }
        #endregion

    }
}
