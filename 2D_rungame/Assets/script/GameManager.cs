using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public Text LoadText;
    public Image LoadBar;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Replay(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void StartLoading()
    {
        StartCoroutine(Loading());
    }

    public IEnumerator Loading()
    {
       AsyncOperation ao = SceneManager.LoadSceneAsync("關卡1");
        ao.allowSceneActivation = false;
        while(ao.isDone==false)
        {
            LoadText.text = ao.progress/0.9f + "/ 100";
            yield return null;
            LoadBar.fillAmount = ao.progress / 0.9f;

            if(ao.progress==0.9f)
            {
                ao.allowSceneActivation = true;
            }

        }
    }

}
