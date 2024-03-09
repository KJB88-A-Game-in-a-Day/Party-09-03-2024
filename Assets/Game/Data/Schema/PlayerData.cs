using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Custom/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float maxPower = 10.0f;
    [SerializeField] private float cooldownTimeMax = 2.5f;
    [SerializeField] private float idleTimeMax = 5.0f;
    public int MaxHealth => maxHealth;
    public float MaxPower => maxPower;
    public float CooldownTimeMax => cooldownTimeMax;
    public float IdleTimeMax => idleTimeMax;
}
