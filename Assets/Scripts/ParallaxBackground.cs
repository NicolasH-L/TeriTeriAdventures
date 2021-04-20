using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private const float ParallaxEffectMultiplier = 0.5f;
    private Transform _cameraTransform;
    private Vector3 _lastCameraPos;
    private Sprite _sprite;
    private Texture2D _texture;
    private float _textureUniteSizeX;

    void Start()
    {
        _cameraTransform = Camera.main.transform;
        _lastCameraPos = _cameraTransform.position;
        _sprite = GetComponent<SpriteRenderer>().sprite;
        _texture = _sprite.texture;
        _textureUniteSizeX = _texture.width / _sprite.pixelsPerUnit;
    }

    void FixedUpdate()
    {
        Vector3 deltaMovement = _cameraTransform.position - _lastCameraPos;
        transform.position -= deltaMovement * ParallaxEffectMultiplier;
        _lastCameraPos = _cameraTransform.position;
        if (Math.Abs(_cameraTransform.position.x - transform.position.x) >= _textureUniteSizeX)
        {
            var offsetPosX = (_cameraTransform.position.x - transform.position.x) % _textureUniteSizeX;
            transform.position = new Vector2(_cameraTransform.position.x + offsetPosX, transform.position.y);
        }

        print(_textureUniteSizeX);
    }
}