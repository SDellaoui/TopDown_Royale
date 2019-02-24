using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameObjectSpawnableType
{
    PROJECTILE,
    ITEM
}

public class GameObjectSpawnableController : MonoBehaviour
{
    public GameObjectSpawnableType gameobjectSpawnableType;

    public GameObjectSpawnableType Get()
    {
        return gameobjectSpawnableType;
    }
}
