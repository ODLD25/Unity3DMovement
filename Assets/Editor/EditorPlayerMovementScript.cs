using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(PlayerMovementScript))]
public class EditorPlayerMovementScript : Editor
{
    public VisualTreeAsset VisualTreeAsset;
    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new VisualElement();

        VisualTreeAsset.CloneTree(root);

        return root;
    }
}
