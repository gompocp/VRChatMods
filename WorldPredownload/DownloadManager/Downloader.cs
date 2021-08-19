using System.Collections.Generic;
using WorldPredownload.Helpers;

// ReSharper disable HeuristicUnreachableCode

namespace WorldPredownload.DownloadManager
{
    internal sealed partial class Downloader : Singleton<Downloader>
    {
        private DownloadState _downloadState;


        private readonly List<IDownloadListener> listeners;

        public Downloader()
        {
            listeners = new List<IDownloadListener>();
        }


        public DownloadState DownloadState
        {
            get => _downloadState;
            set
            {
                _downloadState = value;
                Notify();
            }
        }

        public float Percent { get; set; }

        public string TextStatus { get; set; }

        public void Attach(IDownloadListener listener)
        {
            listeners.Add(listener);
        }

        public void Detach(IDownloadListener listener)
        {
            listeners.Remove(listener);
        }

        private void Notify()
        {
            foreach (var listener in listeners) listener.Update(this);
        }
    }
}