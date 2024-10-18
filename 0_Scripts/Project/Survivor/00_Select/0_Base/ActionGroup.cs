using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// 움직이는 그룹을 모아놓은 자료구조
/// 연결 리스트 자료구조를 확장시킨 형태다
/// </summary>
/// <typeparam name="T"></typeparam>
public class ActionGroup<T> where T : Selectable
{

    /// <summary>
    /// 외부에서 해당 노드를 사용할 수 없다!
    /// </summary>
    protected sealed class Node
    {

        private T val;
        private Node prev;
        private Node next;

        public Node() { }

        public Node(T _val)
        {

            val = _val;
        }

        public T Val 
        { 
            
            set { val = value; }
            get { return val; } 
        }

        public Node Prev 
        {

            set { prev = value; }
            get { return prev; }
        }

        public Node Next
        {

            set { next = value; }
            get { return next; }
        }

        public void Clear()
        {

            prev = null;
            next = null;
            val = default(T);
        }
    }

    protected Node head;                       // 시작
    protected Node tail;                       // 끝

    protected Stack<Node> pool;              // 풀링
    protected Dictionary<T, Node> dic;       // 중복 확인용

    protected int capacity;                 // 허용 용량

    /// <summary>
    /// 해당 팀의 최대 제한
    /// </summary>
    public ActionGroup(int _capacity)
    {

        if (_capacity < 0) capacity = 0;
        else capacity = _capacity;
        
        head = new();
        tail = new();

        head.Next = tail;
        tail.Prev = head;

        dic = new(capacity);
        pool = new(capacity);
    }

    public T First
    {

        get
        {

            Node chk = head.Next;
            if (IsLast(chk)) return default(T);
            else return chk.Val;
        }
    }

    /// <summary>
    /// 현재 쓰는 노드 개수
    /// </summary>
    public int Count => dic.Count;

    // /// <summary>
    // /// 노드 추가 가능 확인 메서드
    // /// </summary>
    // public bool CanAdd => dic.Count < capacity;

    public void Action()
    {

        Node node = head.Next;

        while (!IsLast(node))
        {

            if (node == null)
            {

#if UNITY_EDITOR

                Debug.LogError("ActionGroup의 Node 순서에 이상이 있어\n" +
                    "행동이 정상적으로 작동하지 않습니다.");
#endif

                return;
            }

            node.Val.Action();
            node = node.Next;
        }
    }

    public void GetCommand(Command _cmd, bool _add = false)
    {

        Node node = head.Next;

        while (!IsLast(node))
        {

            if (node == null)
            {

#if UNITY_EDITOR

                Debug.LogError("ActionGroup의 Node 순서에 이상이 있어\n" +
                    "행동이 정상적으로 작동하지 않습니다.");
#endif

                return;
            }

            node.Val.GetCommand(_cmd, _add);
            node = node.Next;
        }
    }

    /// <summary>
    /// 해당 노드 포함 여부
    /// </summary>
    public bool Contains(T _chk) => dic.ContainsKey(_chk);

    /// <summary>
    /// 처음 끝 확인용
    /// </summary>
    protected bool IsFirst(Node _chk) => _chk == head;

    /// <summary>
    /// 처음 끝 확인용
    /// </summary>
    protected bool IsLast(Node _chk) => _chk == tail;


    /// <summary>
    /// 뒤에 유닛 이어 붙이기
    /// </summary>
    public void AddLast(T _add)
    {

        if (dic.ContainsKey(_add))
        {

#if UNITY_EDITOR
            Debug.Log("해당 유닛이 이미 노드에 할당되어 있습니다.");
#endif

            return;
        }

        Node add;
        if (pool.Count > 0) 
        { 
            
            add = pool.Pop();
            add.Val = _add;
        }
        else add = new(_add);

        dic[_add] = add;
        AddLast(add);
    }

    /// <summary>
    /// 유닛 해당 그룹에서 뺀다
    /// </summary>
    public void Pop(T _pop)
    {

        if (!dic.ContainsKey(_pop))
        {

#if UNITY_EDITOR

            Debug.LogError("해당 유닛이 없습니다.");
#endif
            return;
        }

        Node pop = dic[_pop];
        Pop(pop);
    }

    protected void AddLast(Node _add)
    {

        if (IsFirst(_add) || IsLast(_add))
        {

#if UNITY_EDITOR

            Debug.LogError("First 또는 Last를 삽입할 수 없습니다!");
#endif
            return;
        }

        Node tempPrev = tail.Prev;

        tempPrev.Next = _add;

        _add.Prev = tempPrev;
        _add.Next = tail;

        tail.Prev = _add;
    }

    protected void Pop(Node _pop) 
    {
        
        if (IsFirst(_pop) || IsLast(_pop))
        {

#if UNITY_EDITOR

            Debug.LogError("First와 Last는 뺄 수 없습니다!");
#endif

            return;
        }

        Node tempPrev = _pop.Prev;
        Node tempNext = _pop.Next;

        tempPrev.Next = tempNext;
        tempNext.Prev = tempPrev;

        // dic에서 제거 O(1)
        // https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2.remove?view=net-8.0
        dic.Remove(_pop.Val);

        _pop.Clear();
        if (pool.Count < capacity) pool.Push(_pop);
    }
}
