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
            if (ModSettings.showStatusOnQM) WorldDownloadStatus.gameObject.SetText(text);
            if (InviteButton.canChangeText) InviteButton.button.SetText(text);
            if (FriendButton.canChangeText) FriendButton.button.SetText(text);
            if (WorldButton.canChangeText) WorldButton.button.SetText(text);
            if (ModSettings.showStatusOnHud) HudIcon.Update(args.ProgressPercentage / 100f);
        };
    }
}