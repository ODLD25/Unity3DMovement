/*
TODO:
    FIX:
        TryGetComponent with error handling instead of GetComponent in start method
        ForceFoV colliding with camera controller
        Camera tilt with wall running

    DASH:
        Change dash values

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
    
    GROUND TYPES:

    INSPECTOR:
        Option to only recharge dash on the ground

    OPTIMIZE:
        sqrtMagnitude instead of magnitude

    DONE:
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

        Spherecast for ground check - doesnt work good
        Cant use them to push people and launch them - instead of doing this there will be added pistons that will do the same thing

        OTHER PACKAGES:
            Targets
    
    TEST:
        CameraRotation at different framerates
        Pistons

        Custom Inspector

        OTHER PACKAGES:
            Knife throw
            Explosives knifes

Maybe?:
    Breakable wall that breaks when: you run into it at a certain speed or from an explosion
    Optimize brekable wall with object pooling

Other Package:
    Grenade throw
    JumpPad
    Door

Enemy Package:
    Enemy script
    Follow Player script
    Different Attacks
*/