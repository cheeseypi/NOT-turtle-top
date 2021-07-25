using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TSNMenuController : MonoBehaviour
{
    public List<UnityEngine.UI.Button> Options;
    public int DefaultItemIndex = 0;

    private int _selectedOption = 0;
    private bool menuDisabled = false;
    // Start is called before the first frame update
    void Start()
    {
        if (Options.Count == 0)
        {
            Debug.LogError("No options added to menu");
            menuDisabled = true;
        }
        else if (DefaultItemIndex > Options.Count)
        {
            Debug.LogError("Default index larger than array size. Defaulting to 0");
        }
        else
        {
            _selectedOption = DefaultItemIndex;
        }

        if (!menuDisabled)
        {
            Options[_selectedOption].Select();
        }
    }

    public void LoadArena()
    {
        SceneManager.LoadScene(1);
    }

    public void MoveUp(InputAction.CallbackContext context)
    {
        if (!menuDisabled && context.performed)
        {
            Debug.Log("Moving Up");
            menuDisabled = true;
            _selectedOption -= 1;
            if (_selectedOption < 0)
            {
                _selectedOption = Options.Count - 1;
            }
            Options[_selectedOption].Select();
        }
        else if (context.canceled)
        {
            menuDisabled = false;
        }
    }

    public void MoveDown(InputAction.CallbackContext context)
    {
        if (!menuDisabled && context.performed)
        {
            Debug.Log("Moving Down");
            menuDisabled = true;
            _selectedOption += 1;
            if (_selectedOption >= Options.Count)
            {
                _selectedOption = 0;
            }
            Options[_selectedOption].Select();
        }
        else if (context.canceled)
        {
            menuDisabled = false;
        }
    }

    public void Select(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Selecting");
            Options[_selectedOption].onClick.Invoke();
        }
    }
}
