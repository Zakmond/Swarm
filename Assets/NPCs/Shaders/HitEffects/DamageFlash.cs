using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Color _flashColor = Color.red;
    [SerializeField] private float _flashTime = 0.5f;

    private SpriteRenderer _spriteRenderer;
    private Material _material;
    private Coroutine _damageFlashCoroutine;
    // Start is called before the first frame update
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Init();
    }
    private void Init()
    {
        _material = _spriteRenderer.material;
    }

    public void CallFlash()
    {
        Debug.Log("Calling Flash");
        _damageFlashCoroutine = StartCoroutine(Flash());
    }
    private IEnumerator Flash()
    {
        Debug.Log("Called Flash");

        SetFlashColor();

        float currentFlashAmount = 0f;
        float elapsedTime = 0f;
        while (elapsedTime < _flashTime)
        {

            elapsedTime += Time.deltaTime;

            currentFlashAmount = Mathf.Lerp(1f, 0f, (elapsedTime / _flashTime));
            SetFlashAmount(currentFlashAmount);


            yield return null;
        }
    }
    private void SetFlashColor()
    {
        _material.SetColor("_FlashColor", _flashColor);
    }

    private void SetFlashAmount(float amount)
    {
        _material.SetFloat("_FlashAmount", amount);
    }
}
