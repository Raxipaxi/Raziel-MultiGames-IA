using System;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerModel _playerModel;
        private FSM<PlayerStatesConstants> _fsm;

        private void Awake()
        {
            BakeReferences();
        }

        private void Start()
        {
            InitFSM();
        }

        private void InitFSM()
        {
            //--------------- FSM Creation -------------------//                
            // States Creation
            var idle = new PlayerIdleState<PlayerStatesConstants>();
            var move = new PlayerMoveState<PlayerStatesConstants>();
            var interact = new PlayerInteractAndWait<PlayerStatesConstants>();
            var jump = new PlayerJumpState<PlayerStatesConstants>();
            var dead = new PlayerDeadState<PlayerStatesConstants>();
        
            // Idle State
            idle.AddTransition(PlayerStatesConstants.Move, move);
            idle.AddTransition(PlayerStatesConstants.Jump,jump);
            idle.AddTransition(PlayerStatesConstants.Interact,interact);
            idle.AddTransition(PlayerStatesConstants.Dead, dead);
        
            // Move State
            move.AddTransition(PlayerStatesConstants.Idle, idle);
            move.AddTransition(PlayerStatesConstants.Dead, dead);
            move.AddTransition(PlayerStatesConstants.Interact,interact);
            move.AddTransition(PlayerStatesConstants.Jump,jump);
        
            // Interact State
            interact.AddTransition(PlayerStatesConstants.Idle, idle);
            interact.AddTransition(PlayerStatesConstants.Dead, dead);
            
            
            // Jump State
            jump.AddTransition(PlayerStatesConstants.Idle,idle);
            jump.AddTransition(PlayerStatesConstants.Interact,interact);
            jump.AddTransition(PlayerStatesConstants.Dead, dead);
            
            
            
            _fsm = new FSM<PlayerStatesConstants>(idle);

        }
        
        public void BakeReferences()
        {
            _playerModel = GetComponent<PlayerModel>();
        }


        private void Update()
        {
            if (_playerModel!=null)
            {
                _fsm.UpdateState();
            }
        }
    }
}