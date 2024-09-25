using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class AnimateOnInput : MonoBehaviour
{
    public InputActionProperty pinchProperty;
    public InputActionProperty gripProperty;
    public Animator animator;

    void Update()
    {
        float pinchValue = pinchProperty.action.ReadValue<float>();
        animator.SetFloat("Trigger", pinchValue);
        float gripValue = gripProperty.action.ReadValue<float>();
        animator.SetFloat("Grip", gripValue);
    }
}
