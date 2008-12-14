using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OHQ.GameGUI.Graphics
{
    /// <summary>
    /// Copied from http://www.ziggyware.com/readarticle.php?article_id=102&rowstart=1
    /// I'd normally retype but thats a lot of typing
    /// </summary>
    public class QuadBase
    {
        #region Data Fields
        private float m_x;
        private float m_y;
        private float m_width;
        private float m_height;
        private float m_u;
        private float m_v;
        private float m_uvWidth;
        private float m_uvHeight;
        private Color m_color = Color.White;
        private Rectangle m_destRect;
        private Rectangle m_srcRect;
        private Vector2 m_center = new Vector2(0, 0);
        private Texture2D m_texture;
        #endregion

        public QuadBase()
        {
        }
        public QuadBase(float x, float y, float width, float height)
        {
            m_x = x;
            m_y = y;
            m_width = width;
            m_height = height;
            SetCenter();
        }

        public QuadBase(float x, float y, float z, float width, float height)
            : this(x, y, width, height)
        {
            Z = z;
        }

        protected virtual void SetCenter()
        {
            m_center.X = m_x + m_width / 2;
            m_center.Y = m_y + m_height / 2;
        }

        #region Properties
        public Vector2 Center
        {
            get
            {
                return m_center;
            }
        }

        public float X
        {
            get
            {
                return m_x;
            }
            set
            {
                m_x = value;
                SetCenter();
            }
        }

        public float Y
        {
            get
            {
                return m_y;
            }
            set
            {
                m_y = value;
                SetCenter();
            }
        }

        public float Z { get; set; }

        public float Width
        {
            get
            {
                return m_width;
            }
            set
            {
                m_width = value;
                SetCenter();
            }
        }
        public float Height
        {
            get
            {
                return m_height;
            }
            set
            {
                m_height = value;
                SetCenter();
            }
        }
        public Color Color
        {
            get
            {
                return m_color;
            }
            set
            {
                m_color = value;
            }
        }
        public Texture2D Texture
        {
            get
            {
                return m_texture;
            }
            set
            {
                m_texture = value;

                if (m_uvWidth == 0)
                    m_uvWidth = m_texture.Width;

                if (m_uvHeight == 0)
                    m_uvHeight = m_texture.Height;
            }
        }
        public float U
        {
            get
            {
                return m_u;
            }
            set
            {
                m_u = value;
            }
        }
        public float V
        {
            get
            {
                return m_v;
            }
            set
            {
                m_v = value;
            }
        }

        public float UVWidth
        {
            get
            {
                return m_uvWidth;
            }
            set
            {
                m_uvWidth = value;
            }
        }

        public float UVHeight
        {
            get
            {
                return m_uvHeight;
            }
            set
            {
                m_uvHeight = value;
            }
        }
        public Rectangle DestRect
        {
            get
            {
                m_destRect.X = (int)(m_x);
                m_destRect.Y = (int)(m_y);
                m_destRect.Width = (int)(m_width);
                m_destRect.Height = (int)(m_height);
                return m_destRect;
            }
        }

        public Rectangle SrcRect
        {
            get
            {
                m_srcRect.X = (int)m_u;
                m_srcRect.Y = (int)m_v;
                m_srcRect.Width = (int)m_uvWidth;
                m_srcRect.Height = (int)m_uvHeight;
                return m_srcRect;
            }
        }
        #endregion
    }

}