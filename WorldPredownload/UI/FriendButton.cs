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
        public static bool canChangeText { get; set; } = true;
        public static bool initialised { get; set; } = false;
        public static GameObject button { get; set; }

        private const string PATH_TO_GAMEOBJECT_TO_CLONE = "UserInterface/MenuContent/Screens/UserInfo/User Panel/Playlists";
        private const string PATH_TO_CLONE_PARENT = "UserInterface/MenuContent/Screens/UserInfo/User Panel";
        private const string GAMEOBJECT_NAME = "PreloadUserButton";
        private const string BUTTON_DEFAULT_TEXT = "Predownload";
        private const string PATH_TO_GAMEOBJECT_TO_DESTROY = "UserInterface/MenuContent/Screens/UserInfo/User Panel/PreloadWorld/PlaylistsButton/Image/Icon_New";
        private const string PATH_TO_USERINFO = "UserInterface/MenuContent/Screens/UserInfo";
        private const string PATH_TO_BACKGROUND = "UserInterface/MenuContent/Screens/UserInfo/User Panel/Panel";
        private const string PATH_TO_INFO_PANEL = "UserInterface/MenuContent/Screens/UserInfo/User Panel";
        private const string CLICK_ERROR_MESSAGE = "User may have clicked too quickly";
        public static void Setup()
        {
            button = Utilities.CloneGameObject(PATH_TO_GAMEOBJECT_TO_CLONE, PATH_TO_CLONE_PARENT);
            button.GetRectTrans().SetAnchoredPos(Constants.FRIEND_BUTTON_POS);  //213f, 315f
            button.SetName(GAMEOBJECT_NAME);
            button.SetText(BUTTON_DEFAULT_TEXT);
            button.SetButtonActionInChildren(onClick);
            GameObject.Destroy(GameObject.Find(PATH_TO_GAMEOBJECT_TO_DESTROY));

            Transform background = GameObject.Find(PATH_TO_BACKGROUND).transform;
            background.localScale = new Vector3(background.localScale.x, Constants.FRIEND_PANEL_YSCALE, background.localScale.z);
            background.localPosition = new Vector3(background.localPosition.x, Constants.FRIEND_PANEL_YPOS, background.localPosition.z);

            Transform userInfoPanel = GameObject.Find(PATH_TO_INFO_PANEL).transform;
            userInfoPanel.localPosition = new Vector3(userInfoPanel.localPosition.x, Constants.SOCIAL_PANEL_YPOS, userInfoPanel.localPosition.z);
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
                {
                    button.SetText(Constants.BUTTON_ALREADY_DOWNLOADED_TEXT);
                }
                else
                {
                    button.SetText(Constants.BUTTON_IDLE_TEXT);
                }
                
            }
        }
        
        public static void UpdateTextDownloadStopped()
        {
            //Lazy way to check if the user menu is up
            try
            {
                if (CacheManager.HasDownloadedWorld(GetUserInfo().field_Private_ApiWorld_0.id, GetUserInfo().field_Private_ApiWorld_0.version))
                    button.SetText(Constants.BUTTON_ALREADY_DOWNLOADED_TEXT);
                else
                    button.SetText(Constants.BUTTON_IDLE_TEXT);
            }
            catch
            {
                // if it isn't up who the fuck cares what it says
            }
            canChangeText = true;
        }
        
        
        public static Action onClick = delegate
        {
            Utilities.DeselectClickedButton(button);
            try
            {
                if (WorldDownloadManager.downloading || button.GetTextComponentInChildren().text.Equals(Constants.BUTTON_ALREADY_DOWNLOADED_TEXT))
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
            catch
            {
                MelonLogger.Warning(CLICK_ERROR_MESSAGE);
            }
        };

        
    }
}