using System;
using MelonLoader;
using UnityEngine;

namespace StandaloneThirdPerson
{
    public static class ModSettings
    {
        private static readonly string categoryName = "StandaloneThirdPerson";

        private static MelonPreferences_Entry<string> keyBind, freeformKeyBind, moveRearCameraLeftKeyBind, moveRearCameraRightKeyBind;
        private static MelonPreferences_Entry<float> fov, nearClipPlane;
        private static MelonPreferences_Entry<bool> enabled, rearCameraChangerEnabled;


        public static KeyCode KeyBind { get; private set; } = KeyCode.T;
        public static KeyCode FreeformKeyBind { get; private set; } = KeyCode.None;
        public static KeyCode MoveRearCameraLeftKeyBind { get; private set; } = KeyCode.Q;
        public static KeyCode MoveRearCameraRightKeyBind { get; private set; } = KeyCode.E;
        public static float FOV { get; private set; } = 80;
        public static float NearClipPlane { get; private set; } = 0.01f;
        public static bool Enabled { get; private set; } = true;
        public static bool RearCameraChangedEnabled = true;


        public static void RegisterSettings()
        {
            var category = MelonPreferences.CreateCategory(categoryName, categoryName);
            keyBind = category.CreateEntry("Keybind", KeyBind.ToString(), "Keybind");
            freeformKeyBind = category.CreateEntry("Freeform Keybind", FreeformKeyBind.ToString(), "Freeform Keybind");
            fov = category.CreateEntry("Camera FOV", FOV, "Camera FOV");
            nearClipPlane = category.CreateEntry("Camera NearClipPlane Value", NearClipPlane, "Camera NearClipPlane Value");
            enabled = category.CreateEntry("Mod Enabled", Enabled, "Mod Enabled");
            rearCameraChangerEnabled = category.CreateEntry("Rear Camera Changer Enabled", RearCameraChangedEnabled, "Rear Camera Changer Enabled");
            moveRearCameraLeftKeyBind = category.CreateEntry("Move Rear Camera Left KeyBind", MoveRearCameraLeftKeyBind.ToString(), "Move Rear Camera Left KeyBind");
            moveRearCameraRightKeyBind = category.CreateEntry("Move Rear Camera Right KeyBind", MoveRearCameraRightKeyBind.ToString(), "Move Rear Camera Right KeyBind");
        }

        public static void LoadSettings()
        {
            KeyBind = keyBind.TryParseKeyCodePref();
            FreeformKeyBind = freeformKeyBind.TryParseKeyCodePref(canBeNone: true);
            MoveRearCameraLeftKeyBind = moveRearCameraLeftKeyBind.TryParseKeyCodePref();
            MoveRearCameraRightKeyBind = moveRearCameraRightKeyBind.TryParseKeyCodePref();
            NearClipPlane = nearClipPlane.Value;
            FOV = fov.Value;
            Enabled = enabled.Value;
            RearCameraChangedEnabled = rearCameraChangerEnabled.Value;
            Main.UpdateCameraSettings();
        }

        private static KeyCode TryParseKeyCodePref(this MelonPreferences_Entry<string> pref, bool canBeNone = false)
        {
            try
            {
                if (!canBeNone && pref.Value.Equals("None"))
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
