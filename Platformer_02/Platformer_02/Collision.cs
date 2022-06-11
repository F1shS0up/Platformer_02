using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


class Collision
{
    private Rectangle A;
    private List<Rectangle> B;

    public int originX, originY, width, height;
    public Collision(Rectangle pA, List<Rectangle> pB, int pOriginX, int pOriginY)
    {
        B = pB;
        A = pA;
        originX = pOriginX;
        originY = pOriginY;
        width = pA.Width;
        height = pA.Height;
    }
    #region Static
        public static bool IsTouchingLeft(Rectangle A, int velocityX,Rectangle B)
        {
            Rectangle C = A;
            C.X = A.X + velocityX;
            return C.Intersects(B);
        }

        public static bool IsTouchingRight(Rectangle A, int velocityX, Rectangle B)
        {
            Rectangle C = A;
            C.X = A.X + velocityX;
            return C.Intersects(B);
        }

        public static bool IsTouchingTop(Rectangle A, int velocityY, Rectangle B)
        {
            Rectangle C = A;
            C.Y = A.Y + velocityY;
            return C.Intersects(B);
        }

        public static bool IsTouchingBottom(Rectangle A, int velocityY, Rectangle B)
        {
            Rectangle C = A;
            C.Y = A.Y + velocityY;
            return C.Intersects(B);
        }
        #endregion
    #region NonStatic
        public (bool, int) IsTouchingLeft(int velocityX)
        {
            if(B == null)
            {
                Debug.WriteLine("B is null", "Warning");
                return (false, 0);
            }

            Rectangle C = A;
            C = new Rectangle(A.X + velocityX, A.Y + 1, width, height-2);
            bool g = false;

            foreach (Rectangle b in B)
            {
                g = C.Intersects(b);
                if (g && C.Left >= b.Left)
                {
                    return (true, b.Right);
                }
            }

            return (false, 0);
        }

        public (bool, int) IsTouchingRight(int velocityX)
        {
            if (B == null)
            {
                Debug.WriteLine("B is null", "Warning");
                return (false, 0);
            }
            Rectangle C = A;
            C = new Rectangle(A.X + velocityX, A.Y + 1, width, height - 2);
            bool g = false;
            foreach (Rectangle b in B)
            {
                g = C.Intersects(b);
                if (g && C.Left < b.Left)
                {
                    return (true, b.Left);
                }
            }
            return (false, 0);
        }

        public (bool, int) IsTouchingTop(int velocityY)
        {
            if (B == null)
            {
                Debug.WriteLine("B is null", "Warning");
                return (false, 0);
            }
            Rectangle C = A;
            C = new Rectangle(A.X + 1, A.Y + velocityY, width - 2, height);
            bool g = false;
          
            foreach (Rectangle b in B)
            {
                g = C.Intersects(b);
                if (g && C.Top >= b.Top)
                {
                    return (true, b.Bottom);
                }
            }
            return (false, 0);
        }

        public (bool, int, Rectangle) IsTouchingBottom(int velocityY)
        {
            if (B == null)
            {
                Debug.WriteLine("B is null", "Warning");
                return (false, 0, Rectangle.Empty);
            }
            
            Rectangle C = A;
            C = new Rectangle(A.X + 1, A.Y + velocityY, width - 2, height);

            bool g = false;
            foreach(Rectangle b in B)
            {
                g = C.Intersects(b);
                if (g && C.Top < b.Top)
                {
                    return (true, b.Top, b);
                }
            }
            return (false, 0, Rectangle.Empty);
        }
        #endregion

    public void UpdateRectangleA(Rectangle pA)
    {
        A = pA;
        width = pA.Width;
        height = pA.Height;
    }
    public void UpdateRectangleB(List<Rectangle> pB)
    {
        B = pB;
    }

}

