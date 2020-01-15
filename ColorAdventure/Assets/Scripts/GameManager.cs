using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>{
    
    [SerializeField] private Player m_player;
    [SerializeField] private PlayerController m_playerController;
    [SerializeField] private UIManager m_UIManager;

    public Player Player => m_player;
    public PlayerController PlayerController => m_playerController;
    public UIManager UIManager => m_UIManager;
    
    void Start(){
        CursorInitialize();
        m_player.OnStart();
        m_playerController.OnStart();
        m_UIManager.OnStart();
    }

    // Update is called once per frame
    void Update(){
        m_playerController.OnUpdate();
        m_player.OnUpdate();
        m_UIManager.OnUpdate();
    }

    void CursorInitialize() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
