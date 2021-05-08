﻿using System;
using System.Collections.Generic;
using UIExpansionKit.API;
using MelonLoader;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[assembly: MelonInfo(typeof(DownloadFix.DownloadFix), "DownloadFix", "1.0.2", "gompo", "https://github.com/gompocp/VRChatMods/releases/")]
[assembly: MelonGame("VRChat", "VRChat")]

namespace DownloadFix
{
    public class DownloadFix : MelonMod
    {


        public override void OnApplicationStart()
        {
            ICustomLayoutedMenu settingsPage = ExpansionKitApi.GetExpandedMenu(ExpandedMenu.SettingsMenu);
            settingsPage.AddSimpleButton("Unblock unpack queue", delegate { Utilities.UnblockUnPackQueue(); });
        }

        public override void VRChat_OnUiManagerInit()
        {
            GameObject unblockButton = GameObject
                .Instantiate(
                    GameObject.Find(
                            "UserInterface/MenuContent/Popups/LoadingPopup/ProgressPanel/Parent_Loading_Progress/GoButton")
                        .transform,
                    GameObject.Find("UserInterface/MenuContent/Popups/LoadingPopup/ProgressPanel").transform)
                .gameObject;
            unblockButton.GetComponentInChildren<Text>().text = "Unblock Queue";
            unblockButton.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            unblockButton.GetComponent<Button>().onClick.AddListener(DelegateSupport.ConvertDelegate<UnityAction>(
                new Action(delegate
                {
                    Utilities.UnblockUnPackQueue();
                    Utilities.DeselectClickedButton(unblockButton);
                })));
            unblockButton.GetComponent<Transform>().localPosition = new Vector3(-2.4f, -124f, 0);
            GameObject.Find("UserInterface/MenuContent/Popups/LoadingPopup/ProgressPanel/Parent_Loading_Progress")
                .transform.localPosition = new Vector3(0, 17, 0);

        }
    }
}
