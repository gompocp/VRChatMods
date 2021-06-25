﻿using System;
using System.Linq;
using System.Reflection;
using MelonLoader;
using Transmtn.DTO.Notifications;
using UnhollowerBaseLib.Attributes;
using VRC.Core;
using VRC.UI;
using Action = Il2CppSystem.Action;

// ReSharper disable ConditionIsAlwaysTrueOrFalse

namespace WorldPredownload.Helpers
{
    public static class Delegates
    {
        private static LoadUserDelegate loadUserDelegate;

        public static LoadUserDelegate GetLoadUserDelegate
        {
            get
            {
                //Build 1088 menu.Method_Private_Void_ObjectNPublicAcTeAcStGaUnique_0()
                if (loadUserDelegate != null) return loadUserDelegate;
                var loadUserMethod = typeof(PageUserInfo).GetMethods().Where(
                        m =>
                            m.Name.StartsWith("Method_Public_Void_APIUser_PDM_")
                    )
                    .OrderBy(m => m.GetCustomAttribute<CallerCountAttribute>().Count)
                    .Last();
                loadUserDelegate = (LoadUserDelegate) Delegate.CreateDelegate(
                    typeof(LoadUserDelegate),
                    null,
                    loadUserMethod);
                return loadUserDelegate;
            }
        }

        public delegate void LoadUserDelegate(PageUserInfo pageUserInfo, APIUser apiUser);

        private static ClearErrorsDelegate clearErrorsDelegate;
        private static ShowDismissPopupDelegate showDismissPopupDelegate;
        private static ShowOptionsPopupDelegate showOptionsPopupDelegate;
        private static PushUIPageDelegate pushUIPageDelegate;
        private static AdvancedInvitesInviteDelegate advancedInvitesInviteDelegate;

        private static ClearErrorsDelegate GetClearErrorsDelegate
        {
            get
            {
                if (clearErrorsDelegate != null) return clearErrorsDelegate;
                var clearErrors = typeof(AssetBundleDownloadManager).GetMethods().First(
                    m => m.Name.StartsWith("Method_Internal_Void_")
                         && !m.Name.Contains("PDM")
                         && m.ReturnType == typeof(void)
                         && m.GetParameters().Length == 0);
                clearErrorsDelegate = (ClearErrorsDelegate) Delegate.CreateDelegate(
                    typeof(ClearErrorsDelegate),
                    AssetBundleDownloadManager.prop_AssetBundleDownloadManager_0,
                    clearErrors
                );
                return clearErrorsDelegate;
            }
        }

        public static ShowDismissPopupDelegate GetShowDismissPopupDelegate
        {
            get
            {
                if (showDismissPopupDelegate != null) return showDismissPopupDelegate;
                var popupMethod = typeof(VRCUiPopupManager).GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .First(
                        m =>
                            m.GetParameters().Length == 5
                            && m.XRefScanFor("Popups/StandardPopupV2")
                    );

                showDismissPopupDelegate = (ShowDismissPopupDelegate) Delegate.CreateDelegate(
                    typeof(ShowDismissPopupDelegate),
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0,
                    popupMethod
                );
                return showDismissPopupDelegate;
            }
        }

        public static ShowOptionsPopupDelegate GetShowOptionsPopupDelegate
        {
            get
            {
                if (showOptionsPopupDelegate != null) return showOptionsPopupDelegate;
                var popupMethod = typeof(VRCUiPopupManager).GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .Single(
                        m => m.GetParameters().Length == 7 && m.XRefScanFor("Popups/StandardPopupV2"));

                showOptionsPopupDelegate = (ShowOptionsPopupDelegate) Delegate.CreateDelegate(
                    typeof(ShowOptionsPopupDelegate),
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0,
                    popupMethod
                );
                return showOptionsPopupDelegate;
            }
        }

        public static PushUIPageDelegate GetPushUIPageDelegate
        {
            get
            {
                if (pushUIPageDelegate != null) return pushUIPageDelegate;
                var pushPageMethod = typeof(VRCUiManager).GetMethods().First(
                    m => m.GetParameters().Length == 1
                         && m.GetParameters()[0].ParameterType == typeof(VRCUiPage)
                         && !m.Name.Contains("PDM")
                         && m.ReturnType == typeof(VRCUiPage)
                );

                pushUIPageDelegate = (PushUIPageDelegate) Delegate.CreateDelegate(
                    typeof(PushUIPageDelegate),
                    VRCUiManager.prop_VRCUiManager_0,
                    pushPageMethod
                );
                return pushUIPageDelegate;
            }
        }

        public static AdvancedInvitesInviteDelegate GetAdvancedInvitesInviteDelegate
        {
            get
            {
                if (advancedInvitesInviteDelegate != null) return advancedInvitesInviteDelegate;

                //InviteHandler
                var handleNotificationMethod = MelonHandler.Mods.First(
                    m => m.Info.Name.Equals("AdvancedInvites")).Assembly.GetTypes().Single(
                    t => t.Name.Equals("InviteHandler")).GetMethods(BindingFlags.Public | BindingFlags.Static).Single(
                    me => me.GetParameters().Length == 1
                          && me.GetParameters()[0].ParameterType ==
                          typeof(Notification) // Could probably use method name here but ¯\_(ツ)_/¯ 
                );

                advancedInvitesInviteDelegate = (AdvancedInvitesInviteDelegate) Delegate.CreateDelegate(
                    typeof(AdvancedInvitesInviteDelegate),
                    handleNotificationMethod
                );
                return advancedInvitesInviteDelegate;
            }
        }

        public delegate VRCUiPage PushUIPageDelegate(VRCUiPage page);

        public delegate void ShowOptionsPopupDelegate(
            string title,
            string body,
            string leftButtonText,
            Action leftButtonAction,
            string rightButtonText,
            Action rightButtonAction, Il2CppSystem.Action<VRCUiPopup> additionalSetup = null
        );

        public delegate void ShowDismissPopupDelegate(
            string title,
            string body,
            string middleButtonText,
            Action middleButtonAction, Il2CppSystem.Action<VRCUiPopup> additionalSetup = null
        );

        private delegate void ClearErrorsDelegate();

        public delegate void AdvancedInvitesInviteDelegate(Notification notification);
    }
}