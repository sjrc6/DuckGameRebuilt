using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;

namespace DuckGame
{
    public class FrameStackMaterial : Material
    {
        public FrameStackMaterial(int prevFrames)
        {
            previousFramesCount = prevFrames;
            _effect = Content.Load<MTEffect>("Shaders/frameStack");
        }
        int previousFramesCount = 0;
        public override void Apply()
        {
            SetValue("frameCount", previousFramesCount);
            Graphics.device.SamplerStates[1] = SamplerState.PointClamp;
            foreach (EffectPass effectPass in _effect.effect.CurrentTechnique.Passes)
            {
                effectPass.Apply();
            }
        }
    }
}
