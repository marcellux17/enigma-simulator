using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace enigma_simulator
{
    internal class RotorDto
    {
        public string Name;
        public int Turnover;
        public int RingSetting;
        public string Wiring;
    }
    internal class ReflectorDto
    {
        public string Name;
        public string[] Wiring;
    }
    internal class PlugboardDto
    {
        public string[] Wiring;
    }
    internal class EnigmaInventoryDto
    {
        public RotorDto[] rotors;
        public ReflectorDto[] reflectors;
    }
    internal class EnigmaSettingsDto
    {
        public string[] Rotors;
        public string Reflector;
        public string[] Plugboard;
        public int[] RingSettings;
        public int[] RotorPositions;
    }
}
