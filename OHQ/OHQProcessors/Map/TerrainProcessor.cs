using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using OHQProcessors;

namespace OHQProcessors.Terrain
{

    /// <summary>
    /// This is the input from a terrain.xml file.
    /// </summary>
    public class TerrainData
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
    /// This class contains the processed data that is written to disk.
    /// Although this class contains default values, these values are overwritten
    /// by the default values from the TerrainData input class, so those are the
    /// terrain files that really should be changed.
    /// </summary>
    public class TerrainOutput
    {
        // The name of the sprite inside the sprite sheet.
        public string SpriteName;

        // The sprite sheet from which we should take the 
        //public SpriteSheet sheet;
        public ExternalReference<SpriteSheetContent> sheet;

        public bool Borders = false;
        public string BorderName = "";
        public bool IsWalkable = true;

        public int AnimationFrames = 1;

    }

    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to apply custom processing to content data, converting an object of
    /// type TInput to TOutput. The input and output types may be the same if
    /// the processor wishes to alter data without changing its type.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    ///
    /// TODO: change the ContentProcessor attribute to specify the correct
    /// display name for this processor.
    /// </summary>
    [ContentProcessor(DisplayName = "OHQProcessors.Terrain.TerrainProcessor")]
    public class TerrainProcessor : ContentProcessor<TerrainData, TerrainOutput>
    {
        public override TerrainOutput Process(TerrainData input, ContentProcessorContext context)
        {   
            TerrainOutput output = new TerrainOutput();    
        
            output.SpriteName       = input.SpriteName;
            output.sheet            = BuildSpriteSheet(input, context);
            output.IsWalkable       = input.IsWalkable;
            output.Borders          = input.Borders;
            output.BorderName       = input.BorderName;
            output.AnimationFrames  = input.AnimationFrames;
            
            return output;
        }


        private ExternalReference<SpriteSheetContent> BuildSpriteSheet(TerrainData input, ContentProcessorContext context)
        {
            string spriteSheetName = input.SpriteSheet;

            ExternalReference<SpriteSheetFileInput> spriteSheetInputReference =
                                new ExternalReference<SpriteSheetFileInput>(spriteSheetName);

            ExternalReference<SpriteSheetContent> spriteSheet =
                context.BuildAsset<SpriteSheetFileInput,
                                    SpriteSheetContent>(spriteSheetInputReference, "SpriteSheetProcessor");
            return spriteSheet;
        }
    }
}