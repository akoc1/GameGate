using GameGate.Services;
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
                value = ConfigurationManager.AppSettings.Get(key);

                if (value == null)
                    NotificationService.Instance.ShowNotification($"Couldn't fetch key: {key}", NotificationType.Warning);
            }
            catch (Exception ex)
            {
                NotificationService.Instance.ShowNotification($"Error while reading configuration: {ex.Message}", NotificationType.Error);
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
                NotificationService.Instance.ShowNotification($"Error occured while writing to configuration: {ex.Message}", NotificationType.Error);
            }

            return value;
        }
    }
}
