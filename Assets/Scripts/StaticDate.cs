public struct Settings
{
    public readonly int Heigth;
    public readonly int Weidth;
    public readonly int EnemiesNumber;
    public readonly int EnergizerNumber;
    public readonly int PlayerSpeed;
    public readonly int EnemySpeed;

    public Settings(int[] data)
    {
        Heigth = data[0];
        Weidth = data[1];
        EnemiesNumber = data[2];
        EnergizerNumber = data[3];
        PlayerSpeed = data[4];
        EnemySpeed = data[5];
    }
}

public static class StaticDate
{
    public static Settings Settings;

    public static void InitializeStartParams()
    {
        var data = new int[] { 21, 31, 4, 4, 4, 2 };
        Settings = new Settings(data);
    }

    public static void InitializeParams(int[] data)
        => Settings = new Settings(data);

    public static int[] GetParams()
    {
        var data = new int[6];

        data[0] = Settings.Heigth;
        data[1] = Settings.Weidth;
        data[2] = Settings.EnemiesNumber;
        data[3] = Settings.EnergizerNumber;
        data[4] = Settings.PlayerSpeed;
        data[5] = Settings.EnemySpeed;

        return data;
    }
}
