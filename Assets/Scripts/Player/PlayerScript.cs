using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public delegate void ChangeSpecialBgm(int bgmOption);

    public event ChangeSpecialBgm OnChangeSpecialBgm;

    public delegate void WeaponEvent();

    public event WeaponEvent OnWeaponCollected;

    private const string MaxExpText = "MAX";
    private const string KeyMoveRight = "d";
    private const string KeyMoveLeft = "a";
    private const string KeyJump = "space";
    private const string BooleanDirectionRight = "isMovingToTheRight";
    private const string BooleanDirectionLeft = "isMovingToTheLeft";
    private const int MaxJump = 2;
    private const int SoundEffect1 = 0;
    private const int SoundEffect3 = 2;
    private const int MaxHealth = 100;
    private const int Damage = 10;
    private const int InvincibilityDuration = 8;
    private const float SpeedPlayer = 7f;
    private const float JumpHeight = 8f;
    private const float DelayTime = 0.4f;
    private const float Offset = 1f / InvincibilityDuration;
    [SerializeField] private Rigidbody2D playerRigidBody2D;
    [SerializeField] private GameObject judahCross;
    [SerializeField] private SliderScript healthBar;
    [SerializeField] private SliderScript expBar;
    [SerializeField] private SliderScript wepExpBar;
    [SerializeField] private TextMeshProUGUI playerLevel;
    [SerializeField] private TextMeshProUGUI playerHpUiValue;
    [SerializeField] private TextMeshProUGUI playerExpUiValue;
    [SerializeField] private TextMeshProUGUI wepExpUiValue;
    [SerializeField] private List<Image> playerLives;
    [SerializeField] private SpriteRenderer _judahBack;
    private bool _isJudahAquired;
    private Animator _animatorPlayer;
    [SerializeField] private Image invincibleStatus;
    private Animator _invincibilityAnimator;
    private PolygonCollider2D _polygonCollider2D;
    private AudioSource[] _audioSource;
    private bool _isInvincible;
    private int _jumpCounter;
    private int _currentHealth;
    private const int HpGainValue = 10;
    private const int ExpValue = 60;
    private const int MaxLevel = 3;
    private const int BaseLevelRequirement = 100;
    private const int NextLevelExpReqOffset = 50;
    private const int MaxLevelExpReq = BaseLevelRequirement + (MaxLevel - 1) * NextLevelExpReqOffset;
    private int _invincibilityCountdown;
    private int _extraPlayerLives;
    private float _judahRightRotationZ;
    private float _judahLeftRotationZ;
    private bool _hasAttacked;
    private bool _isMovingRight;


    // private int _playerLives;
    private int _playerLevel;
    private int _playerLevelUpReq;
    private int _weaponLevel;
    private int _weaponLevelUpReq;

    void Start()
    {
        // _playerLives = StartingPlayerLives;
        _judahBack.enabled = false;

        if (GameManager.GameManagerInstance != null)
            OnChangeSpecialBgm += GameManager.GameManagerInstance.ChangeToSpecialBgm;
        if (PlayerAttack.PlayerAttackInstance != null)
            OnWeaponCollected += PlayerAttack.PlayerAttackInstance.ObtainWeapon;
        _invincibilityAnimator = GameObject.FindGameObjectWithTag("InvincibleStatus").GetComponent<Animator>();
        _isMovingRight = true;
        _extraPlayerLives = 0;
        _playerLevel = 1;
        _weaponLevel = 1;
        _playerLevelUpReq = BaseLevelRequirement;
        _weaponLevelUpReq = BaseLevelRequirement;
        _animatorPlayer = GetComponent<Animator>();
        _audioSource = GetComponents<AudioSource>();
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
        _jumpCounter = 0;
        _currentHealth = MaxHealth;
        healthBar.SetMaxValue(_currentHealth);
        healthBar.SetValue(_currentHealth);
        expBar.SetMaxValue(BaseLevelRequirement);
        expBar.SetValue(0);
        wepExpBar.SetMaxValue(BaseLevelRequirement);
        wepExpBar.SetValue(0);
        _playerLevelUpReq = BaseLevelRequirement;
        _weaponLevelUpReq = BaseLevelRequirement;

        // SetIdleAnimationBooleans(BooleanDirectionRight, true);
    }

    void Update()
    {
        // var movementPlayerX = Input.GetAxis("Horizontal") * Time.deltaTime * SpeedPlayer;
        // if (movementPlayerX != 0)
        // {
        //     print("entered");
        //     transform.Translate(movementPlayerX, 0f, 0f);
        //     if (Input.GetKey(KeyMoveRight))
        //     {
        //         SetMovingAnimationBooleans(true, false);
        //     }
        //     else if (Input.GetKey(KeyMoveLeft))
        //     {
        //         SetMovingAnimationBooleans(false, true);
        //     }
        // }
        //
        // if (Input.GetKeyUp(KeyMoveRight))
        // {
        //     SetIdleAnimationBooleans(BooleanDirectionRight, true);
        // }
        // else if (Input.GetKeyUp(KeyMoveLeft))
        // {
        //     SetIdleAnimationBooleans(BooleanDirectionLeft, false);
        // }
        //
        // if (Input.GetKeyDown(KeyJump) && _jumpCounter < MaxJump)
        // {
        //     playerRigidBody2D.velocity = new Vector2(0f, JumpHeight);
        //     _jumpCounter++;
        //     _audioSource[SoundEffect1].Play();
        // }

        if (Input.GetKeyUp(KeyMoveRight) || Input.GetKeyUp(KeyMoveLeft) || !Input.GetKey(KeyMoveRight) &&
            !Input.GetKey(KeyMoveLeft))
        {
            SetIdle(true);
            // SetIdleAnimationBooleans(BooleanDirectionLeft, false);
        }

        if (Input.GetKeyDown(KeyJump) && _jumpCounter < MaxJump)
        {
            playerRigidBody2D.velocity = new Vector2(0f, JumpHeight);
            _jumpCounter++;
            _audioSource[SoundEffect1].Play();
        }
    }

    private void FixedUpdate()
    {
        var movementPlayerX = Input.GetAxis("Horizontal") * Time.deltaTime * SpeedPlayer;
        if (movementPlayerX != 0)
        {
            var rotation = _judahBack.transform.localRotation;
            // Debug.Log((rotation.z / 1).ToString());
            SetIdle(false);
            if (Input.GetKey(KeyMoveRight) && !_isMovingRight)
            {
                // SetMovingAnimationBooleans(true, false);
                // if (!_judahBack.enabled || rotation.z / 1 < 0)
                //     return;
                // RotateJudah(rotation);
                ChangeDirection();
            }
            else if (Input.GetKey(KeyMoveLeft) && _isMovingRight)
            {
                // SetMovingAnimationBooleans(false, true);
                // // if (_judahBack.enabled)
                // //     return;
                // if (!_judahBack.enabled || rotation.z / 1 > 0)
                //     return;
                // RotateJudah(rotation);
                Debug.Log("before" + movementPlayerX);

                ChangeDirection();
            }

            if (Input.GetKey(KeyMoveRight) && _isMovingRight)
                transform.Translate(movementPlayerX, 0f, 0f);
            else if (Input.GetKey(KeyMoveLeft) && !_isMovingRight)
            {
                transform.Translate(-movementPlayerX, 0f, 0f);
            }
        }
    }

    private void RotateJudah(Quaternion rotation)
    {
        rotation.z *= -1;
        _judahBack.transform.rotation = rotation;
    }

    // private void SetMovingAnimationBooleans(bool isMoveRight, bool isMoveLeft)
    // {
    //     _animatorPlayer.SetBool("isMovingToTheRight", isMoveRight);
    //     _animatorPlayer.SetBool("isMovingToTheLeft", isMoveLeft);
    //     _animatorPlayer.SetBool("isIdle", false);
    // }
    //
    // private void SetIdleAnimationBooleans(string booleanDirection, bool isIdleRight)
    // {
    //     _animatorPlayer.SetBool(booleanDirection, false);
    //     _animatorPlayer.SetBool("isIdleRight", isIdleRight);
    //     _animatorPlayer.SetBool("isIdle", true);
    // }

    private void SetIdle(bool isIdle)
    {
        _animatorPlayer.SetBool("isIdle", isIdle);
    }


    private void ChangeDirection()
    {
        transform.Rotate(0, 180, 0);
        if (_isMovingRight)
        {
            _isMovingRight = false;
        }
        else
        {
            _isMovingRight = true;
        }
    }


    private void TakeDamage(int damage)
    {
        if (_isInvincible || _currentHealth <= 0 && _extraPlayerLives == 0)
            return;
        // print("ive taken damage");
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            if (_extraPlayerLives > 0)
            {
                ModifyExtraLife(false, 0.5f);
                _currentHealth = healthBar.GetCurrentMaxValue();
            }
            else
            {
                //TODO gameover
                var manager = GameManager.GameManagerInstance;
                OnChangeSpecialBgm -= manager.ChangeToSpecialBgm;
                OnWeaponCollected -= PlayerAttack.PlayerAttackInstance.ObtainWeapon;
                print("Game Over");
            }
        }

        healthBar.SetValue(_currentHealth);
        SetBarTextValue(ref playerHpUiValue, _currentHealth.ToString(), healthBar.GetCurrentMaxValue().ToString());
    }

    //TODO : Fix the coroutine when the player is attacking
    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(DelayTime);
        // _judahCollider.enabled = false;
        // _hasAttacked = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Plateform":
            case "Ground":
            case "Obstacle":
                _jumpCounter = 0;
                break;

            case "Cookie":
            case "Enemy":
                // _audioSource[SoundEffect3].Play();
                TakeDamage(Damage);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Judah":
                _isJudahAquired = true;
                // Debug.Log("collected");
                if (OnWeaponCollected != null)
                {
                    // Debug.Log("entering method");
                    OnWeaponCollected();
                }

                _judahBack.enabled = true;
                // _judahRightRotationZ = _judahBack.transform.position.z;
                // _judahLeftRotationZ = -_judahRightRotationZ;
                break;
            case "InvincibleGourd":
                SetInvincibility();
                break;
            case "GreenGourd":
                GainExp(expBar, ExpValue, ref _playerLevelUpReq, ref _playerLevel, ref playerExpUiValue);
                break;
            case "Potion":
                GainExp(wepExpBar, ExpValue, ref _weaponLevelUpReq, ref _weaponLevel, ref wepExpUiValue);
                break;
            case "PinkGourd":
                GainExtraLife();
                break;
            case "Bandaid":
                GainHp(HpGainValue);
                break;
            case "NextLevel":
                var manager = GameManager.GameManagerInstance;
                if (GameManager.GameManagerInstance != null)
                    manager.OnLevelEndReached += manager.NextLevel;
                break;
        }
    }

    //TODO fix asap
    private void SetInvincibility()
    {
        OnChangeSpecialBgm?.Invoke(1);
        _isInvincible = true;
        _invincibilityCountdown = InvincibilityDuration;
        var tmp = invincibleStatus.color;
        tmp.a = 1f;
        invincibleStatus.color = tmp;
        _invincibilityAnimator.SetBool("isInvincible", true);
        Invoke(nameof(ReduceInvincibilityDuration), 1f);
        // StartCoroutine(ExpireInvincibility());
    }

    private void ReduceInvincibilityDuration()
    {
        var tmp = invincibleStatus.color;
        if (_invincibilityCountdown.Equals(0))
        {
            _invincibilityAnimator.SetBool("isInvincible", false);
            tmp.a = 0f;
            invincibleStatus.color = tmp;
            _isInvincible = false;
            print("ended");
            return;
        }

        --_invincibilityCountdown;
        tmp.a -= Offset;
        invincibleStatus.color = tmp;
        print("countdown " + _invincibilityCountdown.ToString() + " alpha " + tmp.a.ToString() + " invincibility: " +
              _isInvincible.ToString());
        Invoke(nameof(ReduceInvincibilityDuration), 1f);
    }

    private void GainExp(SliderScript bar, int expValue, ref int nextLevelExpReq, ref int currentBarLevel,
        ref TextMeshProUGUI textMeshProUGUI)
    {
        if (currentBarLevel >= MaxLevel)
            return;

        var tmp = bar.GetCurrentValue() + expValue;
        // print(tmp.ToString());
        if (tmp >= bar.GetCurrentMaxValue())
        {
            tmp -= nextLevelExpReq;
            bar.SetValue(tmp);
            nextLevelExpReq += NextLevelExpReqOffset;
            bar.SetMaxValue(nextLevelExpReq);
            ++currentBarLevel;
            // tmp = 0;
            if (bar.CompareTag("PlayerExpUI"))
            {
                playerLevel.text = currentBarLevel.ToString();
            }

            if (currentBarLevel >= MaxLevel)
            {
                print("entered max:" + currentBarLevel);
                bar.SetValue(bar.GetCurrentMaxValue());
                SetBarTextValue(ref textMeshProUGUI, MaxExpText, MaxExpText);
                return;
            }
        }
        else
        {
            bar.SetValue(tmp);
        }

        SetBarTextValue(ref textMeshProUGUI, tmp.ToString(), bar.GetCurrentMaxValue().ToString());
    }

    private void GainExtraLife()
    {
        if (_extraPlayerLives.Equals(playerLives.Count))
        {
            GainHp(healthBar.GetCurrentMaxValue());
            return;
        }

        // print(_extraPlayerLives.ToString());
        ModifyExtraLife(true, 1f);
    }

    private void ModifyExtraLife(bool isAddLife, float alphaValue)
    {
        var index = isAddLife ? _extraPlayerLives : _extraPlayerLives - 1;
        print(index.ToString());
        var tmp = playerLives[index].color;
        tmp.a = alphaValue;
        playerLives[index].color = tmp;
        if (_extraPlayerLives > playerLives.Count)
            return;
        _extraPlayerLives = isAddLife ? ++_extraPlayerLives : --_extraPlayerLives;
    }

    //TODO in class/ meeting
    private void GainHp(int value)
    {
        _currentHealth = healthBar.GetCurrentValue() + value >= healthBar.GetCurrentMaxValue()
            ? healthBar.GetCurrentMaxValue()
            : healthBar.GetCurrentValue() + value;
        healthBar.SetValue(_currentHealth);
        SetBarTextValue(ref playerHpUiValue, _currentHealth.ToString(), healthBar.GetCurrentMaxValue().ToString());
    }

    private void SetBarTextValue(ref TextMeshProUGUI textMeshProUGUI, string value, string maxValue)
    {
        var barValues = value + "/" + maxValue;
        textMeshProUGUI.text = barValues;
    }

    //TODO : Callback
    public void ResetJump()
    {
        _jumpCounter = 0;
    }
}