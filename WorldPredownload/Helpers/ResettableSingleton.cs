namespace WorldPredownload.Helpers
{
    public class ResettableSingleton<T> : Singleton<T> where T : class, IResettable, new()
    {
        public void Reset()
        {
            instance.Reset();
        }
    }
}