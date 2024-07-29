using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Narrator : MonoBehaviour
{
    TextMeshProUGUI narratorText;
    string levelStringName;

    private void Awake()
    {
        narratorText = GetComponent<TextMeshProUGUI>();
        levelStringName = SceneManager.GetActiveScene().name;
    }

    private void Start()
    {
        if (levelStringName == "Level1-1")
        {
            StartCoroutine(Level1IntroCoroutine());
        }
    }

    IEnumerator Level1IntroCoroutine()
    {
        narratorText.text = "Welcome To Calamiquarium!";
        yield return new WaitForSeconds(5);
        narratorText.text = "Your goal in this insane ecosystem is to survive using everything at your disposal!";
        yield return new WaitForSeconds(10);
        narratorText.text = "As time goes on your fish will get hungry! Press the Q Key to drop food!";
        yield return new WaitForSeconds(10);
        narratorText.text = "As of right now you can only drop 1 food at a time, however you can upgrade it!";
    }
}
