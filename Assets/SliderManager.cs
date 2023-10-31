using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class SliderManager : MonoBehaviour
{
    [SerializeField] Slider AnswerSlider;
    public int SurveyNumber = 1; // 하나의 설문 안에서 설문 문항 수를 바꾸면 수정 필요
    public static bool isActive = false;
    int y_threshold = 0;
    float x_threshold = 0;
    float z_threshold = 0;
    public static bool SaveTrigger = false;
    private string CSVDirectoryName = "Survey";
    private string[] CSVHeaders = new string[2] { "Number", "Answer" };
    public string CSVFileName;
    private string CSVSeparator = ",";
    public static int SurveyCountNumber = 1; // 설문지 수가 달라지면 수정 필요
    public static bool SurveyFinish = false;
    public static bool FinalEnd = false;
    public GameObject LastVideoEnd;
    public GameObject VideoPlayer;
    public static string SceneFullName = null;


    // Start is called before the first frame update
    void Start()
    {
        isActive = true;
        SurveyFinish = false;
        FinalEnd = false;

        List<Transform> children = GetChildren(transform);
        // Start Page 실행
        children[0].gameObject.SetActive(true);

        // AnswerSlider.value = 4;
        SceneFullName = SceneManager.GetActiveScene().name;

    }

    // Update is called once per frame
    void Update()
    {
        List<Transform> children = GetChildren(transform);
        if (isActive)
        {
            // 설문 점수 조정키 M, N ------------------------------ to do 왼쪽 오른쪽 화살표로 바꾸자 https://docs.unity3d.com/ScriptReference/KeyCode.html
            if (Input.GetKeyDown(KeyCode.M))
            {
                x_threshold += 1;
                if(x_threshold == 1)
                {
                    if (AnswerSlider.value >= 7)
                    {
                        AnswerSlider.value = 7;
                    }
                    else
                    {
                        AnswerSlider.value += 1;
                    }
                    x_threshold = 0;
                }

            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                x_threshold -= 1;
                if (x_threshold == -1)
                {
                    if (AnswerSlider.value <= 1)
                    {
                        AnswerSlider.value = 1;

                    }
                    else
                    {
                        AnswerSlider.value -= 1;
                    }
                    x_threshold = 0;
                }
            }

            // 설문 문항 조정키 D, A --------------------------------- to do 위 아래 화살표로 바꾸자 https://docs.unity3d.com/ScriptReference/KeyCode.html
            if (Input.GetKeyDown(KeyCode.D))
            {
                z_threshold += 1;
                if (z_threshold == 1)
                {
                    if (SurveyNumber < 12) // 문항 갯수에 따라서 바꿔줘야 함.
                    {
                        children[SurveyNumber].gameObject.SetActive(true);
                        AnswerSlider = children[SurveyNumber].GetComponent<Slider>();
                        if (SurveyNumber > 0)
                        {
                            children[SurveyNumber - 1].gameObject.SetActive(false);
                        }
                        SurveyNumber++;

                    }
                    else if (SurveyNumber == 12) // 문항 갯수에 따라서 바꿔줘야 함.
                    {
                        AnswerSlider = children[SurveyNumber - 1].GetComponent<Slider>();
                        children[SurveyNumber - 2].gameObject.SetActive(false);
                        children[SurveyNumber - 1].gameObject.SetActive(true);
                    }
                    z_threshold = 0;
                }
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                z_threshold -= 1;
                if (z_threshold == -1)
                {
                    if (SurveyNumber > 1 && SurveyNumber <= 12) // 문항 개수에 따라 바꿔줘야 함.
                    {
                        SurveyNumber--;
                        children[SurveyNumber].gameObject.SetActive(false);
                        children[SurveyNumber - 1].gameObject.SetActive(true);
                        AnswerSlider = children[SurveyNumber - 1].GetComponent<Slider>();

                    }
                    else if (SurveyNumber == 1)
                    {
                        children[SurveyNumber - 1].gameObject.SetActive(false);
                        children[SurveyNumber].gameObject.SetActive(true);
                        AnswerSlider = children[SurveyNumber].GetComponent<Slider>();

                    }
                    z_threshold = 0;
                }
            }

        }
        // 설문 종료키 Enter
        if (Input.GetKeyDown(KeyCode.Return))
        {
            y_threshold += 1;
            if (y_threshold == 1)
            {
                if(SurveyCountNumber < 35)
                {
                    y_threshold = 0;
                    SaveTrigger = true;
                    Debug.Log("이번 설문은 " + SurveyCountNumber + "번 째 입니다.");
                    children[SurveyNumber - 1].gameObject.SetActive(false);
                    SurveyFinish = true;
                }
                else if (SurveyCountNumber == 35)
                {
                    SaveTrigger = true;
                    Debug.Log(SurveyCountNumber);
                    gameObject.SetActive(false);
                    FinalEnd = true;
                }
            }
        }
        // 설문 점수 저장될 때
        if (SaveTrigger)
        {
            SaveToCSV();
            if (FinalEnd == true)
            {
                VideoPlayer.SetActive(false);
                LastVideoEnd.SetActive(true);
            }
        }
    }

    List<Transform> GetChildren(Transform parent)
    {
        List<Transform> children = new List<Transform>();

        foreach (Transform child in parent)
        {
            children.Add(child);
        }

        return children;
    }

    public void SaveToCSV()
    {
        CSVFileName = "Result" + SceneFullName + ".csv";
        List<Transform> children = GetChildren(transform);
        float[] Data = new float[2];

        for (int i = 0; i < children.Count; i++)
        {
            // 설문 문항 번호와 설문 점수 저장
            AnswerSlider = children[i].GetComponent<Slider>();
            Data[0] = i + 1; // String으로 바꿔보기 -> 설문 문항으로.
            Data[1] = AnswerSlider.value;
            AppendToCSV(Data);
        }

        // 설문 문항 수 증가하면 수정 필요
        if (SurveyCountNumber < 35)
        {
            SurveyCountNumber++;
        }
    }

    string GetDirectoryPath() // 파일 폴더 찾기
    {
        return Application.dataPath + "/" + CSVDirectoryName;
    }

    string GetFilePath() // 폴더 내 파일 찾기
    {
        return GetDirectoryPath() + "/" + CSVFileName;
    }

    void VerifyDirectory() // 폴더 생성 확인
    {
        string dir = GetDirectoryPath();
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
    }

    public void AppendToCSV(float[] floats)
    {
        VerifyDirectory();
        using(StreamWriter sw = File.AppendText(GetFilePath()))
        {
            string finalString = "";
            for (int i = 0; i < floats.Length; i++)
            {
                if (finalString != "")
                {
                    finalString += CSVSeparator;
                }
                finalString += floats[i];

            }
            finalString += CSVSeparator;
            sw.WriteLine(finalString);
            gameObject.SetActive(false);
        }

    }
}
