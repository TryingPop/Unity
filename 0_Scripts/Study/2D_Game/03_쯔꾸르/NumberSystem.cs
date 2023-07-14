using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberSystem : MonoBehaviour
{

    private AudioManager theAudio;
    public string key_sound;        // ����Ű ����
    public string enter_sound;      // ����Ű ����
    public string cancel_sound;     // ��� && ����Ű ����
    public string correct_sound;    // ���� ����

    private int count;              // �迭�� ũ��, �Է� �ڸ���
    private int selectedTextBox;    // ���õ� �ڸ���
    private int result;             // �÷��̾ �����س� ��
    private int correctNumber;      // ����

    private string tempNumber;

    public GameObject superObject;  // ȭ�� ��� ������ ���� ������Ʈ
    public GameObject[] panel;
    public Text[] number_Text;

    public Animator anim;

    public bool activated;          // ��� return new WaitUntil
    private bool keyInput;          // Űó�� Ȱ��ȭ, ��Ȱ��ȭ
    private bool correctFlag;       // �������� �ƴ��� ����

    public float interval;          // ����

    private void Start()
    {

        theAudio = FindObjectOfType<AudioManager>();
    }

    public bool GetResult()
    {

        return correctFlag;
    }

    public void ShowNumber(int _correctNumber)
    {

        correctNumber = _correctNumber;
        activated = true;
        correctFlag = false;

        string temp = correctNumber.ToString();     // ���ڿ� �ε����� �´��� ����
        for (int i = 0; i < temp.Length; i++)
        {

            count = i;
            panel[i].SetActive(true);
            number_Text[i].text = "0";
        }

        superObject.transform.position = new Vector3(superObject.transform.position.x + (interval * count), 
            superObject.transform.position.y, 
            superObject.transform.position.z);

        selectedTextBox = 0;
        result = 0;
        SetColor();
        anim.SetBool("Appear", true);
        keyInput = true;
    }

    public void SetNumber(string _arrow)
    {

        int temp = int.Parse(number_Text[selectedTextBox].text);    // ���õ� �ڸ����� �ؽ�Ʈ�� Integer ���� �������� ���� �� ��ȯ

        if (_arrow == "DOWN")
        {

            if (temp == 0)
            {

                temp = 9;
            }
            else
            {

                temp--;
            }
        }
        else if (_arrow == "UP")
        {

            if (temp == 9)
            {

                temp = 0;
            }
            else
            {

                temp++;
            }
        }
        number_Text[selectedTextBox].text = temp.ToString();

    }

    public void SetColor()
    {

        Color color = number_Text[0].color;

        color.a = 0.3f;
        for (int i =0; i <= count; i++)
        {

            number_Text[i].color = color;
        }
        color.a = 1f;
        number_Text[selectedTextBox].color = color;
    }

    private void Update()
    {
        
        if (keyInput)
        {

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {

                theAudio.Play(key_sound);
                SetNumber("DOWN");
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {

                theAudio.Play(key_sound);
                SetNumber("UP");
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {

                theAudio.Play(key_sound);
                if (selectedTextBox  < count)
                {

                    selectedTextBox++;
                }
                else
                {

                    selectedTextBox = 0;
                }

                SetColor();
            }
            else if (Input.GetKeyUp(KeyCode.RightArrow))
            {

                theAudio.Play(key_sound);
                if (selectedTextBox > 0)
                { 
                    
                    selectedTextBox--; 
                }
                else
                {

                    selectedTextBox = count;
                }

                SetColor();
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {

                theAudio.Play(key_sound);
                keyInput = false;
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {

                theAudio.Play(key_sound);
                keyInput = false;
            }
        }
    }

    IEnumerator OXCoroutine()
    {

        Color color = number_Text[0].color;
        color.a = 1f;

        for (int i = count; i >= 0; i--)
        {

            number_Text[i].color = color;
            tempNumber += number_Text[i].text;
        }

        yield return new WaitForSeconds(1f);
        result = int.Parse(tempNumber);

        if (result == correctNumber)
        {

            theAudio.Play(correct_sound);
            correctFlag = true;
        }
        else
        {

            theAudio.Play(cancel_sound);
            correctFlag = false;
        }

        StartCoroutine(ExitCoroutine());
    }

    IEnumerator ExitCoroutine()
    {

        result = 0;
        tempNumber = "";
        anim.SetBool("Appear", false);

        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i <= count; i++)
        {

            panel[i].SetActive(false);
        }

        superObject.transform.position = new Vector3(superObject.transform.position.x - (interval * count),
            superObject.transform.position.y,
            superObject.transform.position.z);

        activated = false;
    }
}
