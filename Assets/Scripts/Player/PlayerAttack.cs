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

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        //TODO : Fix attacking
        if (Input.GetKey("j") && !_hasAttacked)
        {
            _audioSource[SoundEffect2].Play();
            _judahCollider.enabled = true;
            _hasAttacked = true;
            // StartCoroutine(Delay());
        }
        else if (Input.GetKeyUp("j"))
        {
            _judahBack.enabled = true;
        }
    }
}
