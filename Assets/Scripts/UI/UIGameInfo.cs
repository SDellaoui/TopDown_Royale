﻿using UnityEngine;
using UnityEngine.UI;

public enum UIGameInfoType
{
    Damage,
    Heal
}

public class UIGameInfo : MonoBehaviour
{
    private bool hasBeenInstantiated = false;
    public UIGameInfoType uiGameInfoType;

    private void Awake()
    {
        GetComponent<Canvas>().worldCamera = GameManager.instance.GetCurrentCamera();
    }

    private void Update()
    {
    }

    public void Init(UIGameInfoType _uiGameInfoType, string value)
    {
        string prefabToSpawn = "Prefabs/UI/Game/";
        switch(_uiGameInfoType)
        {
            case UIGameInfoType.Damage:
                prefabToSpawn += "UI_Game_Element";
                break;
            default:
                prefabToSpawn += "UI_Game_Element";
                break;
        }
        Vector3 randomPos = new Vector2(transform.position.x + Random.Range(-0.8f, 0.8f), transform.position.y + Random.Range(0f, 0.8f));
        GameObject damageGO = Instantiate(Resources.Load(prefabToSpawn), randomPos, Quaternion.identity) as GameObject;
        damageGO.transform.SetParent(transform);
        damageGO.GetComponent<UIGameElement>().Init(uiGameInfoType,value);
        Destroy(gameObject, 1.22f);
        hasBeenInstantiated = true;
    }
}
