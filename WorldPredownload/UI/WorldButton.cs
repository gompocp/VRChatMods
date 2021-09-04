using System;
using MelonLoader;
using UnityEngine;
using VRC.UI;
using WorldPredownload.Cache;
using WorldPredownload.DownloadManager;
using WorldPredownload.Helpers;

namespace WorldPredownload.UI
{
    internal class WorldButton : Singleton<WorldButton>, IDownloadListener, IResettable
    {
        private const string PATH_TO_GAMEOBJECT_TO_CLONE =
            "UserInterface/MenuContent/Screens/WorldInfo/WorldButtons/GoButton";

        private const string PATH_TO_CLONE_PARENT = "UserInterface/MenuContent/Screens/WorldInfo/WorldButtons";
        private const string PATH_TO_WORLDINFO = "UserInterface/MenuContent/Screens/WorldInfo";

        private static readonly Action ONClick = delegate
        {
            Utilities.DeselectClickedButton(Button);
            try
            {
                Utilities.DeselectClickedButton(Button);
                if (DownloadManager.Downloader.Downloading ||
                    Button.GetTextComponentInChildren().text.Equals(Constants.BUTTON_ALREADY_DOWNLOADED_TEXT))
                {
                    DownloadManager.Downloader.CancelDownload();
                    return;
                }

                DownloadManager.Downloader.ProcessDownload(
                    DownloadInfo.CreateWorldPageDownloadInfo(
                        GetWorldInfo().field_Private_ApiWorld_0,
                        GetWorldInfo().field_Public_ApiWorldInstance_0.instanceId,
                        DownloadType.World,
                        GetWorldInfo()
                    )
                );
            }
            catch (Exception e)
            {
                MelonLogger.Error($"Exception Occured In Setup For World Download: {e}");
            }
        };

        public WorldButton()
        {
            Button = Utilities.CloneGameObject(PATH_TO_GAMEOBJECT_TO_CLONE, PATH_TO_CLONE_PARENT);
            Button.GetRectTrans().SetAnchoredPos(Constants.WorldButtonPos);
            Button.SetName(Constants.WORLD_BUTTON_NAME);
            Button.SetText(Constants.BUTTON_IDLE_TEXT);
            Button.SetButtonAction(ONClick);
            Button.SetActive(true);
        }

        private static bool CanChangeText { get; set; } = true;

        public static GameObject Button { get; set; }

        public void Update(DownloadManager.Downloader downloader)
        {
            switch (downloader.DownloadState)
            {
                case DownloadState.Idle:
                    var worldInfo = GetWorldInfo();
                    if (worldInfo == null || worldInfo.field_Private_ApiWorld_0?.id == null) return;
                    if (CacheManager.HasDownloadedWorld(worldInfo.field_Private_ApiWorld_0.assetUrl))
                        Button.SetText(Constants.BUTTON_ALREADY_DOWNLOADED_TEXT);
                    else
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

        private void UpdateText()
        {
            var world = GetWorldInfo().field_Private_ApiWorld_0;
            if (world == null)
                return;
            if (DownloadManager.Downloader.Downloading)
            {
                if (world.id.Equals(DownloadManager.Downloader.DownloadInfo.ApiWorld.id))
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
                if (CacheManager.HasDownloadedWorld(world.assetUrl))
                    Button.SetText(Constants.BUTTON_ALREADY_DOWNLOADED_TEXT);
                else
                    Button.SetText(Constants.BUTTON_IDLE_TEXT);
            }
        }

        public static void UpdateTextDownloadStopped()
        {
            var worldInfo = GetWorldInfo();
            if (worldInfo == null || worldInfo.field_Private_ApiWorld_0?.id == null) return;
            if (CacheManager.HasDownloadedWorld(worldInfo.field_Private_ApiWorld_0.assetUrl))
                Button.SetText(Constants.BUTTON_ALREADY_DOWNLOADED_TEXT);
            else
                Button.SetText(Constants.BUTTON_IDLE_TEXT);
            CanChangeText = true;
        }

        private static PageWorldInfo GetWorldInfo()
        {
            return GameObject.Find(PATH_TO_WORLDINFO).GetComponent<PageWorldInfo>();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}