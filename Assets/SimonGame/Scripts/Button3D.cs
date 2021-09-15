using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Button3D : MonoBehaviour
{
    public static Action<Button3D> OnClick;
    public static Action<Button3D> OnClickDone;

    [SerializeField] private Transform _button3d;
    [SerializeField] private Renderer _buttonRenderer;
    [SerializeField] private AudioSource _audio;

    private Camera _mainCam;
    private Transform _transform;
    private Coroutine _buttonClickRoutine;

    private Vector3 _origPos;
    private Color _origColor;

    private void Awake()
    {
        _mainCam = Camera.main;
        _transform = transform;

        _origPos = _button3d.localPosition;
        _origColor = _buttonRenderer.material.color;
    }

    private void OnDisable()
    {
        if (_buttonClickRoutine != null)
        {
            StopCoroutine(_buttonClickRoutine);
        }
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
            if (_buttonClickRoutine != null)
            {
                StopCoroutine(_buttonClickRoutine);
            }

            _buttonClickRoutine = StartCoroutine(ClickRoutine());
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

        float speed = 5.0f;

        Vector3 pressPos = _origPos - (Vector3.up * 0.3f);
        _button3d.localPosition = pressPos;

        float hue, saturation, colorValue;
        Color.RGBToHSV(_origColor, out hue, out saturation, out colorValue);

        saturation *= 2.0f;
        colorValue *= 0.8f;

        Color pressColor = Color.HSVToRGB(hue, saturation, colorValue);
        _buttonRenderer.material.SetColor("_Color", pressColor);

        float t = 0;
        while (t < 0.9f)
        {
            yield return null; //new WaitForSeconds(0.05f);

            Color deltaColor = Color.Lerp(pressColor, _origColor, t * t);
            Vector3 deltaPos = Vector3.Lerp(pressPos, _origPos, t * t);

            _button3d.localPosition = deltaPos;
            _buttonRenderer.material.SetColor("_Color", deltaColor);

            t += Time.deltaTime * speed;
        }

        _button3d.localPosition = _origPos;
        _buttonRenderer.material.SetColor("_Color", _origColor);
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
