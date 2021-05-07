using MelonLoader;

namespace ActionMenuUtils
{
    public static class ModSettings
    {
        private static string categoryName = "ActionMenuRespawn"; //Old Name
        private static string categoryDisplayName = "ActionMenuUtils"; //New Name
        public static bool confirmRespawn { get; private set; } = false;
        public static bool confirmGoHome { get; private set; } = false;
        public static bool confirmAvatarReset { get; private set; } = false;
        public static bool confirmInstanceRejoin { get; private set; } = true;
        public static bool forceGoHome { get; private set; } = false;

        public static void RegisterSettings()
        {
            MelonPreferences.CreateCategory(categoryName, categoryDisplayName);
            MelonPreferences.CreateEntry(categoryName, "ConfirmRespawn", confirmRespawn, "Add a confirmation for respawn");
            MelonPreferences.CreateEntry(categoryName, "ConfirmGoHome", confirmGoHome, "Add a confirmation for go home");
            MelonPreferences.CreateEntry(categoryName, "ForceGoHome", forceGoHome, "Skip the go home popup");
            MelonPreferences.CreateEntry(categoryName, "ConfirmAvatarReset", confirmAvatarReset, "Add a confirmation for avatar reset");
            MelonPreferences.CreateEntry(categoryName, "ConfirmInstanceReJoin", confirmInstanceRejoin, "Add a confirmation for rejoin instance");
        }

        public static void Apply()
        {
            confirmRespawn = MelonPreferences.GetEntryValue<bool>(categoryName, "ConfirmRespawn");
            confirmGoHome = MelonPreferences.GetEntryValue<bool>(categoryName, "ConfirmGoHome");
            forceGoHome = MelonPreferences.GetEntryValue<bool>(categoryName, "ForceGoHome");
            confirmAvatarReset = MelonPreferences.GetEntryValue<bool>(categoryName, "ConfirmAvatarReset");
            confirmInstanceRejoin = MelonPreferences.GetEntryValue<bool>(categoryName, "ConfirmInstanceReJoin");
        }
    }
}