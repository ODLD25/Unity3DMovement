using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(WallRunScript))]
public class EditorWallRunScript : Editor
{
    public VisualTreeAsset VisualTreeAsset;
    private VisualElement root;
    private WallRunScript wallRunScript;

    [Header("Exit wallrun")]
    private VisualElement exitWallRun;

    [Header("Gravity")]
    private VisualElement gravity;
    private VisualElement gravityCounterForce;

    [Header("Camera")]
    private VisualElement wallRunFoV;
    private VisualElement targetCamera;

    [Header("Detection")]
    private VisualElement detection;

    private bool advancedInspectorBool = false;
    public override VisualElement CreateInspectorGUI()
    {
        root = new VisualElement();

        VisualTreeAsset.CloneTree(root);

        root.Q<Button>("SimpleButton").clicked += SimpleBtnClick;
        root.Q<Button>("AdvancedButton").clicked += AdvancedBtnClick;
        root.Q<Toggle>("UseGravityToggle").RegisterValueChangedCallback(valueChange =>
        {
            LoadInspector();
        });
        root.Q<Toggle>("ForceFoVToggle").RegisterValueChangedCallback(valueChange =>
        {
            LoadInspector();
        });

        LoadVariables();

        LoadInspector();

        return root;
    }

    private void LoadVariables()
    {
        wallRunScript = (WallRunScript)target;

        exitWallRun = root.Q<VisualElement>("ExitingWallRunFoldout");

        gravity = root.Q<VisualElement>("GravityFoldout");
        gravityCounterForce = root.Q<VisualElement>("GravityCounterForceFloatField");
        targetCamera = root.Q<VisualElement>("TargetCameraField");

        wallRunFoV = root.Q<VisualElement>("WallRunFoVFloatField");

        detection = root.Q<VisualElement>("DetectionFoldout");
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
            detection.style.display = DisplayStyle.Flex;

            exitWallRun.style.display = DisplayStyle.Flex;

            gravity.style.display = DisplayStyle.Flex;

            if (wallRunScript.useGravity)
            {
                gravityCounterForce.style.display = DisplayStyle.Flex;
            }
            else
            {
                gravityCounterForce.style.display = DisplayStyle.None;
            }
        }
        else
        {
            detection.style.display = DisplayStyle.None;

            exitWallRun.style.display = DisplayStyle.None;

            gravity.style.display = DisplayStyle.None;
        }

        if (wallRunScript.forceFoV)
        {
            wallRunFoV.style.display = DisplayStyle.Flex;
            targetCamera.style.display = DisplayStyle.Flex;
        }
        else
        {
            wallRunFoV.style.display = DisplayStyle.None;
            targetCamera.style.display = DisplayStyle.None;
        }
    }
}
