using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private bool _hasAttacked;
    private const int SoundEffect2 = 1;
    private AudioSource[] _audioSource;
    private Collider2D _judahCollider;
    [SerializeField] private SpriteRenderer _judahBack;
    private const float DelayTime = 0.4f;

    private Animator _animatorPlayer;


    // Start is called before the first frame update
    void Start()
    {
        _animatorPlayer = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //TODO : Fix attacking

        if (Input.GetKey("j") && !_hasAttacked)
        {
            Attack();
        }
        else if (Input.GetKeyUp("j"))
        {
            _judahBack.enabled = true;
        }
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(DelayTime);
        // _judahCollider.enabled = false;
        // _hasAttacked = false;
    }

    private void Attack()
    {
        _animatorPlayer.SetTrigger("Attack");
        _animatorPlayer.SetBool("isMelee", true);
        _audioSource[SoundEffect2].Play();
        _judahCollider.enabled = true;
        _hasAttacked = true;
        StartCoroutine(Delay());
    }
}