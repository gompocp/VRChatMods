using System;
using System.Diagnostics.CodeAnalysis;
using MelonLoader;
using UnhollowerRuntimeLib;
using WorldPredownload.UI;
using OnDownloadError = AssetBundleDownloadManager.MulticastDelegateNInternalSealedVoStObStUnique;

//using LoadErrorReason = EnumPublicSealedvaNoMiFiUnCoSeAsDuAsUnique;

namespace WorldPredownload.DownloadManager
{
    [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
    [SuppressMessage("ReSharper", "HeuristicUnreachableCode")]
    public partial class WorldDownloadManager
    {
        private static readonly OnDownloadError onErrorDel =
            DelegateSupport.ConvertDelegate<OnDownloadError>(
                new Action<string, string, LoadErrorReason>(
                    (url, message, reason) =>
                    {
                        DownloadInfo.complete = true;
                        Utilities.ClearErrors();
                        HudIcon.Disable();
                        if (ModSettings.hideQMStatusWhenInActive) WorldDownloadStatus.Disable();
                        WorldDownloadStatus.gameObject.SetText(Constants.DOWNLOAD_STATUS_IDLE_TEXT);
                        downloading = false;
                        FriendButton.UpdateTextDownloadStopped();
                        WorldButton.UpdateTextDownloadStopped();
                        InviteButton.UpdateTextDownloadStopped();
                        ClearDownload();
                        if (message.Contains("Request aborted")) return;
                        MelonLogger.Error($"{url} {message} {reason}");
                        Utilities.ShowDismissPopup(
                            Constants.DOWNLOAD_ERROR_TITLE,
                            Constants.DOWNLOAD_ERROR_MSG,
                            Constants.DOWNLOAD_ERROR_BTN_TEXT,
                            new Action(Utilities.HideCurrentPopup)
                        );
                    }
                )
            );
    }
}