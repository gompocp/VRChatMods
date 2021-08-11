using System;
using MelonLoader;
using UnityEngine;
using UnityEngine.Playables;

namespace StandaloneThirdPerson
{
    public static class ModSettings
    {
        private static readonly string categoryName = "StandaloneThirdPerson";

        private static MelonPreferences_Entry<string> keyBind;
        private static MelonPreferences_Entry<float> fov, nearClipPlane;
        private static MelonPreferences_Entry<bool> enabled;


        public static KeyCode KeyBind { get; private set; } = KeyCode.T;
        public static float FOV { get; private set; } = 80;
        public static float NearClipPlane { get; private set; } = 0.01f;
        public static bool Enabled { get; private set; } = true;


        public static void RegisterSettings()
        {
            var category = MelonPreferences.CreateCategory(categoryName, categoryName);
            keyBind = category.CreateEntry("Keybind", KeyBind.ToString(), "Keybind");
            fov = category.CreateEntry("Camera FOV", FOV, "Camera FOV");
            nearClipPlane = category.CreateEntry("Camera NearClipPlane Value", NearClipPlane, "Camera NearClipPlane Value");
            enabled = category.CreateEntry("Mod Enabled", Enabled, "Mod Enabled");
        }

        public static void LoadSettings()
        {
            KeyBind = keyBind.TryParseKeyCodePref();
            NearClipPlane = nearClipPlane.Value;
            FOV = fov.Value;
            Enabled = enabled.Value;
            Main.UpdateCameraSettings();
        }

        private static KeyCode TryParseKeyCodePref(this MelonPreferences_Entry<string> pref, bool canBeNone = false)
        {
            try
            {
                if(!canBeNone && pref.Value.Equals("None")) 
                    throw new ArgumentException();
                return ParseKeyCode(pref.Value);
            }
            catch (ArgumentException)
            {
                MelonLogger.Error($"Failed to parse keybind defaulting back to: {pref.DefaultValue}");
                pref.Value = pref.DefaultValue;
                return ParseKeyCode(pref.Value);
            }
        }

        private static KeyCode ParseKeyCode(string value)
        {
            
            return (KeyCode) Enum.Parse(typeof(KeyCode), value);
        }
    }
}