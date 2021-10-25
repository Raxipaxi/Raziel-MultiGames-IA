using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerModel _playerModel;


        private void Awake()
        {
            BakeReferences();
        }

        public void BakeReferences()
        {
            _playerModel = GetComponent<PlayerModel>();
        }






    }
}