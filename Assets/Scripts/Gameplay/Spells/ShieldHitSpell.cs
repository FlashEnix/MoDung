using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldHitSpell : ISpell
{
    public override string SpellName => "Shield Hit";

    public override Sprite Image => Resources.Load<Sprite>("Samurai_Sword Strike");

    public override int MP => 2;

    public override IGameAction GameAction => new ShieldHitAction();
}
