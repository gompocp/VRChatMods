using MelonLoader;

namespace VRCKeyboardTweaks
{
    public static class ModSettings
    {
        private static string catergoryName = "VRCKeyBoardTweaks";
        
        private static MelonPreferences_Category modCategory;

        private static MelonPreferences_Entry<bool> useTweening;

        private static MelonPreferences_Entry<float> clickVolume, keyboardScale;

        public static bool UseTweening { get; private set; } = true;
        public static float ClickVolume { get; private set; } = 1.0f;
        public static float KeyboardScale { get; private set; } = 1.0f;

        public static void RegisterSettings()
        {
            modCategory = MelonPreferences.CreateCategory(catergoryName);
            useTweening = modCategory.CreateEntry("UseTweening", UseTweening, "Animate Button Clicks") as MelonPreferences_Entry<bool>;
            clickVolume = modCategory.CreateEntry("ClickVolume", ClickVolume, "Click Volume") as MelonPreferences_Entry<float>;
            keyboardScale = modCategory.CreateEntry("KeyboardScale", KeyboardScale, "Keyboard Scale") as MelonPreferences_Entry<float>;
            LoadSettings();
        }

        public static void LoadSettings()
        {
            UseTweening = useTweening.Value;
            ClickVolume = clickVolume.Value;
            KeyboardScale = keyboardScale.Value;
            Main.SetClickVolume(ClickVolume);
            Main.SetKeyboardScale(KeyboardScale);
        }
    }
}