using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChanger : MonoBehaviour {
    public string nextSceneName;
    
    public void SceneChange() {
        SceneManager.LoadScene(nextSceneName);
    }
}
