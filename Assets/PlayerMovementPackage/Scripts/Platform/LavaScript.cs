using System;
using UnityEngine;

public class LavaScript : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            try
            {
                FindAnyObjectByType<ResetScript>().ResetPlayer();
            }
            catch (Exception e)
            {
                Debug.Log($"Couldn't find ResetScript. To fix this add ResetScript to player. Error: {e}");
            }
        }
    }
}
