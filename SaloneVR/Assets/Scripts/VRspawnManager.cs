using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils; 

public class VRspawnManager : MonoBehaviour
{
    public Transform spawnPoint;   // Dove vuoi che inizi il giocatore
    private XROrigin xrOrigin;

    void Start()
    {
        xrOrigin = FindObjectOfType<XROrigin>();
        if (xrOrigin != null && spawnPoint != null)
        {
            // Sposta l'intero rig allo spawn point
            xrOrigin.MoveCameraToWorldLocation(spawnPoint.position);
            xrOrigin.MatchOriginUpCameraForward(spawnPoint.up, spawnPoint.forward);
        }
    }
}
