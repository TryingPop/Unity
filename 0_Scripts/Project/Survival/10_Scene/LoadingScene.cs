using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{

    [SerializeField] private string[] tips;
    [SerializeField] private Text tipText;
    [SerializeField] private Slider slider;
    [SerializeField] private int eventIdx;
    [SerializeField] private string eventTip;

    public static string nextSceneName;
    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);
    private void Start()
    {

        
        // �� ������ ���� ���� �� �ҷ��´�
        StartCoroutine(NextScene());
        StartCoroutine(SetTip());
    }

    public static void NextScene(string _nextSceneName)
    {

        nextSceneName = _nextSceneName;
        SceneManager.LoadScene("1_Loading");
    }



    private IEnumerator NextScene()
    {

        yield return null;

        // �� �ҷ�����
        AsyncOperation op = SceneManager.LoadSceneAsync(nextSceneName);

        // �� �ε��� �Ϸ�Ǿ �������� �ٷ� �ε���� �ʰ� �Ѵ�
        op.allowSceneActivation = false;

        // GC ȣ�� ���� Ȯ��
        bool isGC = false;

        float timer = 0f;
        while (!op.isDone)
        {

            yield return null;



            float progress = op.progress;
            if (progress < 0.9f)
            {

                // ���൵�� ���� ���⿡ ������ �� ����
                slider.value = progress;
            }
            else
            {


                if (!isGC)
                {

                    // 90%���� GC ���� ȣ��
                    System.GC.Collect();
                    isGC = true;
                }
                else
                {

                    // 1�ʰ� ���� �޽�?
                    timer += Time.unscaledDeltaTime;
                    // �ּ� 2��! ����
                    slider.value = Mathf.Lerp(0.9f, 1f, timer * 0.5f);
                    if (slider.value >= 1f)
                    {

                        op.allowSceneActivation = true;
                        yield break;
                    }
                }
            }
        }
    }

    private IEnumerator SetTip()
    {

        yield return null;

        int rand = Random.Range(0, tips.Length);
        bool chkEvent = false;
        if (rand == eventIdx) chkEvent = true;
        string str = tips[rand];

        for (int i = 0; i < str.Length; i++)
        {

            tipText.text += str[i];
            yield return waitTime;
        }

        if (chkEvent)
        {

            // ���⿡ �Ҹ�

            yield return new WaitForSeconds(0.75f);
            str = eventTip;
            tipText.text = "";

            for (int i = 0; i < str.Length; i++)
            {

                tipText.text += str[i];
                yield return waitTime;
            }
        }
    }
}
