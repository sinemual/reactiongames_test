using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class UnitsScreen : BaseScreen
{
    [SerializeField] private List<UnitPanel> panels;

    protected override void ManualStart()
    {
        GameUi.EventBus.UnitPanel.InitUnitsPanelAmount += (num) =>
        {
            foreach (var panel in panels)
                panel.Panel.SetActive(false);

            for (int i = 0; i < num; i++)
                panels[i].Panel.SetActive(true);
        };
        
        GameUi.EventBus.UnitPanel.InitUnitPanel += (num) =>
        {
            panels[num].HealthBar.fillAmount = 1.0f;
            panels[num].SelectUnitButton.OnClickEvent.AddListener(() => SelectUnit(num));
        };

        GameUi.EventBus.UnitPanel.ChangeUnitHealth += (num, amount) =>
        {
            panels[num].HealthBar.fillAmount = amount;
        };
        
        GameUi.EventBus.UnitPanel.SelectNextUnit += SelectUnitFrame;
    }

    private void SelectUnit(int num)
    {
        GameUi.EventBus.UnitsScreen.OnSelectUnitButtonTap(num);
        SelectUnitFrame(num);
    }
    
    private void SelectUnitFrame(int num)
    {
        foreach (var panel in panels)
            panel.SelectedFrame.SetActive(false);
        
        panels[num].SelectedFrame.SetActive(true);
    }

    [Serializable]
    private class UnitPanel
    {
        public GameObject Panel;
        public GameObject SelectedFrame;
        public Image HealthBar;
        public ActionButton SelectUnitButton;
    }
}