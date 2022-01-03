using System.IO;
using System.Text;

public static class StorageManager
{
    private const string pathStartParams = "Params.txt";

    public static void LoadParams()
    {
        if (!File.Exists(pathStartParams))
            StaticDate.InitializeStartParams();
        else
            using (StreamReader sr = new StreamReader(pathStartParams))
            {
                var str = sr.ReadToEnd().ToString().Split(';');
                var data = new int[6];
                for (int i = 0; i < data.Length; i++)
                    data[i] = int.Parse(str[i]);
                StaticDate.InitializeParams(data);
            }
    }

    public static void SaveParams()
    {
        if (!File.Exists(pathStartParams))
            File.Create(pathStartParams);

        var data = StaticDate.GetParams();
        using (var file = new StreamWriter(pathStartParams, false))
        {
            var sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                sb.Append($"{data[i]};");
            file.Write(sb);
        }
    }
}
