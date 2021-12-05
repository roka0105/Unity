using System.Linq;
using UnityEngine;

[DefaultExecutionOrder(-98)]
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour//MonoBehaviour를 상속받는 타입만 T에 들어올 수 있다.
{
    [SerializeField]
    protected bool flag;

    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                Singleton<T>[] objs = FindObjectsOfType<Singleton<T>>(true);

                GameObject obj = objs.Where(item => item.flag == true).FirstOrDefault()?.gameObject; //GameObject.Find(typeof(T).Name);
                if (obj == null)
                {
                    if (objs.Length > 0)
                        return objs[0].GetComponent<T>();
                    obj = new GameObject(typeof(T).Name);
                    instance = obj.AddComponent<T>();
                }
                else
                {
                    instance = obj.GetComponent<T>();
                }
            }
            return instance;
        }
    }
}
