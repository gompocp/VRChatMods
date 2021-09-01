using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using MelonLoader;
using WorldPredownload.Helpers;

namespace WorldPredownload.Cache
{
    public class CacheManager
    {
        private static readonly HashSet<string> Directories = new();
        private static readonly Stopwatch Stopwatch = new();

        public static void UpdateDirectories()
        {
            Stopwatch.Restart();
            Directories.Clear();
            var files = Directory.GetDirectories(GetCache().path, "*", SearchOption.TopDirectoryOnly);
            for (var i = 0; i < files.Length; i++)
                Directories.Add(Path.GetFileName(files[i]));
            Stopwatch.Stop();
            MelonLogger.Msg($"Finished getting {Directories.Count} cache entries in {Stopwatch.ElapsedMilliseconds}ms");
        }

        public static void AddDirectory(string hash)
        {
            Directories.Add(hash);
        }


        public static bool HasDownloadedWorld(string url)
        {
            var hash = ComputeAssetHash(url);
            if (Directories.Contains(hash))
            {
                if (HasVersion(hash, url))
                    return true;
                return false;
            }

            return false;
        }

        public static string ComputeAssetHash(string url)
        {
            var id = Utilities.ExtractFileId(url);
            using var sha256 = SHA256.Create();
            var hash = Utilities.ByteArrayToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(id))).ToUpper();   
            #if DEBUG
            MelonLogger.Msg($"File Id: {id}");
            MelonLogger.Msg($"Hash: {hash}");
            #endif
            
            return hash.Substring(0, 16);
        }

        public static UnityEngine.Cache GetCache()
        {
            return Utilities.GetAssetBundleDownloadManager().field_Private_Cache_0;
        }

        private static bool HasVersion(string hash, string url)
        {
            var versionString = ComputeVersionString(url);
            if (!Directory.Exists(Path.Combine(GetCache().path, hash))) return false;
            foreach (var directoryInfo in new DirectoryInfo(Path.Combine(GetCache().path, hash)).GetDirectories())
                if (directoryInfo.Name.EndsWith(versionString))
                    return true;
            return false;
        }

        public static string ComputeVersionString(string url) //Int to Little Endian Hex String
        {
            var fileversion = Utilities.ExtractFileVersion(url);
            if (string.IsNullOrEmpty(fileversion))
                MelonLogger.Error("This is going to lead to an error shortly. uh oh...");

            var version = int.Parse(fileversion);
            var bytes = BitConverter.GetBytes(version);
            var result = "";
            foreach (var b in bytes) result += b.ToString("x2");
            return result;
        }
        
        public static bool WorldFileExists(string url)
        {
            var expectedLocation = new DirectoryInfo(Path.Combine(GetCache().path, ComputeAssetHash(url)));
            if (!Directory.Exists(expectedLocation.FullName)) return false;
            foreach (var file in expectedLocation.GetFiles("*.*", SearchOption.AllDirectories))
                if (file.Name.Contains("__data"))
                    return true;
            return false;
        }

        public static void CreateInfoFileFor(string file)
        {
            File.WriteAllText(Path.Combine(Path.GetDirectoryName(file), "__info"),
                $"-1\n{((DateTimeOffset) DateTime.Now).ToUnixTimeSeconds()}\n1\n__data\n");
        }
    }
}