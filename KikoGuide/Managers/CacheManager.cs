namespace KikoGuide.Managers;

using System;
using System.Collections.Generic;

// <summary>
// CacheManager provides methods for handling the caching of data. 
// It should primarily be used to help improve performance on resource-intensive tasks.
// </summary>
class CacheManager
{
    private object _cacheItem;
    private long _cacheExpiryTime;
    private long _cacheTime;
    private bool _valid = false;

    // <summary>
    // Instantiates a new CacheManager.
    // </summary>
    public CacheManager(int cacheExpiryTime, object cacheItem)
    {
        _cacheItem = new List<object>();
        _cacheTime = 0;
        _cacheExpiryTime = cacheExpiryTime;
    }

    // <summary>
    // Returns the cache status of this manager. False if invalid, true if valid.
    // </summary>
    public bool IsValid() => _valid;

    // <summary>
    // Returns if the cached item has expired or not. True if yes, false otherwise.
    // </summary>
    public bool HasExpired() => _cacheTime + _cacheExpiryTime < DateTimeOffset.Now.ToUnixTimeMilliseconds();

    // <summary>
    // Invalidates the cache.
    // </summary>
    public void Invalidate() => _valid = false;

    // <summary>
    // Returns the cached item.
    // </summary>
    public object GetCacheItem() => _cacheItem;

    // <summary>
    // Sets the cached item, as well as the time it was cached. If nothing has been cached yet, it will set HasCached to true.
    // </summary>
    public void SetCacheItem(object cacheItem) { this._cacheItem = cacheItem; _valid = true; _cacheTime = DateTimeOffset.Now.ToUnixTimeMilliseconds(); }
}