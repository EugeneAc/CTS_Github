using System;
using System.Runtime.Caching;

namespace CTS_Manual_Input.Helpers
{
	public sealed class Cacher
	{
		private static volatile Cacher instance; 
		private static MemoryCache memoryCache;
		private static object syncRoot = new Object();
		private static double settingCacheExpirationTimeInMinutes;
		private Cacher() { }

		public static Cacher Instance
		{
			get
			{
				if (instance == null)
				{
					lock (syncRoot)
					{
						if (instance == null)
						{
							instance = new Cacher();
							memoryCache = new MemoryCache("MI_Cache");
							settingCacheExpirationTimeInMinutes = 60;
						}
					}
				}
				return instance;
			}
		}

		public void Write(string Key, object Value)
		{

				memoryCache.Add(Key, Value, DateTimeOffset.Now.AddMinutes(settingCacheExpirationTimeInMinutes));
			
		}

		public object Read(string Key)
		{
			return memoryCache.Get(Key);
		}

		public object TryRead(string Key)
		{
			try
			{
				return memoryCache.Get(Key);
			}
			catch (Exception)
			{
				return null;
			}
		}
	}
}