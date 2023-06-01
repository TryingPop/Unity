using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppSound : MonoBehaviour
{

    // 외부 파라미터
    public static AppSound instance = null;

    // 배경음
    [HideInInspector] public zFoxSoundManager fm;
    [HideInInspector] public AudioSource BGM_LOGO;
    [HideInInspector] public AudioSource BGM_TITLE;
    [HideInInspector] public AudioSource BGM_HISCORE;
    [HideInInspector] public AudioSource BGM_HISCORE_RANKIN;
    [HideInInspector] public AudioSource BGM_STAGEA;
    [HideInInspector] public AudioSource BGM_STAGEB;
    [HideInInspector] public AudioSource BGM_STAGEB_ROOMSAKURA;
    [HideInInspector] public AudioSource BGM_BOSSA;
    [HideInInspector] public AudioSource BGM_BOSSB;
    [HideInInspector] public AudioSource BGM_ENDING;

    // 효과음
    [HideInInspector] public AudioSource SE_MENU_OK;
    [HideInInspector] public AudioSource SE_MENU_CANCEL;

    [HideInInspector] public AudioSource SE_ATK_A1;
    [HideInInspector] public AudioSource SE_ATK_A2;
    [HideInInspector] public AudioSource SE_ATK_A3;
    [HideInInspector] public AudioSource SE_ATK_B1;
    [HideInInspector] public AudioSource SE_ATK_B2;
    [HideInInspector] public AudioSource SE_ATK_B3;
    [HideInInspector] public AudioSource SE_ATK_ARIAL;
    [HideInInspector] public AudioSource SE_ATK_SYURIKEN;

    [HideInInspector] public AudioSource SE_HIT_A1;
    [HideInInspector] public AudioSource SE_HIT_A2;
    [HideInInspector] public AudioSource SE_HIT_A3;
    [HideInInspector] public AudioSource SE_HIT_B1;
    [HideInInspector] public AudioSource SE_HIT_B2;
    [HideInInspector] public AudioSource SE_HIT_B3;

    [HideInInspector] public AudioSource SE_MOV_JUMP;

    [HideInInspector] public AudioSource SE_ITEM_KOBAN;
    [HideInInspector] public AudioSource SE_ITEM_HYOUTAN;
    [HideInInspector] public AudioSource SE_ITEM_MAKIMONO;
    [HideInInspector] public AudioSource SE_ITEM_OHBAN;
    [HideInInspector] public AudioSource SE_ITEM_KEY;

    [HideInInspector] public AudioSource SE_OBJ_EXIT;
    [HideInInspector] public AudioSource SE_OBJ_OPENDOOR;
    [HideInInspector] public AudioSource SE_OBJ_SWITCH;
    [HideInInspector] public AudioSource SE_OBJ_BOXBROKEN;

    [HideInInspector] public AudioSource SE_CHECKPOINT;
    [HideInInspector] public AudioSource SE_EXPLOSION;

    // 내부 파라미터
    string sceneName = "non";

    // 코드
    void Awake()
    {

        // 사운드
        fm = GameObject.Find("zFoxSoundManager").GetComponent<zFoxSoundManager>();

        // 배경음
        fm.CreateGroup("BGM");
        BGM_LOGO = fm.LoadResourceSound("BGM", "Logo");
        BGM_TITLE = fm.LoadResourceSound("BGM", "Title");
        BGM_HISCORE = fm.LoadResourceSound("BGM", "HiScore");
        BGM_HISCORE_RANKIN = fm.LoadResourceSound("BGM", "HiScore_Rankin");
        BGM_STAGEA = fm.LoadResourceSound("BGM", "StageA");
        BGM_STAGEB = fm.LoadResourceSound("BGM", "StageB");
        BGM_STAGEB_ROOMSAKURA = fm.LoadResourceSound("BGM", "StageB_RoomSakura");
        BGM_BOSSA = fm.LoadResourceSound("BGM", "BossA");
        BGM_BOSSB = fm.LoadResourceSound("BGM", "BossB");
        BGM_ENDING = fm.LoadResourceSound("BGM", "Ending");

        // 효과음
        fm.CreateGroup("SE");
        fm.SoundFolder = "Sounds/SE";
        SE_MENU_OK = fm.LoadResourceSound("SE", "SE_Menu_ok");
        SE_MENU_CANCEL = fm.LoadResourceSound("SE", "SE_Menu_Cancel");

        SE_ATK_A1 = fm.LoadResourceSound("SE", "SE_ATK_A1");
        SE_ATK_A2 = fm.LoadResourceSound("SE", "SE_ATK_A2");
        SE_ATK_A3 = fm.LoadResourceSound("SE", "SE_ATK_A3");
        SE_ATK_B1 = fm.LoadResourceSound("SE", "SE_ATK_B1");
        SE_ATK_B2 = fm.LoadResourceSound("SE", "SE_ATK_B2");
        SE_ATK_B3 = fm.LoadResourceSound("SE", "SE_ATK_B3");
        SE_ATK_ARIAL = fm.LoadResourceSound("SE", "SE_ATK_Arial");
        SE_ATK_SYURIKEN = fm.LoadResourceSound("SE", "SE_ATK_Syuriken");

        SE_HIT_A1 = fm.LoadResourceSound("SE", "SE_HIT_A1");
        SE_HIT_A2 = fm.LoadResourceSound("SE", "SE_HIT_A2");
        SE_HIT_A3 = fm.LoadResourceSound("SE", "SE_HIT_A3");
#if xxx // 플레이어 캐릭터와 같은 효과음을 사용하므로 삭제
        SE_HIT_B1 = fm.LoadResourceSound("SE", "SE_HIT_B1");
        SE_HIT_B2 = fm.LoadResourceSound("SE", "SE_HIT_B2");
        SE_HIT_B3 = fm.LoadResourceSound("SE", "SE_HIT_B3");
#endif
        SE_HIT_B1 = SE_HIT_A1;
        SE_HIT_B2 = SE_HIT_A2;
        SE_HIT_B3 = SE_HIT_A3;

        SE_MOV_JUMP = fm.LoadResourceSound("SE", "SE_MOV_Jump");

        SE_ITEM_KOBAN = fm.LoadResourceSound("SE", "SE_Item_Koban");
        SE_ITEM_HYOUTAN = fm.LoadResourceSound("SE", "SE_Item_Hyoutan");
        SE_ITEM_MAKIMONO = fm.LoadResourceSound("SE", "SE_Item_Makimono");
        SE_ITEM_OHBAN = fm.LoadResourceSound("SE", "SE_Item_Ohban");
        SE_ITEM_KEY = fm.LoadResourceSound("SE", "SE_Item_Key");

        SE_OBJ_EXIT = fm.LoadResourceSound("SE", "SE_OBJ_Exit");
        SE_OBJ_OPENDOOR = fm.LoadResourceSound("SE", "SE_OBJ_OpenDoor");
        SE_OBJ_SWITCH = fm.LoadResourceSound("SE", "SE_OBJ_Switch");
        SE_OBJ_BOXBROKEN = fm.LoadResourceSound("SE", "SE_OBJ_BoxBroken");

        SE_CHECKPOINT = fm.LoadResourceSound("SE", "SE_CheckPoint");
        SE_EXPLOSION = fm.LoadResourceSound("SE", "SE_Explosion");

        instance = this;
    }

    void Update()
    {
        
        // 씬이 바뀌었는지 검사
        if (sceneName != SceneManager.GetActiveScene().name)
        {

            sceneName = SceneManager.GetActiveScene().name;

            // 배경음 재생
            if (sceneName == "Menu_Logo")
            {

                BGM_LOGO.Play();
            }
            else if (sceneName == "Menu_Title")
            {

                if (!BGM_TITLE.isPlaying)
                {

                    fm.Stop("BGM");
                    BGM_TITLE.Play();
                    fm.FadeInVolume(BGM_TITLE, 1.0f, 1.0f, true);
                }
            }
            else if (sceneName == "Menu_Option" || sceneName == "Menu_HiScore" || 
                sceneName == "Menu_Option")
            {


            }
            else if (sceneName == "StageA")
            {

                fm.FadeOutVolumeGroup("BGM", BGM_STAGEA, 0.0f, 1.0f, false);
                fm.FadeInVolume(BGM_TITLE, 1.0f, 1.0f, true);
                BGM_STAGEA.loop = true;
                BGM_STAGEA.Play();
            }
            else if (sceneName == "StageB_Room")    // 4_stage로 수정!
            {

                fm.Stop("BGM");
                BGM_STAGEB_ROOMSAKURA.loop = true;
                BGM_STAGEB_ROOMSAKURA.Play();
            }
            else if (sceneName == "StageB_Room_A" ||
                sceneName == "StageB_Room_B" || sceneName == "StageB_Room_C")   // 4_stage_A로 쓸 예정
            {

                fm.Stop("BGM");
                BGM_BOSSA.loop = true;
                BGM_BOSSA.Play();
            }
            else
            {
                if (!BGM_STAGEB.isPlaying)
                {

                    fm.Stop("BGM");
                    BGM_STAGEB.loop = true;
                    BGM_STAGEB.Play();
                }
            }
        }

    }
}
