using System;
using NaughtyAttributes;
using UnityEngine;

//Runs any code that is GameState related and holds commonly used methods not relating to any one system.
public class GameManager : MonoBehaviour
{
    [SerializeField] private bool debug;
        private void Log(string contents){ if (debug){ Debug.Log(contents); }}

    [Space(10)]

    [SerializeField] private GameObject parent2D;
    [SerializeField] private GameObject parent3D;

    void Awake(){ Setup(); }
    public void Setup(){  }

    [Button] public void SwitchTo2D()
    {
        parent3D.SetActive(false);
        parent2D.SetActive(true);
    }
    [Button] public void SwitchTo3D()
    {
        parent2D.SetActive(false);
        parent3D.SetActive(true);
    }
}