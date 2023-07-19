using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : BaseScreen
{
    [SerializeField] private ActionButton continueButton;
    [SerializeField] private List<ActionButton> saveButton;
    [SerializeField] private List<ActionButton> loadButton;
    
    protected override void ManualStart()
    {
        continueButton.OnClickEvent.AddListener(GameUi.EventBus.PauseScreen.OnContinueButtonTap);
        
        for (int i = 0; i < saveButton.Count; i++)
        {
            var tempI = i;
            saveButton[i].OnClickEvent.AddListener(() => GameUi.EventBus.PauseScreen.OnSaveButtonTap(tempI));
        }
        
        for (int i = 0; i < loadButton.Count; i++)
        {
            var tempI = i;
            loadButton[i].OnClickEvent.AddListener(() => GameUi.EventBus.PauseScreen.OnLoadButtonTap(tempI));
        }
    }
}