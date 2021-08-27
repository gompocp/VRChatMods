using System.Diagnostics.CodeAnalysis;
using System.Net;
using UIExpansionKit.API;
using WorldPredownload.Helpers;
using WorldPredownload.UI;
using OnDownloadProgress = AssetBundleDownloadManager.MulticastDelegateNInternalSealedVoUnUnique;

namespace WorldPredownload.DownloadManager
{
    [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
    [SuppressMessage("ReSharper", "HeuristicUnreachableCode")]
    public static partial class WorldDownloadManager
    {
        private static readonly DownloadProgressChangedEventHandler OnProgress = async (_, args) =>
        {
            await TaskUtilities.YieldToMainThread();
            if (!Downloading) return;
            var text = $"Progress:{args.ProgressPercentage} %";
            if (ModSettings.showStatusOnQM) WorldDownloadStatus.GameObject.SetText(text);
            //if (InviteButton.CanChangeText) InviteButton.Button.SetText(text);
            if (FriendButton.CanChangeText) FriendButton.Button.SetText(text);
            if (WorldButton.CanChangeText) WorldButton.Button.SetText(text);
            if (ModSettings.showStatusOnHud) HudIcon.Update(args.ProgressPercentage / 100f);
        };
    }
}