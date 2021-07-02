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

    // source: https://www.febucci.com/2018/08/easing-functions/
    public float EaseIn(float t)
    {
        return t * t;
    }
    static float Flip(float x)
    {
        return 1 - x;
    }

    public  float Spike(float t)
    {
        if (t <= .5f)
            return EaseIn(t / .5f);

        return EaseIn(Flip(t) / .5f);
    }

    private IEnumerator ShowRoutine()
    {
        float duration = 0.2f;
        float lapsTime = 0.0f;
        Vector3 scale = new Vector3(1, 0.1f, 1);
        while (scale.y <= 1)
        {
            _button3d.localScale = scale;
            scale = Vector3.Slerp(new Vector3(1, 0.1f, 1), Vector3.one, lapsTime / duration);
            lapsTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        //duration = 0.2f;
        //lapsTime = 0.02f;
        ////scale = new Vector3(1, 0.1f, 1);
        //while (scale.y > 1)
        //{
        //    _button3d.localScale = scale;
        //    scale = Vector3.Slerp(Vector3.one, new Vector3(1, 1.2f, 1), Spike(lapsTime / duration));
        //    lapsTime += Time.deltaTime;
        //    yield return new WaitForEndOfFrame();
        //}
    }
}
