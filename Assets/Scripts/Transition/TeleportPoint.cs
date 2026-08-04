using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour, Iinteractable
{
    public SceneLoadEventSO sceneLoadEventSO;
    public GameScenceSO SceneToGo;
    public Vector3 positionToGo;
    public void TriggerAction()
    {
        Debug.Log("´«ËÍ");
        sceneLoadEventSO.RaiseLoadRequsetEvent(SceneToGo, positionToGo, true);
    }
}
