using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectButtonManager : MonoBehaviour
{
    private LevelLoader levelLoader;

    void Start()
    {
        levelLoader = LevelLoader.instance;
    }

    public void LoadLevel(int level)
    {
        levelLoader.LoadScene(level);
    }
}
