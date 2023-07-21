using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 프로퍼티 이해를 돕기 위한 스크립트
// 프로퍼티는 변수가 아닌 특수한 형태의 메소드이다
// 프로퍼티는 필터기능을 하게 사용할 수 있으므로 코드의 안전성을 높여준다
// 한번 필터링을 거치므로 그냥 변수 사용보다는 느리나 이 차이가 매우 미미하다
public class VolumeInfo
{

    // 간략하게 다양한 단위로 기록하고 출력한다고 가정
    // 실제로는 다르다
    public float megaBytes
    {

        get { return m_bytes * 0.000001f; }

        set
        {

            if (value <= 0)
            {

                m_bytes = 0;
            }
            else
            {

                m_bytes = value * 1000000f;
            }
        }
    }

    public float kiloBytes
    {

        get { return m_bytes * 0.001f; }

        set
        {

            if (value <= 0)
            {

                m_bytes = 0;
            }
            else
            {

                m_bytes = value * 1000f;
            }
        }
    }

    public float bytes
    {

        get { return m_bytes; }

        set
        {

            if (value <= 0)
            {

                m_bytes = 0;
            }
            else
            {

                m_bytes = value;
            }
        }
    }

    private float m_bytes = 0;              // 바이트 단위로 용량 기록

    // public float bytes { get; private set; }
    // 는 다음 코드와 같다
    /*
    
    private float m_bytes;
    public float bytes
    {

        get { return m_bytes; }

        private set { m_bytes = value; }
    }
    */

    // 프로퍼티를 설정함에 있어서 아래와 같이 자기자신을 참조하게 정의해서는 안된다
    // 컴파일러에서 아래 코드가 에러가 없다고 떠서 실행 가능하나
    // 실제로 실행 시키면 스택 오버플로우를 발생시키는 악성코드가 된다
    /*
    public float Bytes
    {

        get { return Bytes; }
    }
    */
}
