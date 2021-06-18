using System.Diagnostics.CodeAnalysis;
using System.Net;
using MelonLoader;
using UIExpansionKit.API;
using WorldPredownload.Cache;
using WorldPredownload.UI;

//using AssetBundleDownload = CustomYieldInstructionPublicObAsByStInStCoBoObInUnique;
//using OnDownloadComplete = AssetBundleDownloadManager.MulticastDelegateNInternalSealedVoObUnique;

namespace WorldPredownload.DownloadManager
{
    [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
    [SuppressMessage("ReSharper", "HeuristicUnreachableCode")]
    public static partial class WorldDownloadManager
    {
        private static readonly DownloadDataCompletedEventHandler complete = async (sender, args) =>
        {
            await TaskUtilities.YieldToMainThread();
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
            MelonLogger.Msg($"Downloaded: {args.Result.Length} bytes");


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