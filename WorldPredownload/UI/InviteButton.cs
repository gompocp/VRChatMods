using System;
using System.Collections;
using MelonLoader;
using UnityEngine;
using VRC.Core;
using WorldPredownload.Cache;
using WorldPredownload.DownloadManager;
using WorldPredownload.Helpers;

namespace WorldPredownload.UI
{
    public class InviteButton
    {
        private const string PATH_TO_GAMEOBJECT_TO_CLONE =
            "UserInterface/QuickMenu/QuickModeMenus/QuickModeInviteResponseMoreOptionsMenu/BlockButton";

        private const string PATH_TO_CLONE_PARENT =
            "UserInterface/QuickMenu/QuickModeMenus/QuickModeInviteResponseMoreOptionsMenu";

        private const string UNABLE_TO_CONVERT_WORLDID = "Error Creating ApiWorld From Notification";
        private static bool canDownload = true;

        public static Action onClick = delegate
        {
            Utilities.DeselectClickedButton(button);
            if (WorldDownloadManager.Downloading || button.GetTextComponentInChildren().text
                .Equals(Constants.BUTTON_ALREADY_DOWNLOADED_TEXT))
            {
                WorldDownloadManager.CancelDownload();
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
                        WorldDownloadManager.ProcessDownload(
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

        public static bool canChangeText { get; set; } = true;
        public static GameObject button { get; set; }
        public static bool initialised { get; set; }

        public static void Setup()
        {
            button = Utilities.CloneGameObject(PATH_TO_GAMEOBJECT_TO_CLONE, PATH_TO_CLONE_PARENT);
            button.GetRectTrans().SetAnchoredPos(Constants.INVITE_BUTTON_POS);
            button.SetName(Constants.INVITE_BUTTON_NAME);
            button.SetText(Constants.BUTTON_IDLE_TEXT);
            button.SetButtonAction(onClick);
            button.SetActive(true);
            button.GetComponent<UiTooltip>().field_Public_String_0 = Constants.INVITE_BUTTON_TOOLTIP;
            initialised = true;
        }


        public static void UpdateTextDownloadStopped()
        {
            button.SetText(Constants.BUTTON_IDLE_TEXT);
            canChangeText = true;
        }

        public static void UpdateText()
        {
            if (Utilities.GetSelectedNotification().notificationType.Equals("invite"))
            {
                button.SetActive(true);
                if (WorldDownloadManager.Downloading)
                {
                    if (Utilities.GetSelectedNotification().GetWorldID()
                        .Equals(WorldDownloadManager.DownloadInfo.ApiWorld.id))
                    {
                        canChangeText = true;
                    }
                    else
                    {
                        canChangeText = false;
                        button.SetText(Constants.BUTTON_BUSY_TEXT);
                    }
                }
                else
                {
                    if (CacheManager.HasDownloadedWorld(Utilities.GetSelectedNotification().GetWorldID()))
                        button.SetText(Constants.BUTTON_ALREADY_DOWNLOADED_TEXT);
                    else button.SetText(Constants.BUTTON_IDLE_TEXT);
                }
            }
            else
            {
                button.SetActive(false);
            }
        }

        public static IEnumerator InviteButtonTimer(int time)
        {
            canDownload = false;
            for (var i = time; i >= 0; i--)
            {
                if (!WorldDownloadManager.Downloading)
                    button.SetText($"Time Left:{i}");
                yield return new WaitForSeconds(1);
            }

            canDownload = true;
            UpdateText();
        }
    }
}