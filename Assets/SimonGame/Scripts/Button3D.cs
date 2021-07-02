using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Button3D : MonoBehaviour
{
    public static Action<Button3D> OnClick;
    public static Action<Button3D> OnClickDone;

    [SerializeField] private Transform _button3d;
    [SerializeField] private AudioSource _audio;

    private Camera _mainCam;
    private Transform _transform;

    private void Awake()
    {
        _mainCam = Camera.main;
        _transform = transform;
    }

    public void OnInteract(InputValue value)
    {
        if (!value.isPressed)
        {
            return;
        }

        Ray ray = _mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && hit.transform == _transform)
        {
            //Debug.Log("interact " + gameObject.name);
            OnClick?.Invoke(this);
            StartCoroutine(ClickRoutine());
        }
    }

    public void InteractWithoutNotify()
    {
        StartCoroutine(ClickRoutine());
    }

    public void Show()
    {
        gameObject.SetActive(true);
        StartCoroutine(ShowRoutine());
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator ClickRoutine()
    {
        if (_audio != null)
        {
            _audio.Play();
        }

        Vector3 origPos = _button3d.localPosition;
        _button3d.localPosition -= Vector3.up * 0.2f;
        yield return new WaitForSeconds(0.05f);
        _button3d.localPosition = origPos;
        OnClickDone?.Invoke(this);
    }

    private IEnumerator ShowRoutine()
    {
        _button3d.localScale = Vector3.zero;
        while (_button3d.localScale.magnitude < 1.3f)
        {
            _button3d.localScale += Vector3.one * 0.02f;
            yield return new WaitForEndOfFrame();
        }

        while (_button3d.localScale.magnitude > 1)
        {
            _button3d.localScale -= Vector3.one * 0.005f;
            yield return new WaitForEndOfFrame();
        }

        _button3d.localScale = Vector3.one;
    }
}
