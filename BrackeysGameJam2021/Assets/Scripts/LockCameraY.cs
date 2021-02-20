using UnityEngine;
using Cinemachine;

/// <summary>
/// An add-on module for Cinemachine Virtual Camera that locks the camera's Z co-ordinate
/// </summary>
[ExecuteInEditMode][SaveDuringPlay][AddComponentMenu("")] // Hide in menu
public class LockCameraY : CinemachineExtension
{
    [Tooltip("Lock the camera's Y position to this value")]
    public float YPosition = 10f;

    public float XRotation;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, 
        ref CameraState state, 
        float deltaTime)
    {
        if (this.enabled && stage == CinemachineCore.Stage.Body)
        {
            var pos = state.RawPosition;
            pos.y = this.YPosition;
            state.RawPosition = pos;

            Quaternion rot = state.RawOrientation;
            Vector3 rotVec = rot.eulerAngles;
            rotVec.x = this.XRotation;
            state.RawOrientation = Quaternion.Euler(rotVec);
        }
    }
}