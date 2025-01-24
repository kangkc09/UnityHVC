using System;
using System.IO;
using System.Collections.Generic;

public static class EnvLoader
{
    private static Dictionary<string, string> envVariables = new Dictionary<string, string>();

    // .env ���� �ε�
    public static void LoadEnvFile(string filePath = ".env")
    {
        if (!File.Exists(filePath))
        {
            UnityEngine.Debug.LogError($"[EnvLoader] .env ������ ã�� �� �����ϴ�: {filePath}");
            return;
        }

        foreach (var line in File.ReadAllLines(filePath))
        {
            if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith("#"))
                continue;

            var split = line.Split('=', 2);
            if (split.Length == 2)
            {
                var key = split[0].Trim();
                var value = split[1].Trim();
                envVariables[key] = value;
            }
        }
    }

    // ȯ�� ���� ��������
    public static string GetEnvVariable(string key, string defaultValue = null)
    {
        return envVariables.ContainsKey(key) ? envVariables[key] : defaultValue;
    }
}