using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerScript : MonoBehaviour
{
    private const string MaxExpText = "MAX";
    private const string KeyMoveRight = "d";
    private const string KeyMoveLeft = "a";
    private const string KeyJump = "space";
    private const string BooleanDirectionRight = "isMovingToTheRight";
    private const string BooleanDirectionLeft = "isMovingToTheLeft";
    private const float SpeedPlayer = 7f;
    private const float JumpHeight = 8f;
    private const float ForceAppliedAttacking = -1000f;
    private const float ForceAppliedRetracting = 950f;
    private const float DelayTime = 0.4f;
    private const int MaxJump = 2;
    private const int SoundEffect1 = 0;
    private const int SoundEffect2 = 1;
    private const int SoundEffect3 = 2;
    private const int MaxHealth = 100;
    private const int Damage = 10;
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

    private Animator _animatorPlayer;
    private PolygonCollider2D _polygonCollider2D;
    private AudioSource[] _audioSource;
    private HingeJoint2D _hingeJoint2D;
    private JointMotor2D _jointMotor2D;
    private Collider2D _judahCollider;
    private bool _isInvincible;
    private bool _hasAttacked;
    private int _jumpCounter;
    private int _currentHealth;
    private const int HpGainValue = 10;
    private const int ExpValue = 60;
    private const int MaxLevel = 3;
    private const int BaseLevelRequirement = 100;
    private const int NextLevelExpReqOffset = 50;
    private const int MaxLevelExpReq = BaseLevelRequirement + (MaxLevel - 1) * NextLevelExpReqOffset;

    private int _extraPlayerLives;

    // private int _playerLives;
    private int _playerLevel;
    private int _playerLevelUpReq;
    private int _weaponLevel;
    private int _weaponLevelUpReq;

    void Start()
    {
        // _playerLives = StartingPlayerLives;
        _extraPlayerLives = 0;
        _playerLevel = 1;
        _weaponLevel = 1;
        _playerLevelUpReq = BaseLevelRequirement;
        _weaponLevelUpReq = BaseLevelRequirement;
        _animatorPlayer = GetComponent<Animator>();
        _hingeJoint2D = GetComponent<HingeJoint2D>();
        _audioSource = GetComponents<AudioSource>();
        _judahCollider = judahCross.GetComponent<Collider2D>();
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
        _jointMotor2D = _hingeJoint2D.motor;
        _judahCollider.enabled = false;
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
        SetIdleAnimationBooleans(BooleanDirectionRight, true);
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
        if (Input.GetKeyUp(KeyMoveRight))
        {
            SetIdleAnimationBooleans(BooleanDirectionRight, true);
        }
        else if (Input.GetKeyUp(KeyMoveLeft))
        {
            SetIdleAnimationBooleans(BooleanDirectionLeft, false);
        }

        if (Input.GetKeyDown(KeyJump) && _jumpCounter < MaxJump)
        {
            playerRigidBody2D.velocity = new Vector2(0f, JumpHeight);
            _jumpCounter++;
            _audioSource[SoundEffect1].Play();
        }

        //TODO : Fix attacking
        if (Input.GetKey("j") && !_hasAttacked)
        {
            _audioSource[SoundEffect2].Play();
            _jointMotor2D.motorSpeed = ForceAppliedAttacking;
            _hingeJoint2D.motor = _jointMotor2D;
            _judahCollider.enabled = true;
            _hasAttacked = true;
            StartCoroutine(Delay());
        }
    }

    private void FixedUpdate()
    {
        var movementPlayerX = Input.GetAxis("Horizontal") * Time.deltaTime * SpeedPlayer;
        if (movementPlayerX != 0)
        {
            transform.Translate(movementPlayerX, 0f, 0f);
            if (Input.GetKey(KeyMoveRight))
            {
                SetMovingAnimationBooleans(true, false);
            }
            else if (Input.GetKey(KeyMoveLeft))
            {
                SetMovingAnimationBooleans(false, true);
            }
        }
    }

    private void SetMovingAnimationBooleans(bool isMoveRight, bool isMoveLeft)
    {
        _animatorPlayer.SetBool("isMovingToTheRight", isMoveRight);
        _animatorPlayer.SetBool("isMovingToTheLeft", isMoveLeft);
        _animatorPlayer.SetBool("isIdle", false);
    }

    private void SetIdleAnimationBooleans(string booleanDirection, bool isIdleRight)
    {
        _animatorPlayer.SetBool(booleanDirection, false);
        _animatorPlayer.SetBool("isIdleRight", isIdleRight);
        _animatorPlayer.SetBool("isIdle", true);
    }

    private void TakeDamage(int damage)
    {
        if (_isInvincible)
            return;
        // print("ive taken damage");
        _currentHealth -= damage;
        healthBar.SetValue(_currentHealth);
        SetBarTextValue(ref playerHpUiValue, _currentHealth.ToString(), healthBar.GetCurrentMaxValue().ToString());
    }

    //TODO : Fix the coroutine when the player is attacking
    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(DelayTime);
        _jointMotor2D.motorSpeed = ForceAppliedRetracting;
        _hingeJoint2D.motor = _jointMotor2D;
        _judahCollider.enabled = false;
        _hasAttacked = false;
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
                manager.OnLevelEndReached += manager.NextLevel;
                break;
        }
    }

    //TODO fix asap
    private void SetInvincibility()
    {
        _isInvincible = true;
        StartCoroutine(ExpireInvincibility());
    }

    private IEnumerator ExpireInvincibility()
    {
        yield return new WaitForSeconds(8);
        _isInvincible = false;
    }

    private void GainExp(SliderScript bar, int expValue, ref int nextLevelExpReq, ref int currentBarLevel,
        ref TextMeshProUGUI textMeshProUGUI)
    {
        if (currentBarLevel >= MaxLevel)
            return;

        var tmp = bar.GetCurrentValue() + expValue;
        print(tmp.ToString());
        if (tmp >= bar.GetCurrentMaxValue())
        {
            bar.SetValue(tmp - nextLevelExpReq);
            nextLevelExpReq += NextLevelExpReqOffset;
            bar.SetMaxValue(nextLevelExpReq);
            ++currentBarLevel;
            tmp = 0;
            if (bar.CompareTag("PlayerExpUI"))
            {
                playerLevel.text = currentBarLevel.ToString();
            }

            if (currentBarLevel >= MaxLevel)
            {
                print("entered max:" +  currentBarLevel);
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

        print(_extraPlayerLives.ToString());
        var tmp = playerLives[_extraPlayerLives].color;
        tmp.a = 1f;
        playerLives[_extraPlayerLives].color = tmp;
        ++_extraPlayerLives;
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