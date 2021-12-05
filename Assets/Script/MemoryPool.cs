using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPool<T> : MonoBehaviour, System.IDisposable where T : Component
{
	Queue<T> queue= new Queue<T>();
	T original;
	int Poolsize;
	Transform parent= null;
	int MaxPoolsize= 2000;
	public MemoryPool(T _original,int poolsize)
	{
		Poolsize = poolsize;
		original = _original;
		for(int i=0;i<Poolsize;++i)
		{
			T obj = GameObject.Instantiate<T>(original);
			obj.gameObject.SetActive(false);
			queue.Enqueue(obj);
		}
	}
	public MemoryPool(T _original,int poolsize,Transform _parent)
	{
		Poolsize = poolsize;
		original = _original;
		parent = _parent;
		for(int i=0;i<(Poolsize>MaxPoolsize?MaxPoolsize:Poolsize);++i)
		{
			T obj = GameObject.Instantiate<T>(original);
			//string[] str=obj.name.Split('(');
			//obj.name = str[0];
			obj.name = queue.Count.ToString();
			obj.transform.SetParent(parent);
			obj.gameObject.SetActive(false);
			queue.Enqueue(obj);
		}
	}
	public void ExpandPoolSize()
	{
		int newsize = Poolsize + (Poolsize / 2);
		for(int i=Poolsize;i<newsize;++i)
		{
			T obj = GameObject.Instantiate<T>(original);
			obj.gameObject.SetActive(false);
			//string[] str = obj.name.Split('(');
			//obj.name = str[0];
			obj.name = queue.Count.ToString();
			if (parent != null)
			{
				obj.transform.SetParent(parent);
			}
			queue.Enqueue(obj);
		}
		Poolsize = newsize;
	}
	public T Spawn(bool expand=true)
	{
		if(queue.Count>0)
		{
			T item = queue.Dequeue();
			return item;
		}
		if(expand)
		{
			ExpandPoolSize();
			T item = queue.Dequeue();
			return item;
		}
		else
		{
			Debug.LogWarning("Pool size over");
			return null;
		}
	}
    public void DeSpawn(T obj)
	{
		if (obj == null) return;
		obj.gameObject.SetActive(false);
		queue.Enqueue(obj);
	}
	public void Dispose()
	{
		foreach(T item in queue)
		{
			GameObject.Destroy(item);
		}
		queue.Clear();
		queue = null;
	}
}
