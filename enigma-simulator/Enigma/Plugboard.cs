using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace enigma_simulator
{
    internal class Plugboard
    {
        int[] wiring;
        public Plugboard(string[] wiringSettings)
        {
            wiring = new int[26];
            for (int i = 0; i < 26; i++)
            {
                wiring[i] = i;
            }
            for (int i = 0; i < wiringSettings.Length; i++)
            {
                string currentMap = wiringSettings[i];
                wiring[currentMap[0] - 'a'] = currentMap[1] - 'a';
                wiring[currentMap[1] - 'a'] = currentMap[0] - 'a';
            }
        }
        public int Transform(int position)
        {
            return wiring[position];
        }
    }
}
