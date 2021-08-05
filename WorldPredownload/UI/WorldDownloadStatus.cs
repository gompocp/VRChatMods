using UnityEngine;
using UnityEngine.UI;
using VRC.UI;
using WorldPredownload.Helpers;

namespace WorldPredownload.UI
{
    public static class WorldDownloadStatus
    {
        private const string PATH_TO_GAMEOBJECT_TO_CLONE =
            "UserInterface/QuickMenu/QuickMenu_NewElements/_InfoBar/PingText";

        private const string PATH_TO_CLONE_PARENT = "UserInterface/QuickMenu/QuickMenu_NewElements/_InfoBar";
        public static GameObject GameObject { get; set; }

        public static void Setup()
        {
            GameObject = Utilities.CloneGameObject(PATH_TO_GAMEOBJECT_TO_CLONE, PATH_TO_CLONE_PARENT);
            GameObject.GetRectTrans().SetAnchoredPos(Constants.DwldStatusPos);
            if (ModSettings.showStatusOnQM && !ModSettings.hideQMStatusWhenInActive) GameObject.SetActive(true);
            else GameObject.SetActive(false);
            GameObject.SetName(Constants.STATUS_NAME);
            GameObject.GetComponent<DebugDisplayText>().enabled = false;
            GameObject.GetComponent<Text>().alignment = TextAnchor.UpperRight;
            GameObject.SetText(Constants.STATUS_IDLE_TEXT);
        }

        public static void Enable()
        {
            if (GameObject != null)
            {
                GameObject.SetActive(true);
                GameObject.SetText(Constants.STATUS_IDLE_TEXT);
            }
        }

        public static void Disable()
        {
            if (GameObject != null)
                GameObject.SetActive(false);
        }
    }
}