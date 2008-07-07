using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace OHQProcessors
{
    /// <summary>
    /// This is the stuff that the xml-sheet will contain.
    /// Anything added to the xml-sheet myst be reflected in this class.
    /// </summary>
    public class SpriteSheetFileInput
    {
        public SpriteSheetEntry[] Entries;
    }


    public class SpriteSheetEntry
    {
        private const int ENTIRE_IMAGE = -1;

        public string name;

        [ContentSerializer(Optional = true)]
        public int numberOfFrames = 1;

        [ContentSerializer(Optional = true)]
        public int FrameWidth = ENTIRE_IMAGE;

        [ContentSerializer(Optional = true)]
        public int FrameHeight = ENTIRE_IMAGE;

        public string GetTrimmedFilename()
        {
            return Path.GetFileNameWithoutExtension(name);
        }
    }
}
