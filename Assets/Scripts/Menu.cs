using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartSinglePlayer()
    {
        SceneManager.LoadScene(1);
    }

    public void StartLocalPvp()
    {
        SceneManager.LoadScene(2);
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
