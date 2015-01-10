using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGameOfMemory
{
    class DisplaySettings
    {
        private int _screenWidth;
        private int _screenHeight;
        private int _coverWidth;
        private int _intAbstandX;
        private int _intAbstandY;
        private int _lineWidth;

        public int screenWidth
        {
            get 
            {
                return _screenWidth;
            }
            
            set 
            {
                _screenWidth = value;
            }
        }

        public int screenHeight
        {
            get 
            {
                return _screenHeight;
            }
            
            set 
            {
                _screenHeight = value;
            }
        }

        public int coverWidth
        {
            get
            {
                return _coverWidth;
            }

            set
            {
                _coverWidth = value;
            }
        }

        public int intAbstandX
        {
            get
            {
                return _intAbstandX;
            }

            set
            {
                _intAbstandX = value;
            }
        }

        public int intAbstandY
        {
            get
            {
                return _intAbstandY;
            }

            set
            {
                _intAbstandY = value;
            }
        }

        public int lineWidth
        {
            get
            {
                return _lineWidth;
            }

            set
            {
                _lineWidth = value;
            }
        }
    }
}
