namespace enigma_simulator
{
    internal class Reflector
    {
        public string Name;
        int[] wiring;
        public Reflector(string name, string[] wiringSettings)
        {
            Name = name;
            wiring = new int[26];
            for (int i = 0; i < 13; i++)
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
