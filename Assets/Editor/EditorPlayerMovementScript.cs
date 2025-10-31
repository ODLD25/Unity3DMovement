using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(PlayerMovementScript))]
public class EditorPlayerMovementScript : Editor
{
    public VisualTreeAsset VisualTreeAsset;
    private VisualElement root;
    private GameObject player;

    [Header("Has movement type")]
    private bool canCrouch;
    private bool canSprint;
    private bool canSlide;
    private bool canDash;
    private bool canWallRun;
    private bool canJump;
    private bool iceDetection;

    [Header("Speed")]
    private VisualElement sprintSpeed;
    private VisualElement crouchSpeed;
    private VisualElement dashSpeed;
    private VisualElement slideSpeed;
    private VisualElement wallRunSpeed;

    [Header("Control")]
    private VisualElement control;
    private VisualElement slideControl;
    private VisualElement iceControl;

    [Header("Drag")]
    private VisualElement drag;
    private VisualElement slideDrag;
    private VisualElement dashDrag;
    private VisualElement wallRunDrag;

    [Header("GroundCheck")]
    private VisualElement extraRaycastParent;
    private VisualElement extraGroundCheckInterval;

    [Header("Slope Handling")]
    private VisualElement slopeHandling;

    private bool advancedInspectorBool = false;
    public override VisualElement CreateInspectorGUI()
    {
        root = new VisualElement();

        VisualTreeAsset.CloneTree(root);

        root.Q<Button>("SimpleButton").clicked += SimpleBtnClick;
        root.Q<Button>("AdvancedButton").clicked += AdvancedBtnClick;

        LoadVariables();
        CheckScripts();

        LoadInspector();

        return root;
    }

    private void LoadVariables()
    {
        dashSpeed = root.Q<VisualElement>("DashSpeedFloatInput");
        slideSpeed = root.Q<VisualElement>("SlideSpeedFloatInput");
        wallRunSpeed = root.Q<VisualElement>("WallRunSpeedFloatInput");
        crouchSpeed = root.Q<VisualElement>("CrouchSpeedFloatInput");
        sprintSpeed = root.Q<VisualElement>("SprintSpeedFloatInput");

        control = root.Q<VisualElement>("ControlFoldout");
        slideControl = root.Q<VisualElement>("SlideControlSlider");
        iceControl = root.Q<VisualElement>("IceControlSlider");

        drag = root.Q<VisualElement>("Drag");
        slideDrag = root.Q<VisualElement>("SlideDrag");
        dashDrag = root.Q<VisualElement>("DashDrag");
        wallRunDrag = root.Q<VisualElement>("WallRunDrag");

        extraRaycastParent = root.Q<VisualElement>("ExtraRaycastParentTransformField");
        extraGroundCheckInterval = root.Q<VisualElement>("ExtraGroundCheckIntervalIntField");

        slopeHandling = root.Q<VisualElement>("SlopeHandling");

        PlayerMovementScript playerMovementScript = (PlayerMovementScript)target;
        player = playerMovementScript.gameObject;
    }

    private void SimpleBtnClick()
    {
        advancedInspectorBool = false;
        CheckScripts();
        LoadInspector();
    }

    private void AdvancedBtnClick()
    {
        advancedInspectorBool = true;
        CheckScripts();
        LoadInspector();
    }

    private void LoadInspector()
    {
        if (advancedInspectorBool)
        {
            control.style.display = DisplayStyle.Flex;
            drag.style.display = DisplayStyle.Flex;

            extraRaycastParent.style.display = DisplayStyle.Flex;
            extraGroundCheckInterval.style.display = DisplayStyle.Flex;

            slopeHandling.style.display = DisplayStyle.Flex;

            if (canSprint)
            {
                sprintSpeed.style.display = DisplayStyle.Flex;
            }
            else
            {
                sprintSpeed.style.display = DisplayStyle.None;
            }

            if (canJump)
            {

            }

            if (canDash)
            {
                dashSpeed.style.display = DisplayStyle.Flex;
                dashDrag.style.display = DisplayStyle.Flex;
            }
            else
            {
                dashSpeed.style.display = DisplayStyle.None;
                dashDrag.style.display = DisplayStyle.None;
            }

            if (canSlide)
            {
                slideSpeed.style.display = DisplayStyle.Flex;
                slideDrag.style.display = DisplayStyle.Flex;
                slideControl.style.display = DisplayStyle.Flex;
            }
            else
            {
                slideSpeed.style.display = DisplayStyle.None;
                slideDrag.style.display = DisplayStyle.None;
                slideControl.style.display = DisplayStyle.None;
            }

            if (canCrouch)
            {
                crouchSpeed.style.display = DisplayStyle.Flex;
            }
            else
            {
                crouchSpeed.style.display = DisplayStyle.None;
            }

            if (iceDetection)
            {
                iceControl.style.display = DisplayStyle.Flex;
            }
            else
            {
                iceControl.style.display = DisplayStyle.None;
            }

            if (canWallRun)
            {
                wallRunDrag.style.display = DisplayStyle.Flex;
                wallRunSpeed.style.display = DisplayStyle.Flex;
            }
            else
            {
                wallRunDrag.style.display = DisplayStyle.None;
                wallRunSpeed.style.display = DisplayStyle.None;
            }
        }
        else
        {
            control.style.display = DisplayStyle.None;
            drag.style.display = DisplayStyle.None;

            extraRaycastParent.style.display = DisplayStyle.None;
            extraGroundCheckInterval.style.display = DisplayStyle.None;

            slopeHandling.style.display = DisplayStyle.None;

            if (canSprint)
            {
                sprintSpeed.style.display = DisplayStyle.Flex;
            }
            else
            {
                sprintSpeed.style.display = DisplayStyle.None;
            }

            if (canJump)
            {

            }

            if (canDash)
            {
                dashSpeed.style.display = DisplayStyle.Flex;
                dashDrag.style.display = DisplayStyle.None;
            }
            else
            {
                dashSpeed.style.display = DisplayStyle.None;
                dashDrag.style.display = DisplayStyle.None;
            }

            if (canSlide)
            {
                slideSpeed.style.display = DisplayStyle.Flex;
                slideDrag.style.display = DisplayStyle.None;
                slideControl.style.display = DisplayStyle.None;
            }
            else
            {
                slideSpeed.style.display = DisplayStyle.None;
                slideDrag.style.display = DisplayStyle.None;
                slideControl.style.display = DisplayStyle.None;
            }

            if (canCrouch)
            {
                crouchSpeed.style.display = DisplayStyle.Flex;
            }
            else
            {
                crouchSpeed.style.display = DisplayStyle.None;
            }

            if (iceDetection)
            {
                iceControl.style.display = DisplayStyle.None;
            }
            else
            {
                iceControl.style.display = DisplayStyle.None;

            }

            if (canWallRun)
            {
                wallRunSpeed.style.display = DisplayStyle.Flex;
                wallRunDrag.style.display = DisplayStyle.None;
            }
            else
            {
                wallRunSpeed.style.display = DisplayStyle.None;
                wallRunDrag.style.display = DisplayStyle.None;
            }
        }
    }

    private void CheckScripts()
    {
        if (player.GetComponent<SprintScript>()) canSprint = true;
        else canSprint = false;
        if (player.GetComponent<CrouchScript>()) canCrouch = true;
        else canCrouch = false;
        if (player.GetComponent<JumpScript>()) canJump = true;
        else canJump = false;
        if (player.GetComponent<DashScript>()) canDash = true;
        else canDash = false;
        if (player.GetComponent<SlidingScript>()) canSlide = true;
        else canSlide = false;
        if (player.GetComponent<WallRunScript>()) canWallRun = true;
        else canWallRun = false;
        if (player.GetComponent<IceDetection>()) iceDetection = true;
        else iceDetection = false;
    }

}