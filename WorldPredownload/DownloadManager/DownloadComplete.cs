using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Il2CppSystem;
using MelonLoader;
using UIExpansionKit.API;
using UnhollowerRuntimeLib;
using UnityEngine;
using WorldPredownload.Cache;
using WorldPredownload.UI;
using AsyncOperation = UnityEngine.AsyncOperation;

//using AssetBundleDownload = CustomYieldInstructionPublicObAsByStInStCoBoObInUnique;
//using OnDownloadComplete = AssetBundleDownloadManager.MulticastDelegateNInternalSealedVoObUnique;

namespace WorldPredownload.DownloadManager
{
    [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
    [SuppressMessage("ReSharper", "HeuristicUnreachableCode")]
    public static partial class WorldDownloadManager
    {
        private static readonly AsyncCompletedEventHandler complete = async (sender, args) =>
        {
            await TaskUtilities.YieldToMainThread();
            
            webClient.Dispose();
            if (ModSettings.showHudMessages) Utilities.QueueHudMessage("Download Finished");
            if (ModSettings.hideQMStatusWhenInActive) WorldDownloadStatus.Disable();
            DownloadInfo.complete = true;
            downloading = false;
            CacheManager.AddDirectory(CacheManager.ComputeAssetHash(DownloadInfo.ApiWorld.id));
            HudIcon.Disable();
            InviteButton.UpdateTextDownloadStopped();
            FriendButton.UpdateTextDownloadStopped();
            WorldButton.UpdateTextDownloadStopped();
            WorldDownloadStatus.gameObject.SetText(Constants.DOWNLOAD_STATUS_IDLE_TEXT);
            MelonLogger.Msg($"Finished downloading world");
            
            var operation = AssetBundle.RecompressAssetBundleAsync(file, file, new BuildCompression()
            {
                compression = CompressionType.Lz4,
                level = CompressionLevel.High,
                blockSize = 131072U
            }, 0, ThreadPriority.Normal);
            operation.add_completed(DelegateSupport.ConvertDelegate<Action<AsyncOperation>>(new System.Action<AsyncOperation>(
                delegate(AsyncOperation asyncOperation)
                {
                    MelonLogger.Msg($"Finished recompressing world with result: {operation.result}");
                    CacheManager.CreateInfoFileFor(file);
                }))
            );
            
            
            switch (DownloadInfo.DownloadType)
            {
                case DownloadType.Friend:
                    if (!ModSettings.autoFollowFriends)
                    {
                        if (ModSettings.showPopupsOnComplete)
                            DisplayFriendPopup();
                    }
                    else
                    {
                        Utilities.GoToWorld(DownloadInfo.ApiWorld, DownloadInfo.InstanceIDTags, false);
                    }

                    break;
                case DownloadType.Invite:
                    if (!ModSettings.autoFollowInvites)
                    {
                        if (ModSettings.showPopupsOnComplete)
                            DisplayInvitePopup();
                    }
                    else
                    {
                        Utilities.GoToWorld(DownloadInfo.ApiWorld, DownloadInfo.InstanceIDTags, true);
                    }

                    break;
                case DownloadType.World:
                    if (!ModSettings.autoFollowWorlds)
                    {
                        if (ModSettings.showPopupsOnComplete)
                            DisplayWorldPopup();
                    }
                    else
                    {
                        Utilities.GoToWorld(DownloadInfo.ApiWorld, DownloadInfo.InstanceIDTags, false);
                    }

                    break;
            }
            
            
        };
    }
}