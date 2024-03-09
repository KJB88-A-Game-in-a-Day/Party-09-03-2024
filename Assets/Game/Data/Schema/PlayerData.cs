using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Custom/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int maxPower = 1;
    [SerializeField] private float speedMod = 20.0f;
    [SerializeField] private float cooldownTimeMax = 2.5f;
    [SerializeField] private float idleTimeMax = 5.0f;
    [SerializeField] private float recoveryTime = 0.5f;
    [SerializeField] private float thrustPower = 40.0f;
    public int MaxHealth => maxHealth;
    public int MaxPower => maxPower;
    public float SpeedMod => speedMod;
    public float CooldownTimeMax => cooldownTimeMax;
    public float IdleTimeMax => idleTimeMax;
    public float RecoveryTime => recoveryTime;
    public float ThrustPower => thrustPower;
}
