using System;
using System.Collections.Generic;
using System.Linq;

namespace OneHUDData.AnalysisData
{
    class AnalysisData
    {
        private float _speed;
        private float _rpm;
        private float _brake;
        private float _steering;

        public float Steering
        {
            get
            {
                return _steering;
            }
            set
            {
                _steering = value;
            }
        }

        public float Brake
        {
            get
            {
                return _brake;
            }
            set
            {
                _brake = value;
            }
        }

        public float Rpm
        {
            get
            {
                return _rpm;
            }
            set
            {
                _rpm = value;
            }
        }

        public float Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                _speed = value;
            }
        }
    }
}
