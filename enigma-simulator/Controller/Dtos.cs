using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace enigma_simulator
{
    internal class RotorDto
    {
        public string? Name { get; set; }
        public int Turnover { get; set; }
        public int RingSetting { get; set; }
        public string? Wiring { get; set; }
    }
    internal class ReflectorDto
    {
        public string? Name { get; set; }
        public string[]? Wiring { get; set; }
    }
    internal class PlugboardDto
    {
        public string[]? Wiring { get; set; }
    }
    internal class EnigmaInventoryDto
    {
        public RotorDto[]? rotors { get; set; }
        public ReflectorDto[]? reflectors { get; set; }
    }
    internal class EnigmaSettingsDto
    {
        public string[]? Rotors { get; set; }
        public string? Reflector { get; set; }
        public string[]? Plugboard { get; set; }
        public int[]? RingSettings { get; set; }
        public int[]? RotorPositions { get; set; }
    }
}
