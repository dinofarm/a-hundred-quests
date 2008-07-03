#region File Description
//-----------------------------------------------------------------------------
// AnimationWriter.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using OHQData;
using OHQData.Sprites;
#endregion

namespace OHQProcessors
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to write the specified data type into binary .xnb format.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    /// </summary>
    [ContentTypeWriter]
    public class AnimationWriter : OHQWriter<Animation>
    {
        protected override void Write(ContentWriter output, Animation value)
        {
            output.Write(String.IsNullOrEmpty(value.TextureName) ? String.Empty : value.TextureName);
            output.Write(value.FrameCount);
            output.Write(value.FramesPerSecond);
            output.Write(value.Looping);
        }
    }
}
