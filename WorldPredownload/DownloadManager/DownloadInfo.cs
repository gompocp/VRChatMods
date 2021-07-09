using Transmtn.DTO.Notifications;
using VRC.Core;
using VRC.UI;

namespace WorldPredownload.DownloadManager
{
    public class DownloadInfo
    {
        public DownloadInfo(
            ApiWorld ApiWorld,
            string InstanceIDTags,
            DownloadType DownloadType,
            PageUserInfo PageUserInfo = null!,
            PageWorldInfo PageWorldInfo = null!,
            Notification Notification = null!)
        {
            this.ApiWorld = ApiWorld;
            this.InstanceIDTags = InstanceIDTags;
            this.DownloadType = DownloadType;
            this.PageUserInfo = PageUserInfo;
            if (PageUserInfo != null) APIUser = PageUserInfo.field_Public_APIUser_0;
            this.PageWorldInfo = PageWorldInfo;
            this.Notification = Notification;
        }

        public ApiWorld ApiWorld { get; set; }
        public string InstanceIDTags { get; set; }
        public DownloadType DownloadType { get; set; }
        public PageUserInfo? PageUserInfo { get; set; }

        public APIUser? APIUser { get; set; }
        public PageWorldInfo? PageWorldInfo { get; set; }
        public Notification? Notification { get; set; }

        public static DownloadInfo CreateInviteDownloadInfo(
            ApiWorld ApiWorld,
            string InstanceIDTags,
            DownloadType DownloadType,
            Notification Notification)
        {
            return new(ApiWorld, InstanceIDTags, DownloadType, null, null, Notification);
        }

        public static DownloadInfo CreateWorldPageDownloadInfo(
            ApiWorld ApiWorld,
            string InstanceIDTags,
            DownloadType DownloadType,
            PageWorldInfo PageWorldInfo)
        {
            return new(ApiWorld, InstanceIDTags, DownloadType, null, PageWorldInfo);
        }

        public static DownloadInfo CreateUserPageDownloadInfo(
            ApiWorld ApiWorld,
            string InstanceIDTags,
            DownloadType DownloadType,
            PageUserInfo PageUserInfo)
        {
            return new(ApiWorld, InstanceIDTags, DownloadType, PageUserInfo);
        }
    }
}