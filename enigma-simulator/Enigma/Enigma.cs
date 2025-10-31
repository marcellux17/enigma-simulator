using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace enigma_simulator
{
    internal class Enigma
    {
        Reflector reflector;
        Plugboard plugboard;
        Rotor[] rotors;
        public Enigma(Rotor[] rotorsGiven, Reflector reflectorGiven, Plugboard plugboardGiven)
        {
            rotors = rotorsGiven;
            reflector = reflectorGiven;
            plugboard = plugboardGiven;
        }
        private int getLetterPosition(char letter)
        {
            return letter - 'a';
        }
        private char getLetterFromPosition(int position)
        {
            return (char)('a' + position);
        }
        private char TransformLetter(char letter)
        {
            int letterPosition = getLetterPosition(letter);
            int transformed = plugboard.Transform(letterPosition);
            for (int i = rotors.Length - 1; i >= 0; i--)
            {
                transformed = rotors[i].InForward(transformed);
            }
            transformed = reflector.Transform(transformed);
            for (int i = 0; i < rotors.Length; i++)
            {
                transformed = rotors[i].InBackward(transformed);
            }
            transformed = plugboard.Transform(transformed);
            return getLetterFromPosition(transformed);
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
            return formatOutput(outputText);
        }
        private string formatOutput(string output)
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
            while (i < rotors.Length)
            {
                if (i == rotors.Length - 1)
                {
                    rotors[i].Rotate();
                    break;
                }

                if (rotors[i + 1].IsAtTurnover())
                {
                    rotors[i].Rotate();
                    rotors[i + 1].Rotate();
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
