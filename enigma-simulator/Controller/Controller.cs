using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace enigma_simulator
{
    internal class Controller
    {
        Dictionary<string, Rotor> rotorsAvailble; //key: rotor's name
        Dictionary<string, Reflector> reflectorsAvailble; //key reflector's name
        Enigma enigma;
        public Controller() 
        {
            rotorsAvailble = new Dictionary<string, Rotor>();
            reflectorsAvailble = new Dictionary<string, Reflector>();
        }
        public void Initialize(string inputFileName)
        {
            EnigmaInventoryDto inventoryDto = ReadEnigmaInventoryFile("enigma-inventory.json");
            ConvertEnigmaInventoryDto(inventoryDto);

            EnigmaSettingsDto settingsDto = ReadEnigmaSettingsFile("enigma-settings.json");
            ConvertEnigmaSettingsDto(settingsDto);

            TypeInputFile(inputFileName, "output.txt");
        }
        private void TypeInputFile(string inputFileName, string outputFileName)
        {
            string plainText = ReadFile(inputFileName);
           
            string outPutText = enigma!.TypeText(plainText);

            string workingDirectory = Directory.GetCurrentDirectory();
            StreamWriter sw = new StreamWriter(Path.Combine(workingDirectory, outputFileName));

            sw.WriteLine(outPutText);
            sw.Close();

            Console.WriteLine($"Output created: {outputFileName}.");
        }

        private EnigmaSettingsDto ReadEnigmaSettingsFile(string fileName)
        {
            string lines = ReadFile(fileName);
            EnigmaSettingsDto settings = JsonConvert.DeserializeObject<EnigmaSettingsDto>(lines);
            return settings;
        }
        private EnigmaInventoryDto ReadEnigmaInventoryFile(string fileName)
        {
            string lines = ReadFile(fileName);
            EnigmaInventoryDto parts = JsonConvert.DeserializeObject<EnigmaInventoryDto>(lines);
            return parts;
        }
        private string ReadFile(string fileName)
        {
            string lines = "";
            string line;
            string workingDirectory = Directory.GetCurrentDirectory();
            StreamReader sr = new StreamReader(Path.Combine(workingDirectory, fileName));
            line = sr.ReadLine();
            while (line != null)
            {
                lines = lines + line;
                line = sr.ReadLine();
            }
            sr.Close();
            return lines;
        }
        private void ConvertEnigmaSettingsDto(EnigmaSettingsDto enigmaSettingsDto)
        {
            Rotor[] rotors = GetRotorsFromSettings(enigmaSettingsDto);
            Reflector reflector = GetReflectorFromSettings(enigmaSettingsDto);
            Plugboard plugboard = GetPlugboardFromSettings(enigmaSettingsDto);
            enigma = new Enigma(rotors, reflector, plugboard);
        }

        private Plugboard GetPlugboardFromSettings(EnigmaSettingsDto enigmaSettingsDto)
        {
            bool[] lettersChecked = new bool[26];
            for (int i = 0; i < enigmaSettingsDto.Plugboard.Length; i++)
            {
                string currentPair = enigmaSettingsDto.Plugboard[i];
                if (currentPair.Length != 2)
                {
                    throw new Exception("Plugboard settings must have pairs of letters. Check your settings file.");
                }
                currentPair = currentPair.ToLower();
                if (!LetterPartOfAlphabet(currentPair[0]) || !LetterPartOfAlphabet(currentPair[1]))
                {
                    throw new Exception($"Invalid character found in plugboard: {currentPair}. Check your settings file.");
                }
                if (lettersChecked[currentPair[0] - 'a'])
                {
                    throw new Exception($"Duplicate characters found in plugboard: {currentPair[0]}. Check your settings file.");
                }
                lettersChecked[currentPair[0] - 'a'] = true;
                if (lettersChecked[currentPair[1] - 'a'])
                {
                    throw new Exception($"Duplicate characters found in plugboard: {currentPair[1]}. Check your settings file.");
                }
                lettersChecked[currentPair[1] - 'a'] = true;
            }
            return new Plugboard(enigmaSettingsDto.Plugboard.Select(pair => pair.ToLower()).ToArray());
        }

        private Reflector GetReflectorFromSettings(EnigmaSettingsDto enigmaSettingsDto)
        {
            if (enigmaSettingsDto.Reflector == null)
            {
                throw new Exception("A reflector name must be specified for a valid enigma setting. Check your settings file.");
            }
            if (!reflectorsAvailble.TryGetValue(enigmaSettingsDto.Reflector, out Reflector reflector))
            {
                throw new Exception($"Specified reflector does not exist: {enigmaSettingsDto.Reflector}. Check your settings file.");
            }
            return reflector;
        }

        private Rotor[] GetRotorsFromSettings(EnigmaSettingsDto enigmaSettingsDto)
        {
            if (enigmaSettingsDto.Rotors == null || enigmaSettingsDto.Rotors.Length == 0)
            {
                throw new Exception("At least one rotor must be present for a valid enigma setting. Check your settings file.");
            }
            if (enigmaSettingsDto.RingSettings == null)
            {
                throw new Exception("Ring settings must be specified for a valid enigma setting. Check your settings file.");
            }
            if (enigmaSettingsDto.RingSettings.Length != enigmaSettingsDto.Rotors.Length)
            {
                throw new Exception("Ring settings must be of the same length as rotors. Check your settings file.");
            }
            if (enigmaSettingsDto.RotorPositions == null)
            {
                throw new Exception("Rotor positions must be specified for a valid enigma setting. Check your settings file.");
            }
            if (enigmaSettingsDto.RotorPositions.Length != enigmaSettingsDto.Rotors.Length)
            {
                throw new Exception("Rotor positions must be of the same length as rotors. Check your settings file.");
            }
            Rotor[] rotors = new Rotor[enigmaSettingsDto.Rotors.Length];

            for (int i = 0; i < enigmaSettingsDto.Rotors.Length; i++)
            {
                string rotorName = enigmaSettingsDto.Rotors[i];
                if (!rotorsAvailble.TryGetValue(enigmaSettingsDto.Rotors[i], out Rotor rotor))
                {
                    throw new Exception($"Specified rotor does not exist: {enigmaSettingsDto.Rotors[i]}. Check your settings file.");
                }
                if (enigmaSettingsDto.RingSettings[i] < 1 || enigmaSettingsDto.RingSettings[i] > 26)
                {
                    throw new Exception($"Invalid ring setting found for rotor: {rotor.Name}. Ring setting must be a number between 1 and 26. Check your settings file.");
                }
                if (enigmaSettingsDto.RotorPositions[i] < 1 || enigmaSettingsDto.RotorPositions[i] > 26)
                {
                    throw new Exception($"Invalid rotor positions found for rotor: {rotor.Name}. Rotor position must be a number between 1 and 26. Check your settings file.");
                }
                rotor.ringOffset = enigmaSettingsDto.RingSettings[i] - 1;
                rotor.currentRotorPosition = enigmaSettingsDto.RotorPositions[i] - 1;
                rotors[i] = rotor;
            }
            return rotors;
        }

        private void ConvertEnigmaInventoryDto(EnigmaInventoryDto enigmaInventoryDto)
        {
            if (enigmaInventoryDto.rotors == null || enigmaInventoryDto.rotors.Length == 0)
            {
                throw new Exception("No rotors were specified.");
            }
            if (enigmaInventoryDto.reflectors == null || enigmaInventoryDto.reflectors.Length == 0)
            {
                throw new Exception("No reflectos were specified.");
            }
            for (int i = 0; i < enigmaInventoryDto.rotors.Length; i++)
            {
                Rotor r = ConvertRotorDto(enigmaInventoryDto.rotors[i]);
                rotorsAvailble[r.Name] = r;
            }
            for (int i = 0; i < enigmaInventoryDto.reflectors.Length; i++)
            {
                Reflector r = ConvertReflectorDto(enigmaInventoryDto.reflectors[i]);
                reflectorsAvailble[r.Name] = r;
            }
        }
        private Reflector ConvertReflectorDto(ReflectorDto reflectorDto)
        {
            if (reflectorDto.Name == null || reflectorDto.Name == string.Empty)
            {
                throw new Exception("Reflectors must have their names specified.");
            }
            if (reflectorDto.Wiring == null || reflectorDto.Wiring.Length != 13)
            {
                throw new Exception($"Wiring of a reflector must be an array of length 13, containing strings of length 2. Reflector at fault: {reflectorDto.Name}");
            }

            bool[] lettersChecked = new bool[26];
            for (int i = 0; i < 13; i++)
            {
                string currentPair = reflectorDto.Wiring[i];
                if (currentPair.Length != 2)
                {
                    throw new Exception($"Wiring of a reflector must be an array of length 13, containing strings of length 2. Reflector at fault: {reflectorDto.Name}");
                }
                if (!LetterPartOfAlphabet(currentPair[0]) || !LetterPartOfAlphabet(currentPair[1]))
                {
                    throw new Exception($"Invalid character found in wiring: {currentPair}. Reflector at fault: {reflectorDto.Name}.");
                }
                currentPair = currentPair.ToLower();
                if (lettersChecked[currentPair[0] - 'a'])
                {
                    throw new Exception($"Duplicate characters found in wiring: {currentPair[0]}. Reflector at fault: {reflectorDto.Name}.");
                }
                lettersChecked[currentPair[0] - 'a'] = true;
                if (lettersChecked[currentPair[1] - 'a'])
                {
                    throw new Exception($"Duplicate characters found in wiring: {currentPair[1]}. Reflector at fault: {reflectorDto.Name}.");
                }
                lettersChecked[currentPair[1] - 'a'] = true;
            }
            return new Reflector(reflectorDto.Name, reflectorDto.Wiring.Select(pairing => pairing.ToLower()).ToArray());
        }
        private Rotor ConvertRotorDto(RotorDto rotorDto)
        {
            if (rotorDto.Name == null || rotorDto.Name == string.Empty)
            {
                throw new Exception("Rotors must have their names specified.");
            }
            if (rotorDto.Turnover < 1 || rotorDto.Turnover > 26)
            {
                throw new Exception($"Invalid turnover number: {rotorDto.Turnover}. Turnover must be between 1 and 26. Rotor at fault: {rotorDto.Name}.");
            }
            if (rotorDto.RingSetting < 1 || rotorDto.RingSetting > 26)
            {
                throw new Exception($"Invalid ring setting: {rotorDto.RingSetting}. Ring setting must be between 1 and 26. Rotor at fault: {rotorDto.Name}.");
            }
            if (rotorDto.Wiring.Length != 26)
            {
                throw new Exception($"Wiring of a rotor must be a string of length 26, containing all letters from a to z. Rotor at fault: {rotorDto.Name}.");
            }
            if (rotorDto.Wiring == null)
            {
                throw new Exception($"No wiring was found. Rotor at fault: {rotorDto.Name}.");
            }
            bool[] lettersChecked = new bool[26];
            for (int i = 0; i < 26; i++)
            {
                char currentLetter = rotorDto.Wiring[i];
                if (!LetterPartOfAlphabet(currentLetter))
                {
                    throw new Exception($"Invalid character found in wiring: {currentLetter}. Rotor at fault: {rotorDto.Name}.");
                }
                currentLetter = char.ToLower(currentLetter);
                if (lettersChecked[currentLetter - 'a'])
                {
                    throw new Exception($"Duplicate letter found in wiring: {currentLetter}. Rotor at fault: {rotorDto.Name}.");
                }
                lettersChecked[currentLetter - 'a'] = true;
            }
            return new Rotor(rotorDto.Name, rotorDto.Wiring.ToLower(), rotorDto.Turnover, rotorDto.RingSetting);
        }
        private bool LetterPartOfAlphabet(char letter)
        {
            return 'a' <= letter && letter <= 'z' || 'A' <= letter && letter <= 'Z';
        }
    }
}
