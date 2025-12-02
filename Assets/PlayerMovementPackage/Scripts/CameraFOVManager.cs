using System.Collections;
using UnityEngine;

public class CameraFOVManager : MonoBehaviour
{
    [Tooltip("Normal Field of View.")] private static float normalFov = 60f;

    public static IEnumerator ChangeFoV(float duration, float targetFoV)
    {
        //Create variables
        float timer = 0f;
        float t;
        float startFoV = Camera.main.fieldOfView;

        //While loop to smoothly transition between the FoV
        while (timer < duration)
        {
            t = timer / duration;

            Camera.main.fieldOfView = Mathf.Lerp(startFoV, targetFoV, t);

            timer += Time.deltaTime;

            yield return null;
        }
    }

    public static IEnumerator ChangeFoV(float duration, float targetFoV, Camera targetCamera)
    {
        //Create variables
        float timer = 0f;
        float t;
        float startFoV = Camera.main.fieldOfView;

        //While loop to smoothly transition between the FoV
        while (timer < duration)
        {
            t = timer / duration;

            targetCamera.fieldOfView = Mathf.Lerp(startFoV, targetFoV, t);

            timer += Time.deltaTime;

            yield return null;
        }
    }

    public static IEnumerator ResetFoV(float duration, Camera targetCamera)
    {
        //Create variables
        float timer = 0f;
        float t;
        float startFoV = Camera.main.fieldOfView;

        //While loop to smoothly transition between the FoV
        while (timer < duration)
        {
            t = timer / duration;

            targetCamera.fieldOfView = Mathf.Lerp(startFoV, normalFov, t);

            timer += Time.deltaTime;

            yield return null;
        }
    }

    public static IEnumerator ResetFoV(float duration)
    {
        //Create variables
        float timer = 0f;
        float t;
        float startFoV = Camera.main.fieldOfView;

        //While loop to smoothly transition between the FoV
        while (timer < duration)
        {
            t = timer / duration;

            Camera.main.fieldOfView = Mathf.Lerp(startFoV, normalFov, t);

            timer += Time.deltaTime;

            yield return null;
        }
    }

    public static void SetFoV(float FoV)
    {
        Camera.main.fieldOfView = FoV;
    }
    
    public static void SetFoV(float FoV, Camera targetCamera)
    {
        targetCamera.fieldOfView = FoV;
    }
}
