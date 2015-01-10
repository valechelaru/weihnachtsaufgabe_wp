using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGameOfMemory
{
    class Button
    {
        private bool _pressed;
        private bool _released;

        public bool pressed
        {
            get
            {
                return _pressed;
            }
            set
            {
                _pressed = value;
            }
        }

        public bool released
        {
            get
            {
                return _released;
            }
            set
            {
                _released = value;
            }
        }

    }
}
