using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(SlidingScript))]
public class EditorSlidingScript : Editor
{
    public VisualTreeAsset VisualTreeAsset;
    private VisualElement root;
    private SlidingScript slidingScript;

    [Header("Start Slide")]
    private VisualElement minSpeedToStartSlide;
    private VisualElement speedToStopSlide;

    [Header("Slide Bypass")]
    private VisualElement slideBypass;

    private bool advancedInspectorBool = false;
    public override VisualElement CreateInspectorGUI()
    {
        root = new VisualElement();

        VisualTreeAsset.CloneTree(root);

        root.Q<Button>("SimpleButton").clicked += SimpleBtnClick;
        root.Q<Button>("AdvancedButton").clicked += AdvancedBtnClick;

        root.Q<EnumField>("StartSlideTypeEnumField").RegisterValueChangedCallback(enumFieldChanges =>
        {
            LoadInspector();
        });

        LoadVariables();
        LoadInspector();

        return root;
    }

    private void LoadVariables()
    {
        minSpeedToStartSlide = root.Q<VisualElement>("MinSpeedToStartSlideFloatInput");
        speedToStopSlide = root.Q<VisualElement>("SpeedToStopSlideFloatField");

        slideBypass = root.Q<VisualElement>("SlideBypassFoldout");

        slidingScript = (SlidingScript)target;
    }

    private void LoadInspector()
    {
        if (advancedInspectorBool)
        {
            slideBypass.style.display = DisplayStyle.Flex;
        }
        else
        {
            slideBypass.style.display = DisplayStyle.None;
        }

        if (slidingScript.startSlideType == StartSlideType.Input || slidingScript.startSlideType == StartSlideType.None)
        {
            minSpeedToStartSlide.style.display = DisplayStyle.None;
            speedToStopSlide.style.display = DisplayStyle.None;
        }
        else
        {
            minSpeedToStartSlide.style.display = DisplayStyle.Flex;
            speedToStopSlide.style.display = DisplayStyle.Flex;
        }
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

}
