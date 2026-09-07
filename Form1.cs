namespace MyWinformApp;

using System;
using System.Numerics;
using System.Windows.Forms;
using System.Text.RegularExpressions;

public partial class Form1 : Form
{
    string inMes;
    Button btm;
    Button openFile;
    Button outFolder;
    TextBox inputFile;
    TextBox outFilePath;
    TextBox OFsName;
    ProgressBar pbar;
    public Form1()
    {
        InitializeComponent();
        // --------------------------------------
        btm = new Button();
        btm.Text = "点击此钮";
        btm.Location = new Point(50, 90);
        btm.Click += Button_Click;
        Controls.Add(btm);

        // --------------------------------------
        openFile = new Button();
        openFile.Text = "...";
        openFile.Location = new Point(170, 10);
        openFile.Size = new Size(40, 25);
        openFile.Click += OpenFile;
        Controls.Add(openFile);
        // --------------------------------------
        outFolder = new Button();
        outFolder.Text = "...";
        outFolder.Location = new Point(170, 50);
        outFolder.Size = new Size(40, 25);
        outFolder.Click += OpenMl;
        Controls.Add(outFolder);
        // --------------------------------------
        Label litext = new Label()
        {
            Text = "输入文件",
            Location = new Point(10, 10),
            AutoSize = true
        };
        Controls.Add(litext);

        inputFile = new TextBox();
        inputFile.Location = new Point(70, 10);
        Controls.Add(inputFile);
        // --------------------------------------
        Label outext = new Label()
        {
            Text = "输出路径",
            Location = new Point(10, 50),
            AutoSize = true
        };
        Controls.Add(outext);

        outFilePath = new TextBox();
        outFilePath.Location = new Point(70, 50);
        Controls.Add(outFilePath);
        // --------------------------------------
        Label nameSquer = new Label()
        {
            Text = "输出文件名",
            Location = new Point(230, 50),
            AutoSize = true
        };
        Controls.Add(nameSquer);

        OFsName = new TextBox();
        OFsName.Location = new Point(300, 50);
        Controls.Add(OFsName);
        // --------------------------------------
        pbar = new ProgressBar();
        pbar.Location = new Point(10, 130);
        pbar.Size = new Size(310, 10);
        pbar.Minimum = 0;
        pbar.Maximum = 100;
        pbar.Value = 0;
        Controls.Add(pbar);

    }
    private void Button_Click(object sder, EventArgs ea)
    {
        btm.Enabled = false;
        pbar.Value = 10;
        //MessageBox.Show("Hello,DreanForms!");
        inMes = inputFile.Text;
        if (inputFile.Text == "")
        {
            MessageBox.Show("文件名为空");
        }
        else
        {
            try
            {
                if (OFsName.Text == "")
                {
                    Maq(inMes);
                }
                else
                {
                    Maq(inMes, OFsName.Text);
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("找不到文件，请重试");
            }
        }pbar.Value=0;btm.Enabled=true;
        //MessageBox.Show(inMes);
    }

    private void OpenFile(object sder, EventArgs ea)
    {
        using (OpenFileDialog dlg = new OpenFileDialog())
        {
            dlg.Filter = "文本文件|*.txt;|所有文件|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                //pbox.Image = Image.FromFile(dlg.FileName);
                inputFile.Text = dlg.FileName;
            }
        }
    }
    private void OpenMl(object sder, EventArgs ea)
    {
        using (FolderBrowserDialog dlg = new FolderBrowserDialog())
        {
            dlg.Description = "选择文件夹";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                //pbox.Image = Image.FromFile(dlg.FileName);
                outFilePath.Text = dlg.SelectedPath;
            }
        }
    }

    void Maq(string fName, string outName = "Default_File.tres")
    {
        StreamReader sr = File.OpenText(fName);

        string nextLine = sr.ReadToEnd();
        int iex = 0;

        string fakeArray = "";

        while (!nextLine[iex].Equals(']'))
        {
            fakeArray += nextLine[iex];
            iex++;
        }
        //Console.WriteLine(fakeArray);
        nextLine = "";

        var nums = Regex.Matches(fakeArray, @"[-+]?\d+\.?\d*")
                        .Cast<Match>()
                        .Select(m => float.Parse(m.Value))
                        .ToList();

        Vector3[] v3s = new Vector3[65455];

        //Console.WriteLine(nums.ToArray().Length);

        for (int i = 0; i < nums.ToArray().Length; i++)
        {
            if (i % 3 == 0)
            {
                v3s[i / 3] = new Vector3(nums[i], nums[i + 1], nums[i + 2]);
            }
        }
        pbar.Value = 30;
        v3s = v3s.Distinct().ToArray();

        var otpt = "";
        int slen = (int)(Math.Sqrt(v3s.Length - 1));
        //Console.WriteLine(slen);

        sr.Close();

        Vector3[,] Jzhen = new Vector3[slen, slen];

        QuickSortX(v3s, 0, v3s.Length - 2);
        pbar.Value = 50;
        Vector3 tpm;

        for (int i = 0; i < slen; i++)
        {
            for (int j = 0; j < slen; j++)
            {
                Jzhen[i, j] = v3s[i * slen + j];
            }
        }
        pbar.Value = 80;

        Vector3[] ttmp = new Vector3[slen];
        for (int i = 0; i < slen; i++)
        {
            for (int j = 0; j < slen; j++)
            {
                ttmp[j] = Jzhen[i, j];
            }
            QuickSortZ(ttmp, 0, slen - 1);
            for (int j = 0; j < slen; j++)
            {
                Jzhen[i, j] = ttmp[j];
            }
        }

        pbar.Value = 90;
        for (int i = 0; i < slen; i++)
        {
            for (int j = 0; j < slen; j++)
            {
                if (i == slen - 1 && j == slen - 1) { break; }
                otpt += Jzhen[i, j].Y + "," + " ";
            }
        }
        otpt += Jzhen[slen - 1, slen - 1].Y;

        string uide = "uid://of1xf5qqmgcr";
        string message = "[gd_resource type=\"HeightMapShape3D\" format=3 uid=\"" + uide + "\"]\n\n[resource]\nmap_width = " + slen + "\nmap_depth = " + slen + "\nmap_data = PackedFloat32Array(";
        message += otpt + ")" + "\n";

        string outSname = outFilePath.Text + "/" + outName;
        File.WriteAllText(outSname, message);
        pbar.Value = 100;
        MessageBox.Show("工事已毕。");
    }

    public void QuickSortX(Vector3[] A, int lo, int hi)
    {
        if (lo > hi)//递归退出条件
        {
            return;
        }
        int i = lo;
        int j = hi;
        Vector3 temp = A[i];//取得基准数，空出一个位置
        while (i < j)//当i=j时推出，表示temp左边的数都比temp小，右边的数都比temp大
        {
            while (i < j && temp.X <= A[j].X)//从后往前找比temp小的数，将比temp小的数往前移
            {
                j--;
            }
            A[i] = A[j];//将比基准数小的数放在空出的位置，j的位置又空了出来
            while (i < j && temp.X >= A[i].X)//从前往后找比temp大的数，将比temp大的数往后移
            {
                i++;
            }
            A[j] = A[i];//将比基准数大的数放在hi空出来的位置,如此，i所在的位置又空了出来
        }
        A[i] = temp;
        QuickSortX(A, lo, i - 1);//对lo到i-1之间的数再使用快速排序，每次快速排序的结果是找到了基准数应该在的位置
                                 //其左边的数都<=它，右边的数都>=它，它此时在数组中的位置就是排序好时其应该在的位置。
        QuickSortX(A, i + 1, hi);//对i+1到hi之间的数再使用快速排序
    }

    public void QuickSortZ(Vector3[] A, int lo, int hi)
    {
        if (lo > hi)//递归退出条件
        {
            return;
        }
        int i = lo;
        int j = hi;
        Vector3 temp = A[i];//取得基准数，空出一个位置
        while (i < j)//当i=j时推出，表示temp左边的数都比temp小，右边的数都比temp大
        {
            while (i < j && temp.Z <= A[j].Z)//从后往前找比temp小的数，将比temp小的数往前移
            {
                j--;
            }
            A[i] = A[j];//将比基准数小的数放在空出的位置，j的位置又空了出来
            while (i < j && temp.Z >= A[i].Z)//从前往后找比temp大的数，将比temp大的数往后移
            {
                i++;
            }
            A[j] = A[i];//将比基准数大的数放在hi空出来的位置,如此，i所在的位置又空了出来
        }
        A[i] = temp;
        QuickSortZ(A, lo, i - 1);//对lo到i-1之间的数再使用快速排序，每次快速排序的结果是找到了基准数应该在的位置
                                 //其左边的数都<=它，右边的数都>=它，它此时在数组中的位置就是排序好时其应该在的位置。
        QuickSortZ(A, i + 1, hi);//对i+1到hi之间的数再使用快速排序
    }
}
