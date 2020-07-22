using UnityEngine;

public class PillarOfFireSpell : ISpell
{
    public GameObject prefab;
    public GameObject Trail;
    public override string SpellName => "Столб огня";

    public override Sprite Image => Resources.Load<Sprite>("Samurai_Anger");

    public override IGameAction GameAction => new PillarOfFireAction();

    public override int MP => 2;

    public GameObject CreatePrefab()
    {
        return Instantiate(prefab);
    }
}
