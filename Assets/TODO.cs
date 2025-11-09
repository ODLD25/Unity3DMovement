/*
TODO:
    FIX:
        ForceFoV colliding with camera controller
        Camera tilt with wall running

        Ground mat
        Slide start default = Speed(it seems more intuitive to Palo)
        UI na slide cooldown 

    DASH:
        Change dash values
        Sprite for no dash cursor

    MOVABLE PLATFORMS:
        Fix
        With higher speed like 5 player slides of them at sudden speed change.

    MECHANICS:
        Climbing Ladders
        Stamina
        Jumping while sliding will boost you forward
        Change glass from objects to particles

        Toogle in sprint, crouch and slide(maybe)
        when holding jump button the player jumps higher

    GENERAL:
        Redo every comment
        Add more tooltips
        Change default values to correct values
        Use Namespace
        Toggle - riplenish air jumps on ground

    GROUND TYPES:

    INSPECTOR:

    OPTIMIZE:
        sqrtMagnitude instead of magnitude
        TryGetComponent with error handling instead of GetComponent in start method
        DashUpdateCanvas() move away from Update

    DONE:
        Checkpoints - toggle if other claimed checkpoints should disable on new checkpoint claimed
        Gizmos
        Color code wall run walls
        Tri planar shader
        Option to only recharge dash on the ground
        Pistons
        Slope with bigger angle than max slope angle
        Fix dash recharge
        Kunai falls off on explosion
        Ground types - Lava
        InputAction.Disable
        Byppas slide cooldown should hide settings for it
        Movable platforms with Rigidbody.MovePosition
        Change FoV values
        Jumping on moving platform
        Correct scale on slide end
        Crosshair for Dashing
        Reset Pos
        Jumping in air
        Jumping on a slope
        Dash not working when sprinting
        Ground types - Ice
        Wall Run
        FoV
        Show Dash Crosshair should hide settings for it 

        Input and speed settings colliding when stopping slide - not needed a fix, just change speedToStopSlide variable to be included in both input and speed
        Spherecast for ground check - doesnt work good
        Cant use them to push people and launch them - instead of doing this there will be added pistons that will do the same thing
        Fov higher when sprinting
        Dash rechargin wierd

        OTHER PACKAGES:
            Targets
            JumpPad
            Knife throw
            Explosives knifes
    
    TEST:
        CameraRotation at different framerates

        Custom Inspector

        OTHER PACKAGES:

Maybe?:
    On start slide only add force required to reach a certain speed 

    Breakable wall that breaks when: you run into it at a certain speed or from an explosion
    Optimize brekable wall with object pooling

Other Package:
    Grenade throw
    Door
    Throwable kinife that will create a zero gravity sphere around it.

Enemy Package:
    Enemy script
    Follow Player script
    Different Attacks
*/