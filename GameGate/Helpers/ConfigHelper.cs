using System;
using System.Configuration;

namespace GameGate.Helpers
{
    public static class ConfigHelper
    {
        public static bool FirstTimeOpening => Convert.ToBoolean(Read("FirstTimeOpening"));

        public static object Read(string key)
        {
            object value = new object();

            try
            {
                value = ConfigurationManager.AppSettings[key];
            } catch (Exception ex)
            {
                // handle exception
            }

            return value;
        }

        public static object Write(string key, object value)
        {
            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                
                if (config.AppSettings.Settings[key] != null)
                    config.AppSettings.Settings[key].Value = value.ToString();
                else
                    config.AppSettings.Settings.Add(key, value.ToString());

                config.Save();

                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                // handle exception
            }

            return value;
        }
    }
}
