using UnityEditor;
using UnityEngine;

public class ColorChangerEditorWindow : EditorWindow
{
    private Color objectColor = Color.white;

    [MenuItem("Tools/Color Changer")] 
    public static void ShowWindow()
    {
        GetWindow<ColorChangerEditorWindow>("Color Changer");
    }

    void OnGUI()
    {
        GUILayout.Label("Apply Color to Selected Objects", EditorStyles.boldLabel);

        objectColor = EditorGUILayout.ColorField("Choose Color:", objectColor);

        if (GUILayout.Button("Apply Color"))
        {
            if (Selection.gameObjects.Length == 0)
            {
                EditorUtility.DisplayDialog("No Objects Selected", "Please select one or more objects in the Hierarchy to apply color.", "OK");
                return;
            }

            foreach (GameObject obj in Selection.gameObjects)
            {
                Renderer renderer = obj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = objectColor;

                    EditorUtility.SetDirty(obj);
                }
                else
                {
                    Debug.LogWarning($"Object '{obj.name}' has no Renderer component to apply color.", obj);
                }
            }

            Debug.Log($"Color '{objectColor}' applied to {Selection.gameObjects.Length} selected objects.");
        }
    }
}