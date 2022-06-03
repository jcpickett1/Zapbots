using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerDataHandler : MonoBehaviour
{
    private PlayerStats _stats;
    private PlayerConstants _constants;

    // get stats
    public float RunSpeed { get { return _stats.RunSpeed; } set { _stats.RunSpeed = value; }}
    public float JumpForce { get { return _stats.JumpForce; } set { _stats.JumpForce = value; }}
    public float JumpTimeout { get { return _stats.JumpTimeout; } set { _stats.JumpTimeout = value; }}
    public float MaxHealth { get { return _stats.MaxHealth; } set { _stats.MaxHealth = value; }}
    public float RegenRate { get { return _stats.RegenRate; } set { _stats.RegenRate = value; }}

    // get constants
    public float GroundedRadius { get { return _constants.GroundedRadius; }}
    public float GravityScale { get { return _constants.GravityScale; }}
    public float GlobalGravity { get { return _constants.GlobalGravity; }}
    public float GroundedGravity { get { return _constants.GroundedGravity; }}
    public float TerminalVelocity { get { return _constants.TerminalVelocity; }}
    public float StepSmooth { get { return _constants.StepSmooth; }}
    public float StepRange { get { return _constants.StepRange; }}


    public void SavePlayerStats()
    {
        PlayerStats stats = new PlayerStats(this);
        string playerStats = JsonUtility.ToJson(stats);
        File.WriteAllText(Application.dataPath + "/Data/PlayerStats.json", playerStats);
    }

    public void LoadPlayerStats()
    {
        _stats = JsonUtility.FromJson<PlayerStats>(File.ReadAllText(Application.dataPath + "/Data/PlayerStats.json"));
    }

    public void LoadPlayerConstants()
    {
        _constants = JsonUtility.FromJson<PlayerConstants>(File.ReadAllText(Application.dataPath + "/Data/PlayerConstants.json"));
    }
}
