using System;
using System.Collections;

public interface IGameAction
{
    CreatureStats Source { get; }

    MatchSystem.actionStatuses status { get; set; }
    IEnumerator Execute();
    bool Check();
}
