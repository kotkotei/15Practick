using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms;

namespace PR14sistem
{
    public partial class Form1 : Form
    {
        string selectedPath="";
        public Form1()
        {
            InitializeComponent();
            PopulateTreeView();
        }
       
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
     

        private void PopulateTreeView()
        { // Добавление корневого узла для назначения клиентских узлов.
            TreeNode rootNode;

            DirectoryInfo info = new DirectoryInfo(@"C:\Users\Lenovo\Desktop\Тест");
            if (info.Exists)//определяет значение, определяющая наличие каталога
            {
                rootNode = new TreeNode(info.Name);
                rootNode.Tag = info;//сведения об узле дерева
                GetDirectories(info.GetDirectories(), rootNode);
                treeView1.Nodes.Add(rootNode);//добавляет новый узел дерева в конец колекции узлов дерева
               
            }
        }

        private void GetDirectories(DirectoryInfo[] subDirs,
            TreeNode nodeToAddTo)//Отображение и работа с корневой папкой
        {
            TreeNode aNode;
            DirectoryInfo[] subSubDirs;//метод экземпляра класса для создания перечисления,перемещения в катологах и подкаталогах, помещение в массив
            foreach (DirectoryInfo subDir in subDirs)//перебор 
            {
                aNode = new TreeNode(subDir.Name, 0, 0);
                aNode.Tag = subDir;//сведения о узле дерева
                aNode.ImageKey = "folder";//Возвращает или задает ключ для изображения, связанного с этим узлом дерева, при нахождении этого узла в невыбранном состоянии.
                subSubDirs = subDir.GetDirectories();
                if (subSubDirs.Length != 0)//если длина не равна 0 добавляется католог
                {
                    GetDirectories(subSubDirs, aNode);//Возвращает имена подкаталогов (включая пути) в указанном каталоге
                }
                nodeToAddTo.Nodes.Add(aNode);
            }
        }
 
        public void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {    //клик мыши по узлу
            TreeNode newSelected = e.Node;
            selectedPath = e.Node.Text;
            listView1.Items.Clear();// удаление всех элементов
            DirectoryInfo nodeDirInfo = (DirectoryInfo)newSelected.Tag;
            ListViewItem.ListViewSubItem[] subItems;//подэлемент listViewItem
            ListViewItem item = null;

            foreach (DirectoryInfo dir in nodeDirInfo.GetDirectories())//Запись данных Directory в listView1
            {
                item = new ListViewItem(dir.Name, 0);
                subItems = new ListViewItem.ListViewSubItem[]
                    {new ListViewItem.ListViewSubItem(item, "Directory"),
             new ListViewItem.ListViewSubItem(item,
                dir.LastAccessTime.ToShortDateString())};
                item.SubItems.AddRange(subItems);
                listView1.Items.Add(item);
            }
            foreach (FileInfo file in nodeDirInfo.GetFiles())//Запись данных File в listView1
            {
                item = new ListViewItem(file.Name, 1);
                subItems = new ListViewItem.ListViewSubItem[]
                    { new ListViewItem.ListViewSubItem(item, "File"),
             new ListViewItem.ListViewSubItem(item,
                file.LastAccessTime.ToShortDateString())};

                item.SubItems.AddRange(subItems);
                listView1.Items.Add(item);
            }

            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);//Изменение размера колонок в зависимости размера текста
        }
  
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
       
        private void button2_Click(object sender, EventArgs e)
        {//удаление
            TreeNode selectedNode = treeView1.SelectedNode;
            string file = listView1.SelectedItems[0].SubItems[0].Text;               
            string path = @"C:\Users\Lenovo\Desktop\Тест\" + selectedPath + "\\" + file;
            FileInfo fileInf = new FileInfo(path);
            fileInf.Delete();
            treeView1.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //добавление директории
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                // Создаем новую директорию в выбранном пути
                string newPath = Path.Combine(dialog.SelectedPath, "Новая директория");
                Directory.CreateDirectory(newPath);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }
    }
}
