namespace WorldPredownload.DownloadManager
{
    public enum DownloadState
    {
        StartingDownload,
        Downloading,
        FinishedRecompressing,
        Idle,
        RefreshUI
    }
}