using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Move2NextScene : MonoBehaviour
{
    private string SceneFullName = null;
    public int SceneNumber = 0;
    public Button quitButton;
    private void Start()
    {
        SceneFullName = SceneManager.GetActiveScene().name;
        SceneNumber = LastNumberFromSceneName(SceneFullName) + 1;
        SliderController.SurveyFinish = false;
        SliderController.FinalEnd = false;
        SliderController.SaveTrigger = false;
        SliderController.SurveyCountNumber = 1;
        VideoPlayerController.PlayAccept = false;
        if (SceneNumber < 6) //������ ���� ���� 5��
        {
            quitButton.onClick.AddListener(NextScene);
        }
        else
        {
            quitButton.onClick.AddListener(Quit);
        }
    }
    private int LastNumberFromSceneName(string SceneName) //Scene�̸� �ҷ�����
    {
        string[] parts = SceneName.Split('_');

        string LastPart = parts[parts.Length - 1];

        int LastNumber = int.Parse(LastPart);

        return LastNumber;
    }
    private void NextScene() //Scene�̸����� ���� ������ Scene ����
    {
        SceneManager.LoadScene("VideoScene_" + SceneNumber);
    }
    private void Quit()
    {
        Application.Quit();
    }
}