using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization.Tables;

namespace Fsi.Localization
{
    public static class FsiLocalizationUtilityEditor
    { 
        public static void EnsureStringEntryExists(string collectionName, string key, string defaultValue = "")
        {
            StringTableCollection collection = LocalizationEditorSettings.GetStringTableCollection(collectionName);
            if (collection == null)
            {
                Debug.LogError($"String table collection '{collectionName}' was not found.");
                return;
            }

            SharedTableData sharedData = collection.SharedData;

            // Create the shared key if it does not exist
            if (!sharedData.Contains(key))
            {
                sharedData.AddKey(key);
                EditorUtility.SetDirty(sharedData);
            }

            // Ensure each locale table has an entry for the key
            foreach (var table in collection.StringTables)
            {
                if (table.GetEntry(key) == null)
                {
                    table.AddEntry(key, defaultValue);
                    EditorUtility.SetDirty(table);
                }
            }

            AssetDatabase.SaveAssets();
        }
    }
}
