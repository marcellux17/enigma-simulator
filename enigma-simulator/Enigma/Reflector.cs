namespace enigma_simulator
{
    internal class Reflector
    {
        readonly public string Name;
        readonly int[] _wiring;
        public Reflector(string name, string[] wiringSettings)
        {
            Name = name;
            _wiring = new int[26];
            for (int i = 0; i < 13; i++)
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
