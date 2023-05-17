using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Collision Layers", menuName = "Scriptable Objects/Collision Layers")]
public class CollisionLayers : ScriptableObject
{
    public int PlayerLayer;
    public int EnemyLayer;
    public int BananaLayer;
    public int GroundLayer;
    public int AttackLayer;
    public int EffectLayer;
    public int IgnoreExplosionLayer;
    public int IgnoreIceLayer;
    public int IgnoreEletricLayer;
    public int WallLayer;
    public int BossLayer;
    public int TriggerFlipLayer;
    public int TriggerLevelLayer;
    public int SpikeLayer;
    public int TriggerMovePointLayer;
}
