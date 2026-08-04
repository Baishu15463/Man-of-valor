using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/SceneLoadEventSO")]
public class SceneLoadEventSO : ScriptableObject
{
    public UnityAction<GameScenceSO, Vector3, bool> LoadRequestEvent;
    
    public void RaiseLoadRequsetEvent(GameScenceSO locationToLoad , Vector3 posToGo , bool fadeScreen)
    {
        LoadRequestEvent?.Invoke(locationToLoad, posToGo, fadeScreen);
    }
}