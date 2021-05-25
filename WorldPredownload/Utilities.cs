using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using Il2CppSystem;
using MelonLoader;
using Transmtn.DTO.Notifications;
using UnhollowerRuntimeLib.XrefScans;
using UnityEngine;
using UnityEngine.EventSystems;
using VRC.Core;
using VRC.SDKBase;
using WorldPredownload.DownloadManager;
using Delegate = System.Delegate;
using Exception = System.Exception;
using OnDownloadComplete = AssetBundleDownloadManager.MulticastDelegateNInternalSealedVoObUnique;
using OnDownloadProgress = AssetBundleDownloadManager.MulticastDelegateNInternalSealedVoUnUnique;
using OnDownloadError = AssetBundleDownloadManager.MulticastDelegateNInternalSealedVoStObStUnique;
using StringComparison = System.StringComparison;
using UnpackType = AssetBundleDownloadManager.EnumNInternalSealedva3vUnique;

namespace WorldPredownload
{
    [SuppressMessage("ReSharper", "HeuristicUnreachableCode")]
    public static class Utilities
    {
        private static List<string> downloadWorldKeyWords = new(new[] {"vrcw", "Worlds", "Failed to parse world '", "' UnityVersion '"});
        
        private static DownloadWorldDelegate downloadWorldDelegate;

        private static ClearErrorsDelegate clearErrorsDelegate;

        private static ShowDismissPopupDelegate showDismissPopupDelegate;

        private static ShowOptionsPopupDelegate showOptionsPopupDelegate;

        private static PushUIPageDelegate pushUIPageDelegate;

        private static AdvancedInvitesInviteDelegate advancedInvitesInviteDelegate;
        
        private static DownloadWorldDelegate GetDownloadWorldDelegate
        {
            get
            {
                if (downloadWorldDelegate != null) return downloadWorldDelegate;
                downloadWorldDelegate = (DownloadWorldDelegate)Delegate.CreateDelegate(
                    typeof(DownloadWorldDelegate),
                    AssetBundleDownloadManager.prop_AssetBundleDownloadManager_0,
                    WorldDownloadMethodInfo
                );
                return downloadWorldDelegate;
            }
        }

        private static ClearErrorsDelegate GetClearErrorsDelegate
        {
            get
            {
                if (clearErrorsDelegate != null) return clearErrorsDelegate;
                MethodInfo clearErrors = typeof(AssetBundleDownloadManager).GetMethods().First(
                    m => m.Name.StartsWith("Method_Internal_Void_") 
                    && !m.Name.Contains("PDM")
                    && m.ReturnType == typeof(void) 
                    && (m.GetParameters().Length == 0));
                clearErrorsDelegate = (ClearErrorsDelegate)Delegate.CreateDelegate(
                        typeof(ClearErrorsDelegate),
                        AssetBundleDownloadManager.prop_AssetBundleDownloadManager_0,
                        clearErrors
                    );
                return clearErrorsDelegate;
            }
        }
        
        private static ShowDismissPopupDelegate GetShowDismissPopupDelegate
        {
            get
            {
                if (showDismissPopupDelegate != null) return showDismissPopupDelegate;
                MethodInfo popupMethod = typeof(VRCUiPopupManager).GetMethods(BindingFlags.Public | BindingFlags.Instance).First(
                    m => 
                    m.GetParameters().Length == 5 
                    
                    && m.XRefScanFor("Popups/StandardPopupV2")
                );

                showDismissPopupDelegate = (ShowDismissPopupDelegate)Delegate.CreateDelegate(
                            typeof(ShowDismissPopupDelegate),
                            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0,
                            popupMethod
                );
                return showDismissPopupDelegate;
            }
        }

        private static ShowOptionsPopupDelegate GetShowOptionsPopupDelegate
        {
            get
            {
                if (showOptionsPopupDelegate != null) return showOptionsPopupDelegate;
                MethodInfo popupMethod = typeof(VRCUiPopupManager).GetMethods(BindingFlags.Public | BindingFlags.Instance).Single(
                    m => m.GetParameters().Length == 7 && m.XRefScanFor("Popups/StandardPopupV2"));

                showOptionsPopupDelegate = (ShowOptionsPopupDelegate)Delegate.CreateDelegate(
                    typeof(ShowOptionsPopupDelegate),
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0,
                    popupMethod
                );
                return showOptionsPopupDelegate;
            }
        }

        private static PushUIPageDelegate GetPushUIPageDelegate
        {
            get
            {
                if (pushUIPageDelegate != null) return pushUIPageDelegate;
                MethodInfo pushPageMethod = typeof(VRCUiManager).GetMethods().First(
                    m => m.GetParameters().Length == 1
                    && m.GetParameters()[0].ParameterType == typeof(VRCUiPage)
                    && !m.Name.Contains("PDM")
                    && m.ReturnType == typeof(VRCUiPage)
                );

                pushUIPageDelegate = (PushUIPageDelegate)Delegate.CreateDelegate(
                    typeof(PushUIPageDelegate),
                    VRCUiManager.prop_VRCUiManager_0,
                    pushPageMethod
                );
                return pushUIPageDelegate;
            }
        }

        private static AdvancedInvitesInviteDelegate GetAdvancedInvitesInviteDelegate
        {
            get
            {
                if (advancedInvitesInviteDelegate != null) return advancedInvitesInviteDelegate;

                //InviteHandler
                var handleNotificationMethod = MelonHandler.Mods.First(
                    m => m.Info.Name.Equals("AdvancedInvites")).Assembly.GetTypes().Single(
                        t => t.Name.Equals("InviteHandler")).GetMethods(BindingFlags.Public | BindingFlags.Static).Single(
                            me => me.GetParameters().Length == 1 
                            && me.GetParameters()[0].ParameterType == typeof(Notification) // Could probably use method name here but ¯\_(ツ)_/¯ 
                        );

                advancedInvitesInviteDelegate = (AdvancedInvitesInviteDelegate)Delegate.CreateDelegate(
                    typeof(AdvancedInvitesInviteDelegate),
                    handleNotificationMethod
                );
                return advancedInvitesInviteDelegate;
            }
        }

        public static void AdvancedInvitesHandleInvite(Notification notification)
        {
#if DEBUG
            try
            {
                GetAdvancedInvitesInviteDelegate(notification);
            }
            catch (Exception e)
            {
                MelonLogger.LogError($"Beep boop, Something went wrong trying to used advanced invites {e}");
            }
#else
            GetAdvancedInvitesInviteDelegate(notification);
#endif
        }

        public static void DownloadApiWorld(ApiWorld world, OnDownloadProgress onProgress, OnDownloadComplete onSuccess, OnDownloadError onError, bool bypassDownloadSizeLimit, UnpackType unpackType)
        {
            GetDownloadWorldDelegate(world, onProgress, onSuccess, onError, bypassDownloadSizeLimit, unpackType);
        }

        public static void ClearErrors() => GetClearErrorsDelegate();
 

        public static void ShowOptionPopup(string title, string body, string leftButtonText, Action leftButtonAction, string rightButtonText, Action rightButtonAction)
        {
            GetShowOptionsPopupDelegate(title, body, leftButtonText, leftButtonAction, rightButtonText, rightButtonAction);
        }

        public static void ShowDismissPopup(string title, string body, string middleButtonText, Action buttonAction)
        {
            GetShowDismissPopupDelegate(title, body, middleButtonText, buttonAction);
        }

        public static MethodInfo worldDownloadMethodInfo;

        public static MethodInfo WorldDownloadMethodInfo
        {
            get
            {
                if (worldDownloadMethodInfo != null) return worldDownloadMethodInfo;
                worldDownloadMethodInfo = typeof(AssetBundleDownloadManager).GetMethods().Single(m => m.Name.StartsWith("Method_Internal_Void_") && CheckXrefStrings(m, downloadWorldKeyWords));
                return worldDownloadMethodInfo;
            }
        }
        
        
        public static void ShowPage(VRCUiPage page) => GetPushUIPageDelegate(page);

        public static AssetBundleDownloadManager GetAssetBundleDownloadManager()
        {
            return AssetBundleDownloadManager.prop_AssetBundleDownloadManager_0;
        }

        public static void HideCurrentPopup() => VRCUiManager.prop_VRCUiManager_0.HideScreen("POPUP");


        public static Notification GetSelectedNotification()
        {
            return NotificationMoreActions.selectedNotification;
        }
        
        public static GameObject CloneGameObject(string pathToGameObject, string pathToParent)
        {
            return GameObject.Instantiate(GameObject.Find(pathToGameObject).transform, GameObject.Find(pathToParent).transform).gameObject;
        }

        public static void DeselectClickedButton(GameObject button)
        {
            if (EventSystem.current.currentSelectedGameObject == button)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }

        public static void GoToWorld(ApiWorld apiWorld, string tags, bool isInvite)
        {
            if (isInvite)
            {
                if (ModSettings.tryUseAdvancedInvitePopup && ModSettings.AdvancedInvites)
                {
                    try
                    {
                        GetAdvancedInvitesInviteDelegate(WorldDownloadManager.DownloadInfo.Notification);
                    }
                    catch (Exception e)
                    {
                        MelonLogger.Error("Unable to execute Advanced Invite's Invite Handler Func" + e);
                    }
                }
                else
                    Networking.GoToRoom($"{apiWorld.id}:{tags}");
                    //new PortalInternal().Method_Private_Void_String_String_PDM_0(apiWorld.id, tags);
            }
            else 
                Networking.GoToRoom($"{apiWorld.id}:{tags}");
        }

        public static bool isInSameWorld(APIUser user)
        {
            if (user.location.Contains(RoomManager.field_Internal_Static_ApiWorld_0.id))
                return true;
            else
                return false;
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static bool HasMod(string modName) =>
            MelonHandler.Mods.Any(mod => mod.Info.Name.Equals(modName));

        public static bool CheckXrefStrings(MethodBase m, List<string> keywords)
        {
            try
            {
                foreach (string keyword in keywords)
                {

                    if (!XrefScanner.XrefScan(m).Any(
                    instance => instance.Type == XrefType.Global && instance.ReadAsObject() != null && instance.ReadAsObject().ToString()
                                   .Equals(keyword, StringComparison.OrdinalIgnoreCase)))
                    {
                        return false;
                    }
                }
                return true;
            }
            catch { }
            return false;
        }

        public static bool XRefScanFor(this MethodBase methodBase, string searchTerm)
        {
            return XrefScanner.XrefScan(methodBase).Any(
                xref => xref.Type == XrefType.Global && xref.ReadAsObject()?.ToString().IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private static bool checkXrefNoStrings(MethodBase m)
        {
            try
            {
                foreach (XrefInstance instance in XrefScanner.XrefScan(m))
                {
                    if (instance.Type != XrefType.Global || instance.ReadAsObject() == null) continue;
                    return false;
                }
                return true;
            }
            catch (Exception e) { MelonLogger.Msg("For loop failed:" + e); }
            return false;

        }
        
        
        public static void QueueHudMessage(string msg)
        {
            VRCUiManager.prop_VRCUiManager_0.field_Private_List_1_String_0.Add(msg);
            VRCUiManager.prop_VRCUiManager_0.field_Private_List_1_String_0.Add("");
        }


        private delegate VRCUiPage PushUIPageDelegate(VRCUiPage page); 

        private delegate void ShowOptionsPopupDelegate(
            string title, 
            string body, 
            string leftButtonText, 
            Action leftButtonAction, 
            string rightButtonText, 
            Action rightButtonAction, 
            Action<VRCUiPopup> additionalSetup = null
        );

        private delegate void ShowDismissPopupDelegate(
            string title, 
            string body, 
            string middleButtonText, 
            Action middleButtonAction, 
            Action<VRCUiPopup> additionalSetup = null
        );
        

        private delegate void ClearErrorsDelegate();

        private delegate void AdvancedInvitesInviteDelegate(Notification notification);

        private delegate void DownloadWorldDelegate(ApiWorld world, OnDownloadProgress onProgress, OnDownloadComplete onSuccess, OnDownloadError onError, bool bypassDownloadSizeLimit, UnpackType unpackType);

    }
}
