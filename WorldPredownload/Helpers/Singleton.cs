using System;

// ReSharper disable HeuristicUnreachableCode

namespace WorldPredownload.Helpers
{
    // https://stackoverflow.com/questions/2319075/generic-singletont
    public class Singleton<T> where T : class, new()
    {
        protected static T instance;
        private static readonly object @lock = new();

        public static T Instance
        {
            get
            {
                if (instance != null) return instance;
                lock (@lock) // This shouldn't be necessary but just in case, its here
                {
                    instance ??= Activator.CreateInstance<T>();
                }

                return instance;
            }
        }
    }
}