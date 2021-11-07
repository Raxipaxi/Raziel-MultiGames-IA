using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDoor : MonoBehaviour,IInteractable
{
    [SerializeField] private SceneManagement.Scenes sceneToLoad;

    public bool OnInteract()
    {
       GameManager.Instance._sceneManagement.MakeChangeScene(sceneToLoad);
       return true;
    }

    public bool OnTriggerContinueInteract()
    {
        return true;
    }

    public Canvas InteractCanvas { get; }
}
