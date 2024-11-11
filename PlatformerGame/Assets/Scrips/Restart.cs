using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

    
    public class Restart : MonoBehaviour
    {
        [SerializeField]private GameObject restartCanvas;
    	public void RestartGame() {
    		SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
            restartCanvas.SetActive(false);
    	}

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                restartCanvas.SetActive(!restartCanvas.activeSelf);
            }
        }
    }
