namespace enigma_simulator
{
    internal class Plugboard
    {
        int[] _wiring;
        public Plugboard(string[] wiringSettings)
        {
            _wiring = new int[26];
            for (int i = 0; i < 26; i++)
            {
                _wiring[i] = i;
            }
            for (int i = 0; i < wiringSettings.Length; i++)
            {
                string currentMap = wiringSettings[i];
                _wiring[currentMap[0] - 'a'] = currentMap[1] - 'a';
                _wiring[currentMap[1] - 'a'] = currentMap[0] - 'a';
            }
        }
        public int Transform(int position)
        {
            return _wiring[position];
        }
    }
}
