using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneTransition", menuName = "Scene Persistent Data")]
public class SceneTransition : ScriptableObject
{
    public bool clearedLevel;
    public int levelIndex;

    [Header("Map Specific")]
    public Vector3 prevPosition;
    public string enemyNpc;
    public List<string> defeatedEnemyNpcs;

    public void ResetSceneData()
    {
        clearedLevel = false;
        levelIndex = 0;
        prevPosition = new Vector3(-8f, -3f, 0f);
        enemyNpc = null;
        defeatedEnemyNpcs = new List<string>();
    }
}
