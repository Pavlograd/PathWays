using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject sceneLoader; // Prefab with this script and the loadingscreen

    public static void PauseGame()
    {
        Debug.Log("pause");
        Time.timeScale = 0;
    }
    public static void ResumeGame()
    {
        Debug.Log("resume");
        Time.timeScale = 1;
    }

    public void LoadScene(string sceneName) // Can be called everywhere
    {
        GameObject newObject = Instantiate(sceneLoader, Vector3.zero, Quaternion.identity);

        newObject.GetComponent<SceneLoader>().Init(sceneName);
    }
}
