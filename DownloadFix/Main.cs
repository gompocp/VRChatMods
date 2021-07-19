using System.IO;
using Il2CppSystem;
using MelonLoader;
using UIExpansionKit.API;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Rendering;
using Action = System.Action;

/*
 *  As of 2020/06/17 this mod is now pointless as the bug it aimed at helping with has been patched,
 *  so I don't really know why you are looking here lol
 */



[assembly: MelonInfo(typeof(DownloadFix.DownloadFix), "DownloadFix", "1.0.3", "gompo", "https://github.com/gompocp/VRChatMods/releases/")]
[assembly: MelonGame("VRChat", "VRChat")]

namespace DownloadFix
{
    public partial class DownloadFix : MelonMod
    {
        
        public override void OnApplicationStart()
        {
            ICustomLayoutedMenu settingsPage = ExpansionKitApi.GetExpandedMenu(ExpandedMenu.SettingsMenu);
            settingsPage.AddSimpleButton("Unblock unpack queue", Utilities.UnblockUnPackQueue);
            ExpansionKitApi.OnUiManagerInit += OnUiManagerInit;
            HarmonyInstance.PatchAll();
        }

        public void OnUiManagerInit()
        {
            
            GameObject unblockButton = GameObject.Instantiate(
                    GameObject.Find("UserInterface/MenuContent/Popups/LoadingPopup/ProgressPanel/Parent_Loading_Progress/GoButton").transform,
                    GameObject.Find("UserInterface/MenuContent/Popups/LoadingPopup/ProgressPanel").transform)
                .gameObject;
            unblockButton.GetComponentInChildren<Text>().text = "Unblock Queue";
            unblockButton.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            unblockButton.GetComponent<Button>().onClick.AddListener(new Action(
                delegate 
                {
                    Utilities.UnblockUnPackQueue();
                    Utilities.DeselectClickedButton(unblockButton);
                }));
            unblockButton.GetComponent<Transform>().localPosition = new Vector3(-2.4f, -124f, 0);
            GameObject.Find("UserInterface/MenuContent/Popups/LoadingPopup/ProgressPanel/Parent_Loading_Progress").transform.localPosition = new Vector3(0, 17, 0);

        }
    }
}
