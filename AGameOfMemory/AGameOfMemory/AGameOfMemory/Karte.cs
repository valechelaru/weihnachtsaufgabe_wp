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

    }
}
