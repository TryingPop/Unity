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

    public static string nextSceneName;

    private void Start()
    {

        tipText.text = tips[Random.Range(0, tips.Length)];

        // �� ������ ���� ���� �� �ҷ��´�
        StartCoroutine(NextScene());
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

                // ���⿡ ������ �� ����!
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
                    slider.value = Mathf.Lerp(0.9f, 1f, timer);
                    if (slider.value >= 1f)
                    {

                        op.allowSceneActivation = true;
                        yield break;
                    }
                }
            }
        }
    }
}
