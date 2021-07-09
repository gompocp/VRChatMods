using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using Il2CppSystem;
using MelonLoader;
using UIExpansionKit.API;
using UnhollowerRuntimeLib;
using UnityEngine;
using WorldPredownload.Cache;
using WorldPredownload.Helpers;
using WorldPredownload.UI;
using AsyncOperation = UnityEngine.AsyncOperation;

namespace WorldPredownload.DownloadManager
{
    [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
    [SuppressMessage("ReSharper", "HeuristicUnreachableCode")]
    public static partial class WorldDownloadManager
    {
        private static readonly AsyncCompletedEventHandler complete = async (_, args) =>
        {
            await TaskUtilities.YieldToMainThread();
            webClient.Dispose();
            if (!CacheManager.WorldFileExists(DownloadInfo.ApiWorld.id))
            {
                if (ModSettings.hideQMStatusWhenInActive) WorldDownloadStatus.Disable();
                Downloading = false;
                HudIcon.Disable();
                InviteButton.UpdateTextDownloadStopped();
                FriendButton.UpdateTextDownloadStopped();
                WorldButton.UpdateTextDownloadStopped();
                WorldDownloadStatus.gameObject.SetText(Constants.DOWNLOAD_STATUS_IDLE_TEXT);
                if (!string.IsNullOrEmpty(file)) File.Delete(file);
                if (!args.Cancelled)
                    MelonLogger.Error(
                        $"World failed to download. Why you might ask?... I don't know! This exception might help: {args.Error}");
                return;
            }

            var operation = AssetBundle.RecompressAssetBundleAsync(file, file, new BuildCompression
            {
                compression = CompressionType.Lz4,
                level = CompressionLevel.High,
                blockSize = 131072U
            }, 0, ThreadPriority.Normal);
            operation.add_completed(DelegateSupport.ConvertDelegate<Action<AsyncOperation>>(
                new System.Action<AsyncOperation>(
                    delegate
                    {
                        MelonLogger.Msg($"Finished recompressing world with result: {operation.result}");
                        var task = new Task(OnRecompress);
                        // I don't really know how else to ensure that this the recompress operation runs on the main thread, if you know feel free to bonk me for being dumb
                        task.NoAwait("WorldPredownload OnRecompress");
                        task.Start();
                    }))
            );
        };

        private static async void OnRecompress()
        {
            await TaskUtilities.YieldToMainThread();
            CacheManager.CreateInfoFileFor(file);
            if (ModSettings.showHudMessages) Utilities.QueueHudMessage("Download Finished");
            if (ModSettings.hideQMStatusWhenInActive) WorldDownloadStatus.Disable();
            Downloading = false;
            CacheManager.AddDirectory(CacheManager.ComputeAssetHash(DownloadInfo.ApiWorld.id));
            HudIcon.Disable();
            InviteButton.UpdateTextDownloadStopped();
            FriendButton.UpdateTextDownloadStopped();
            WorldButton.UpdateTextDownloadStopped();
            WorldDownloadStatus.gameObject.SetText(Constants.DOWNLOAD_STATUS_IDLE_TEXT);
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
        }
    }
}