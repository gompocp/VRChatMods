using System.Diagnostics.CodeAnalysis;
using System.Net;
using UIExpansionKit.API;
using OnDownloadProgress = AssetBundleDownloadManager.MulticastDelegateNInternalSealedVoUnUnique;

namespace WorldPredownload.DownloadManager
{
    [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
    [SuppressMessage("ReSharper", "HeuristicUnreachableCode")]
    internal partial class Downloader
    {
        private static readonly DownloadProgressChangedEventHandler OnProgress = async (_, args) =>
        {
            await TaskUtilities.YieldToMainThread();
            var downloader = Instance;
            if (downloader.DownloadState == DownloadState.Idle) return;
            var text = $"Progress:{args.ProgressPercentage} %";
            downloader.TextStatus = text;
            downloader.Percent = args.ProgressPercentage / 100f;
            downloader.DownloadState = DownloadState.Downloading;
        };
    }
}