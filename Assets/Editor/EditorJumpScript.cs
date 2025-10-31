using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(JumpScript))]
public class EditorJumpScript : Editor
{
    public VisualTreeAsset VisualTreeAsset;
    private VisualElement root;

    public override VisualElement CreateInspectorGUI()
    {
        root = new VisualElement();

        VisualTreeAsset.CloneTree(root);

        return root;
    }
}
