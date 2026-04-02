using System;
using NaughtyAttributes;
using UnityEngine;

//Runs any code that is GameState related and holds commonly used methods not relating to any one system.
public class GameManager : MonoBehaviour
{
    [SerializeField] private bool debug;
        private void Log(string contents){ if (debug){ Debug.Log(contents); }}

    [Space(10)]

    [SerializeField] private int var1;

    void Awake(){ Setup(); }
    public void Setup(){  }
}