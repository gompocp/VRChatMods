using Transmtn.DTO.Notifications;
using VRC.Core;
using VRC.UI;

namespace WorldPredownload.DownloadManager
{
    public class DownloadInfo
    {
        public DownloadInfo(
            ApiWorld apiWorld,
            string instanceIDTags,
            DownloadType downloadType,
            PageUserInfo pageUserInfo = null!,
            PageWorldInfo pageWorldInfo = null!,
            Notification notification = null!)
        {
            ApiWorld = apiWorld;
            InstanceIDTags = instanceIDTags;
            DownloadType = downloadType;
            PageUserInfo = pageUserInfo;
            if (pageUserInfo != null) APIUser = pageUserInfo.field_Public_APIUser_0;
            PageWorldInfo = pageWorldInfo;
            Notification = notification;
        }

        public ApiWorld ApiWorld { get; }
        public string InstanceIDTags { get; }
        public DownloadType DownloadType { get; }
        public PageUserInfo? PageUserInfo { get; }

        public APIUser? APIUser { get; }
        public PageWorldInfo? PageWorldInfo { get; }
        public Notification? Notification { get; }

        public static DownloadInfo CreateInviteDownloadInfo(
            ApiWorld apiWorld,
            string instanceIDTags,
            DownloadType downloadType,
            Notification notification)
        {
            return new(apiWorld, instanceIDTags, downloadType, null!, null!, notification);
        }

        public static DownloadInfo CreateWorldPageDownloadInfo(
            ApiWorld apiWorld,
            string instanceIDTags,
            DownloadType downloadType,
            PageWorldInfo pageWorldInfo)
        {
            return new(apiWorld, instanceIDTags, downloadType, null!, pageWorldInfo);
        }

        public static DownloadInfo CreateUserPageDownloadInfo(
            ApiWorld apiWorld,
            string instanceIDTags,
            DownloadType downloadType,
            PageUserInfo pageUserInfo)
        {
            return new(apiWorld, instanceIDTags, downloadType, pageUserInfo);
        }
    }
}