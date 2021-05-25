using UnityEngine;
using UnityEngine.UI;
using VRC.UI;

namespace WorldPredownload.UI
{
    public static class WorldDownloadStatus
    {
        private const string PATH_TO_GAMEOBJECT_TO_CLONE =
            "UserInterface/QuickMenu/QuickMenu_NewElements/_InfoBar/PingText";

        private const string PATH_TO_CLONE_PARENT = "UserInterface/QuickMenu/QuickMenu_NewElements/_InfoBar";
        public static GameObject gameObject { get; set; }

        public static void Setup()
        {
            gameObject = Utilities.CloneGameObject(PATH_TO_GAMEOBJECT_TO_CLONE, PATH_TO_CLONE_PARENT);
            gameObject.GetRectTrans().SetAnchoredPos(Constants.DWLD_STATUS_POS);
            if (ModSettings.showStatusOnQM && !ModSettings.hideQMStatusWhenInActive) gameObject.SetActive(true);
            else gameObject.SetActive(false);
            gameObject.SetName(Constants.DOWNLOAD_STATUS_NAME);
            gameObject.GetComponent<DebugDisplayText>().enabled = false;
            gameObject.GetComponent<Text>().alignment = TextAnchor.UpperRight;
            gameObject.SetText(Constants.DOWNLOAD_STATUS_IDLE_TEXT);
        }

        public static void Enable()
        {
            if (gameObject != null)
            {
                gameObject.SetActive(true);
                gameObject.SetText(Constants.DOWNLOAD_STATUS_IDLE_TEXT);
            }
        }

        public static void Disable()
        {
            if (gameObject != null)
                gameObject.SetActive(false);
        }
    }
}