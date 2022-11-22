using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSXShaderKit
{
    public class PSXPostProcessEffect : MonoBehaviour
    {
        private enum DitheringMatrixSize
        {
            Dither2x2,
            Dither4x4
        };

        [Header("Color")]
        [SerializeField]
        [Tooltip("The color depth, or amount of values per color channel. 255 is the console-accurate value to simulate a 24-bit color depth")]
        private Vector3 colorDepth = new Vector3(255, 255, 255);
        [SerializeField]
        [Tooltip("The color depth of the dithered output. Dithering happens after the color depth above is applied. 31 is the console-accurate value for a 15 bit color depth but doesn't look too good in linear color space.")]
        private Vector3 ditherDepth = new Vector3(31, 31, 31);
        [SerializeField]
        [Tooltip("Controls the quality of the dither through the size of the pattern used across the screen. 2x2 is console-accurate.")]
        private DitheringMatrixSize ditheringMatrixSize = DitheringMatrixSize.Dither2x2;
        [Header("Interlacing")]
        [SerializeField]
        [Tooltip("The amount of rows of pixels that get affected by interlacing. 1 is console-accurate but only works on lower resolutions.")]
        private int InterlacingSize = 1;

        [Header("Shaders")]
        [SerializeField]
        private Shader _PostProcessShader;
        private Material _PostProcessMaterial;
        private RenderTexture _CurrentFrame;

        [SerializeField]
        private Shader _InterlacingShader;
        private Material _InterlacingMaterial;
        private RenderTexture _PreviousFrame;

        private bool _IsFirstFrame = true;

        void Start()
        {
            if (_PostProcessShader != null && _PostProcessShader.isSupported)
            {
                _PostProcessMaterial = new Material(_PostProcessShader);
            }
            else
            {
                enabled = false;
                return;
            }

            if (_InterlacingShader != null && _InterlacingShader.isSupported)
            {
                _InterlacingMaterial = new Material(_InterlacingShader);
            }
            else
            {
                InterlacingSize = -1;
            }
        }

        void OnDisable()
        {
            if (_CurrentFrame)
            {
                RenderTexture.ReleaseTemporary(_CurrentFrame);
            }

            if (_PreviousFrame)
            {
                RenderTexture.ReleaseTemporary(_PreviousFrame);
            }

            _IsFirstFrame = true;
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            _PostProcessMaterial.SetVector("_ColorResolution", colorDepth);
            _PostProcessMaterial.SetVector("_DitherResolution", ditherDepth);
            _PostProcessMaterial.SetFloat("_HighResDitherMatrix", ditheringMatrixSize == DitheringMatrixSize.Dither2x2 ? 0.0f : 1.0f);

            if (InterlacingSize <= 0)
            {
                Graphics.Blit(source, destination, _PostProcessMaterial);
                _IsFirstFrame = true;
            }
            else
            {
                if (_CurrentFrame)
                {
                    RenderTexture.ReleaseTemporary(_CurrentFrame);
                }
                _CurrentFrame = RenderTexture.GetTemporary(source.descriptor);
                Graphics.Blit(source, _CurrentFrame, _PostProcessMaterial);

                _InterlacingMaterial.SetFloat("_InterlacedFrameIndex", Time.frameCount % 2);
                _InterlacingMaterial.SetFloat("_InterlacingSize", InterlacingSize);
                _InterlacingMaterial.SetTexture("_PreviousFrame", _IsFirstFrame ? _CurrentFrame : _PreviousFrame);
                _IsFirstFrame = false;
                Graphics.Blit(_CurrentFrame, destination, _InterlacingMaterial);

                if (_PreviousFrame)
                {
                    RenderTexture.ReleaseTemporary(_PreviousFrame);
                }
                _PreviousFrame = RenderTexture.GetTemporary(source.descriptor);
                Graphics.Blit(_CurrentFrame, _PreviousFrame);

                RenderTexture.active = destination;
            }
        }
    }
}
