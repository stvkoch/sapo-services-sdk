﻿using System;
using System.Configuration;
using System.Linq;
using System.Reflection;

public class AlternateAppConfig : IDisposable
{
    private readonly string oldConfig =
        AppDomain.CurrentDomain.GetData("APP_CONFIG_FILE").ToString();

    private bool disposedValue;

    public AlternateAppConfig(string path)
    {
        AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", path);
        ResetConfigMechanism();
    }

    public void Dispose()
    {
        if (!disposedValue)
        {
            AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", oldConfig);
            ResetConfigMechanism();


            disposedValue = true;
        }
        GC.SuppressFinalize(this);
    }

    private static void ResetConfigMechanism()
    {
        typeof(ConfigurationManager)
            .GetField("s_initState", BindingFlags.NonPublic |
                                        BindingFlags.Static)
            .SetValue(null, 0);

        typeof(ConfigurationManager)
            .GetField("s_configSystem", BindingFlags.NonPublic |
                                        BindingFlags.Static)
            .SetValue(null, null);

        typeof(ConfigurationManager)
            .Assembly.GetTypes()
            .Where(x => x.FullName ==
                        "System.Configuration.ClientConfigPaths")
            .First()
            .GetField("s_current", BindingFlags.NonPublic |
                                    BindingFlags.Static)
            .SetValue(null, null);
    }
}