using MelonLoader;
using WorldPredownload.DownloadManager;
using WorldPredownload.UI;

namespace WorldPredownload
{
    public static class ModSettings
    {
        private static readonly string categoryName = "WorldPredownload";
        internal static MelonPreferences_Entry<bool> AutoFollowInvites;
        internal static MelonPreferences_Entry<bool> AutoFollowFriends = null;
        internal static MelonPreferences_Entry<bool> AutoFollowWorlds = null;
        internal static MelonPreferences_Entry<bool> ShowStatusOnQM = null;
        internal static MelonPreferences_Entry<bool> HideQMStatusWhenInActive = null;
        internal static MelonPreferences_Entry<bool> ShowHudMessages = null;
        internal static MelonPreferences_Entry<bool> ShowStatusOnHud = null;
        internal static MelonPreferences_Entry<bool> ShowPopupsOnComplete = null;
        internal static MelonPreferences_Entry<bool> TryUseAdvancedInvitePopup = null;
        public static bool autoFollowInvites { get; private set; }
        public static bool autoFollowFriends { get; private set; }
        public static bool autoFollowWorlds { get; private set; }
        public static bool showStatusOnQM { get; private set; } = true;
        public static bool hideQMStatusWhenInActive { get; private set; }
        public static bool showHudMessages { get; private set; } = true;
        public static bool showStatusOnHud { get; private set; } = true;
        public static bool showPopupsOnComplete { get; private set; } = true;
        public static bool tryUseAdvancedInvitePopup { get; private set; } = true;
        public static bool AdvancedInvites { get; private set; }

        public static void RegisterSettings()
        {
            if (Utilities.HasMod("AdvancedInvites"))
                AdvancedInvites = true;
            var category = MelonPreferences.CreateCategory(categoryName, categoryName);
            AutoFollowInvites =
                (MelonPreferences_Entry<bool>) category.CreateEntry("AutoFollowInvites", false,
                    "Auto Follow Invite Predownloads");
            //TODO: Sort this mess out
            MelonPreferences.CreateEntry(categoryName, "AutoFollowWorlds", autoFollowInvites,
                "Auto Join World Predownloads");
            MelonPreferences.CreateEntry(categoryName, "AutoFollowFriends", autoFollowFriends,
                "Auto Join Friend Predownloads");
            MelonPreferences.CreateEntry(categoryName, "ShowStatusOnQM", showStatusOnQM,
                "Display download status on QM");
            MelonPreferences.CreateEntry(categoryName, "HideQMStatusWhenInActive", hideQMStatusWhenInActive,
                "Hide status on QM when not downloading");
            MelonPreferences.CreateEntry(categoryName, "ShowStatusOnHud", showStatusOnHud,
                "Display download status on HUD");
            MelonPreferences.CreateEntry(categoryName, "ShowHudMessages", showHudMessages, "Show Hud Messages");
            MelonPreferences.CreateEntry(categoryName, "ShowPopupsOnComplete", showPopupsOnComplete,
                "Show Popup On Complete");
            if (AdvancedInvites)
                MelonPreferences.CreateEntry(categoryName, "UseAdvancedInvitesPopup", tryUseAdvancedInvitePopup,
                    "Accept invites using AdvancedInvites popup");
        }

        public static void Apply()
        {
            autoFollowInvites = MelonPreferences.GetEntryValue<bool>(categoryName, "AutoFollowInvites");
            autoFollowWorlds = MelonPreferences.GetEntryValue<bool>(categoryName, "AutoFollowWorlds");
            autoFollowFriends = MelonPreferences.GetEntryValue<bool>(categoryName, "AutoFollowFriends");
            showStatusOnQM = MelonPreferences.GetEntryValue<bool>(categoryName, "ShowStatusOnQM");
            hideQMStatusWhenInActive = MelonPreferences.GetEntryValue<bool>(categoryName, "HideQMStatusWhenInActive");
            showStatusOnHud = MelonPreferences.GetEntryValue<bool>(categoryName, "ShowStatusOnHud");
            showHudMessages = MelonPreferences.GetEntryValue<bool>(categoryName, "ShowHudMessages");
            showPopupsOnComplete = MelonPreferences.GetEntryValue<bool>(categoryName, "ShowPopupsOnComplete");
            if (AdvancedInvites)
                tryUseAdvancedInvitePopup =
                    MelonPreferences.GetEntryValue<bool>(categoryName, "UseAdvancedInvitesPopup");
            if (showStatusOnQM)
                WorldDownloadStatus.Enable();
            else
                WorldDownloadStatus.Disable();
            if (hideQMStatusWhenInActive && !WorldDownloadManager.downloading)
                WorldDownloadStatus.Disable();
            else
                WorldDownloadStatus.Enable();
        }
    }
}