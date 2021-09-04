using MelonLoader;
using WorldPredownload.Helpers;
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

        internal static MelonPreferences_Entry<string> DownloadUserAgent;

        public static bool autoFollowInvites { get; private set; }
        public static bool autoFollowFriends { get; private set; }
        public static bool autoFollowWorlds { get; private set; }
        public static bool showStatusOnQM { get; private set; } = true;
        public static bool hideQMStatusWhenInActive { get; private set; }
        public static bool showHudMessages { get; private set; } = true;
        public static bool showStatusOnHud { get; private set; } = true;
        public static bool showPopupsOnComplete { get; private set; } = true;
        public static bool tryUseAdvancedInvitePopup { get; private set; } = true;

        public static string downloadUserAgent { get; private set; } = UserAgent.GetRandomUserAgent();

        //public static bool cvrStyle { get; private set; }
        public static bool AdvancedInvites { get; private set; }


        public static void RegisterSettings()
        {
            if (Utilities.HasMod("AdvancedInvites"))
                AdvancedInvites = true;
            var category = MelonPreferences.CreateCategory(categoryName, categoryName);
            AutoFollowInvites =
                category.CreateEntry("AutoFollowInvites", autoFollowInvites, "Auto Follow Invite Predownloads");
            AutoFollowWorlds =
                category.CreateEntry("AutoFollowWorlds", autoFollowWorlds, "Auto Join World Predownloads");
            AutoFollowFriends =
                category.CreateEntry("AutoFollowFriends", autoFollowFriends, "Auto Join Friend Predownloads");
            ShowStatusOnQM = category.CreateEntry("ShowStatusOnQM", showStatusOnQM, "Display download status on QM");
            HideQMStatusWhenInActive = category.CreateEntry("HideQMStatusWhenInActive", hideQMStatusWhenInActive,
                "Hide status on QM when not downloading");
            ShowStatusOnHud =
                category.CreateEntry("ShowStatusOnHud", showStatusOnHud, "Display download status on HUD");
            ShowHudMessages = category.CreateEntry("ShowHudMessages", showHudMessages, "Show Hud Messages");
            ShowPopupsOnComplete =
                category.CreateEntry("ShowPopupsOnComplete", showPopupsOnComplete, "Show Popup On Complete");
            DownloadUserAgent = category.CreateEntry("DownloadUserAgent", downloadUserAgent, null, null, true);

            //CVRStyle = category.CreateEntry("OverrideVRChatJoinWorldButtons", cvrStyle, "Override VRChat Join Buttons (CVR Style  & Requires Restart to Apply)") as MelonPreferences_Entry<bool>;
            if (AdvancedInvites)
                TryUseAdvancedInvitePopup = category.CreateEntry("UseAdvancedInvitesPopup", tryUseAdvancedInvitePopup, "Accept invites using AdvancedInvites popup");
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
            downloadUserAgent = DownloadUserAgent.Value;
            //cvrStyle = CVRStyle.Value;
            if (AdvancedInvites)
                tryUseAdvancedInvitePopup = TryUseAdvancedInvitePopup.Value;
            if (showStatusOnQM)
                WorldDownloadStatus.Enable();
            else
                WorldDownloadStatus.Disable();
            if (hideQMStatusWhenInActive && !DownloadManager.Downloader.Downloading)
                WorldDownloadStatus.Disable();
            else
                WorldDownloadStatus.Enable();
        }
    }
}