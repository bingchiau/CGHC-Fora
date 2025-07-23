using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateResponder : MonoBehaviour
{
    [SerializeField] private ActivationPlate plate;       // Reference to your activation plate
    [SerializeField] private Animator plateAnimator;      // Animator that uses "Activate" bool
    [SerializeField] private GameObject[] targetObjects;  // Objects to deactivate/activate
    [SerializeField] private bool permanentDeactivation = false;

    private bool wasActivated = false;

    private void Update()
    {
        bool isActivated = plateAnimator.GetBool("Activate");

        if (isActivated && !wasActivated)
        {
            OnPlateActivated();
            wasActivated = true;
        }
        else if (!isActivated && wasActivated)
        {
            OnPlateDeactivated();
            wasActivated = false;
        }
    }

    private void OnPlateActivated()
    {
        Debug.Log("Plate activated! Deactivating target objects.");
        foreach (GameObject obj in targetObjects)
        {
            if (obj != null)
                obj.SetActive(false);
        }
    }

    private void OnPlateDeactivated()
    {
        Debug.Log("Plate deactivated! Reactivating target objects.");
        foreach (GameObject obj in targetObjects)
        {
            if (obj != null && !permanentDeactivation)
            {
                obj.SetActive(true);
            }   
        }
    }
}
