namespace enigma_simulator
{
    internal class Rotor
    {
        public string Name;
        public int currentRotorPosition; //0-25
        public int ringOffset;
        int turnover;//0-25
        int[] wiringForward;
        int[] wiringBackward;

        public Rotor(string name, string wiring, int turnoverAt, int ringSetting)//turnoverAt 1-26, ringSetting 1-26
        {
            currentRotorPosition = 0;
            Name = name;
            turnover = turnoverAt - 1;
            ringOffset = ringSetting - 1;
            wiringForward = GetWiringMapForward(wiring);
            wiringBackward = GetWiringMapBackward(wiring);

        }
        private int[] GetWiringMapBackward(string wiring)
        {
            int[] wiringMap = new int[26];
            for (int i = 0; i < 26; i++)
            {
                char currentLetter = wiring[i];
                wiringMap[currentLetter - 'a'] = wiring[(i + 25) % 26] - 'a';
            }
            return wiringMap;
        }
        private int[] GetWiringMapForward(string wiring)
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
            int contactPosition = (position + currentRotorPosition + ringOffset) % 26;
            int mapsTo = wiringForward[contactPosition];
            return (mapsTo - ringOffset - currentRotorPosition + 52) % 26;
        }
        public int InBackward(int position)//0-25
        {
            int contactPosition = (position + currentRotorPosition + ringOffset) % 26;
            int mapsFrom = wiringBackward[contactPosition];
            return (mapsFrom - ringOffset - currentRotorPosition + 52) % 26;
        }
        public void Rotate()
        {
            currentRotorPosition = (currentRotorPosition + 1) % 26;
        }
        public bool IsAtTurnover()
        {
            return turnover == currentRotorPosition;
        }
    }
}
