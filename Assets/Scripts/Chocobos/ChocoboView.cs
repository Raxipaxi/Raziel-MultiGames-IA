using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChocoboView : MonoBehaviour
{
        private Animator _animator;


        private void Awake()
        {
                BakeReferences();
        }

        void BakeReferences()
        {
                _animator = GetComponent<Animator>();
        }

        public void IdleAnimation()
        {
              // _animator.Play("ChocoboIdle");
        }


        public void MoveAnimation()
        {
               // _animator.Play("ChocoboWalk");
        }
}
