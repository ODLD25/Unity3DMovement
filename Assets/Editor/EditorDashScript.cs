using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(DashScript))]
public class EditorDashScript : Editor
{
    public VisualTreeAsset VisualTreeAsset;
    private VisualElement root;
    private DashScript dashScript;

    [Header("Dash")]
    private VisualElement dashDuration;

    [Header("Canvas")]
    private VisualElement dashCrosshair;
    private VisualElement dashCrosshairSize;
    private VisualElement dashCanvas;

    [Header("Camera")]
    private VisualElement targetCamera;
    private VisualElement dashFov;

    [Header("Settings")]
    private VisualElement settings;
    
    private bool advancedInspectorBool = false;
    public override VisualElement CreateInspectorGUI()
    {
        root = new VisualElement();

        VisualTreeAsset.CloneTree(root);

        root.Q<Button>("SimpleButton").clicked += SimpleBtnClick;
        root.Q<Button>("AdvancedButton").clicked += AdvancedBtnClick;

        root.Q<Toggle>("ForceFoVToggle").RegisterValueChangedCallback(valueChanged =>
        {
            LoadInspector();
        });
        root.Q<Toggle>("ShowDashCrosshairToggle").RegisterValueChangedCallback(valueChanged =>
        {
            LoadInspector();
        });

        LoadVariables();

        LoadInspector();

        return root;
    }

    private void LoadVariables()
    {
        dashDuration = root.Q<VisualElement>("DashDurationFloatField");

        dashCrosshair = root.Q<VisualElement>("DashCrosshairImageField");
        dashCrosshairSize = root.Q<VisualElement>("DashCrosshairSize");
        dashCanvas = root.Q<VisualElement>("DashCanvasGameObjectField");
        
        targetCamera = root.Q<VisualElement>("CameraField");
        dashFov = root.Q<VisualElement>("DashFoVFloatField");

        settings = root.Q<VisualElement>("Settings");

        dashScript = (DashScript)target;
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

    private void LoadInspector()
    {
        if (advancedInspectorBool)
        {
            dashDuration.style.display = DisplayStyle.Flex;

            settings.style.display = DisplayStyle.Flex;
        }
        else
        {
            dashDuration.style.display = DisplayStyle.None;

            settings.style.display = DisplayStyle.None;
        }

        if (dashScript.showDashCrosshair)
        {
            dashCrosshair.style.display = DisplayStyle.Flex;
            dashCrosshairSize.style.display = DisplayStyle.Flex;
            dashCanvas.style.display = DisplayStyle.Flex;
        }
        else
        {
            dashCrosshair.style.display = DisplayStyle.None;
            dashCrosshairSize.style.display = DisplayStyle.None;
            dashCanvas.style.display = DisplayStyle.None;
        }

        if (dashScript.forceFoV)
        {
            targetCamera.style.display = DisplayStyle.Flex;
            dashFov.style.display = DisplayStyle.Flex;
        }
        else
        {
            targetCamera.style.display = DisplayStyle.None;
            dashFov.style.display = DisplayStyle.None;
        }
    }
}
