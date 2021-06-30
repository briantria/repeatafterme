using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Button3D : MonoBehaviour
{
    public static Action<Button3D> OnClick;

    [SerializeField] private Transform _button3d;

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
            StartCoroutine(ClickRoutine());
            OnClick?.Invoke(this);
        }
    }

    private IEnumerator ClickRoutine()
    {
        Vector3 origPos = _button3d.localPosition;
        _button3d.localPosition -= Vector3.up * 0.2f;
        yield return new WaitForSeconds(0.05f);
        _button3d.localPosition = origPos;
    }
}
