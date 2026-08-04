using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEditor.Experimental.GraphView;
using System;
public class CameraControl : MonoBehaviour
{
    public VoidEventSO afterSceneLoadingEvent;
    private CinemachineConfiner2D confiner2D;
    public CinemachineImpulseSource impulseSource;
    public VoidEventSO cameraShakeEvent;
    private void Awake()
    {
        confiner2D = GetComponent<CinemachineConfiner2D>();
    }

    //private void Start()
    //{
    //    GetNewCameraBounds();
    //}

    public void OnEnable()
    {
        cameraShakeEvent.OnEventRaised += OnCameraShake;
        afterSceneLoadingEvent.OnEventRaised += OnAfterfterSceneLoadingEvent;
    }

    private void OnDisable()
    {
        cameraShakeEvent.OnEventRaised -= OnCameraShake;
        afterSceneLoadingEvent.OnEventRaised -= OnAfterfterSceneLoadingEvent;
    }

    private void OnAfterfterSceneLoadingEvent()
    {
        GetNewCameraBounds();
    }

    private void OnCameraShake()
    {
        impulseSource.GenerateImpulse();
    }

    //TODO:当玩家进入新的场景时，调用GetNewCameraBounds方法来更新摄像机的边界

    private void GetNewCameraBounds()
    {
        var obj = GameObject.FindGameObjectWithTag("CameraBounds");
        if (obj == null)
        {
            return;
        }
        else
        {
            confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();
            confiner2D.InvalidateCache();
        }
    }
}
