using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class SliderManager : MonoBehaviour
{
    [SerializeField] Slider AnswerSlider;
    public int SurveyNumber = 1; // �ϳ��� ���� �ȿ��� ���� ���� ���� �ٲٸ� ���� �ʿ�
    public static bool isActive = false;
    int y_threshold = 0;
    float x_threshold = 0;
    float z_threshold = 0;
    public static bool SaveTrigger = false;
    private string CSVDirectoryName = "Survey";
    private string[] CSVHeaders = new string[2] { "Number", "Answer" };
    public string CSVFileName;
    private string CSVSeparator = ",";
    public static int SurveyCountNumber = 1; // ������ ���� �޶����� ���� �ʿ�
    public static bool SurveyFinish = false;
    public static bool FinalEnd = false;
    public GameObject LastVideoEnd;
    public GameObject VideoPlayer;
    public static string SceneFullName = null;
    public static string[] Questions =
    {
        "���� ���󺸴� �ڽ��� ��ġ ���� �ӿ� �־��� �� ���� ������ �����.",
        "���� ���󺸴� ������ ��û�ϴ� ���� �ٸ� ������ ������ ��ó�� ��������.",
        "���� ���󺸴� ������ ��û�ϴ� ���� ������ ����� �����̴� ���� ������ �����.",
        "���� ���󺸴� ������ ����Ǵ� ���� �ð��� ���� ������ �� ����.",
        "���� ���󺸴� ������ ��û�ϴ� ���� �ڽ��� ���� ��, ��� ��������.",
        "���� ���󺸴� ���� �� ȭ���� �����ϰ� �ִٰ� ��������.",
        "���� ���󺸴� ������ ��û�ϴ� ���� ȭ���� �����ϴٰ� ��������.",
        "���� ���󺸴� ������ ��û�ϴ� ���� �ػ󵵰� ���ٰ� ������.",
        "���� ���󺸴� ������ ������ �� �����ϰ� ��������.",
        "���� ���󺸴� ȭ�� �ӿ��� �����̴� �ι� �� �繰�� �� �ֺ��� �������� ��ó�� ��������.",
        "���� ���󺸴� ȭ�� �ӿ��� �����̴� �ι� �� �繰�� ȭ�鿡�� Ƣ��� �� ���Ҵ�."
    };


    // Start is called before the first frame update
    void Start()
    {
        isActive = true;
        SurveyFinish = false;
        FinalEnd = false;

        List<Transform> children = GetChildren(transform);
        // Start Page ����
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
            // ���� ���� ����Ű M, N ------------------------------ to do ���� ������ ȭ��ǥ�� �ٲ��� https://docs.unity3d.com/ScriptReference/KeyCode.html
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

            // ���� ���� ����Ű D, A --------------------------------- to do �� �Ʒ� ȭ��ǥ�� �ٲ��� https://docs.unity3d.com/ScriptReference/KeyCode.html
            if (Input.GetKeyDown(KeyCode.D))
            {
                z_threshold += 1;
                if (z_threshold == 1)
                {
                    if (SurveyNumber < 12) // ���� ������ ���� �ٲ���� ��.
                    {
                        children[SurveyNumber].gameObject.SetActive(true);
                        AnswerSlider = children[SurveyNumber].GetComponent<Slider>();
                        if (SurveyNumber > 0)
                        {
                            children[SurveyNumber - 1].gameObject.SetActive(false);
                        }
                        SurveyNumber++;

                    }
                    else if (SurveyNumber == 12) // ���� ������ ���� �ٲ���� ��.
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
                    if (SurveyNumber > 1 && SurveyNumber <= 12) // ���� ������ ���� �ٲ���� ��.
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
        // ���� ����Ű Enter
        if (Input.GetKeyDown(KeyCode.Return))
        {
            y_threshold += 1;
            if (y_threshold == 1)
            {
                if (SurveyCountNumber < 27)
                {
                    y_threshold = 0;
                    SaveTrigger = true;
                    Debug.Log("�̹� ������ " + SurveyCountNumber + "�� ° �Դϴ�.");
                    children[SurveyNumber - 1].gameObject.SetActive(false);
                    SurveyFinish = true;
                }
                else if (SurveyCountNumber == 27)
                {
                    SaveTrigger = true;
                    Debug.Log(SurveyCountNumber);
                    gameObject.SetActive(false);
                    FinalEnd = true;
                }
            }
        }
        // ���� ���� ����� ��
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
        string[] Data2 = new string[2];

        for (int i = 0; i < children.Count - 1; i++)
        {
            // ���� ���� ��ȣ�� ���� ���� ����
            AnswerSlider = children[i].GetComponent<Slider>();
            Data2[0] = Questions[i];
            Data2[1] = AnswerSlider.value.ToString();
            AppendToCSV(Data2);
        }

        // ���� ���� �� �����ϸ� ���� �ʿ�
        if (SurveyCountNumber < 35)
        {
            SurveyCountNumber++;
        }
    }

    string GetDirectoryPath() // ���� ���� ã��
    {
        return Application.dataPath + "/" + CSVDirectoryName;
    }

    string GetFilePath() // ���� �� ���� ã��
    {
        return GetDirectoryPath() + "/" + CSVFileName;
    }

    void VerifyDirectory() // ���� ���� Ȯ��
    {
        string dir = GetDirectoryPath();
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
    }

    public void AppendToCSV(string[] strs)
    {
        VerifyDirectory();
        using (StreamWriter sw = File.AppendText(GetFilePath()))
        {
            string finalString = "";
            for (int i = 0; i < strs.Length; i++)
            {
                if (finalString != "")
                {
                    finalString += CSVSeparator;
                }
                finalString += strs[i];

            }
            finalString += CSVSeparator;
            sw.WriteLine(finalString);
            gameObject.SetActive(false);
        }

    }
}