using System;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Compilation;
#endif

// Created by Sebastian Lague
// https://www.youtube.com/channel/UCmtyQOKKmrMVaKuRXz02jbQ
public static class ComputeHelper
{
    // Only run compute shaders if this is true
    // This is only relevant for compute shaders that run outside of playmode
    public static bool CanRunEditModeCompute => CheckIfCanRunInEditMode();

    // Subscribe to this event to be notified when buffers created in edit mode should be released
    // (i.e before script compilation occurs, and when exitting edit mode)
    public static event Action shouldReleaseEditModeBuffers;

    // Convenience method for dispatching a compute shader.
    // It calculates the number of thread groups based on the number of iterations needed.
    public static void Run(ComputeShader cs, int numIterationsX, int numIterationsY = 1, int numIterationsZ = 1,
        int kernelIndex = 0)
    {
        Vector3Int threadGroupSizes = GetThreadGroupSizes(cs, kernelIndex);
        int numGroupsX = Mathf.CeilToInt(numIterationsX / (float) threadGroupSizes.x);
        int numGroupsY = Mathf.CeilToInt(numIterationsY / (float) threadGroupSizes.y);
        int numGroupsZ = Mathf.CeilToInt(numIterationsZ / (float) threadGroupSizes.y);
        cs.Dispatch(kernelIndex, numGroupsX, numGroupsY, numGroupsZ);
    }

    // Set all values from settings object on the shader. Note, variable names must be an exact match in the shader.
    // Settings object can be any class/struct containing vectors/ints/floats/bools

    public static void CreateStructuredBuffer<T>(ref ComputeBuffer buffer, int count)
    {
        int stride = Marshal.SizeOf(typeof(T));
        bool createNewBuffer =
            buffer == null || !buffer.IsValid() || buffer.count != count || buffer.stride != stride;
        if (createNewBuffer)
        {
            Release(buffer);
            buffer = new ComputeBuffer(count, stride);
        }
    }

    public static void CreateStructuredBuffer<T>(ref ComputeBuffer buffer, T[] data)
    {
        CreateStructuredBuffer<T>(ref buffer, data.Length);
        buffer.SetData(data);
    }


    // Test

    public static void CreateAndSetBuffer<T>(ref ComputeBuffer buffer, int length, ComputeShader cs, string nameID,
        int kernelIndex = 0)
    {
        CreateStructuredBuffer<T>(ref buffer, length);
        cs.SetBuffer(kernelIndex, nameID, buffer);
    }

    // Releases supplied buffer/s if not null
    public static void Release(params ComputeBuffer[] buffers)
    {
        for (var i = 0; i < buffers.Length; i++)
        {
            if (buffers[i] != null)
                buffers[i].Release();
        }
    }

    public static Vector3Int GetThreadGroupSizes(ComputeShader compute, int kernelIndex = 0)
    {
        uint x, y, z;
        compute.GetKernelThreadGroupSizes(kernelIndex, out x, out y, out z);
        return new Vector3Int((int) x, (int) y, (int) z);
    }

    // https://cmwdexint.com/2017/12/04/computeshader-setfloats/

    private static bool CheckIfCanRunInEditMode()
    {
        var isCompilingOrExitingEditMode = false;
#if UNITY_EDITOR
        isCompilingOrExitingEditMode |= EditorApplication.isCompiling;
        isCompilingOrExitingEditMode |= playModeState == PlayModeStateChange.ExitingEditMode;
#endif
        bool canRun = !isCompilingOrExitingEditMode;
        return canRun;
    }

    // Editor helpers:

#if UNITY_EDITOR
    private static PlayModeStateChange playModeState;

    static ComputeHelper()
    {
        // Monitor play mode state
        EditorApplication.playModeStateChanged -= MonitorPlayModeState;
        EditorApplication.playModeStateChanged += MonitorPlayModeState;
        // Monitor script compilation
        CompilationPipeline.compilationStarted -= OnCompilationStarted;
        CompilationPipeline.compilationStarted += OnCompilationStarted;
    }

    private static void MonitorPlayModeState(PlayModeStateChange state)
    {
        playModeState = state;
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            if (shouldReleaseEditModeBuffers != null)
                shouldReleaseEditModeBuffers(); //
        }
    }

    private static void OnCompilationStarted(object obj)
    {
        if (shouldReleaseEditModeBuffers != null) shouldReleaseEditModeBuffers();
    }
#endif
}