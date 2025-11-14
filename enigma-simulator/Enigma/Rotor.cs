namespace enigma_simulator
{
    internal class Rotor
    {
        public int CurrentRotorPosition; //0-25
        public int RingOffset;

        readonly public string Name;
        readonly int _turnover;//0-25
        readonly int[] _wiringForward;
        readonly int[] _wiringBackward;

        public Rotor(string name, string wiring, int turnoverAt, int ringSetting)//turnoverAt 1-26, ringSetting 1-26
        {
            _wiringForward = GetWiringMapForward(wiring);
            _wiringBackward = GetWiringMapBackward(wiring);
            _turnover = turnoverAt - 1;
            CurrentRotorPosition = 0;
            RingOffset = ringSetting - 1;
            Name = name;
        }
        int[] GetWiringMapBackward(string wiring)
        {
            int[] wiringMap = new int[26];
            for (int i = 0; i < 26; i++)
            {
                char currentLetter = wiring[i];
                wiringMap[currentLetter - 'a'] = wiring[(i + 25) % 26] - 'a';
            }
            return wiringMap;
        }
        int[] GetWiringMapForward(string wiring)
        {
            int[] wiringMap = new int[26];
            for (int i = 0; i < 26; i++)
            {
                char currentLetter = wiring[i];
                wiringMap[currentLetter - 'a'] = wiring[(i + 1) % 26] - 'a';
            }
            return wiringMap;
        }
        public int InForward(int position)//0-25
        {
            int contactPosition = (position + CurrentRotorPosition + RingOffset) % 26;
            int mapsTo = _wiringForward[contactPosition];
            return (mapsTo - RingOffset - CurrentRotorPosition + 52) % 26;
        }
        public int InBackward(int position)//0-25
        {
            int contactPosition = (position + CurrentRotorPosition + RingOffset) % 26;
            int mapsFrom = _wiringBackward[contactPosition];
            return (mapsFrom - RingOffset - CurrentRotorPosition + 52) % 26;
        }
        public void Rotate()
        {
            CurrentRotorPosition = (CurrentRotorPosition + 1) % 26;
        }
        public bool IsAtTurnover()
        {
            return _turnover == CurrentRotorPosition;
        }
    }
}
