﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private static PlayerAttack _playerAttack;
    public static PlayerAttack PlayerAttackInstance => _playerAttack;

    private bool _hasAttacked;
    private const int SoundEffect2 = 1;
    private const int AttackAudioSourceIndex = 2;
    private PolygonCollider2D _judahCollider;
    private AudioSource[] _audioSource;

    [SerializeField] private List<AudioClip> _listAttackClips;
    [SerializeField] private List<GameObject> _judahWeapons;
    [SerializeField] private SpriteRenderer _judahBack;
    private const string AttackInpuKey = "j";
    private const float DelayTime = 0.4f;
    private Animator _animatorPlayer;
    private List<Animator> _liste;
    private float _appearTime;
    private bool _hasWeapon;
    private int _audioClipIndex;

    private void Awake()
    {
        if (_playerAttack != null && _playerAttack != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _playerAttack = this;
        }
    }

    private void Start()
    {
        _audioSource = GetComponents<AudioSource>();
        _audioClipIndex = 0;
        _audioSource[AttackAudioSourceIndex].clip = _listAttackClips[_audioClipIndex];
        _liste = new List<Animator>();
        _liste.AddRange(GetComponentsInChildren<Animator>());
        _animatorPlayer = _liste[1];
    }

    private void Update()
    {
        //TODO : Fix attacking

        if (Input.GetKey(AttackInpuKey) && !_hasAttacked)
        {
            Attack();
        }
        else if (Input.GetKeyUp(AttackInpuKey) && _hasWeapon ||
                 !Input.GetKey(AttackInpuKey) && _hasWeapon)
        {
            Invoke(nameof(AppearBack), _appearTime);
            _judahWeapons[0].SetActive(false);
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
        _judahBack.enabled = true;
    }

    private void Attack()
    {
        if (_hasAttacked || !_hasWeapon)
            return;
        _appearTime = _animatorPlayer.runtimeAnimatorController.animationClips.Length;
        _animatorPlayer.SetTrigger("Attack");
        _audioSource[SoundEffect2].Play();
        _audioSource[AttackAudioSourceIndex].Play();

        _judahWeapons[0].SetActive(true);
        _judahBack.enabled = false;
        _hasAttacked = true;
        StartCoroutine(Delay());
    }

    private void ChangeAttackAudioClip()
    {
        ++_audioClipIndex;
        if (_audioClipIndex + 1 > _listAttackClips.Count)
        {
            _audioClipIndex = 0;
        }

        _audioSource[AttackAudioSourceIndex].clip = _listAttackClips[_audioClipIndex];
    }

    public void ObtainWeapon()
    {
        _hasWeapon = true;
    }
}