using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{

    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] visualGameObjectArray;


    public void Start()
    {
        Player.Instance.OnSelectecCounterChange += Player_OnSelectecCounterChange;
    }

    private void Player_OnSelectecCounterChange(object sender, Player.OnSelectedCounterChangeEventArgs e)
    {
        if (e.argSelectedCounter == baseCounter) 
            Show();
        else
            Hide();
    }

    private void Show()
    {
        foreach (GameObject visualGameObject in visualGameObjectArray) 
        {
            visualGameObject.SetActive(true);
        }
        
    }

    private void Hide()
    {
        foreach (GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(false);
        }
    }

}
