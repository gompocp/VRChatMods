using System.Diagnostics.CodeAnalysis;
using System.Net;
using UIExpansionKit.API;
using WorldPredownload.UI;
using OnDownloadProgress = AssetBundleDownloadManager.MulticastDelegateNInternalSealedVoUnUnique;

namespace WorldPredownload.DownloadManager
{
    [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
    [SuppressMessage("ReSharper", "HeuristicUnreachableCode")]
    public static partial class WorldDownloadManager
    {
        private static readonly DownloadProgressChangedEventHandler progress = async (sender, args) =>
        {
            await TaskUtilities.YieldToMainThread();
            var progress = $"Progress:{args.ProgressPercentage} %";
            if (ModSettings.showStatusOnQM) WorldDownloadStatus.gameObject.SetText(progress);
            if (InviteButton.canChangeText) InviteButton.button.SetText(progress);
            if (FriendButton.canChangeText) FriendButton.button.SetText(progress);
            if (WorldButton.canChangeText) WorldButton.button.SetText(progress);
            if (ModSettings.showStatusOnHud) HudIcon.Update(args.ProgressPercentage / 100f);
        };
    }
}