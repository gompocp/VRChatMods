using UnityEngine;
using UnityEngine.UI;
using VRC.UI;
using WorldPredownload.DownloadManager;
using WorldPredownload.Helpers;

namespace WorldPredownload.UI
{
    internal class WorldDownloadStatus : Singleton<WorldDownloadStatus>, IDownloadListener
    {
        private const string PATH_TO_GAMEOBJECT_TO_CLONE =
            "UserInterface/QuickMenu/QuickMenu_NewElements/_InfoBar/PingText";

        private const string PATH_TO_CLONE_PARENT = "UserInterface/QuickMenu/QuickMenu_NewElements/_InfoBar";
        private readonly GameObject _gameObject;

        public WorldDownloadStatus()
        {
            _gameObject = Utilities.CloneGameObject(PATH_TO_GAMEOBJECT_TO_CLONE, PATH_TO_CLONE_PARENT);
            _gameObject.GetRectTrans().SetAnchoredPos(Constants.DwldStatusPos);
            if (ModSettings.showStatusOnQM && !ModSettings.hideQMStatusWhenInActive) _gameObject.SetActive(true);
            else _gameObject.SetActive(false);
            _gameObject.SetName(Constants.STATUS_NAME);
            _gameObject.GetComponent<DebugDisplayText>().enabled = false;
            _gameObject.GetComponent<Text>().alignment = TextAnchor.UpperRight;
            _gameObject.SetText(Constants.STATUS_IDLE_TEXT);
        }

        public void Update(DownloadManager.Downloader downloader)
        {
            switch (downloader.DownloadState)
            {
                case DownloadState.Idle:
                    if (ModSettings.hideQMStatusWhenInActive)
                        Disable();
                    _gameObject.SetText(Constants.STATUS_IDLE_TEXT);
                    return;
                case DownloadState.Downloading:
                    if (ModSettings.showStatusOnQM)
                        _gameObject.SetText(downloader.TextStatus);
                    return;
                case DownloadState.FinishedRecompressing:
                    Disable();
                    return;
                case DownloadState.StartingDownload:
                    if (ModSettings.showStatusOnQM)
                        Enable();
                    return;
            }
        }

        public static void Enable()
        {
            if (Instance._gameObject != null)
            {
                Instance._gameObject.SetActive(true);
                Instance._gameObject.SetText(Constants.STATUS_IDLE_TEXT);
            }
        }

        public static void Disable()
        {
            if (Instance._gameObject != null)
                Instance._gameObject.SetActive(false);
        }
    }
}