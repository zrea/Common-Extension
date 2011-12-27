using System;

namespace System
{
    public interface IMemoryCache<T>
    {
        bool Add(string key, T value);
        T AddOrGetExisting(string key, T value);
        TimeSpan Interval { get; set; }
        string RegionName { get; set; }
        T this[string key] { get; set; }
        bool TryGetValue(string key, out T value);
    }
}
