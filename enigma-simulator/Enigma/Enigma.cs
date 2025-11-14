namespace enigma_simulator
{
    internal class Enigma
    {
        Reflector _reflector;
        Plugboard _plugboard;
        Rotor[] _rotors;
        public Enigma(Rotor[] rotors, Reflector reflectors, Plugboard plugboard)
        {
            _rotors = rotors;
            _reflector = reflectors;
            _plugboard = plugboard;
        }
        int GetLetterPosition(char letter)
        {
            return letter - 'a';
        }
        char GetLetter(int position)
        {
            return (char)('a' + position);
        }
        char TransformLetter(char letter)
        {
            int letterPosition = GetLetterPosition(letter);
            int transformed = _plugboard.Transform(letterPosition);
            for (int i = _rotors.Length - 1; i >= 0; i--)
            {
                transformed = _rotors[i].InForward(transformed);
            }
            transformed = _reflector.Transform(transformed);
            for (int i = 0; i < _rotors.Length; i++)
            {
                transformed = _rotors[i].InBackward(transformed);
            }
            transformed = _plugboard.Transform(transformed);
            return GetLetter(transformed);
        }
        public string TypeText(string text)
        {
            string lowered = text.ToLower();
            string outputText = "";
            for (int i = 0; i < lowered.Length; i++)
            {
                if (!char.IsLetter(lowered[i])) continue;
                outputText += TransformLetter(lowered[i]);
                RotateRotors();
            }
            return FormatOutputText(outputText);
        }
        private string FormatOutputText(string output)
        {
            int numberOfGroupsPerLine = 10;
            int groupLetterCount = 7;
            string formattedText = "";
            int lineGroups = 0;
            for(int i = 0; i < output.Length; i++)
            {
                formattedText += output[i];
                
                if(i % groupLetterCount == groupLetterCount-1)
                {
                    lineGroups++;
                    if(lineGroups == numberOfGroupsPerLine)
                    {
                        formattedText += '\n';
                        lineGroups = 0;
                    }
                    else
                    {
                        formattedText += ' ';
                    }
                }
            }
            return formattedText;
        }
        private void RotateRotors()
        {
            int i = 0;
            while (i < _rotors.Length)
            {
                if (i == _rotors.Length - 1)
                {
                    _rotors[i].Rotate();
                    break;
                }

                if (_rotors[i + 1].IsAtTurnover())
                {
                    _rotors[i].Rotate();
                    _rotors[i + 1].Rotate();
                    i += 2;
                }
                else
                {
                    i++;
                }
            }
        }
    }
}
