using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class SingletonTest<T> : MonoBehaviour where T:Component 
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject newObject = new GameObject();
                instance = newObject.AddComponent<T>();
                newObject.name = instance.GetType().Name; //try to change obj name as script file name
            }
            return instance;
        }
    }

    private void Awake() 
    {
        //Singleton
        if (instance == null)
        {
            instance = this as T;
        }
        if (instance != this)
        {
            Destroy(gameObject);
        }
       
    }
}