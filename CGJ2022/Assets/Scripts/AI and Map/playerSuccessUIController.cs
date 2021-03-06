using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerSuccessUIController : MonoBehaviour
{
    public Transform transform_ui;


    public void onClickHome()
    {
        SceneManager.LoadScene("");//此处填写主页面的编号
    }

    public void onClickReplay()//此处应适配注册的Scene
    {
        int replayIndex = transform_ui.GetComponent<gameController>().levelIndex;
        SceneManager.LoadScene(replayIndex);
    }

    public void onClickNext()//此处应适配注册的Scene
    {
        int nextIndex = transform_ui.GetComponent<gameController>().levelIndex+1;
        SceneManager.LoadScene(nextIndex);
    }
}
