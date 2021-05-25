using System;
using System.Diagnostics.CodeAnalysis;
using UnhollowerRuntimeLib;
using UnityEngine.Networking;
using WorldPredownload.UI;
using OnDownloadProgress = AssetBundleDownloadManager.MulticastDelegateNInternalSealedVoUnUnique;

namespace WorldPredownload.DownloadManager
{
    [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
    [SuppressMessage("ReSharper", "HeuristicUnreachableCode")]
    public static partial class WorldDownloadManager
    {
        private static readonly OnDownloadProgress onProgressDel =
            DelegateSupport.ConvertDelegate<OnDownloadProgress>(
                new Action<UnityWebRequest>(
                    request =>
                    {
                        if (cancelled)
                        {
                            request.Abort();
                            cancelled = false;
                            return;
                        }

                        //string size = request.GetResponseHeader("Content-Length");
                        if (request.downloadProgress >= 0 && 0.9 >= request.downloadProgress)
                        {
                            var progress = $"Progress:{(request.downloadProgress / 0.9 * 100).ToString("0")} %";
                            if (ModSettings.showStatusOnQM) WorldDownloadStatus.gameObject.SetText(progress);
                            if (InviteButton.canChangeText) InviteButton.button.SetText(progress);
                            if (FriendButton.canChangeText) FriendButton.button.SetText(progress);
                            if (WorldButton.canChangeText) WorldButton.button.SetText(progress);
                            if (ModSettings.showStatusOnHud) HudIcon.Update((float) (request.downloadProgress / 0.9));
                        }
                    }
                )
            );
    }
}