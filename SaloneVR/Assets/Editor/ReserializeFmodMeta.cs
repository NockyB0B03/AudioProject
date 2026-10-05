using System.Linq;
using UnityEditor;
using UnityEngine;

// Mettere in una cartella "Editor" (es. Assets/Editor/ReserializeFmodMeta.cs)
// Menu: Tools > FMOD > Aggiorna .meta dei plugin
//
// Fa via codice quello che chiede il warning ("Open and re-save"):
// marca ogni PluginImporter di FMOD come modificato e lo risalva,
// come premere Apply nell'Inspector. Le impostazioni di piattaforma
// vengono lette dal .meta esistente e riscritte identiche.
public static class ReserializeFmodMeta
{
    const string FmodFolder = "Assets/Plugins/FMOD/";

    [MenuItem("Tools/FMOD/Aggiorna .meta dei plugin")]
    static void Run()
    {
        var importers = PluginImporter.GetAllImporters()
            .Where(i => i.assetPath.StartsWith(FmodFolder))
            .ToArray();

        if (importers.Length == 0)
        {
            Debug.LogWarning($"Nessun plugin trovato sotto {FmodFolder}");
            return;
        }

        try
        {
            AssetDatabase.StartAssetEditing();
            foreach (var importer in importers)
            {
                EditorUtility.SetDirty(importer);
                importer.SaveAndReimport();
            }
        }
        finally
        {
            AssetDatabase.StopAssetEditing();
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"Risalvati {importers.Length} plugin importer sotto {FmodFolder}");
    }
}
