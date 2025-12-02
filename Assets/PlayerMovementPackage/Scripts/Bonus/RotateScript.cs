using UnityEngine;

public class RotateScript : MonoBehaviour
{
    [SerializeField] private Vector3 rotateRate;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotateRate * Time.deltaTime);
    }
}
