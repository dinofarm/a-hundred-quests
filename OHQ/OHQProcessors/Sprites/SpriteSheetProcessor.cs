#region File Description
//-----------------------------------------------------------------------------
// SpriteSheetProcessor.cs
//
// Microsoft Game Technology Group
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
#endregion

namespace OHQProcessors
{

    /// <summary>
    /// Custom content processor takes an array of individual sprite filenames (which
    /// will typically be imported from an XML file), reads them all into memory,
    /// arranges them onto a single larger texture, and returns the resulting sprite
    /// sheet object.
    /// </summary>
    [ContentProcessor]
    public class SpriteSheetProcessor : ContentProcessor<SpriteSheetFileInput, SpriteSheetContent>
    {

        private ContentProcessorContext context;
        private SpriteSheetContent spriteSheet;
        private List<BitmapContent> sourceSprites;

        /// <summary>
        /// Converts an array of sprite filenames into a sprite sheet object.
        /// </summary>
        public override SpriteSheetContent Process(SpriteSheetFileInput input,
                                                   ContentProcessorContext context)
        {
            InitializeProcessing(context);
            LoadAllImages(input);
            PackImages();

            SpriteSheetContent retVal = FinishProcessing();
            return retVal;
        }

        private void InitializeProcessing(ContentProcessorContext context)
        {
            this.context = context;
            spriteSheet = new SpriteSheetContent();
            sourceSprites = new List<BitmapContent>();
        }

        private SpriteSheetContent FinishProcessing()
        {
            SpriteSheetContent retVal = spriteSheet;
            spriteSheet = null;
            sourceSprites = null;
            return retVal;
        }

        private void LoadAllImages(SpriteSheetFileInput input)
        {
            // Loop over each input sprite filename.
            foreach (SpriteSheetEntry imageFile in input.Entries)
            {
                LoadImage(imageFile);
            }
        }


        private void PackImages()
        {

            // Pack all the sprites into a single large texture.
            BitmapContent packedSprites = SpritePacker.PackSprites(sourceSprites,
                                                                    spriteSheet.SpriteRectangles,
                                                                    context);
            spriteSheet.Texture.Mipmaps.Add(packedSprites);
        }


        private void LoadImage(SpriteSheetEntry imageFile)
        {
            spriteSheet.SpriteNames.Add(imageFile.GetTrimmedFilename(), sourceSprites.Count);

            if (imageFile.numberOfFrames <= 1)
            {
                LoadSingleFrame(imageFile);
            }
            else
            {
                LoadMultipleFrames(imageFile);
            }
        }

        private void LoadSingleFrame(SpriteSheetEntry imageFile)
        {
            TextureContent texture = LoadTextureIntoMemory(imageFile, context);

            // Store the name of this sprite.
            sourceSprites.Add(texture.Faces[0][0]);
            return;
        }

        private void LoadMultipleFrames(SpriteSheetEntry imageFile)
        {
            TextureContent texture = LoadTextureIntoMemory(imageFile, context);

            BitmapContent source = texture.Faces[0][0];

            int sourceWidth = source.Width;
            int sourceHeight = source.Height;
            int x = 0;
            int y = 0;
            for (int i = 0; i < imageFile.numberOfFrames; i++)
            {
                ExtractFrame(imageFile, source, x, y);

                x += imageFile.FrameWidth;
                if (x >= sourceWidth)
                {
                    x = 0;
                    y += imageFile.FrameHeight;
                }
            }
        }

        private void ExtractFrame(SpriteSheetEntry imageFile, BitmapContent source, int sourceX, int sourceY)
        {
            BitmapContent extracted = new PixelBitmapContent<Color>(imageFile.FrameWidth, imageFile.FrameHeight);
            BitmapContent.Copy(source, new Rectangle(sourceX, sourceY, imageFile.FrameWidth, imageFile.FrameHeight),
                                extracted, new Rectangle(0, 0, imageFile.FrameWidth, imageFile.FrameHeight));

            sourceSprites.Add(extracted);
        }

        private TextureContent LoadTextureIntoMemory(SpriteSheetEntry imageFile, ContentProcessorContext context)
        {
            ExternalReference<TextureContent> textureReference =
                            new ExternalReference<TextureContent>(imageFile.name);

            TextureContent texture =
                    context.BuildAndLoadAsset<TextureContent,
                                              TextureContent>(textureReference, null);
            return texture;
        }
    }
}
