using System;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Compilation;
#endif

namespace Editor
{
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
            var threadGroupSizes = GetThreadGroupSizes(cs, kernelIndex);
            var numGroupsX = Mathf.CeilToInt(numIterationsX / (float) threadGroupSizes.x);
            var numGroupsY = Mathf.CeilToInt(numIterationsY / (float) threadGroupSizes.y);
            var numGroupsZ = Mathf.CeilToInt(numIterationsZ / (float) threadGroupSizes.y);
            cs.Dispatch(kernelIndex, numGroupsX, numGroupsY, numGroupsZ);
        }

        // Set all values from settings object on the shader. Note, variable names must be an exact match in the shader.
        // Settings object can be any class/struct containing vectors/ints/floats/bools
        public static void SetParams(object settings, ComputeShader shader, string variableNamePrefix = "",
            string variableNameSuffix = "")
        {
            var fields = settings.GetType().GetFields();
            foreach (var field in fields)
            {
                var fieldType = field.FieldType;
                var shaderVariableName = variableNamePrefix + field.Name + variableNameSuffix;

                if (fieldType == typeof(Vector4) || fieldType == typeof(Vector3) || fieldType == typeof(Vector2))
                    shader.SetVector(shaderVariableName, (Vector4) field.GetValue(settings));
                else if (fieldType == typeof(int))
                    shader.SetInt(shaderVariableName, (int) field.GetValue(settings));
                else if (fieldType == typeof(float))
                    shader.SetFloat(shaderVariableName, (float) field.GetValue(settings));
                else if (fieldType == typeof(bool))
                    shader.SetBool(shaderVariableName, (bool) field.GetValue(settings));
                else
                    Debug.Log($"Type {fieldType} not implemented");
            }
        }

        public static void CreateStructuredBuffer<T>(ref ComputeBuffer buffer, int count)
        {
            var stride = Marshal.SizeOf(typeof(T));
            var createNewBuffer =
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

        public static ComputeBuffer CreateAndSetBuffer<T>(T[] data, ComputeShader cs, string nameID,
            int kernelIndex = 0)
        {
            ComputeBuffer buffer = null;
            CreateAndSetBuffer(ref buffer, data, cs, nameID, kernelIndex);
            return buffer;
        }

        public static void CreateAndSetBuffer<T>(ref ComputeBuffer buffer, T[] data, ComputeShader cs, string nameID,
            int kernelIndex = 0)
        {
            var stride = Marshal.SizeOf(typeof(T));
            CreateStructuredBuffer<T>(ref buffer, data.Length);
            buffer.SetData(data);
            cs.SetBuffer(kernelIndex, nameID, buffer);
        }

        public static ComputeBuffer CreateAndSetBuffer<T>(int length, ComputeShader cs, string nameID,
            int kernelIndex = 0)
        {
            ComputeBuffer buffer = null;
            CreateAndSetBuffer<T>(ref buffer, length, cs, nameID, kernelIndex);
            return buffer;
        }

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
        public static float[] PackFloats(params float[] values)
        {
            var packed = new float[values.Length * 4];
            for (var i = 0; i < values.Length; i++)
            {
                packed[i * 4] = values[i];
            }

            return values;
        }

        private static bool CheckIfCanRunInEditMode()
        {
            var isCompilingOrExitingEditMode = false;
#if UNITY_EDITOR
            isCompilingOrExitingEditMode |= EditorApplication.isCompiling;
            isCompilingOrExitingEditMode |= playModeState == PlayModeStateChange.ExitingEditMode;
#endif
            var canRun = !isCompilingOrExitingEditMode;
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
}