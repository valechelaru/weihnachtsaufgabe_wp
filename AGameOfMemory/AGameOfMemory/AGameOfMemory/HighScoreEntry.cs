using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGameOfMemory
{
    class HighScoreEntry
    {
        private string _name;
        private int _numOfAttempts;
        private float _timer;
        private double _score;

        public string name
        {
            get{ return _name; }
            set { _name = value; }
        }

        public int numOfAttempts
        {
            get { return _numOfAttempts; }
            set { _numOfAttempts = value; }
        }

        public float timer
        {
            get { return _timer; }
            set { _timer = value; }
        }

        public double score
        {
            get { return _score; }
            set { _score = value; }
        }
    }
}
