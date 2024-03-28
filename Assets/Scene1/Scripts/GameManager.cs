using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    int scene;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            switch (scene)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    scene++;
                    SceneManager.LoadScene(scene);
                    break;
            }
        } 
    }
}
