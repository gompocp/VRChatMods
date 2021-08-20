using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Il2CppSystem;
using MelonLoader;
using Transmtn.DTO.Notifications;
using UnityEngine;
using UnityEngine.EventSystems;
using VRC.Core;
using VRC.SDKBase;
using WorldPredownload.DownloadManager;
using Exception = System.Exception;
using Object = UnityEngine.Object;

namespace WorldPredownload.Helpers
{
    [SuppressMessage("ReSharper", "HeuristicUnreachableCode")]
    public static class Utilities
    {
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
            Delegates.GetAdvancedInvitesInviteDelegate(notification);
#endif
        }

        public static void ShowOptionPopup(string title, string body, string leftButtonText, Action leftButtonAction,
            string rightButtonText, Action rightButtonAction)
        {
            Delegates.GetShowOptionsPopupDelegate(title, body, leftButtonText, leftButtonAction, rightButtonText,
                rightButtonAction);
        }

        public static void ShowDismissPopup(string title, string body, string middleButtonText, Action buttonAction)
        {
            Delegates.GetShowDismissPopupDelegate(title, body, middleButtonText, buttonAction);
        }


        public static void ShowPage(VRCUiPage page)
        {
            Delegates.GetPushUIPageDelegate(page);
        }

        public static AssetBundleDownloadManager GetAssetBundleDownloadManager()
        {
            return AssetBundleDownloadManager.prop_AssetBundleDownloadManager_0;
        }

        public static void HideCurrentPopup()
        {
            VRCUiManager.prop_VRCUiManager_0.HideScreen("POPUP");
        }


        public static Notification GetSelectedNotification()
        {
            return NotificationMoreActions.SelectedNotification;
        }

        public static GameObject CloneGameObject(string pathToGameObject, string pathToParent)
        {
            return Object
                .Instantiate(GameObject.Find(pathToGameObject).transform, GameObject.Find(pathToParent).transform)
                .gameObject;
        }

        public static void DeselectClickedButton(GameObject button)
        {
            if (EventSystem.current.currentSelectedGameObject == button)
                EventSystem.current.SetSelectedGameObject(null);
        }

        public static void GoToWorld(ApiWorld apiWorld, string tags, bool isInvite)
        {
            if (isInvite)
            {
                if (ModSettings.tryUseAdvancedInvitePopup && ModSettings.AdvancedInvites)
                    try
                    {
                        Delegates.GetAdvancedInvitesInviteDelegate(WorldDownloadManager.DownloadInfo.Notification);
                    }
                    catch (Exception e)
                    {
                        MelonLogger.Error("Unable to execute Advanced Invite's Invite Handler Func" + e);
                    }
                else
                    Networking.GoToRoom($"{apiWorld.id}:{tags}");

                //new PortalInternal().Method_Private_Void_String_String_PDM_0(apiWorld.id, tags);
            }
            else
            {
                Networking.GoToRoom($"{apiWorld.id}:{tags}");
            }
        }

        public static bool IsInSameWorld(APIUser user)
        {
            if (user.location.Contains(RoomManager.field_Internal_Static_ApiWorld_0.id))
                return true;
            return false;
        }

        public static string ByteArrayToString(byte[] ba)
        {
            var hex = new StringBuilder(ba.Length * 2);
            foreach (var b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static bool HasMod(string modName)
        {
            return MelonHandler.Mods.Any(mod => mod.Info.Name.Equals(modName));
        }


        public static void QueueHudMessage(string msg)
        {
            VRCUiManager.prop_VRCUiManager_0.field_Private_List_1_String_0.Add(msg);
            VRCUiManager.prop_VRCUiManager_0.field_Private_List_1_String_0.Add("");
        }

        public static string ExtractFileId(string txt)
        {
            return fileIdRegex.Match(txt).Groups[1].Value;
        }
        
        public static string ExtractFileVersion(string txt)
        {
            return fileVersionRegex.Match(txt).Groups[1].Value;
        }

        private static Regex fileIdRegex = new ("/file_[0-9A-Za-z-]+/", RegexOptions.Compiled);

        private static Regex fileVersionRegex = new Regex("/(?:\\/file_[0-9A-Za-z-]+\\/)([0-9]+)/gi", RegexOptions.Compiled);
        
    }
}