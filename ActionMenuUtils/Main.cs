using System;
using System.IO;
using System.Linq;
using System.Reflection;
using ActionMenuApi.Api;
using gompoCommon;
using Harmony;
using MelonLoader;
using UnhollowerRuntimeLib;
using UnhollowerRuntimeLib.XrefScans;
using UnityEngine;
using UnityEngine.UI;
using VRC.Animation;
using VRC.Core;
using VRC.SDKBase;
using Main = ActionMenuUtils.Main;

[assembly: MelonGame("VRChat", "VRChat")]
[assembly: MelonOptionalDependencies("ActionMenuApi")]
[assembly: MelonInfo(typeof(Main), "ActionMenuUtils", "1.3.7", "gompo", "https://github.com/gompocp/VRChatMods/releases/")]

namespace ActionMenuUtils
{
    public class Main : MelonMod
    {
        private static AssetBundle iconsAssetBundle;
        private static Texture2D respawnIcon;
        private static Texture2D helpIcon;
        private static Texture2D goHomeIcon;
        private static Texture2D resetAvatarIcon;
        private static Texture2D rejoinInstanceIcon;
        private static ActionMenuAPI actionMenuApi;
        private static MelonMod Instance;
        public static HarmonyInstance HarmonyInstance => Instance.Harmony;


        public override void OnApplicationStart()
        {
            Instance = this;
            try
            {
                //Adapted from knah's JoinNotifier mod found here: https://github.com/knah/VRCMods/blob/master/JoinNotifier/JoinNotifierMod.cs 
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ActionMenuUtils.icons"))
                using (var tempStream = new MemoryStream((int)stream.Length))
                {
                    stream.CopyTo(tempStream);

                    iconsAssetBundle = AssetBundle.LoadFromMemory_Internal(tempStream.ToArray(), 0);
                    iconsAssetBundle.hideFlags |= HideFlags.DontUnloadUnusedAsset;
                }
                respawnIcon = iconsAssetBundle.LoadAsset_Internal("Assets/Resources/Refresh.png", Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
                respawnIcon.hideFlags |= HideFlags.DontUnloadUnusedAsset;
                helpIcon = iconsAssetBundle.LoadAsset_Internal("Assets/Resources/Help.png", Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
                helpIcon.hideFlags |= HideFlags.DontUnloadUnusedAsset;
                goHomeIcon = iconsAssetBundle.LoadAsset_Internal("Assets/Resources/Home.png", Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
                goHomeIcon.hideFlags |= HideFlags.DontUnloadUnusedAsset;
                resetAvatarIcon = iconsAssetBundle.LoadAsset_Internal("Assets/Resources/Avatar.png", Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
                resetAvatarIcon.hideFlags |= HideFlags.DontUnloadUnusedAsset;
                rejoinInstanceIcon = iconsAssetBundle.LoadAsset_Internal("Assets/Resources/Pin.png", Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
                rejoinInstanceIcon.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            }
            catch (Exception e) {
                MelonLogger.Warning("Consider checking for newer version as mod possibly no longer working, Exception occured OnAppStart(): " + e.Message);
            }
            ModSettings.RegisterSettings();
            ModSettings.Apply();
            if(MelonHandler.Mods.Any(m => m.Info.Name.Equals("ActionMenuApi"))) {
                SetupButtonsForAMAPI();
            }
            else
            {
                actionMenuApi = new ActionMenuAPI();
                SetupButtons();
            }
            
        }
        
        

       
        private static void SetupButtonsForAMAPI()
        {
            VRCActionMenuPage.AddSubMenu(ActionMenuPage.Options, "SOS",
                () =>
                {
                    
                    if (ModSettings.confirmRespawn)
                        CustomSubMenu.AddSubMenu("Respawn", 
                            () => CustomSubMenu.AddButton("Confirm Respawn",Respawn, respawnIcon),
                            respawnIcon
                        );
                    else
                        CustomSubMenu.AddButton("Respawn",Respawn, respawnIcon);

                    //Reset Avatar
                    if (ModSettings.confirmAvatarReset)
                        CustomSubMenu.AddSubMenu("Reset Avatar",
                            () => CustomSubMenu.AddButton("Confirm Reset Avatar", ResetAvatar, resetAvatarIcon), 
                            resetAvatarIcon
                        );
                    else
                        CustomSubMenu.AddButton("Reset Avatar", ResetAvatar, resetAvatarIcon);
                   
                    //Instance Rejoin
                    if (ModSettings.confirmInstanceRejoin)
                        CustomSubMenu.AddSubMenu("Rejoin Instance", 
                            () => CustomSubMenu.AddButton("Confirm Instance Rejoin",RejoinInstance, rejoinInstanceIcon), 
                            rejoinInstanceIcon
                        );
                    else
                        CustomSubMenu.AddButton("Rejoin Instance",RejoinInstance, rejoinInstanceIcon);
                    
                    //Go Home
                    if (ModSettings.confirmGoHome)
                        CustomSubMenu.AddSubMenu("Go Home", 
                            () => CustomSubMenu.AddButton("Confirm Go Home",GoHome, goHomeIcon), 
                            goHomeIcon
                        );
                    else
                        CustomSubMenu.AddButton("Go Home",GoHome, goHomeIcon);

                }, helpIcon
            );
        }

        public override void OnPreferencesLoaded() => ModSettings.Apply();
        public override void OnPreferencesSaved() => ModSettings.Apply();


        private static void SetupButtons()
        {
           
            actionMenuApi.AddPedalToExistingMenu(ActionMenuAPI.ActionMenuPageType.Options, delegate
            {
                actionMenuApi.CreateSubMenu(() => {
                    AddRespawnButton();
                    AddGoHomeButton();
                    AddResetAvatarButton();
                    AddInstanceRejoinButton();
                });
            }, "Help", helpIcon);
        }

        private static void AddResetAvatarButton()
        {
            if (ModSettings.confirmAvatarReset)
                actionMenuApi.AddPedalToCustomMenu(() =>
                        actionMenuApi.CreateSubMenu(() =>
                        actionMenuApi.AddPedalToCustomMenu(ResetAvatar, "Confirm Reset Avatar", resetAvatarIcon)
                    ), "Reset Avatar", resetAvatarIcon
                );
            else
                actionMenuApi.AddPedalToCustomMenu(ResetAvatar, "Reset Avatar", resetAvatarIcon);
        }

        private static void AddGoHomeButton()
        {
            if (ModSettings.confirmGoHome)
                actionMenuApi.AddPedalToCustomMenu(() =>
                        actionMenuApi.CreateSubMenu( () =>
                        actionMenuApi.AddPedalToCustomMenu(GoHome, "Confirm Go Home", goHomeIcon)
                    ), "Go Home", goHomeIcon
                );
            else
                actionMenuApi.AddPedalToCustomMenu(GoHome, "Go Home", goHomeIcon);
        }

        private static void AddRespawnButton()
        {
            if (ModSettings.confirmRespawn)
                actionMenuApi.AddPedalToCustomMenu(() => 
                
                    actionMenuApi.CreateSubMenu(() =>
                        actionMenuApi.AddPedalToCustomMenu(Respawn, "Confirm Respawn", respawnIcon)
                    ), "Respawn", respawnIcon
                );
            else
                actionMenuApi.AddPedalToCustomMenu(Respawn, "Respawn", respawnIcon);
        }

        private static void AddInstanceRejoinButton()
        {
            if (ModSettings.confirmInstanceRejoin)
            {
                actionMenuApi.AddPedalToCustomMenu(() => 
                
                    actionMenuApi.CreateSubMenu(() =>
                        actionMenuApi.AddPedalToCustomMenu(RejoinInstance, "Confirm Instance Rejoin", rejoinInstanceIcon)
                    ), "Rejoin Instance", rejoinInstanceIcon
                );
            }
            else
                actionMenuApi.AddPedalToCustomMenu(RejoinInstance, "Rejoin Instance", rejoinInstanceIcon);
        }

        private static void Respawn()
        {
            GameObject.Find("UserInterface/QuickMenu/ShortcutMenu/RespawnButton").GetComponent<Button>().onClick.Invoke();
            VRCPlayer.field_Internal_Static_VRCPlayer_0.GetComponent<VRCMotionState>().Reset();
        }
        private static void RejoinInstance()
        {
            var instance = RoomManager.field_Internal_Static_ApiWorldInstance_0;
            Networking.GoToRoom($"{instance.instanceWorld.id}:{instance.idWithTags}");
        }

        private static void GoHome()
        {
            if (ModSettings.forceGoHome)
                Utils.GoHome();
            else
                GameObject.Find("UserInterface/QuickMenu/ShortcutMenu/GoHomeButton").GetComponent<Button>().onClick.Invoke();
        }

        private static void ResetAvatar()
        {
            ObjectPublicAbstractSealedApBoApObStBoApApUnique.Method_Public_Static_Void_ApiAvatar_String_ApiAvatar_0(API.Fetch<ApiAvatar>("avtr_c38a1615-5bf5-42b4-84eb-a8b6c37cbd11"), "fallbackAvatar");
        }

        public Main()
        {
            LoaderCheck.CheckForRainbows();
        }
    }

    static class Utils
    {
        //Gracefully taken from Advanced Invites https://github.com/Psychloor/AdvancedInvites/blob/master/AdvancedInvites/Utilities.cs#L356
        public static bool XRefScanFor(this MethodBase methodBase, string searchTerm)
        {
            return XrefScanner.XrefScan(methodBase).Any(
                xref => xref.Type == XrefType.Global && xref.ReadAsObject()?.ToString().IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0);
        }
        public static GoHomeDelegate GetGoHomeDelegate
        {
            get
            {
                if (goHomeDelegate != null) return goHomeDelegate;
                MethodInfo goHomeMethod = typeof(VRCFlowManager).GetMethods(BindingFlags.Public | BindingFlags.Instance).First(
                    m => m.GetParameters().Length == 0 && m.ReturnType == typeof(void) && m.XRefScanFor("Going to Home Location: "));

                goHomeDelegate = (GoHomeDelegate)Delegate.CreateDelegate(
                    typeof(GoHomeDelegate),
                    VRCFlowManager.prop_VRCFlowManager_0,
                    goHomeMethod);
                return goHomeDelegate;
            }
        }
        public static void GoHome() => GetGoHomeDelegate();
        private static GoHomeDelegate goHomeDelegate;
        public delegate void GoHomeDelegate();
    }
}