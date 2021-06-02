using MelonLoader;
using WorldPredownload.DownloadManager;
using WorldPredownload.UI;

namespace WorldPredownload
{
    public static class ModSettings
    {
        private static readonly string categoryName = "WorldPredownload";

        internal static MelonPreferences_Entry<bool> AutoFollowInvites,
            AutoFollowFriends,
            AutoFollowWorlds,
            ShowStatusOnQM,
            HideQMStatusWhenInActive,
            ShowHudMessages,
            ShowStatusOnHud,
            ShowPopupsOnComplete,
            TryUseAdvancedInvitePopup; //CVRStyle;
        public static bool autoFollowInvites { get; private set; }
        public static bool autoFollowFriends { get; private set; }
        public static bool autoFollowWorlds { get; private set; }
        public static bool showStatusOnQM { get; private set; } = true;
        public static bool hideQMStatusWhenInActive { get; private set; }
        public static bool showHudMessages { get; private set; } = true;
        public static bool showStatusOnHud { get; private set; } = true;
        public static bool showPopupsOnComplete { get; private set; } = true;
        public static bool tryUseAdvancedInvitePopup { get; private set; } = true;
        //public static bool cvrStyle { get; private set; }
        public static bool AdvancedInvites { get; private set; }
        

        public static void RegisterSettings()
        {
            if (Utilities.HasMod("AdvancedInvites"))
                AdvancedInvites = true;
            var category = MelonPreferences.CreateCategory(categoryName, categoryName);
            AutoFollowInvites = category.CreateEntry("AutoFollowInvites", autoFollowInvites, "Auto Follow Invite Predownloads") as MelonPreferences_Entry<bool>;
            AutoFollowWorlds = category.CreateEntry("AutoFollowWorlds", autoFollowWorlds, "Auto Join World Predownloads") as MelonPreferences_Entry<bool>;
            AutoFollowFriends = category.CreateEntry("AutoFollowFriends", autoFollowFriends, "Auto Join Friend Predownloads") as MelonPreferences_Entry<bool>;
            ShowStatusOnQM = category.CreateEntry("ShowStatusOnQM", showStatusOnQM, "Display download status on QM") as MelonPreferences_Entry<bool>;
            HideQMStatusWhenInActive = category.CreateEntry("HideQMStatusWhenInActive", hideQMStatusWhenInActive, "Hide status on QM when not downloading") as MelonPreferences_Entry<bool>;
            ShowStatusOnHud = category.CreateEntry("ShowStatusOnHud", showStatusOnHud, "Display download status on HUD") as MelonPreferences_Entry<bool>;
            ShowHudMessages = category.CreateEntry("ShowHudMessages", showHudMessages, "Show Hud Messages") as MelonPreferences_Entry<bool>;
            ShowPopupsOnComplete = category.CreateEntry("ShowPopupsOnComplete", showPopupsOnComplete, "Show Popup On Complete") as MelonPreferences_Entry<bool>;
            //CVRStyle = category.CreateEntry("OverrideVRChatJoinWorldButtons", cvrStyle, "Override VRChat Join Buttons (CVR Style  & Requires Restart to Apply)") as MelonPreferences_Entry<bool>;
            if (AdvancedInvites)
                TryUseAdvancedInvitePopup = category.CreateEntry("UseAdvancedInvitesPopup", tryUseAdvancedInvitePopup, "Accept invites using AdvancedInvites popup") as MelonPreferences_Entry<bool>;

        }

        public static void LoadSettings()
        {
            autoFollowInvites = AutoFollowInvites.Value;
            autoFollowWorlds = AutoFollowWorlds.Value;
            autoFollowFriends = AutoFollowFriends.Value;
            showStatusOnQM = ShowStatusOnQM.Value;
            hideQMStatusWhenInActive = HideQMStatusWhenInActive.Value;
            showStatusOnHud = ShowStatusOnHud.Value;
            showHudMessages = ShowHudMessages.Value;
            showPopupsOnComplete = ShowPopupsOnComplete.Value;
            //cvrStyle = CVRStyle.Value;
            if (AdvancedInvites)
                tryUseAdvancedInvitePopup = TryUseAdvancedInvitePopup.Value;
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