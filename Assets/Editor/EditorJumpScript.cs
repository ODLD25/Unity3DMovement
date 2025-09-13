using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(JumpScript))]
public class EditorJumpScript : Editor
{
    public VisualTreeAsset VisualTreeAsset;
    private VisualElement root;
    private bool advancedInspectorBool = false;

    [Header("Settings")]
    private VisualElement settingsFoldout;


    public override VisualElement CreateInspectorGUI()
    {
        root = new VisualElement();

        VisualTreeAsset.CloneTree(root);

        root.Q<Button>("SimpleButton").clicked += SimpleBtnClick;
        root.Q<Button>("AdvancedButton").clicked += AdvancedBtnClick;

        LoadVariables();

        LoadInspector();

        return root;
    }

    private void SimpleBtnClick()
    {
        advancedInspectorBool = false;
        LoadInspector();
    }

    private void AdvancedBtnClick()
    {
        advancedInspectorBool = true;
        LoadInspector();
    }

    private void LoadVariables()
    {
        settingsFoldout = root.Q<VisualElement>("SettingsFoldout");
    }

    private void LoadInspector()
    {
        if (advancedInspectorBool)
        {
            settingsFoldout.style.display = DisplayStyle.Flex;
        }
        else
        {
            settingsFoldout.style.display = DisplayStyle.None;
        }
    }
}
