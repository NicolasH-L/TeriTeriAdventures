using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public delegate void ChangeSpecialBgm(int bgmOption);

    public event ChangeSpecialBgm OnChangeSpecialBgm;

    public delegate void WeaponEvent();

    public event WeaponEvent OnWeaponCollected;

    public delegate void GameFinished(bool isDead);

    public event GameFinished OnGameEnded;

    private static PlayerScript _player;

    public static PlayerScript GetPlayerInstance => _player;


    private const string MaxExpText = "MAX";
    private const string KeyMoveRight = "d";
    private const string KeyMoveLeft = "a";
    private const string KeyJump = "space";
    private const int MaxJump = 2;
    private const int SoundEffect1 = 0;
    private const int PlayerHitAudioSourceIndex = 3;
    private const int MaxHealth = 100;
    private const int WeaponBaseDamage = 5000;
    private const int ContactDamage = 25;
    private const int InvincibilityDuration = 8;
    private const float BasePlayerSpeed = 4f;
    private const float TeriTicketPlayerSpeedBoost = 4f;
    private const float JumpHeight = 8f;
    private const float DelayTime = 0.5f;
    private const float Offset = 1f / InvincibilityDuration;

    [SerializeField] private Rigidbody2D playerRigidBody2D;

    // [SerializeField] private GameObject judahCross;
    [SerializeField] private SliderScript healthBar;
    [SerializeField] private SliderScript expBar;
    [SerializeField] private SliderScript wepExpBar;
    [SerializeField] private TextMeshProUGUI playerLevel;
    [SerializeField] private TextMeshProUGUI playerHpUiValue;
    [SerializeField] private TextMeshProUGUI playerExpUiValue;
    [SerializeField] private TextMeshProUGUI wepExpUiValue;
    [SerializeField] private List<Image> playerLives;
    [SerializeField] private SpriteRenderer _judahBack;
    private const string TeriTicket = "TeriTicket";
    private const string Judah = "Judah";
    private const string PinkGourd = "PinkGourd";
    private const string GreenGourd = "GreenGourd";
    private const string InvincibleGourd = "InvincibleGourd";
    private const string Potion = "Potion";
    private const string Bandaid = "Bandaid";
    private const string NextLevel = "NextLevel";
    private const string InvincibilityAnimatorBool = "isInvincible";
    private const string PlayerExpUiTag = "PlayerExpUI";
    private const string WepExpUiTag = "WepExpUI";
    private const int FinalLevelScene = 3;

    private Animator _animatorPlayer;
    [SerializeField] private Image invincibleStatus;
    private Animator _invincibilityAnimator;
    private AudioSource[] _audioSource;
    private bool _isInvincible;
    private int _jumpCounter;
    private int _currentHealth;
    private const int HpGainValue = 10;
    private const int ExpValue = 20;
    private const int MaxLevel = 3;
    private const int BaseLevelRequirement = 100;
    private const int NextLevelExpReqOffset = 50;
    private int _invincibilityCountdown;
    private int _extraPlayerLives;
    private float _judahRightRotationZ;
    private float _judahLeftRotationZ;
    private float _playerSpeed;
    private bool _hasAttacked;
    private bool _isMovingRight;
    private bool _isHurtSoundPlayed;
    private int _weaponDamage;
    private int _playerLevel;
    private int _playerLevelUpReq;
    private int _weaponLevel;
    private int _weaponLevelUpReq;
    private static readonly int IsInvincible = Animator.StringToHash(InvincibilityAnimatorBool);
    private static readonly int IsIdle = Animator.StringToHash("isIdle");

    private void Awake()
    {
        if (_player != null && _player != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _player = this;
        }
    }

    private void Start()
    {
        // _playerLives = StartingPlayerLives;
        _judahBack.enabled = false;

        if (GameManager.GameManagerInstance != null)
        {
            OnChangeSpecialBgm += GameManager.GameManagerInstance.ChangeToSpecialBgm;
            OnGameEnded += GameManager.GameManagerInstance.GameOver;
        }

        if (PlayerAttack.PlayerAttackInstance != null)
            OnWeaponCollected += PlayerAttack.PlayerAttackInstance.ObtainWeapon;
        _invincibilityAnimator = GameObject.FindGameObjectWithTag("InvincibleStatus").GetComponent<Animator>();
        _weaponDamage = WeaponBaseDamage;
        _isMovingRight = true;
        _extraPlayerLives = 0;
        _playerSpeed = BasePlayerSpeed;
        _playerLevel = 1;
        _weaponLevel = 1;
        _playerLevelUpReq = BaseLevelRequirement;
        _weaponLevelUpReq = BaseLevelRequirement;
        _animatorPlayer = GetComponent<Animator>();
        _audioSource = GetComponents<AudioSource>();
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
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyMoveRight) || Input.GetKeyUp(KeyMoveLeft) || !Input.GetKey(KeyMoveRight) &&
            !Input.GetKey(KeyMoveLeft))
        {
            SetIdle(true);
        }

        if (!Input.GetKeyDown(KeyJump) || _jumpCounter >= MaxJump) return;
        playerRigidBody2D.velocity = new Vector2(0f, JumpHeight);
        _jumpCounter++;
        _audioSource[SoundEffect1].Play();
    }

    private void FixedUpdate()
    {
        if (!Input.GetKey(KeyMoveLeft) && !Input.GetKey(KeyMoveRight))
            return;
        var movementPlayerX = Input.GetAxis("Horizontal") * Time.deltaTime * _playerSpeed;
        if (movementPlayerX == 0) return;
        SetIdle(false);
        if (Input.GetKey(KeyMoveRight) && !_isMovingRight && !Input.GetKey(KeyMoveLeft))
        {
            ChangeDirection();
        }
        else if (Input.GetKey(KeyMoveLeft) && _isMovingRight && !Input.GetKey(KeyMoveRight))
        {
            ChangeDirection();
        }

        if (Input.GetKey(KeyMoveRight) && _isMovingRight)
            transform.Translate(movementPlayerX, 0f, 0f);
        else if (Input.GetKey(KeyMoveLeft) && !_isMovingRight)
        {
            transform.Translate(-movementPlayerX, 0f, 0f);
        }
    }

    private void SetIdle(bool isIdle)
    {
        _animatorPlayer.SetBool(IsIdle, isIdle);
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


    public void TakeDamage(int damage)
    {
        if (_isInvincible || _currentHealth <= 0 && _extraPlayerLives == 0)
            return;
        if (!_isHurtSoundPlayed)
        {
            _audioSource[PlayerHitAudioSourceIndex].Play();
            StartCoroutine(DelayNextHurtSound());
        }

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
                var manager = GameManager.GameManagerInstance;
                if (manager == null)
                    return;
                OnChangeSpecialBgm -= manager.ChangeToSpecialBgm;
                OnWeaponCollected -= PlayerAttack.PlayerAttackInstance.ObtainWeapon;
                OnGameEnded?.Invoke(true);
            }
        }

        healthBar.SetValue(_currentHealth);
        SetBarTextValue(ref playerHpUiValue, _currentHealth.ToString(), healthBar.GetCurrentMaxValue().ToString());
    }

    private IEnumerator DelayNextHurtSound()
    {
        yield return new WaitForSeconds(DelayTime);
        _isHurtSoundPlayed = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Plateform":
            case "Ground":
            case "Obstacle":
                ResetJump();
                break;
            case "Enemy":
            case "Cookie":
                // TakeDamage(ContactDamage);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case Judah:
                OnWeaponCollected?.Invoke();
                _judahBack.enabled = true;
                break;
            case InvincibleGourd:
                SetInvincibility();
                break;
            case GreenGourd:
                GainExp(expBar, ExpValue, ref _playerLevelUpReq, ref _playerLevel, ref playerExpUiValue);
                break;
            case Potion:
                GainExp(wepExpBar, ExpValue, ref _weaponLevelUpReq, ref _weaponLevel, ref wepExpUiValue);
                break;
            case PinkGourd:
                GainExtraLife();
                break;
            case Bandaid:
                GainHp(HpGainValue);
                break;
            case TeriTicket:
                GainSpeed();
                break;
            case NextLevel:
                var manager = GameManager.GameManagerInstance;
                if (GameManager.GameManagerInstance != null)
                    manager.OnLevelEndReached += manager.NextLevel;
                break;
        }
    }


    private void GainSpeed()
    {
        _playerSpeed += TeriTicketPlayerSpeedBoost;
        Invoke(nameof(DecreaseSpeed), 1f);
    }

    private void DecreaseSpeed()
    {
        if (_playerSpeed.Equals(BasePlayerSpeed))
            return;
        --_playerSpeed;
        Invoke(nameof(DecreaseSpeed), 1f);
    }

    private void SetInvincibility()
    {
        OnChangeSpecialBgm?.Invoke(1);
        _isInvincible = true;
        _invincibilityCountdown = InvincibilityDuration;
        var tmp = invincibleStatus.color;
        tmp.a = 1f;
        invincibleStatus.color = tmp;
        _invincibilityAnimator.SetBool(IsInvincible, true);
        Invoke(nameof(ReduceInvincibilityDuration), 1f);
    }

    private void ReduceInvincibilityDuration()
    {
        var tmp = invincibleStatus.color;
        if (_invincibilityCountdown.Equals(0))
        {
            _invincibilityAnimator.SetBool(IsInvincible, false);
            tmp.a = 0f;
            invincibleStatus.color = tmp;
            _isInvincible = false;
            return;
        }

        --_invincibilityCountdown;
        tmp.a -= Offset;
        invincibleStatus.color = tmp;
        Invoke(nameof(ReduceInvincibilityDuration), 1f);
    }

    private void GainExp(SliderScript bar, int expValue, ref int nextLevelExpReq, ref int currentBarLevel,
        ref TextMeshProUGUI textMeshProUGUI)
    {
        if (currentBarLevel >= MaxLevel)
            return;

        var tmp = bar.GetCurrentValue() + expValue;
        if (tmp >= bar.GetCurrentMaxValue())
        {
            tmp -= nextLevelExpReq;
            bar.SetValue(tmp);
            nextLevelExpReq += NextLevelExpReqOffset;
            bar.SetMaxValue(nextLevelExpReq);
            ++currentBarLevel;
            if (bar.CompareTag(PlayerExpUiTag))
            {
                playerLevel.text = currentBarLevel.ToString();
            }
            else if (bar.CompareTag(WepExpUiTag))
            {
                _weaponDamage += WeaponBaseDamage;
            }

            if (currentBarLevel >= MaxLevel)
            {
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

        ModifyExtraLife(true, 1f);
    }

    private void ModifyExtraLife(bool isAddLife, float alphaValue)
    {
        var index = isAddLife ? _extraPlayerLives : _extraPlayerLives - 1;
        var tmp = playerLives[index].color;
        tmp.a = alphaValue;
        playerLives[index].color = tmp;
        if (_extraPlayerLives > playerLives.Count)
            return;
        _extraPlayerLives = isAddLife ? ++_extraPlayerLives : --_extraPlayerLives;
    }

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

    private void ResetJump()
    {
        _jumpCounter = 0;
    }

    public int GetWeaponDamage()
    {
        return _weaponDamage;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().buildIndex <= FinalLevelScene) return;
        Destroy(invincibleStatus);
        Destroy(gameObject);
    }
}