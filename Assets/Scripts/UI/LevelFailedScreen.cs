using System;
using UnityEngine;

public class LevelFailedScreen : BaseScreen
{
    [SerializeField] private ActionButton restartLevelButton;
    protected override void ManualStart()
    {
        restartLevelButton.OnClickEvent.AddListener(GameUi.EventBus.LevelFailedScreen.OnRestartLevelButtonTap);
    }
}
