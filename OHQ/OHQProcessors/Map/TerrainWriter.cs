#region File Description
//-----------------------------------------------------------------------------
// ArmorWriter.cs
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
using OHQProcessors.Terrain;
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
    public class TerrainWriter : OHQWriter<TerrainOutput>
    {
        protected override void Write(ContentWriter output, TerrainOutput value)
        {
            output.Write(value.SpriteName);
            output.WriteExternalReference<SpriteSheetContent>(value.sheet);
            output.Write(value.IsWalkable);
            output.Write(value.Borders);
            output.Write(value.BorderName);
            output.Write(value.AnimationFrames);
        }


        /// <summary>
        /// Tells the content pipeline what worker type
        /// will be used to load the sprite sheet data.
        /// </summary>
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(OHQData.Terrain.TerrainReader).AssemblyQualifiedName;
        }


        /// <summary>
        /// Tells the content pipeline what CLR type the sprite sheet
        /// data will be loaded into at runtime.
        /// </summary>
        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(OHQData.Terrain).AssemblyQualifiedName;
        }
    }

}
