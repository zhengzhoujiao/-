using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tetris
{
    public partial class FrmTetris : Form
    {
        public FrmTetris()
        {
            InitializeComponent();
        }
        private Palette p;
        private Keys downKey;//下降键
        private Keys dropKey;//丢下键
        private Keys moveLeftKey;//左移键
        private Keys moveRightKey;//右移键
        private Keys deasilRotateKey;//顺时针旋转键
        private Keys contraRotateKey;//逆时针旋转键
        private int paletteWidth;//画板宽度
        private int paletteHeight;//画板高度
        private Color paletteColor;//背景色
        private int rectPix;//每个单元格像素





        private void btnStart_Click(object sender, EventArgs e)
        {
            if (p != null)
            {
                p.Close();
            }
           
            p = new Palette(paletteWidth, paletteHeight, rectPix , paletteColor, 
                Graphics .FromHwnd(pbRun .Handle ), 
                Graphics .FromHwnd (lblReady .Handle ));
            p.Start();
        }

        private void FrmTetris_Load(object sender, EventArgs e)
        {
            //读取xml文件中的参数配置信息，并以此赋给私有成员变量
            Config config = new Config();
            config.LoadFromXmlFile();
            downKey = config.DownKey;
            dropKey = config.DropKey;
            moveLeftKey = config.MoveLeftKey;
            moveRightKey = config.MoveRightKey;
            deasilRotateKey = config.DeasilRotateKey;
            contraRotateKey = config.ContraRotateKey;
            paletteWidth = config.CoorWidth;
            paletteHeight = config.CoorHeight;
            paletteColor = config.BackColor;
            rectPix = config.RectPix;
            //根据画板的长度和宽度信息动态改变窗体及画板大小
            this.Width = paletteWidth * rectPix + 180;
            this.Height = paletteHeight * rectPix + 58;
            pbRun.Width = paletteWidth * rectPix;
            pbRun.Height = paletteHeight * rectPix;

        }

        private void FrmTetris_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 32)//屏蔽回车键
            {
                e.Handled = true;
            }
            if (e.KeyCode == downKey)//下降
            {
                p.Down();
            }
            if (e.KeyCode == dropKey)//丢下
            {
                p.Drop();
            }
            if (e.KeyCode == moveLeftKey)//左移
            {
                p.MoveLeft ();
            }
            if (e.KeyCode == moveRightKey)//右移
            {
                p.MoveRight();

            }
            if (e.KeyCode == deasilRotateKey )//顺时针旋转
            {
                p.DeasilRotate();
            }
            if (e.KeyCode == contraRotateKey)//逆时针旋转
            {
                p.ContraRotate();
            }
            
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (p == null)
            {
                return;
            }
            if (btnPause.Text == "暂停")
            {               
                p.Pause(); 
                btnPause.Text = "继续";
                
            }
            else
            {             
                p.EndPause (); 
                btnPause.Text = "暂停";
            }
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            if (btnPause.Text == "暂停")
            {
                btnPause.PerformClick();
            }
            using (FrmConfig frmconfig = new FrmConfig())
            {
                frmconfig.ShowDialog();
                FrmTetris_Load(null, null);
                
            }

        }

        private void FrmTetris_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (p!=null )
            {
                p.Close();
            }

        }

       private void pbRun_Paint(object sender, PaintEventArgs e)
        {
            if (p != null)
            {
                p.PaintPalette(e.Graphics);
            }
        }

        private void lblReady_Paint(object sender, PaintEventArgs e)
        {
            if (p != null)
            {
                p.PaintReady(e.Graphics);
            }
        }

            


    }
}
