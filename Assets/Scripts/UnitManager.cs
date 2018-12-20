using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour {

    public static UnitManager Instance { get; private set; }

    public enum UnitContent
    {
        empty,
        bomb,
        box,
        newBox,
        item
    }
    
    public List<GameObject> units = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        for (int i = 0; i < transform.GetChildCount(); i++)
        {
            units.Add(transform.GetChild(i).gameObject);
        }
    }

    public void ResetUnits()
    {
        foreach (GameObject unit in units)
        {
            unit.GetComponent<UnitBehave>().Empty();
        }
    }

    public void ResetUnit(Vector2 pos)
    {
        var result = AccesUnit(pos);
        if (result != null)
        {
            result.Empty();
        }
        else Debug.Log("Wrong position");
    }

    private UnitBehave AccesUnit(Vector2 pos)
    {
        Vector3 v3pos = pos;

        foreach (GameObject unit in units)
        {
            if (unit.transform.position == v3pos)
            {
                return unit.GetComponent<UnitBehave>();
            }            
        }
        return null;
    }

    public bool CheckUnit(Vector2 pos)
    {
        var result = AccesUnit(pos);
        if (result.content == UnitContent.empty)
        {
            return true;
        }
        else return false;
    }

    public bool ChangeUnitState(Vector2 pos, GameObject contentObject, UnitContent content , GameObject player)
    {
        var targetUnit = AccesUnit(pos);

        if (targetUnit != null)
        {
            targetUnit.content = content;
            targetUnit.contentObject = contentObject;
            targetUnit.player = player;
            return true;
        }
        else return false;
    }

    public bool IsEmpty(Vector2 pos)
    {
        var targetUnit = AccesUnit(pos);
        if (targetUnit.content == UnitContent.empty)
        {
            return true;
        }
        else return false;
    }

    public Vector2 GetRandomUnitPos()
    {
        int unit =  Random.Range(0,units.Count);
        return units[unit].transform.position;
    }
}
