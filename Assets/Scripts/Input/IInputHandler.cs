using UnityEngine;

public interface IInputHandler
{
    void OnInputStart(Vector3 screenPosition);
    void OnInputDrag(Vector3 screenPosition);
    void OnInputRelease(Vector3 screenPosition);
}