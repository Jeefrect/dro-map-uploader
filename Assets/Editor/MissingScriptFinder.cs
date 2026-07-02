using UnityEditor;
using UnityEngine;

public static class MissingScriptFinder
{
    [MenuItem("Tools/Find Missing Scripts In Scene")]
    public static void FindMissingScripts()
    {
        var objects = Resources.FindObjectsOfTypeAll<GameObject>();
        int count = 0;

        foreach (var go in objects)
        {
            if (EditorUtility.IsPersistent(go))
                continue;

            var components = go.GetComponents<Component>();

            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] == null)
                {
                    go.hideFlags = HideFlags.None;

                    Selection.activeGameObject = go;
                    EditorGUIUtility.PingObject(go);

                    Debug.LogError($"Missing script on: {GetPath(go.transform)} | active: {go.activeInHierarchy} | hideFlags: {go.hideFlags}", go);
                    count++;
                }
            }
        }

        Debug.Log($"Missing scripts found: {count}");
    }

    private static string GetPath(Transform t)
    {
        string path = t.name;

        while (t.parent != null)
        {
            t = t.parent;
            path = t.name + "/" + path;
        }

        return path;
    }
}
