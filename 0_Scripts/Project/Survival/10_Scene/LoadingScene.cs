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

        // 한 프레임 쉬고 다음 씬 불러온다
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

                // 여기에 게이지 바 ㄱㄱ!
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
