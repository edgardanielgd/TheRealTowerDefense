using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevel : MonoBehaviour
{
    public void ChangeScene(string name)
    {
        Debug.Log("AAA");
        SceneManager.LoadScene(name);
    }

    void Start(){
        Debug.Log("AAA");
    }
}
