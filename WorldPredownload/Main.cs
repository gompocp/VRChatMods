﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using HarmonyLib;
using MelonLoader;
using UIExpansionKit.API;
using WorldPredownload.UI;

[assembly: MelonInfo(typeof(WorldPredownload.WorldPredownload), "WorldPredownload", "1.5.1", "gompo", "https://github.com/gompocp/VRChatMods/releases/")]
[assembly: MelonGame("VRChat", "VRChat")]

namespace WorldPredownload
{
    internal partial class WorldPredownload : MelonMod
    {
        private static MelonMod Instance;
        

        public new static HarmonyLib.Harmony HarmonyInstance => Instance.HarmonyInstance;

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

        private void UiManagerInit()
        {
            if (string.IsNullOrEmpty(ID)) return;
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
        
        private static string ID = "gompo";
        
    }
}