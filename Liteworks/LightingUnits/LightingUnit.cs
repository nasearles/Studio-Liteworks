using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liteworks.LightingUnits
{
    class LightingUnit
    {
        private bool _isAnimated { get; set; }
        
        private byte[] _colorValue;
        private int _arraySize { get; set; }
        private int _arrayPort { get; set; }
        public string Name { get; set; }

        public LightingUnit()
        {
            _colorValue = new byte[3];
            _isAnimated = false;
            _arraySize = 60;
            _arrayPort = 9;
            Name = "default";
        }

        public LightingUnit(string n, int s, int p)
        {
            _colorValue = new byte[3];
            _colorValue[0] = 0;
            _colorValue[1] = 0;
            _colorValue[2] = 0;
            Name = n;
            _arraySize = s;
            _arrayPort = p;
        }

        public void SetColor(byte red, byte green, byte blue)
        {
            _colorValue[0] = red;
            _colorValue[1] = green;
            _colorValue[2] = blue;
            
        }


    }
}
