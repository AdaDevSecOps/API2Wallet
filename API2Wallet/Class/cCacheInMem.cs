using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Caching;

namespace API2Wallet.Class
{
    /// <summary>
    /// 
    /// </summary>
    public class cCacheInMem
    {
        /// <summary>
        ///     Check key in memory cahce.
        /// </summary>
        /// 
        /// <param name="ptKey">Key.</param>
        /// 
        /// <returns>
        ///     true : Found key.<br/>
        ///     false : Key not found.
        /// </returns>
        public bool C_CAHbCheckKey(string ptKey)
        {
            MemoryCache oMemCache;
            try
            {
                oMemCache = MemoryCache.Default;
                if (oMemCache.Contains(ptKey))
                {
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                oMemCache = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

        /// <summary>
        ///     Add value to memory cache.
        /// </summary>
        /// 
        /// <param name="ptKey">
        ///     Key cache.
        /// </param>
        /// <param name="poValue">
        ///     Vaule.
        /// </param>
        /// <param name="pnDuration">
        ///     Duration expired in minutes.
        /// </param>
        /// 
        /// <returns>
        ///     true : add complete.<br/>
        ///     fasle : add error.
        /// </returns>
        public bool C_CAHbAddValue(string ptKey, object poValue, int pnDuration = 1)
        {
            MemoryCache oMemCache;
            CacheItemPolicy oCachPolicy;
            bool bStaPrc;

            try
            {
                oCachPolicy = new CacheItemPolicy();
                oCachPolicy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(pnDuration);
                //oCachPolicy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(pnDuration);

                oMemCache = MemoryCache.Default;

                bStaPrc = oMemCache.Add(ptKey, poValue, oCachPolicy);

                return bStaPrc;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                oMemCache = null;
                oCachPolicy = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

        /// <summary>
        ///     Get value from cache.
        /// </summary>
        /// 
        /// <typeparam name="T">Type return.</typeparam>
        /// <param name="ptKey">Key.</param>
        /// 
        /// <returns>
        ///     Value of key.
        /// </returns>
        public T C_CAHoGetValue<T>(string ptKey)
        {
            MemoryCache oMemCache;
            T oResult = default(T); /*(T)Activator.CreateInstance(typeof(T))*/;

            try
            {
                oMemCache = MemoryCache.Default;
                if (oMemCache.Contains(ptKey))
                {
                    oResult = (T)oMemCache.Get(ptKey, null);
                }

                return oResult;
            }
            catch (Exception)
            {

            }
            finally
            {
                oMemCache = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }

            return oResult;
        }

        /// <summary>
        ///     Remove memory cache value.
        /// </summary>
        /// 
        /// <param name="ptKey">
        ///     Key cache.
        /// </param>
        /// 
        /// <returns>
        ///     true : remove complete.<br/>
        ///     fasle : remove error.
        /// </returns>
        public bool C_CAHbRemoveValue(string ptKey)
        {
            MemoryCache oMemCache;

            try
            {
                oMemCache = MemoryCache.Default;
                if (oMemCache.Contains(ptKey))
                {
                    oMemCache.Remove(ptKey);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                oMemCache = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }

        }
    }
}