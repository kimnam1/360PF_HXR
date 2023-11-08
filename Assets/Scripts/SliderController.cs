using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;


public class SliderController : MonoBehaviour
{
    [SerializeField] Slider AnswerSlider;
    public int SurveyNumber = 1; //하나의 설문 안에서 설문 문항 수를 바꾸면 수정 필요
    public static bool Activation = false;
    int y_threshold = 0;
    float x_threshold = 0;
    float z_threshold = 0;
    public static bool SaveTrigger = false;
    private string csvDirectoryName = "Survey";
    private string[] csvHeaders = new string[2] { "Number", "Answer" };
    public string csvFileName;
    private string csvSeparator = ",";
    public static int SurveyCountNumber = 1; //설문지 수가 달라지면 수정 필요
    public static bool SurveyFinish = false;
    public static bool FinalEnd = false;
    public GameObject LastVideoEnd;
    public GameObject VideoPlayer;
    public static string SceneFullName = null;

    //Start is called before the first frame update
    public void Start()
    {
        Activation = true;
        SurveyFinish = false;
        FinalEnd = false;

        List<Transform> children = GetChildren(transform);

        children[0].gameObject.SetActive(true);

        AnswerSlider.value = 4;
        SceneFullName = SceneManager.GetActiveScene().name;
    }

    //Update is called once per frame
    public void Update()
    {
        List<Transform> children = GetChildren(transform);
        if (Activation)
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                x_threshold += 1;
                if (x_threshold == 1)
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
            if (Input.GetKeyDown(KeyCode.D))
            {
                z_threshold += 1;
                if (z_threshold == 1)
                {
                    if (SurveyNumber < 12) //문항 갯수에 따라 바꿔줘야 합니다.
                    {
                        children[SurveyNumber].gameObject.SetActive(true);
                        AnswerSlider = children[SurveyNumber].GetComponent<Slider>();
                        if (SurveyNumber > 0)
                        {
                            children[SurveyNumber - 1].gameObject.SetActive(false);
                        }
                        SurveyNumber++;
                    }
                    else if (SurveyNumber == 12) //문항 갯수에 따라 바꿔줘야 합니다.
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
                    if (SurveyNumber > 1 && SurveyNumber <= 12) //문항 갯수에 따라 바꿔줘야 합니다.
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
            if (Input.GetKeyDown(KeyCode.Return))
            {
                y_threshold += 1;
                if (y_threshold == 1)
                {
                    if (SurveyCountNumber < 7)
                    {
                        y_threshold = 0;
                        SaveTrigger = true;
                        Debug.Log("이번 설문은 " + SurveyCountNumber + " 번 째 입니다.");
                        children[SurveyNumber - 1].gameObject.SetActive(false);
                        SurveyFinish = true;
                    }
                    else if (SurveyCountNumber == 7)
                    {
                        SaveTrigger = true;
                        Debug.Log(SurveyCountNumber);
                        gameObject.SetActive(false);
                        FinalEnd = true;
                    }
                }
            }
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
        csvFileName = "Result" + SceneFullName + ".csv";
        List<Transform> children = GetChildren(transform);
        float[] Data = new float[2];
        for (int i = 0; i < children.Count; i++)
        {
            AnswerSlider = children[i].GetComponent<Slider>();
            Data[0] = i + 1; //String으로 바꿔보기 -> 설문 문항으로
            Data[1] = AnswerSlider.value;
            AppendToCsv(Data);
        }
        //설문 문항 수 증가하면 수정 필요
        if (SurveyCountNumber < 7)
        {
            SurveyCountNumber++;
        }
    }
    string GetDirectoryPath()
    {
        return Application.dataPath + "/" + csvDirectoryName;
    }

    string GetFilePath()
    {
        return GetDirectoryPath() + "/" + csvFileName;
    }

    void VerifyDirectory()
    {
        string dir = GetDirectoryPath();
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
    }

    /*void VerifyFile()
    {
        string file = GetFilePath();
        if (!File.Exists(file))
        {
            CreateCsv();
        }
    }

    public void CreateCsv()
    {
        VerifyDirectory();
        using (StreamWriter sw = File.CreateText(GetFilePath()))
        {
            string finalString = "";
            for (int i = 0; i < csvHeaders.Length; i++)
            {
                if (finalString != "")
                {
                    finalString += csvSeparator;
                }
                finalString += csvHeaders[i];
            }
            finalString += csvSeparator;
            sw.WriteLine(finalString);
        }
    }*/

    public void AppendToCsv(float[] floats)
    {
        VerifyDirectory();
        //VerifyFile();
        using (StreamWriter sw = File.AppendText(GetFilePath()))
        {
            string finalString = "";
            for (int i = 0; i < floats.Length; i++)
            {
                if (finalString != "")
                {
                    finalString += csvSeparator;
                }
                finalString += floats[i];
            }
            finalString += csvSeparator;          
            sw.WriteLine(finalString);
            gameObject.SetActive(false);
        }
    }
}
