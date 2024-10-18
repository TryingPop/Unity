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

        
        // 한 프레임 쉬고 다음 씬 불러온다
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

        // 씬 불러오기
        AsyncOperation op = SceneManager.LoadSceneAsync(nextSceneName);

        // 씬 로딩이 완료되어도 다음씬이 바로 로드되지 않게 한다
        op.allowSceneActivation = false;

        // GC 호출 여부 확인
        bool isGC = false;

        float timer = 0f;
        while (!op.isDone)
        {

            yield return null;



            float progress = op.progress;
            if (progress < 0.9f)
            {

                // 진행도에 따른 여기에 게이지 바 증가
                slider.value = progress;
            }
            else
            {


                if (!isGC)
                {

                    // 90%에서 GC 강제 호출
                    System.GC.Collect();
                    isGC = true;
                }
                else
                {

                    // 1초간 강제 휴식?
                    timer += Time.unscaledDeltaTime;
                    // 최소 2초! 보장
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

            // 여기에 소리

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
