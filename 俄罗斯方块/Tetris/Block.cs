using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Drawing;


namespace Tetris
{
    class Block
    {
        protected Point[] structArr;//存放砖块组成信息的坐标数组
        protected int _xPos;//砖块中心点所在的x坐标
        protected int _yPos;//砖块中心点所在的y坐标
        protected Color _blockColor;//砖块颜色
        protected Color disapperColor;//查出颜色
        protected int rectPix;//设单元格像素

        public Block()//默认构造函数，声明此函好是为了子类能创建
        {
        }
        public Block(Point[] sa, Color bColor, Color dColor, int pix)
        {//重载构造函数，给成员变量赋值
            _blockColor = bColor;
            disapperColor = dColor;
            rectPix = pix;
            structArr = sa;
        }
        public Point this[int index]//索引器，根据索引访问砖块里的小方块坐标
        {
            get
            {
                return structArr[index];
            }
        }
        public int Length//属性，表示structArr的长度
        {
            get
            {
                return structArr.Length;
            }
        }
        #region 成员变量相应的属性
        public int XPos
        {
            get
            {
                return _xPos;
            }
            set 
            {
                _xPos = value;
            }
        }
        public int YPos
        {
            get
            {
                return _yPos;
            }
            set
            {
                _yPos = value;
            }
        }
        public Color BlockColor
        {
            get
            {
                return _blockColor;
            }

        }
        #endregion
        public void DeasilRotate()//顺时针旋转
        {
            int temp;//旋转公式为x1=y;y1=-x
            for (int i = 0; i < structArr.Length; i++)
            {
                temp = structArr[i].X;
                structArr[i].X = structArr[i].Y;
                structArr[i].Y = -temp;
            }
        }
        public void ContraRotate()//逆时针旋转
        {
            int temp;//旋转公式为x1=-y;y1=x
            for (int i = 0; i < structArr.Length; i++)
            {
                temp = structArr[i].X;
                structArr[i].X = -structArr[i].Y;
                structArr[i].Y = temp;
            }
        }
        private Rectangle PointToRect(Point p)//把一个坐标点转化为画布的坐标值
        {
            return new Rectangle((_xPos + p.X) * rectPix + 1,
                (_yPos - p.Y) * rectPix + 1, rectPix - 2, rectPix - 2);

        }
        public virtual void Paint(Graphics gp)//在指定画板上绘制砖块
        {
            SolidBrush sb = new SolidBrush(_blockColor);
            foreach (Point p in structArr)
            {
                lock (gp)
                {
                    gp.FillRectangle(sb, PointToRect(p));
                }
            }
        }
        public virtual void Erase(Graphics gp)//擦除矩形
        {
            SolidBrush sb = new SolidBrush(disapperColor );
            foreach (Point p in structArr)
            {
                lock (gp)
                {
                    gp.FillRectangle(sb, PointToRect(p));
                }
            }
        }

    }
}
