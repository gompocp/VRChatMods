using MelonLoader;
using UIExpansionKit.API;
using WorldPredownload.Helpers;
using WorldPredownload.UI;

[assembly: MelonInfo(typeof(WorldPredownload.WorldPredownload), "WorldPredownload", "1.6.2", "gompo", "https://github.com/gompocp/VRChatMods/releases/")]
[assembly: MelonGame("VRChat", "VRChat")]
[assembly: VerifyLoaderVersion(0, 4, 3, true)]

namespace WorldPredownload
{
    internal partial class WorldPredownload : MelonMod
    {
        private static MelonMod instance;

        private static readonly string ID = "gompo";

        public new static HarmonyLib.Harmony HarmonyInstance => instance.HarmonyInstance;

        public override void OnApplicationStart()
        {
            instance = this;
            ModSettings.RegisterSettings();
            ModSettings.LoadSettings();
            SocialMenuSetup.Patch();
            WorldInfoSetup.Patch();
            NotificationMoreActions.Patch();
            ExpansionKitApi.OnUiManagerInit += UiManagerInit;
        }

        private void UiManagerInit()
        {
            if (string.IsNullOrEmpty(ID)) return;
            var downloader = Singleton<DownloadManager.Downloader>.Instance;
            downloader.Attach(ResettableSingleton<InviteButton>.Instance);
            downloader.Attach(Singleton<FriendButton>.Instance);
            downloader.Attach(Singleton<WorldButton>.Instance);
            downloader.Attach(Singleton<WorldDownloadStatus>.Instance);
            downloader.Attach(Singleton<HudIcon>.Instance);
        }

        public override void OnPreferencesLoaded()
        {
            ModSettings.LoadSettings();
        }

        public override void OnPreferencesSaved()
        {
            ModSettings.LoadSettings();
        }
    }
}