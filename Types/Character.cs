namespace p3rpc.ui.cerebral.awfulfinishers.Types
{
    public enum Character
    {
        None = 0,
        Player,
        Yukari,
        Stupei,
        Akihiko,
        Mitsuru,
        Fuuka,
        Aigis,
        Ken,
        Koromaru,
        Shinjiro,
        Metis,
        AigisDLC
    }
    public static class Characters
    {
        public static string GetName(Character character)
        {
            static string name_num(Character c)
                => $"{c}";
            if (character == Character.Stupei)
            {
                return $"Junpei";
            }
            else if (character == Character.AigisDLC)
            {
                return $"Aigis (EA)";
            }
            else
            {
                return name_num(character);
            }
        }
    }
}
