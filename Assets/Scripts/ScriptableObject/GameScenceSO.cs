using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "Game Scene/GameScenceSO")]
public class GameScenceSO : ScriptableObject
{
    public SceneType sceneType;
    public AssetReference sceneReference;
} 