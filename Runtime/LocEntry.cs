using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Fsi.Localization
{
	[Serializable]
	public class LocEntry
	{
		[SerializeField]
		private LocalizedString entry;

		public bool IsSet => !entry.IsEmpty;

		public string GetLocalizedString(Dictionary<string, string> args, string fallback = "no_loc")
		{
			if (entry.IsEmpty)
			{
				Debug.LogError($"LocEntry | No loc found. \nUsing fallback ({fallback}).");
			}
			return entry.IsEmpty ? fallback : entry.GetLocalizedString(args);
		}

		public string GetLocalizedString(string fallback = "no_loc")
		{
		    if (entry.IsEmpty)
		    {
		        Debug.LogError($"LocEntry | No loc found. \nUsing fallback ({fallback}).");
		        return fallback;
		    }

		    // Avoid calling into Localization/AssetDatabase during editor domain backup/update.
		    // When the editor is compiling or updating, synchronous lookups can touch AssetDatabase
		    // and trigger 'restricted during domain backup' errors. In those cases, return fallback.
			#if UNITY_EDITOR
		    if (!Application.isPlaying && (UnityEditor.EditorApplication.isCompiling 
		                                   || UnityEditor.EditorApplication.isUpdating))
		    {
		        return fallback;
		    }
		    #endif

		    return entry.GetLocalizedString();
		}

        /// <summary>
        /// Async version that avoids synchronous AssetDatabase/Addressables access. Prefer this in editor tools and gameplay code.
        /// </summary>
        public async Task<string> GetLocalizedStringAsync(string fallback = "no_loc")
        {
            if (entry.IsEmpty)
            {
                Debug.LogError($"LocEntry | No loc found. \nUsing fallback ({fallback}).");
                return fallback;
            }

            AsyncOperationHandle<string> handle = entry.GetLocalizedStringAsync();
            try
            {
                return await handle.Task;
            }
            catch (Exception)
            {
                return fallback;
            }
        }
	}
}