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
    
    public GameObject pauseMenu;

    private UI pauseMenuUI;

    protected override void Awake()
    {
        base.Awake();
        inputModule = GetComponent<InputSystemUIInputModule>();
        uIRayCaster = pauseMenu.GetComponent<GraphicRaycaster>();
    }

    private void Start()
    {
        pointerData = new PointerEventData(EventSystem.current);
        pointerResults = new List<RaycastResult>();
        pauseMenuUI = pauseMenu.GetComponent<UI>();
    }

    private void Update()
    {
        if (inputModule.leftClick.action.WasReleasedThisFrame())
        {
            GetUIElements();
        }

        GetUIElements();
    }
    
    private void GetUIElements()
    {
        pointerData.position = inputModule.point.action.ReadValue<Vector2>();
        pointerResults.Clear();
        
        uIRayCaster.Raycast(pointerData, pointerResults);

        foreach (RaycastResult result in pointerResults)
        {
            GameObject uiElement = result.gameObject;

            if (uiElement.name == "Armor Slot")
            {
                ArmorSlot armorSlot = uiElement.GetComponent<ArmorSlot>();
                ArmorDataSO armorDataSo = armorSlot.item.itemDataSo as ArmorDataSO;

                if (armorSlot.item.itemDataSo == null)
                {
                    return;
                }
                
                pauseMenuUI.itemTooltip.ShowArmorTooltip(armorDataSo);
            }
            else if (uiElement.name == "Weapon Slot")
            {
                WeaponSlot weaponSlot = uiElement.GetComponent<WeaponSlot>();
                WeaponDataSO weaponDataSo = weaponSlot.item.itemDataSo as WeaponDataSO;

                if (weaponSlot.item.itemDataSo == null)
                {
                    return;
                }
                
                pauseMenuUI.itemTooltip.ShowWeaponTooltip(weaponDataSo);
            }
        }
    }
}