//using AssetBundleDownload = CustomYieldInstructionPublicObAsByStInStCoBoObInUnique;

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;
using VRC.Core;
using WorldPredownload.Cache;
using WorldPredownload.Helpers;
using WorldPredownload.UI;
using OnDownloadProgress = AssetBundleDownloadManager.MulticastDelegateNInternalSealedVoUnUnique;

//using LoadErrorReason = EnumPublicSealedvaNoMiFiUnCoSeAsDuAsUnique;

namespace WorldPredownload.DownloadManager
{
    public static partial class WorldDownloadManager
    {
        public static DownloadInfo DownloadInfo;

        private static WebClient webClient;

        private static string file;
        public static bool downloading { get; set; }
        private static bool cancelled { get; set; }

        public static void CancelDownload()
        {
            if (downloading)
            {
                if (ModSettings.showHudMessages) Utilities.QueueHudMessage("Download Cancelled");
                cancelled = true;
                webClient.CancelAsync();
                webClient.Dispose();
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
                    GameObject.Find("UserInterface/QuickMenu/ShortcutMenu/WorldsButton").GetComponent<Button>().onClick
                        .Invoke();
                    Utilities.ShowPage(DownloadInfo.PageWorldInfo);

                    DownloadInfo.PageWorldInfo
                        .Method_Public_Void_ApiWorld_ApiWorldInstance_Boolean_Boolean_Boolean_APIUser_0(
                            DownloadInfo.ApiWorld, DownloadInfo.PageWorldInfo.field_Public_ApiWorldInstance_0);
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
                new Action(delegate
                {
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
                    GameObject.Find("UserInterface/QuickMenu/ShortcutMenu/SocialButton").GetComponent<Button>().onClick
                        .Invoke();
                    _ = DownloadInfo.APIUser ?? throw new NullReferenceException("Friend User Info Null Uh Oh");
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

        private static void DownloadWorld(ApiWorld apiWorld)
        {
            if (!downloading)
            {
                if (ModSettings.showStatusOnHud) HudIcon.Enable();
                if (ModSettings.showStatusOnQM) WorldDownloadStatus.Enable();
                if (ModSettings.showHudMessages) Utilities.QueueHudMessage("Starting Download");
                downloading = true;
                Download(apiWorld, progress, complete, null);
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

        [SuppressMessage("ReSharper", "HeuristicUnreachableCode")]
        public static void Download(ApiWorld apiWorld, DownloadProgressChangedEventHandler progress,
            AsyncCompletedEventHandler complete, CancelEventHandler cancel)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            webClient?.Dispose();
            webClient = new WebClient();
            webClient.Headers.Add("user-agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36");
            webClient.DownloadProgressChanged += progress;
            webClient.DownloadFileCompleted += complete;

            var cachePath = CacheManager.GetCache().path;
            var assetHash = CacheManager.ComputeAssetHash(apiWorld.id);
            var dir = Path.Combine(cachePath, assetHash);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            var assetVersionDir = Path.Combine(dir,
                "000000000000000000000000" + CacheManager.ComputeVersionString(apiWorld.version));
            if (!Directory.Exists(assetVersionDir)) Directory.CreateDirectory(assetVersionDir);

            var fileName = Path.Combine(assetVersionDir, "__data");
            MelonLogger.Msg($"Starting world download for: {apiWorld.name}");
            file = fileName;

            if (string.IsNullOrEmpty(apiWorld.assetUrl))
                MelonLogger.Error("World asset link missing! Did VRChat fail to load the world info quick enough?");
            webClient.DownloadFileAsync(new Uri(apiWorld.assetUrl), fileName);
        }
    }
}