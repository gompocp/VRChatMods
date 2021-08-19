using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using HarmonyLib;
using MelonLoader;
using Transmtn.DTO.Notifications;
using UnhollowerBaseLib;
using UnhollowerBaseLib.Attributes;
using VRC.Core;
using VRC.UI;
using WorldPredownload.Cache;
using WorldPredownload.DownloadManager;
using WorldPredownload.Helpers;
using WorldPredownload.UI;
using InfoType = VRC.UI.PageUserInfo.EnumNPublicSealedvaNoOnOfSeReBlInFa10Unique;
using ListType = UiUserList.EnumNPublicSealedvaNoInFrOnOfSeInFa9vUnique;

//using OnDownloadComplete = AssetBundleDownloadManager.MulticastDelegateNInternalSealedVoObUnique;

namespace WorldPredownload
{
    [HarmonyPatch(typeof(NetworkManager), "OnLeftRoom")]
    internal class OnLeftRoomPatch
    {
        private static void Prefix()
        {
            DownloadManager.Downloader.CancelDownload();
        }
    }

    [HarmonyPatch(typeof(NetworkManager), "OnJoinedRoom")]
    internal class OnJoinedRoomPatch
    {
        private static void Prefix()
        {
            CacheManager.UpdateDirectories();
            Singleton<DownloadManager.Downloader>.Instance.DownloadState = DownloadState.Idle;
        }
    }

    internal class WorldInfoSetup
    {
        private static WorldInfoSetupDelegate worldInfoSetupDelegate;

        public static void Patch()
        {
            unsafe
            {
                var setupMethod = typeof(PageWorldInfo).GetMethods()
                    .Where(m =>
                        m.Name.StartsWith("Method_Public_Void_ApiWorld_ApiWorldInstance_Boolean_Boolean_") &&
                        !m.Name.Contains("PDM"))
                    .OrderBy(m => m.GetCustomAttribute<CallerCountAttribute>().Count)
                    .Last();

                // Thanks to Knah
                var originalMethod = *(IntPtr*) (IntPtr) UnhollowerUtils
                    .GetIl2CppMethodInfoPointerFieldForGeneratedMethod(setupMethod).GetValue(null);

                MelonUtils.NativeHookAttach((IntPtr) (&originalMethod),
                    typeof(WorldInfoSetup).GetMethod(nameof(Postfix), BindingFlags.Static | BindingFlags.Public)!
                        .MethodHandle.GetFunctionPointer());

                worldInfoSetupDelegate = Marshal.GetDelegateForFunctionPointer<WorldInfoSetupDelegate>(originalMethod);
            }
        }

        public static void Postfix(IntPtr thisPtr, IntPtr apiWorldPtr, IntPtr apiWorldInstancePtr, byte something1,
            byte something2, IntPtr additionalJunkPtr)
        {
            try
            {
                worldInfoSetupDelegate(thisPtr, apiWorldPtr, apiWorldInstancePtr, something1, something2,
                    additionalJunkPtr);
                if (apiWorldPtr != IntPtr.Zero)
                    Singleton<DownloadManager.Downloader>.Instance.DownloadState = DownloadState.RefreshUI;
            }
            catch (Exception e)
            {
                MelonLogger.Error($"Something went horribly wrong in WorldInfoSetup Patch, pls report to gompo: {e}");
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void WorldInfoSetupDelegate(IntPtr thisPtr, IntPtr apiWorld, IntPtr apiWorldInstance,
            byte something1, byte something2, IntPtr additionalJunk);
    }

    //I accidently found that this neat little method which opens the notification more actions page a while ago while fixing up advanced invites 
    //[HarmonyPatch(typeof(NotificationManager), "Method_Private_Void_Notification_1")]
    internal class NotificationMoreActions
    {
        public static Notification SelectedNotification { get; private set; }

        public static void Patch()
        {
            var openMoreActionsMethod = typeof(NotificationManager).GetMethods()
                .Where(m =>
                    m.Name.StartsWith("Method_Private_Void_Notification_") &&
                    m.XRefScanFor("AcceptNotification for notification:") &&
                    !m.Name.Contains("PDM"))
                .OrderBy(m => m.GetCustomAttribute<CallerCountAttribute>().Count)
                .Last();
            WorldPredownload.HarmonyInstance.Patch(openMoreActionsMethod,
                new HarmonyMethod(typeof(NotificationMoreActions).GetMethod(nameof(Prefix))));
        }

        public static void Prefix(Notification __0)
        {
            SelectedNotification = __0;
            MelonLogger.Msg("Called patch");
            Singleton<DownloadManager.Downloader>.Instance.DownloadState = DownloadState.RefreshUI;
        }
    }

    internal class SocialMenuSetup
    {
        public static void Patch()
        {
            WorldPredownload.HarmonyInstance.Patch(typeof(PageUserInfo).GetMethods().Where(
                        m => m.ReturnType == typeof(void)
                             && m.GetParameters().Length == 3
                             && m.GetParameters()[0].ParameterType == typeof(APIUser)
                             && m.GetParameters()[1].ParameterType == typeof(InfoType)
                             && m.GetParameters()[2].ParameterType == typeof(ListType)
                             && !m.Name.Contains("PDM")
                    ).OrderBy(m => m.GetCustomAttribute<CallerCountAttribute>().Count)
                    .Last(),
                null,
                new HarmonyMethod(typeof(SocialMenuSetup).GetMethod(nameof(Postfix)))
            );
        }

        public static void Postfix(APIUser __0 = null) //, InfoType __1, ListType __2 = ListType.None
        {
            if (__0 == null) return;
            if (__0.location == null)
            {
                FriendButton.Button.SetActive(false);
                return;
            }

            if (!__0.isFriend ||
                Utilities.IsInSameWorld(__0) ||
                __0.location.ToLower().Equals("private") ||
                __0.location.ToLower().Equals("offline")
            )
            {
                FriendButton.Button.SetActive(false);
            }
            else
            {
                FriendButton.Button.SetActive(true);
                Singleton<DownloadManager.Downloader>.Instance.DownloadState = DownloadState.RefreshUI;
            }
        }
    }
}