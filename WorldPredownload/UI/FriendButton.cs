using System;
using System.Collections;
using MelonLoader;
using UnityEngine;
using VRC.UI;
using WorldPredownload.Cache;
using WorldPredownload.DownloadManager;
using WorldPredownload.Helpers;

namespace WorldPredownload.UI
{
    internal class FriendButton : Singleton<FriendButton>, IDownloadListener
    {
        private const string PATH_TO_GAMEOBJECT_TO_CLONE =
            "UserInterface/MenuContent/Screens/UserInfo/OnlineFriendButtons/Invite";

        private const string PATH_TO_CLONE_PARENT = "UserInterface/MenuContent/Screens/UserInfo/OnlineFriendButtons";
        private const string GAMEOBJECT_NAME = "PredownloadUserButton";
        private const string BUTTON_DEFAULT_TEXT = "Predownload";

        private const string CLICK_ERROR_MESSAGE = "User may have clicked too quickly";


        public static Action ONClick = delegate
        {
            Utilities.DeselectClickedButton(Button);
            try
            {
                if (DownloadManager.Downloader.Downloading || Button.GetTextComponentInChildren().text
                    .Equals(Constants.BUTTON_ALREADY_DOWNLOADED_TEXT))
                {
                    DownloadManager.Downloader.CancelDownload();
                    return;
                }

                DownloadManager.Downloader.ProcessDownload(
                    DownloadInfo.CreateUserPageDownloadInfo(Utilities.GetUserInfo().field_Private_ApiWorld_0, Utilities.GetUserInfo().field_Public_APIUser_0.location.Split(':')[1],
                        DownloadType.Friend, Utilities.GetUserInfo()
                    ));
            }
            catch (Exception e)
            {
                MelonLogger.Warning(CLICK_ERROR_MESSAGE + $" {e}");
            }
        };

        public FriendButton()
        {
            Button = Utilities.CloneGameObject(PATH_TO_GAMEOBJECT_TO_CLONE, PATH_TO_CLONE_PARENT);
            Button.GetRectTrans().SetAnchoredPos(Constants.FriendButtonPos); //213f, 315f
            Button.SetName(GAMEOBJECT_NAME);
            Button.SetText(BUTTON_DEFAULT_TEXT);
            Button.SetButtonActionInChildren(ONClick);
            Button.SetActive(true);
        }

        private static bool CanChangeText { get; set; } = true;
        public static GameObject Button { get; set; }

        public void Update(DownloadManager.Downloader downloader)
        {
            switch (downloader.DownloadState)
            {
                case DownloadState.Idle:
                    var userInfo = Utilities.GetUserInfo();
                    if (userInfo == null || userInfo.field_Private_ApiWorld_0?.id == null) return;
                    if (CacheManager.HasDownloadedWorld(userInfo.field_Private_ApiWorld_0.id,
                        userInfo.field_Private_ApiWorld_0.version))
                        Button.SetText(Constants.BUTTON_ALREADY_DOWNLOADED_TEXT);
                    else
                        Button.SetText(Constants.BUTTON_IDLE_TEXT);
                    return;
                case DownloadState.Downloading:
                    if (CanChangeText)
                        Button.SetText(downloader.TextStatus);
                    return;
                case DownloadState.RefreshUI:
                    if (Button.active)
                        MelonCoroutines.Start(UpdateText());
                    return;
            }
        }

        private static IEnumerator UpdateText()
        {
            while (Utilities.GetUserInfo().field_Private_Boolean_0 != true) yield return null;
            Button.SetActive(true);
            if (DownloadManager.Downloader.Downloading)
            {
                if (Utilities.GetUserInfo().field_Public_APIUser_0.id.Equals(DownloadManager.Downloader.DownloadInfo.APIUser.id))
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
                while (Utilities.GetUserInfo().field_Private_ApiWorld_0 == null) yield return null;
                if (CacheManager.HasDownloadedWorld(Utilities.GetUserInfo().field_Private_ApiWorld_0.id, Utilities.GetUserInfo().field_Private_ApiWorld_0.version))
                    Button.SetText(Constants.BUTTON_ALREADY_DOWNLOADED_TEXT);
                else
                    Button.SetText(Constants.BUTTON_IDLE_TEXT);
            }
        }
    }
}