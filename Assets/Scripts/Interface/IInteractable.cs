using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    bool OnInteract(); //Initial interactions. Returns if there's only 1 interaction possible

    bool OnTriggerContinueInteract();// Continued interactions

    Canvas InteractCanvas { get; }

}
