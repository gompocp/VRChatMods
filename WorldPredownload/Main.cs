using gompoCommon;
using Harmony;
using MelonLoader;
using UIExpansionKit.API;
using WorldPredownload.UI;

[assembly: MelonInfo(typeof(WorldPredownload.WorldPredownload), "WorldPredownload", "1.5.0", "gompo", "https://github.com/gompocp/VRChatMods/releases/")]
[assembly: MelonGame("VRChat", "VRChat")]

namespace WorldPredownload
{
    public class WorldPredownload : MelonMod
    {
        private static MelonMod Instance;

        public WorldPredownload()
        {
            LoaderCheck.CheckForRainbows();
        }

        public static HarmonyInstance HarmonyInstance => Instance.Harmony;

        public override void OnApplicationStart()
        {
            Instance = this;
            ModSettings.RegisterSettings();
            ModSettings.LoadSettings();
            SocialMenuSetup.Patch();
            WorldInfoSetup.Patch();
            NotificationMoreActions.Patch();
            ExpansionKitApi.OnUiManagerInit += UiManagerInit;
        }

        public void UiManagerInit()
        {
            InviteButton.Setup();
            FriendButton.Setup();
            WorldButton.Setup();
            WorldDownloadStatus.Setup();
            HudIcon.Setup();
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