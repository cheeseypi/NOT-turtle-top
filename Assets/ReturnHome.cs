using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ReturnHome : MonoBehaviour
{
    private TextMeshProUGUI _textElement;
    private bool enabled = false;
    private void Start()
    {
        StartCoroutine(EnableAfter2Secs());
        _textElement = gameObject.GetComponent<TextMeshProUGUI>();
        _textElement.text = "";
    }
    public void ReturnToMenu(InputAction.CallbackContext callback)
    {
        if (enabled)
        {
            SceneManager.LoadScene(0);
        }
    }

    IEnumerator EnableAfter2Secs()
    {
        yield return new WaitForSeconds(2);
        enabled = true;
        _textElement.text = "Press 'Space' or Right Trigger to Continue";
    }
}
