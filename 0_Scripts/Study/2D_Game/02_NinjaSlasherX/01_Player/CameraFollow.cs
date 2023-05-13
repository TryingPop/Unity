using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum CAMERATARGET    // ī�޶� ���� ��� ����
{

    PLAYER,                 // �÷��̾� ��ǥ
    PLAYER_MARGIN,          // �÷��̾� ��ǥ (���� �þ߸� Ȯ���Ѵ�. ���� ����)
    PLAYER_GROUND,          // ���ſ� �÷��̾ ������ ���� ��ǥ(���� �þ߸� Ȯ���Ѵ�. ���� ����)
}

public enum CAMERAHOMING    // ī�޶� ȣ�� ����
{

    DIRECT,                 // ī�޶� ��ǥ�� ��� ��ǥ�� ���� �����Ѵ�
    LERP,                   // ī�޶�� ��� ��ǥ�� ���� �����Ѵ�
    SLERP,                  // ī�޶�� ��� ��ǥ�� � �����Ѵ�
    STOP,                   // ī�޶� �����
}

public class CameraFollow : MonoBehaviour
{

    [System.Serializable]
    public class Param
    {

        public CAMERATARGET targetType = CAMERATARGET.PLAYER_GROUND;
        public CAMERAHOMING homingType = CAMERAHOMING.LERP;
        public Vector2 margin = new Vector2(2.0f, 2.0f);
        public Vector2 homing = new Vector2(0.1f, 0.2f);
        public bool borderCheck = false;
        public GameObject borderLeftTop;
        public GameObject borderRightBottom;
        public bool viewAreaCheck = true;
        public Vector2 viewAreaMinMargin = new Vector2(0.0f, 0.0f);
        public Vector2 viewAreaMaxMargin = new Vector2(0.0f, 2.0f);

        public bool orthographicEnabled = true;
        public float screenOGSize = 5.0f;
        public float screenOGSizeHoming = 0.1f;
        public float screenPSSize = 50.0f;
        public float screenPSSizeHoming = 0.1f;
    }

    public Param param;

    // ĳ��
    GameObject player;
    Transform playerTrfm;
    PlayerController playerCtrl;

    float screenOGSizeAdd = 0.0f;
    float screenPSSizeAdd = 0.0f;

    public new Camera camera;

    // �ڵ� (Monobehaviour �⺻ ��� ����)
    void Awake()
    {

        player = PlayerController.GetGameObject();
        playerTrfm = player.transform;
        playerCtrl = player.GetComponent<PlayerController>();

        camera = GetComponent<Camera>();
    }

    void LateUpdate()
    {

        float targetX = playerTrfm.position.x;          
        float targetY = playerTrfm.position.y;
        float pX = transform.position.x;
        float pY = transform.position.y;
        float screenOGSize = camera.orthographicSize;
        float screenPSSize = camera.fieldOfView;

        // ��� ����
        switch (param.targetType)
        {

            case CAMERATARGET.PLAYER:
                // targetX = playerTrfm.position.x;
                // targetY = playerTrfm.position.y;
                break;
            case CAMERATARGET.PLAYER_MARGIN:    // ���� �������� �����ڸ� ����ŭ x, y�� ���Ѵ�
                targetX = playerTrfm.position.x +
                    param.margin.x * playerCtrl.dir;
                targetY = playerTrfm.position.y + param.margin.y;
                break;
            case CAMERATARGET.PLAYER_GROUND:    // 
                targetX = playerTrfm.position.x +
                    param.margin.x * playerCtrl.dir;
                targetY = playerCtrl.groundY + param.margin.y;
                break;
        }

        /*
        // �ǵ����� ���� �ڵ�� �Ǻ��� ����
        // ī�޶� �̵� �Ѱ� ��輱 �˻�
        if (param.borderCheck)  // ���� �ڵ尡 ���� �̻��ϴ�
        {

            // �÷��̾ ��輱 ���� ������� Ȯ��
            float cX = playerTrfm.transform.position.x;
            float cY = playerTrfm.transform.position.y;

            if (cX < param.borderLeftTop?.transform.position.x ||
                cX > param.borderRightBottom?.transform.position.x ||
                cY > param.borderLeftTop?.transform.position.y ||
                cY < param.borderRightBottom?.transform.position.y)
            {

                return;
            }
        }        
        */

        // �÷��̾ ī�޶� ������ �ȿ� ���Դ��� �˻�
        if (param.viewAreaCheck)
        {

            float z = playerTrfm.position.z - transform.position.z;
            Vector3 minMargin = param.viewAreaMinMargin;
            Vector3 maxMargin = param.viewAreaMaxMargin;
            Vector2 min = Camera.main.ViewportToWorldPoint(
                new Vector3(0.0f, 0.0f, z)) - minMargin;
            Vector2 max = Camera.main.ViewportToWorldPoint(
                new Vector3(1.0f, 1.0f, z)) - maxMargin;

            if (playerTrfm.position.x < min.x || playerTrfm.position.x > max.x)
            {

                targetX = playerTrfm.position.x;
            }
            if (playerTrfm.position.y < min.y || playerTrfm.position.y > max.y)
            {

                targetY = playerTrfm.position.y;
                playerCtrl.groundY = playerTrfm.position.y;
            }
        }

        // ī�޶� �̵� �Ѱ� ��輱 �˻�
        // �ǵ�ġ ���� ȭ���� ���� �� �ֱ⿡
        // ��輱 �˻縦 ���ڷ� �̷Ｍ �켱 ������ ������
        if (param.borderCheck) 
        {

            float halfHeight = camera.orthographicSize;                     // ȭ���� ���� ������ ����
            float halfWidth = camera.orthographicSize * camera.aspect;      // ȭ���� ���� ������ ����

            // ���� �˻��̰� ���� �켱
            if (targetX - halfWidth < param.borderLeftTop.transform.position.x)
            {

                targetX = param.borderLeftTop.transform.position.x + halfWidth;
            }
            else if (targetX + halfWidth > param.borderRightBottom.transform.position.x)
            {

                targetX = param.borderRightBottom.transform.position.x - halfWidth;
            }

            // ���� �˻� �Ʒ��� �켱
            if (targetY - halfHeight < param.borderRightBottom.transform.position.y)
            {

                targetY = param.borderRightBottom.transform.position.y + halfHeight;
            }
            else if (targetY + halfHeight > param.borderLeftTop.transform.position.y)
            {

                targetY = param.borderLeftTop.transform.position.y - halfHeight;
            }
        }

        // ī�޶� �̵� (ȣ��)
        switch (param.homingType) 
        {

            case CAMERAHOMING.DIRECT:
                pX = targetX;
                pY = targetY;
                screenOGSize = param.screenOGSize;
                screenPSSize = param.screenPSSize;
                break;
            case CAMERAHOMING.LERP:
                pX = Mathf.Lerp(transform.position.x, targetX, param.homing.x);
                pY = Mathf.Lerp(transform.position.y, targetY, param.homing.y);
                screenOGSize = Mathf.Lerp(screenOGSize, param.screenOGSize,
                    param.screenOGSizeHoming);
                screenPSSize = Mathf.Lerp(screenPSSize, param.screenPSSize,
                    param.screenPSSizeHoming);
                break;
            case CAMERAHOMING.SLERP:
                pX = Mathf.SmoothStep(transform.position.x, targetX, param.homing.x);
                pY = Mathf.SmoothStep(transform.position.y, targetY, param.homing.y);
                screenOGSize = Mathf.SmoothStep(screenOGSize, param.screenOGSize,
                    param.screenOGSizeHoming);
                screenPSSize = Mathf.SmoothStep(screenPSSize, param.screenPSSize,
                    param.screenPSSizeHoming);
                break;
            case CAMERAHOMING.STOP:
                break;
        }

        transform.position = new Vector3(pX, pY, transform.position.z);
        camera.orthographic = param.orthographicEnabled;
        camera.orthographicSize = screenOGSize + screenOGSizeAdd;
        camera.fieldOfView = screenPSSize + screenPSSizeAdd;
        camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, 2.5f, 10.0f);
        camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, 30.0f, 100.0f);

        // ī�޶��� Ư�� �� ȿ�� ���
        screenOGSizeAdd *= 0.99f;           // ���Ŀ� �ڷ�ƾ �������� ó��!
        screenPSSizeAdd *= 0.99f;
    }

    // �ڵ� (�� ��)
    public void SetCamera(Param cameraPara)
    {

        param = cameraPara;
    }

    public void AddCameraSize(float ogAdd, float psAdd)
    {

        screenOGSizeAdd += ogAdd;
        screenPSSizeAdd += psAdd;
    }
}
