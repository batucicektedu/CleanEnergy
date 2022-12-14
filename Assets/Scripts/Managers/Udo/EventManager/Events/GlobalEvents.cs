using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using udoEventSystem;

public class TestEventWithoutParameter : AEvent
{
}

public class TestEventWithParameter : AEvent<bool>
{
}

public class StartUserInput : AEvent
{
}

public class StopUserInput : AEvent
{
}

public class StartForwardMovement : AEvent
{
}

public class StopForwardMovement : AEvent
{
}

public class LevelCompleted : AEvent
{
    
}

public class StartGame : AEvent
{
    
}
public class PauseGame : AEvent
{
    
}
public class UpdateMoneyAmountText : AEvent<int>
{
    
}

public class UpdateMoneyAmountTextWithCounter : AEvent<int>
{
    
}

public class OnGameStateChanged : AEvent<GameState>
{
    
}

public class CameraShake : AEvent
{
    
}

public class LevelFailed : AEvent
{
    
}

public class SlowTheGameDown : AEvent<float>
{
    
}

public class CableConnectedFromCity : AEvent<int>
{
    
}

public class SpawnThrashOnCity : AEvent<int>
{
    
}

public class SpawnTutorialThrash : AEvent
{
    
}

