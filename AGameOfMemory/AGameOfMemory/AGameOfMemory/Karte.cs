using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGameOfMemory
{
    class Karte
    {
        private int _id;

        private bool _show;

        private bool _solved;

        private float _rotationAngle;

        public int id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        public bool show
        {
            get
            {
                return _show;
            }
            set
            {
                _show = value;
            }
        }

        public bool solved
        {
            get
            {
                return _solved;
            }
            set
            {
                _solved = value;
            }
        }

        public float rotationAngle
        {
            get
            {
                return _rotationAngle;
            }
            set
            {
                _rotationAngle = value;
            }
        }
    }
}
