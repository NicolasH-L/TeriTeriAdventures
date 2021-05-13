using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private static PlayerAttack _playerAttack;
    public static PlayerAttack PlayerAttackInstance => _playerAttack;

    private bool _hasAttacked;
    private const int SoundEffect2 = 1;
    private AudioSource[] _audioSource;
    private PolygonCollider2D _judahCollider;
    [SerializeField] private SpriteRenderer _judahBack;
    private const float DelayTime = 0.4f;
    [SerializeField] private List<GameObject> _judahWeapons;
    private Animator _animatorPlayer;
    private List<Animator> _liste;
    private float _appearTime;
    private bool _hasWeapon;

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

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponents<AudioSource>();
        _liste = new List<Animator>();
        _liste.AddRange(GetComponentsInChildren<Animator>());
        _animatorPlayer = _liste[1];
        Debug.Log(_animatorPlayer.name + " " + _liste.Count.ToString());
        _judahCollider = _judahWeapons[0].GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //TODO : Fix attacking

        if (Input.GetKey("j") && !_hasAttacked)
        {
            Attack();
        }
        else if (Input.GetKeyUp("j") && _hasWeapon)
        {
            
            Invoke(nameof(AppearBack), _appearTime);
            _judahWeapons[0].SetActive(false);
            _judahCollider.enabled = false;
            _animatorPlayer.SetTrigger("");
            _appearTime = 0;
        }
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(DelayTime);
        _judahCollider.enabled = false;
        _hasAttacked = false;
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
        _judahWeapons[0].SetActive(true);
        _judahCollider.enabled = true;
        _judahBack.enabled = false;
        _hasAttacked = true;
        StartCoroutine(Delay());
    }

    public void ObtainWeapon()
    {
        _hasWeapon = true;
    }
}