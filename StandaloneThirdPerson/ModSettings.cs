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


        public static KeyCode KeyBind { get; private set; } = KeyCode.T;
        public static float FOV { get; private set; } = 80;
        public static float NearClipPlane { get; private set; } = 5;


        public static void RegisterSettings()
        {
            var category = MelonPreferences.CreateCategory(categoryName, categoryName);
            keyBind = category.CreateEntry("Keybind", KeyBind.ToString(), "Keybind");
            fov = category.CreateEntry("Camera FOV", FOV, "Camera FOV");
            nearClipPlane = category.CreateEntry("Camera NearClipPlane Value", NearClipPlane, "Camera NearClipPlane Value");
        }

        public static void LoadSettings()
        {
            try
            {
                if(keyBind.Value.Equals("None")) 
                    throw new ArgumentException();
                KeyBind = (KeyCode) Enum.Parse(typeof(KeyCode), keyBind.Value);
            }
            catch (ArgumentException)
            {
                MelonLogger.Error($"Failed to parse keybind defaulting back to: {keyBind.DefaultValue}");
                keyBind.Value = keyBind.DefaultValue;
                KeyBind =  (KeyCode) Enum.Parse(typeof(KeyCode), keyBind.Value);
            }
            
            NearClipPlane = nearClipPlane.Value;
            FOV = fov.Value;
            Main.UpdateCameraSettings();
        }
    }
}