//using AssetBundleDownload = CustomYieldInstructionPublicObAsByStInStCoBoObInUnique;
using OnDownloadComplete = AssetBundleDownloadManager.MulticastDelegateNInternalSealedVoObUnique;
using OnDownloadProgress = AssetBundleDownloadManager.MulticastDelegateNInternalSealedVoUnUnique;
using OnDownloadError = AssetBundleDownloadManager.MulticastDelegateNInternalSealedVoStObStUnique;
//using LoadErrorReason = EnumPublicSealedvaNoMiFiUnCoSeAsDuAsUnique;
using UnpackType = AssetBundleDownloadManager.EnumNInternalSealedva3vUnique;
using WorldPredownload.UI;
using System;
using Harmony;
using VRC.Core;
using UnityEngine;
using UnityEngine.UI;
using MelonLoader;

namespace WorldPredownload.DownloadManager
{
    public static partial class WorldDownloadManager
    {
        public static bool downloading { get; set; } = false;
        public static bool cancelled { get; set; }= false;
        public static DownloadInfo DownloadInfo;
        
        public static void CancelDownload() {
            if (downloading)
            {
                if(ModSettings.showHudMessages) Utilities.QueueHudMessage("Download Cancelled");
                cancelled = true;
            }
        }
        
        public static void ClearDownload()
        {
            //DownloadInfo = null; Ignore this lel
        }

        public static void DisplayWorldPopup()
        {
            if (GameObject.Find("UserInterface/MenuContent/Screens/WorldInfo").active)
            {
                ClearDownload();
                return;
            }
            Utilities.ShowOptionPopup(
                Constants.DOWNLOAD_SUCCESS_TITLE,
                Constants.DOWNLOAD_SUCCESS_MSG,
                Constants.DOWNLOAD_SUCCESS_LEFT_BTN_TEXT,
                new Action(delegate
                {
                    Utilities.HideCurrentPopup();
                    GameObject.Find("UserInterface/QuickMenu/ShortcutMenu/WorldsButton").GetComponent<Button>().onClick.Invoke();
                    Utilities.ShowPage(DownloadInfo.PageWorldInfo);
                    DownloadInfo.PageWorldInfo.Method_Public_Void_ApiWorld_ApiWorldInstance_Boolean_Boolean_0(DownloadInfo.ApiWorld, DownloadInfo.PageWorldInfo.field_Public_ApiWorldInstance_0);
                    ClearDownload();
                }),
                Constants.DOWNLOAD_SUCCESS_RIGHT_BTN_TEXT,
                new Action(delegate
                {
                    Utilities.HideCurrentPopup();
                    ClearDownload();
                })
            );
        }

        public static void DisplayInvitePopup()
        {
            Utilities.ShowDismissPopup(
                Constants.DOWNLOAD_SUCCESS_TITLE,
                Constants.DOWNLOAD_SUCCESS_MSG, 
                Constants.DOWNLOAD_SUCCESS_RIGHT_BTN_TEXT, 
                new Action(delegate {
                    Utilities.HideCurrentPopup();
                    ClearDownload();
                })
            );
        }

        public static void DisplayFriendPopup()
        {
            if (GameObject.Find("UserInterface/MenuContent/Screens/UserInfo").active)
            {
                ClearDownload();
                return;
            }
            Utilities.ShowOptionPopup(
                Constants.DOWNLOAD_SUCCESS_TITLE,
                Constants.DOWNLOAD_SUCCESS_MSG,
                Constants.DOWNLOAD_SUCCESS_LEFT_BTN_TEXT_F,
                new Action(delegate
                {
                    Utilities.HideCurrentPopup();
                    GameObject.Find("UserInterface/QuickMenu/ShortcutMenu/SocialButton").GetComponent<Button>().onClick.Invoke();
                    _ = DownloadInfo.APIUser ?? throw new NullReferenceException(message: "Friend User Info Null Uh Oh");
                    Utilities.ShowPage(DownloadInfo.PageUserInfo);
                    DownloadInfo.PageUserInfo.LoadUser(DownloadInfo.APIUser);
                    //FriendButton.UpdateTextDownloadStopped();
                    ClearDownload();
                }),
                Constants.DOWNLOAD_SUCCESS_RIGHT_BTN_TEXT,
                new Action(delegate
                {
                    Utilities.HideCurrentPopup();
                    ClearDownload();
                })
            );
        }

        public static void DownloadWorld(ApiWorld apiWorld)
        {
            
            if (!downloading)
            {
                if(ModSettings.showStatusOnHud) HudIcon.Enable();
                if(ModSettings.showStatusOnQM) WorldDownloadStatus.Enable();
                if(ModSettings.showHudMessages) Utilities.QueueHudMessage("Starting Download");
                downloading = true;
                Utilities.DownloadApiWorld(
                    apiWorld,
                    onProgressDel,
                    onCompleteDel,
                    onErrorDel,
                    true,
                    UnpackType.EnumValue1
                );
            }
            else
            {
                cancelled = true;
                InviteButton.button.SetText(Constants.BUTTON_IDLE_TEXT);
                WorldButton.button.SetText(Constants.BUTTON_IDLE_TEXT);
                FriendButton.button.SetText(Constants.BUTTON_IDLE_TEXT);
            }
        }

        public static void ProcessDownload(DownloadInfo downloadInfo)
        {
            DownloadInfo = downloadInfo;
            if (downloadInfo.DownloadType == DownloadType.Invite && !downloading)
                MelonCoroutines.Start(InviteButton.InviteButtonTimer(15));
            DownloadWorld(downloadInfo.ApiWorld);
        }
    }
}
