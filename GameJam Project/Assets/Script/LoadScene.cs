using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Die"))
        {
            Debug.Log("Dead");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
}
