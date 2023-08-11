using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Interactable : MonoBehaviour
{
    public UnityEvent onEnter;
    public UnityEvent onExit;
    
    public UnityEvent onInteract;
}