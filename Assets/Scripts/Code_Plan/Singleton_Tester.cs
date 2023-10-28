using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton_Tester : SingletonTest<Singleton_Tester> {

    public int Num_Teller;

    private void Start() 
    {
        DontDestroyOnLoad(this);
    }
    private void Update() 
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            GameObject newObject = new GameObject("Player Health");
            transform.parent = newObject.transform;
            Destroy(newObject);
        }
    }
}