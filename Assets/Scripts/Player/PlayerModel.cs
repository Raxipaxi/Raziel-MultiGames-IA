using UnityEngine;
using UnityEditor;

[RequireComponent (typeof (Rigidbody))]

public class PlayerModel : MonoBehaviour
{
     private PlayerView _playerView;
     private Rigidbody _rb;

    private PlayerData _playerData;


    private void Awake()
    {
        BakeReferences();
    }
    public void BakeReferences()
    {
        _playerView = GetComponent<PlayerView>();
        _rb = GetComponent<Rigidbody>();
    }
}