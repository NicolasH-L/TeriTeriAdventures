﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerAttack : MonoBehaviour
    {
        private static PlayerAttack _playerAttack;
        public static PlayerAttack PlayerAttackInstance => _playerAttack;

        private const int SoundEffect2 = 1;
        private const int AttackAudioSourceIndex = 2;
        private const string AttackInpuKey = "j";
        private const float DelayTime = 0.6f;
        [SerializeField] private List<AudioClip> listAttackClips;
        [SerializeField] private GameObject judahWeapon;
        [SerializeField] private SpriteRenderer judahBack;
        private PolygonCollider2D _judahCollider;
        private AudioSource[] _audioSource;
        private Animator _animatorPlayer;
        private List<Animator> _liste;
        private float _appearTime;
        private bool _hasAttacked;
        private bool _hasWeapon;
        private int _audioClipIndex;
        private static readonly int AttackTrigger = Animator.StringToHash("Attack");

        private void Awake()
        {
            if (_playerAttack != null && _playerAttack != null)
                Destroy(gameObject);
            else
                _playerAttack = this;
        }

        private void Start()
        {
            _audioSource = GetComponents<AudioSource>();
            _audioClipIndex = 0;
            _audioSource[AttackAudioSourceIndex].clip = listAttackClips[_audioClipIndex];
            _liste = new List<Animator>();
            _liste.AddRange(GetComponentsInChildren<Animator>());
            _animatorPlayer = _liste[1];
        }

        private void Update()
        {
            if (Input.GetKey(AttackInpuKey) && !_hasAttacked)
                Attack();
            else if (Input.GetKeyUp(AttackInpuKey) && _hasWeapon ||
                     !Input.GetKey(AttackInpuKey) && _hasWeapon)
            {
                Invoke(nameof(AppearBack), _appearTime);
                judahWeapon.SetActive(false);
                _appearTime = 0;
            }
        }

        private IEnumerator Delay()
        {
            yield return new WaitForSeconds(DelayTime);
            _hasAttacked = false;
            ChangeAttackAudioClip();
        }

        private void AppearBack()
        {
            judahBack.enabled = true;
        }

        private void Attack()
        {
            if (_hasAttacked || !_hasWeapon)
                return;
            _appearTime = _animatorPlayer.runtimeAnimatorController.animationClips.Length;
            _animatorPlayer.SetTrigger(AttackTrigger);
            _audioSource[SoundEffect2].Play();
            _audioSource[AttackAudioSourceIndex].Play();

            judahWeapon.SetActive(true);
            judahBack.enabled = false;
            _hasAttacked = true;
            StartCoroutine(Delay());
        }

        private void ChangeAttackAudioClip()
        {
            ++_audioClipIndex;
            if (_audioClipIndex + 1 > listAttackClips.Count)
                _audioClipIndex = 0;

            _audioSource[AttackAudioSourceIndex].clip = listAttackClips[_audioClipIndex];
        }

        public void ObtainWeapon()
        {
            _hasWeapon = true;
        }
    }
}