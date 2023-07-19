using System.Collections.Generic;
using Client.Data.Core;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [Header("Screens")] 
    public PauseScreen PauseScreen;
    public UnitsScreen UnitsScreen;
    public LevelFailedScreen LevelFailedScreen;
    public LevelCompleteScreen LevelCompleteScreen;

    private List<BaseScreen> _screens;
    private List<BaseScreen> _worldUiScreens;
    
    // Services:
    [HideInInspector] public UserInterfaceEventBus EventBus;

    private bool IsUiEnabled;
    private bool IsWorldUiEnabled;

    public void Inject(
        SharedData sharedData,
        UserInterfaceEventBus uiEventBus)
    {
        EventBus = uiEventBus;
        
        _screens = new List<BaseScreen>();
        _screens.AddRange(GetComponentsInChildren<BaseScreen>(true));

        foreach (var screen in _screens)
        {
            screen.gameObject.SetActive(true);
            screen.Inject(sharedData, this);
            screen.Init();
            screen.gameObject.SetActive(false);
        }

        IsUiEnabled = true;
    }

    public void TriggerShowStateAllScreen()
    {
        IsUiEnabled = !IsUiEnabled;
        foreach (var screen in _screens)
            screen.gameObject.SetActive(IsUiEnabled);
    }
    
    public void TriggerShowStateAllWorldUiScreen()
    {
        IsWorldUiEnabled = !IsWorldUiEnabled;
        foreach (var screen in _worldUiScreens)
            screen.gameObject.SetActive(IsWorldUiEnabled);
    }
    
    public void AddScreenToWorldUiScreens(BaseScreen worldUiScreen)
    {
        _worldUiScreens.Add(worldUiScreen);
    }
    
}