/* Currently unused by the plugin, kept for future use. */

namespace KikoGuide.Managers;

using System;
using System.Collections.Generic;

/// <summary>
///     Provides methods for handling the caching of data, used to help improve perf on resource-intensive tasks.
/// </summary>
sealed class CacheManager
{
    private object _cacheItem;
    private long _cacheExpiryTime;
    private long _cacheTime;
    private bool _valid = false;

    /// <summary>
    ///     Instantiates a new CacheManager with the provided settings.
    /// </summary>
    public CacheManager(int cacheExpiryTime, object cacheItem)
    {
        _cacheItem = new List<object>();
        _cacheTime = 0;
        _cacheExpiryTime = cacheExpiryTime;
    }


    /// <summary>
    ///     Returns the cache status of this manager. False if invalid/expired, true if valid.
    /// </summary>
    public bool IsValid() => _cacheTime + _cacheExpiryTime > DateTimeOffset.Now.ToUnixTimeMilliseconds() && _valid;


    /// <summary>
    ///     Invalidates the cache.
    /// </summary>
    public void Invalidate() => _valid = false;


    /// <summary>
    ///     Returns the cached item.
    /// </summary>
    public object GetCacheItem() => _cacheItem;


    /// <summary>
    ///     Sets the cached item and the time it was cached, as well as marking the cache as valid.
    /// </summary>
    public void SetCacheItem(object cacheItem) { this._cacheItem = cacheItem; _valid = true; _cacheTime = DateTimeOffset.Now.ToUnixTimeMilliseconds(); }
}