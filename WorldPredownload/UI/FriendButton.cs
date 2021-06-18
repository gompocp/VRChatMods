using System;
using System.Collections;
using MelonLoader;
using UnityEngine;
using VRC.UI;
using WorldPredownload.Cache;
using WorldPredownload.DownloadManager;

namespace WorldPredownload.UI
{
    public class FriendButton
    {
        private const string PATH_TO_GAMEOBJECT_TO_CLONE =
            "UserInterface/MenuContent/Screens/UserInfo/OnlineFriendButtons/Invite";

        private const string PATH_TO_CLONE_PARENT = "UserInterface/MenuContent/Screens/UserInfo/OnlineFriendButtons";
        private const string GAMEOBJECT_NAME = "PredownloadUserButton";
        private const string BUTTON_DEFAULT_TEXT = "Predownload";

        private const string CLICK_ERROR_MESSAGE = "User may have clicked too quickly";

        private const string PATH_TO_USERINFO = "UserInterface/MenuContent/Screens/UserInfo";


        public static Action onClick = delegate
        {
            Utilities.DeselectClickedButton(button);
            try
            {
                if (WorldDownloadManager.downloading || button.GetTextComponentInChildren().text
                    .Equals(Constants.BUTTON_ALREADY_DOWNLOADED_TEXT))
                {
                    WorldDownloadManager.CancelDownload();
                    return;
                }

                WorldDownloadManager.ProcessDownload(
                    DownloadInfo.CreateUserPageDownloadInfo(GetUserInfo().field_Private_ApiWorld_0,
                        GetUserInfo().field_Public_APIUser_0.location.Split(':')[1],
                        DownloadType.Friend,
                        GetUserInfo()
                    ));
            }
            catch (Exception e)
            {
                MelonLogger.Warning(CLICK_ERROR_MESSAGE + $" {e}");
            }
        };

        public static bool canChangeText { get; set; } = true;
        public static bool initialised { get; set; }
        public static GameObject button { get; set; }

        public static void Setup()
        {
            button = Utilities.CloneGameObject(PATH_TO_GAMEOBJECT_TO_CLONE, PATH_TO_CLONE_PARENT);
            button.GetRectTrans().SetAnchoredPos(Constants.FRIEND_BUTTON_POS); //213f, 315f
            button.SetName(GAMEOBJECT_NAME);
            button.SetText(BUTTON_DEFAULT_TEXT);
            button.SetButtonActionInChildren(onClick);
            button.SetActive(true);
            initialised = true;
        }

        public static PageUserInfo GetUserInfo()
        {
            return GameObject.Find(PATH_TO_USERINFO).GetComponent<PageUserInfo>();
        }

        public static IEnumerator UpdateText()
        {
            while (GetUserInfo().field_Private_Boolean_0 != true) yield return null;
            button.SetActive(true);
            if (WorldDownloadManager.downloading)
            {
                if (GetUserInfo().field_Public_APIUser_0.id.Equals(WorldDownloadManager.DownloadInfo.APIUser.id))
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
                while (GetUserInfo().field_Private_ApiWorld_0 == null) yield return null;
                if (CacheManager.HasDownloadedWorld(GetUserInfo().field_Private_ApiWorld_0.id,
                    GetUserInfo().field_Private_ApiWorld_0.version))
                    button.SetText(Constants.BUTTON_ALREADY_DOWNLOADED_TEXT);
                else
                    button.SetText(Constants.BUTTON_IDLE_TEXT);
            }
        }

        public static void UpdateTextDownloadStopped()
        {
            var userInfo = GetUserInfo();
            if (userInfo == null || userInfo.field_Private_ApiWorld_0?.id == null) return;
            if (CacheManager.HasDownloadedWorld(userInfo.field_Private_ApiWorld_0.id,
                userInfo.field_Private_ApiWorld_0.version))
                button.SetText(Constants.BUTTON_ALREADY_DOWNLOADED_TEXT);
            else
                button.SetText(Constants.BUTTON_IDLE_TEXT);
            canChangeText = true;
        }
    }
}