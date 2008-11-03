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
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using OHQData;
#endregion

namespace OHQProcessors.Terrain
{

    /// <summary>
    /// This is the input from a terrain.xml file.
    /// </summary>
    public class TerrainContent
    {
        /// <summary>
        ///  The name of the sprite inside the sprite sheet.
        /// </summary>
        public string SpriteName;

        /// <summary>
        ///  The SpriteSheet from which we should take the terrain graphics.
        /// </summary>
        public string SpriteSheet;

        [ContentSerializer(Optional = true)]
        public bool IsWalkable = true;

        [ContentSerializer(Optional = true)]
        public bool Borders = false;

        [ContentSerializer(Optional = true)]
        public string BorderName = "";

        [ContentSerializer(Optional = true)]
        public int AnimationFrames = 1;

    }

    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to write the specified data type into binary .xnb format.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    /// </summary>
    [ContentTypeWriter]
    public class TerrainWriter : OHQWriter<TerrainContent>
    {
        protected override void Write(ContentWriter output, TerrainContent value)
        {
            output.Write(value.SpriteName);
            output.Write(value.SpriteSheet);    
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
