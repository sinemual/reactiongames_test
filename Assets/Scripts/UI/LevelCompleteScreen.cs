using UnityEngine;

public class LevelCompleteScreen : BaseScreen
{
    [SerializeField] private ActionButton nextLevelButton;
    protected override void ManualStart()
    {
        nextLevelButton.OnClickEvent.AddListener(GameUi.EventBus.LevelCompleteScreen.OnNextLevelButtonTap);
    }
}