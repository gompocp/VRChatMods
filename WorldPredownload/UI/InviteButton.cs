using System;
using System.Collections;
using MelonLoader;
using UnityEngine;
using UnityEngine.Playables;
using VRC.Core;
using WorldPredownload.Cache;
using WorldPredownload.DownloadManager;
using WorldPredownload.Helpers;

namespace WorldPredownload.UI
{
    internal class InviteButton : Singleton<InviteButton>, IDownloadListener, IResettable
    {
        private const string PATH_TO_GAMEOBJECT_TO_CLONE =
            "UserInterface/QuickMenu/QuickModeMenus/QuickModeInviteResponseMoreOptionsMenu/BlockButton";

        private const string PATH_TO_CLONE_PARENT =
            "UserInterface/QuickMenu/QuickModeMenus/QuickModeInviteResponseMoreOptionsMenu";

        private const string UNABLE_TO_CONVERT_WORLDID = "Error Creating ApiWorld From Notification";
        private static bool canDownload = true;

        public static Action ONClick = delegate
        {
            Utilities.DeselectClickedButton(Button);
            if (DownloadManager.Downloader.Downloading || Button.GetTextComponentInChildren().text
                .Equals(Constants.BUTTON_ALREADY_DOWNLOADED_TEXT))
            {
                DownloadManager.Downloader.CancelDownload();
                return;
            }

            if (!canDownload)
            {
                Utilities.QueueHudMessage("Please wait a while before trying to download again");
                return;
            }
            //MelonCoroutines.Start(InviteButtonTimer(15));

            //Credit: https://github.com/Psychloor/AdvancedInvites/blob/master/AdvancedInvites/InviteHandler.cs
            API.Fetch<ApiWorld>(Utilities.GetSelectedNotification().GetWorldID(),
                new Action<ApiContainer>(
                    container =>
                    {
                        DownloadManager.Downloader.ProcessDownload(
                            DownloadInfo.CreateInviteDownloadInfo(
                                container.Model.Cast<ApiWorld>(),
                                Utilities.GetSelectedNotification().GetInstanceIDWithTags(),
                                DownloadType.Invite,
                                Utilities.GetSelectedNotification()
                            )
                        );
                    }),
                new Action<ApiContainer>(delegate { MelonLogger.Msg(UNABLE_TO_CONVERT_WORLDID); }));
        };


        public InviteButton()
        {
            Button = Utilities.CloneGameObject(PATH_TO_GAMEOBJECT_TO_CLONE, PATH_TO_CLONE_PARENT);
            Button.GetRectTrans().SetAnchoredPos(Constants.InviteButtonPos);
            Button.SetName(Constants.INVITE_BUTTON_NAME);
            Button.SetText(Constants.BUTTON_IDLE_TEXT);
            Button.SetButtonAction(ONClick);
            Button.SetActive(true);
            Button.GetComponent<UiTooltip>().field_Public_String_0 = Constants.INVITE_BUTTON_TOOLTIP;
        }

        public static bool CanChangeText { get; set; } = true;
        public static GameObject Button { get; set; }

        public void Update(DownloadManager.Downloader downloader)
        {
            switch (downloader.DownloadState)
            {
                case DownloadState.Idle:
                    Button.SetText(Constants.BUTTON_IDLE_TEXT);
                    CanChangeText = true;
                    return;
                case DownloadState.Downloading:
                    if (CanChangeText)
                        Button.SetText(downloader.TextStatus);
                    return;
                case DownloadState.RefreshUI:
                    if (Button.active)
                        UpdateText();
                    return;
            }
        }

        public void Reset()
        {
            if (Button != null)
                Button.Destroy();
        }


        private static void UpdateText()
        {
            if (Utilities.GetSelectedNotification().notificationType.Equals("invite"))
            {
                Button.SetActive(true);
                if (DownloadManager.Downloader.Downloading)
                {
                    if (Utilities.GetSelectedNotification().GetWorldID()
                        .Equals(DownloadManager.Downloader.DownloadInfo.ApiWorld.id))
                    {
                        CanChangeText = true;
                    }
                    else
                    {
                        CanChangeText = false;
                        Button.SetText(Constants.BUTTON_BUSY_TEXT);
                    }
                }
                else
                {
                    if (CacheManager.HasDownloadedWorld(Utilities.GetSelectedNotification().GetWorldID()))
                        Button.SetText(Constants.BUTTON_ALREADY_DOWNLOADED_TEXT);
                    else Button.SetText(Constants.BUTTON_IDLE_TEXT);
                }
            }
            else
            {
                Button.SetActive(false);
            }
        }

        public static IEnumerator InviteButtonTimer(int time)
        {
            canDownload = false;
            for (var i = time; i >= 0; i--)
            {
                if (!DownloadManager.Downloader.Downloading)
                    Button.SetText($"Time Left:{i}");
                yield return new WaitForSeconds(1);
            }

            canDownload = true;
            Singleton<DownloadManager.Downloader>.Instance.DownloadState = DownloadState.RefreshUI;
        }
    }
}