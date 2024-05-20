using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class UIInputManager : SingletonMonoBehavior<UIInputManager>
{
    private InputSystemUIInputModule inputModule;
    private GraphicRaycaster uIRayCaster;
    private PointerEventData pointerData;
    private List<RaycastResult> pointerResults;

    protected override void Awake()
    {
        base.Awake();
        inputModule = GetComponent<InputSystemUIInputModule>();
    }

    private void Start()
    {
        pointerData = new PointerEventData(EventSystem.current);
        pointerResults = new List<RaycastResult>();
    }

    public static Vector2 GetMousePosition()
    {
        if (Instance == null)
        {
            return new Vector2();
        }
        else
        {
            return Instance.inputModule.point.action.ReadValue<Vector2>();
        }
    }
    
    private void GetUIElements()
    {
        pointerData.position = inputModule.point.action.ReadValue<Vector2>();
        pointerResults.Clear();
        
        uIRayCaster.Raycast(pointerData, pointerResults);

        foreach (RaycastResult result in pointerResults)
        {
            Debug.Log(result);
            GameObject uiElement = result.gameObject;
                
        }
    }
}