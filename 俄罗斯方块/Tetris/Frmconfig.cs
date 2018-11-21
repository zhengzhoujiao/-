using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tetris
{
    public partial class FrmConfig : Form
    {
        public FrmConfig()
        {
            InitializeComponent();
        }
        private bool[,] struArr = new bool[5, 5];
        private Color blockColor = Color.Red;
        private Config config = new Config();

        private void lblMode_Paint(object sender, PaintEventArgs e)
        {
            Graphics gp = e.Graphics;
            gp.Clear(Color.Black);
            Pen p = new Pen(Color.White);
           
            for (int i=31;i<156;i=i+31)//画横白线
            gp .DrawLine(p ,1,i,155,i);
            for (int i=31;i<156;i=i+31)//画竖白线
            gp .DrawLine(p ,i,1,i,155);
            //填充矩阵中的方框
            SolidBrush s = new SolidBrush(blockColor);
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    if (struArr[x, y])
                    {
                        gp.FillRectangle(s, 31 * x + 1, 31 * y + 1, 30, 30);
                    }
                }
            }
        }

        private void lblMode_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            int xPos, yPos;
            xPos = e.X / 31;
            yPos = e.Y / 31;
            struArr[xPos, yPos] = !struArr[xPos, yPos];
            bool b = struArr[xPos, yPos];
            Graphics gp = lblMode.CreateGraphics();
            SolidBrush s = new SolidBrush(b ? blockColor : Color.Black);
            gp.FillRectangle(s, 31 * xPos + 1, 31 * yPos + 1, 30, 30);
            gp.Dispose ();//释放Graphics
        }

        private void lblColor_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();//打开颜色对话框
            blockColor = colorDialog1.Color;
            lblColor.BackColor=colorDialog1.Color;
            lblMode.Invalidate();//重画lblmode, lblMode_Paint执行
        }

        private void butAdd_Click(object sender, EventArgs e)
        {
            bool isEmpty = false;//首先查找图案是否为空
            foreach (bool i in struArr)
            {
                if (i)
                {
                    isEmpty = true;
                    break;
                }
            }
            if (!isEmpty)
            {
                MessageBox.Show("图案为空，请先用鼠标点击左边窗口绘制图案！","提示窗口",MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }
            StringBuilder sb = new StringBuilder(25);
            foreach (bool i in struArr)
            {
                sb.Append(i ? "1" : "0");
            }
            string blockstring = sb.ToString();
            //检查是否有重复图案
            foreach (ListViewItem item in LsvBlockSet.Items)
            {
                if (item.SubItems[0].Text == blockstring)
                {
                    MessageBox.Show("该图案已经存在！", "提示窗口", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            //把新砖块图案信息添加进ListView
            ListViewItem myItem =  LsvBlockSet.Items.Add(blockstring);
            myItem.SubItems.Add(Convert.ToString(blockColor.ToArgb()));

        }

        private void LsvBlockSet_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)//避免重复执行事件
            {
                blockColor = Color.FromArgb(int.Parse(e.Item.SubItems[1].Text));//把字符串信息转换为颜色
                lblColor.BackColor = blockColor;
                string s = e.Item.SubItems[0].Text;
                for (int i = 0; i < s.Length; i++)
                {
                    struArr[i / 5, i % 5] = (s[i] == '1') ? true : false;
                }
                lblMode.Invalidate();
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (LsvBlockSet.SelectedItems.Count == 0)
            {
                MessageBox.Show("请在右边窗口选择一个条目进行删除！", "提示窗口", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            LsvBlockSet.Items.Remove(LsvBlockSet.SelectedItems[0]);
            btnClear.PerformClick();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            for (int x = 0; x < 5; x++)//把struArr内的所有元素置为false，实现清空
            {
                for (int y = 0; y < 5; y++)
                {
                    struArr[x, y] = false;
                }
            }
            lblMode.Invalidate();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (LsvBlockSet.SelectedItems.Count == 0)
            {
                MessageBox.Show("请在右边窗口选择一个条目进行删除！", "提示窗口", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            bool isEmpty = false;//首先查找图案是否为空
            foreach (bool i in struArr)
            {
                if (i)
                {
                    isEmpty = true;
                    break;
                }
            }
            if (!isEmpty)
            {
                MessageBox.Show("图案为空，请先用鼠标点击左边窗口绘制图案！", "提示窗口", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            StringBuilder sb = new StringBuilder(25);
            foreach (bool i in struArr)
            {
                sb.Append(i ? "1" : "0");
            }
            LsvBlockSet.SelectedItems[0].SubItems[0].Text = sb.ToString();
            LsvBlockSet.SelectedItems[0].SubItems[1].Text = Convert.ToString(blockColor .ToArgb ());
        }

        private void txtContra_KeyDown(object sender, KeyEventArgs e)
        {//排除一些不适合的键
            if ((e.KeyValue >= 33 && e.KeyValue <= 36) || (e.KeyValue >= 45 && e.KeyValue <= 46) ||
                (e.KeyValue >= 48 && e.KeyValue <= 57) || (e.KeyValue >= 65 && e.KeyValue <= 90) ||
                (e.KeyValue >= 96 && e.KeyValue <= 107) || (e.KeyValue >= 109 && e.KeyValue <= 111) ||
                (e.KeyValue >= 186 && e.KeyValue <= 192) || (e.KeyValue >= 219 && e.KeyValue <= 222))
            {
                //检查是否存在冲突的快捷键
                foreach (Control c in gbKeySet.Controls)
                {
                    Control TempC = c as TextBox;
                    if (TempC != null && ((TextBox)TempC).Text!="")
                    {
                        if (((int)((TextBox)TempC).Tag) == e.KeyValue)
                        {
                            ((TextBox)TempC).Text = "";
                            ((TextBox)TempC).Tag = Keys.None;
                        }
                    }
                }
                ((TextBox)sender).Text = e.KeyCode.ToString();
                ((TextBox)sender).Tag = (Keys)e.KeyValue;
            }
        }

        private void lblBackColor_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();//打开颜色对话框
            lblBackColor.BackColor = colorDialog1.Color;
           
        }

        private void FrmConfig_Load(object sender, EventArgs e)
        {
            config.LoadFromXmlFile();//读取xml文件
            InfoArr info = config.Info;
            //读砖块样式
            ListViewItem myItem = new ListViewItem();
            for (int i = 0; i < info.Length; i++)
            {
                myItem = LsvBlockSet.Items.Add(info[i].GetIdStr());
                myItem.SubItems.Add(info[i].GetColorStr());
            }
           //读快捷键
            txtDown.Text = ((Keys)config.DownKey).ToString();
            txtDown.Tag = config.DownKey;
            txtDrop.Text = ((Keys)config.DropKey).ToString();
            txtDrop.Tag = config.DropKey;
            txtLeft.Text =((Keys )config .MoveLeftKey) .ToString ();
            txtLeft .Tag=config .MoveLeftKey;
            txtRight.Text=((Keys )config .MoveRightKey).ToString ();
            txtRight.Tag =config .MoveRightKey;
            txtDeasil.Text =((Keys )config .DeasilRotateKey) .ToString ();
            txtDeasil .Tag=config .DeasilRotateKey ;
            txtContra .Text =((Keys )config .ContraRotateKey).ToString ();
            txtContra .Tag=config .ContraRotateKey ;
            //读取环境设置参数
            txtCoorHeight.Text = config.CoorHeight.ToString();
            txtCoorWidth.Text = config.CoorWidth.ToString();
            txtRectPix.Text = config.RectPix.ToString();
            lblBackColor.BackColor = config.BackColor;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            InfoArr info = new InfoArr();
            foreach (ListViewItem item in LsvBlockSet.Items)//从LsvBlockSet内读取砖块信息，并存入info内
            {
                info.Add(item.SubItems[0].Text, item.SubItems[1].Text);
            }
            config.Info = info;//把info赋给config对象的Info属性
            config.DownKey = (Keys)txtDown.Tag;
            config.DropKey= (Keys)txtDrop.Tag;
            config.MoveLeftKey = (Keys)txtLeft.Tag;
            config.MoveRightKey= (Keys)txtRight.Tag;
            config.DeasilRotateKey = (Keys)txtDeasil.Tag  ;
            config.ContraRotateKey  = (Keys)txtContra.Tag;
            config.CoorWidth = int.Parse (txtCoorWidth.Text );
            config.CoorHeight= int.Parse (txtCoorHeight .Text );
            config.RectPix  =int.Parse (txtRectPix .Text );
            config.BackColor  = lblBackColor .BackColor ;
            config .SaveTOXmlFile ();//保存成xml文件
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

      




    }
}
