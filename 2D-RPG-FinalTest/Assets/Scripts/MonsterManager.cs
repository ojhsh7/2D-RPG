using UnityEngine;
using System.Collections.Generic;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance { get; private set; }
    private List<GameObject> monsters = new List<GameObject>();

    void Awake()
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
    }

    public void AddMonster(GameObject monster)
    {
        monsters.Add(monster);
    }

    public void RemoveMonster(GameObject monster)
    {
        monsters.Remove(monster);
    }

    public int GetMonsterCount()
    {
        return monsters.Count;
    }
}
