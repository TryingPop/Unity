using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferMap : MonoBehaviour
{

    public string transferMapName;          // 이동할 맵 이름

    public Transform target;
    public BoxCollider2D targetBound;

    public bool flag = true;                // 씬변환 체크 변수
                                            // 이 변환에서 주의할 껀 재생성되면서
                                            // true로 할당되어 씬 이동 시에 적용안될 수도 있기 때문에
                                            // 따로 스크립트를 만들어야 한다
    private CameraManager theCamera;
    private PlayerManager thePlayer;

    private void Start()
    {

        if (!flag)
        {

            theCamera = FindObjectOfType<CameraManager>();
        }
        thePlayer = FindObjectOfType<PlayerManager>();       // 하이라키의 모든 객체에 대해 해당 컴포넌트를 검색해서 리턴

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.name == "Player")
        {

            thePlayer.currentMapName = transferMapName;
            

            // 초기화 주의!
            if (flag)
            {

                SceneManager.LoadScene(transferMapName);
            }
            else    
            {

                theCamera.SetBound(targetBound);
                theCamera.transform.position = new Vector3(
                    this.transform.position.x, this.transform.position.y,
                    theCamera.transform.position.z);

                thePlayer.transform.position = target.transform.position;
            }
        }
    }
}
