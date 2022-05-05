using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCotnrol : MonoBehaviour
{
    [SerializeField]
    private GameObject ball = null;
    [SerializeField]
    private GameManager gameManager = null;
    private bool activated = false;
    private void OnTriggerEnter(Collider other)
    {
        if (gameManager != null && !activated && ball != null && GameObject.ReferenceEquals(other.transform.gameObject, ball))
        {
            activated = true;
            gameManager.CallEnd();
        }
    }
}
