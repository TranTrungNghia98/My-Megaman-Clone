using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUIHandle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
    Application.Quit();
#endif
        }

        else if (Input.anyKey)
        {
            SceneManager.LoadScene("Main Scene");
        }
    }
}
