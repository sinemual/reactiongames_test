using System;
using System.Globalization;
using Client.Infrastructure;
using UnityEngine.Events;

public class UserInterfaceEventBus
{
    public readonly LevelCompleteScreenEvents LevelCompleteScreen = new LevelCompleteScreenEvents();
    public readonly LevelFailedScreenEvents LevelFailedScreen = new LevelFailedScreenEvents();
    public readonly PauseScreenEvents PauseScreen = new PauseScreenEvents();
    public readonly UnitsScreenEvents UnitsScreen = new UnitsScreenEvents();
    public readonly UnitPanelEvents UnitPanel = new UnitPanelEvents();

    public class LevelCompleteScreenEvents
    {
        public event Action NextLevelButtonTap;
        public void OnNextLevelButtonTap() => NextLevelButtonTap?.Invoke();
    }
    
    public class LevelFailedScreenEvents
    {
        public event Action RestartLevelButtonTap;
        public void OnRestartLevelButtonTap() => RestartLevelButtonTap?.Invoke();
    }
    
    public class PauseScreenEvents
    {
        public event Action<int> SaveButtonTap;
        public event Action<int> LoadButtonTap;
        public event Action ContinueButtonTap;
        
        public void OnSaveButtonTap(int num) => SaveButtonTap?.Invoke(num);
        public void OnLoadButtonTap(int num) => LoadButtonTap?.Invoke(num);
        public void OnContinueButtonTap() => ContinueButtonTap?.Invoke();
    }
    
    public class UnitPanelEvents
    {
        public event Action<int> InitUnitsPanelAmount;
        public void OnInitUnitsPanelAmount(int num) => InitUnitsPanelAmount?.Invoke(num);
        
        public event Action<int> InitUnitPanel;
        public void OnInitUnitPanel(int num) => InitUnitPanel?.Invoke(num);
        
        public event Action<int, float> ChangeUnitHealth;
        public void OnChangeUnitHealth(int num, float amount) => ChangeUnitHealth?.Invoke(num, amount);
        
        public event Action<int> SelectNextUnit;
        public void OnSelectNextUnit(int num) => SelectNextUnit?.Invoke(num);
    }
    
    public class UnitsScreenEvents
    {
        public event Action<int> SelectUnitButtonTap;
        public void OnSelectUnitButtonTap(int num) => SelectUnitButtonTap?.Invoke(num);
    }
}