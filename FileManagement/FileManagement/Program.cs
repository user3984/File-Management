using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileManagement
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }

    // Ŀ¼���
    public class Node
    {
        public FCB fcb = new FCB();
        public Node firstChild = null;      // ����
        public Node nextSibling = null;     // ���ֵ�
        public Node parent = null;          // �����

        public Node() { }
        public Node(FCB file)
        {
            fcb.fileName = file.fileName;
            fcb.time = file.time;
            fcb.type = file.type;
            fcb.size = file.size;
            fcb.start = file.start;
        }
        public Node(string name, int type)
        {
            fcb.fileName = name;
            fcb.time = DateTime.Now.ToString();
            fcb.type = type;
            fcb.size = 0;
            fcb.start = -1;
        }
    }

    // Ŀ¼
    public class Directory
    {
        public Node root;    // Ŀ¼�ĸ����

        public Directory()
        {
            root = null;
        }
        public Directory(FCB rootName)
        {
            root = new Node(rootName);
            if (root == null)
            {
                return;
            }
        }

        // ���ļ����д������
        public void CreateNode(Node parentNode, FCB fcb)
        {
            if (root == null || parentNode == null)
            {
                return;
            }
            if (parentNode.firstChild == null)  // ���ļ���Ϊ��
            {
                parentNode.firstChild = new Node(fcb);
                parentNode.firstChild.parent = parentNode;
                return;
            }
            else
            {
                Node tmp = parentNode.firstChild;
                while (tmp.nextSibling != null)
                {
                    tmp = tmp.nextSibling;
                }
                tmp.nextSibling = new Node(fcb);
                tmp.nextSibling.parent = parentNode;
            }
        }

        // ɾ�����
        public void DeleteNode(Node node, Disk disk)
        {
            Node n;
            if (node != this.root)
            {
                if (node == node.parent.firstChild)
                {
                    node.parent.firstChild = node.nextSibling;
                }
                else
                {
                    n = node.parent.firstChild;
                    while (n.nextSibling != node)
                    {
                        n = n.nextSibling;
                    }
                    n.nextSibling = node.nextSibling;
                }
            }
            
            // ɾ��node�����к���
            n = node.firstChild;
            while (n != null)
            {
                DeleteNode(n, disk);
                n = n.nextSibling;
            }

            if (node.fcb.type == 0)   // ɾ���ļ���Ҫ�ͷŴ��̿ռ�
            {
                disk.FreeDiskSpace(node.fcb.start, node.fcb.size);
            }
        }
        
        // �жϸ�Ŀ¼���Ƿ��Ѵ���ͬ���ļ�
        public bool NameExist(string name, Node node, int type)
        {
            node = node.firstChild;
            if (node == null)
            {
                return false;
            }
            if (node.fcb.fileName == name && node.fcb.type == type)
            {
                return true;
            }
            else
            {
                Node tmp = node.nextSibling;
                while (tmp != null)
                {
                    if (tmp.fcb.fileName == name && tmp.fcb.type == type)
                    {
                        return true;
                    }
                    tmp = tmp.nextSibling;
                }
                return false;
            }
        }

        struct NodeChild
        {
            public Node node;
            public int chd;
            public NodeChild(Node node, int chd)
            {
                this.node = node;
                this.chd = chd;
            }
        };
        // ���ļ��ж���Ŀ¼
        public void ReadDirectory()
        {
            BinaryReader br = new BinaryReader(new FileStream(Application.StartupPath + "\\dir.dat", FileMode.Open));
            Queue<NodeChild> parents = new Queue<NodeChild>();
            string fileName = br.ReadString(), date = br.ReadString();
            int type = br.ReadInt32(), sz = br.ReadInt32(), start = br.ReadInt32();
            Node rt = new Node(new FCB(fileName, type, date, 0));
            if (br.ReadBoolean())
            {
                parents.Enqueue(new NodeChild(rt, 0));
            }
            if (br.ReadBoolean())
            {
                parents.Enqueue(new NodeChild(rt, 1));
            }
            while (br.BaseStream.Position < br.BaseStream.Length)
            {
                fileName = br.ReadString();
                date = br.ReadString();
                type = br.ReadInt32();
                sz = br.ReadInt32();
                start = br.ReadInt32();
                NodeChild nc = parents.Dequeue();
                if (nc.chd == 0)  // ����
                {
                    nc.node.firstChild = new Node(new FCB(fileName, type, date, sz, start));
                    nc.node.firstChild.parent = nc.node;
                    if (br.ReadBoolean())
                    {
                        parents.Enqueue(new NodeChild(nc.node.firstChild, 0));
                    }
                    if (br.ReadBoolean())
                    {
                        parents.Enqueue(new NodeChild(nc.node.firstChild, 1));
                    }
                }
                else
                {
                    nc.node.nextSibling = new Node(new FCB(fileName, type, date, sz, start));
                    nc.node.nextSibling.parent = nc.node.parent;
                    if (br.ReadBoolean())
                    {
                        parents.Enqueue(new NodeChild(nc.node.nextSibling, 0));
                    }
                    if (br.ReadBoolean())
                    {
                        parents.Enqueue(new NodeChild(nc.node.nextSibling, 1));
                    }
                }
            }
            br.Close();
            this.root = rt;
        }

        public void WriteDirectory()
        {
            Queue<Node> q = new Queue<Node>();
            BinaryWriter bw = new BinaryWriter(new FileStream(Application.StartupPath + "\\dir.dat", FileMode.Create));
            Node n = this.root;
            bw.Write(n.fcb.fileName);
            bw.Write(n.fcb.time);
            bw.Write(n.fcb.type);
            bw.Write(n.fcb.size);
            bw.Write(n.fcb.start);
            bw.Write(n.firstChild != null);
            bw.Write(n.nextSibling != null);
            if (n.firstChild != null)
            {
                q.Enqueue(n.firstChild);
            }
            if (n.nextSibling != null)
            {
                q.Enqueue(n.nextSibling);
            }
            while (q.Count > 0)
            {
                n = q.Dequeue();
                bw.Write(n.fcb.fileName);
                bw.Write(n.fcb.time);
                bw.Write(n.fcb.type);
                bw.Write(n.fcb.size);
                bw.Write(n.fcb.start);
                bw.Write(n.firstChild != null);
                bw.Write(n.nextSibling != null);
                if (n.firstChild != null)
                {
                    q.Enqueue(n.firstChild);
                }
                if (n.nextSibling != null)
                {
                    q.Enqueue(n.nextSibling);
                }
            }
            bw.Close();
        }
    }

    public class FCB
    {
        public string fileName;       // �ļ���
        public int start;             // �ļ��׸����̿�Ŀ��
        public int type;              // �ļ�����,0:�ļ�,1:�ļ��� 
        public string time;           // �޸�ʱ��         
        public int size;              // �ļ���С

        public FCB() { }
        public FCB(string fileName, int type, string time, int size)
        {
            this.fileName = fileName;
            this.type = type;
            this.time = time;
            this.size = size;
        }
        public FCB(string fileName, int type, string time, int size, int start)
        {
            this.fileName = fileName;
            this.type = type;
            this.time = time;
            this.size = size;
            this.start = start;
        }
    }

    public class Disk
    {
        public int size;                             // ��������
        public int blocksRemain;                     // ����ʣ�����
        public int free;                             // ���д��̿���ͷ
        public int blockSize;                        // ���̿��С
        public int blockNum;                         // ���̿���
        public string[] block = new string[] { };    // ���̿�
        public int[] fat = new int[] { };            // FAT


        public Disk(int sz, int blocksize)
        {
            size = sz;
            blockSize = blocksize;
            blockNum = size / blockSize;
            blocksRemain = blockNum;
            free = 0;
            block = new string[blockNum];
            fat = new int[blockNum];
            for (int i = 0; i < blockNum - 1; i++)
            {
                fat[i] = i + 1;        // ��ʼ��λͼ��Ϊȫ������
                block[i] = "";         // ��ʼ����������Ϊ��
            }
            fat[blockNum - 1] = -1;
            block[blockNum - 1] = "";
        }

        // Ϊ�ļ�������̿鲢д������
        public bool AllocateSpace(FCB fcb, string content)
        {
            if (content.Length == 0)
            {
                fcb.start = -1;   // ����Ϊ��, ����Ҫ����洢�ռ�
                fcb.size = 0;
                return true;
            }
            int blocksNeeded = GetBlockSize(content.Length);   // ������̿���
            if (blocksNeeded <= blocksRemain)
            {
                // �ӿ�����ͷȡ���������
                fcb.start = free;
                for (int i = 0; i < blocksNeeded - 1; ++i)
                {
                    block[free] = content.Substring(blockSize * i, blockSize);
                    free = fat[free];
                }
                block[free] = content.Substring(blockSize * (blocksNeeded - 1), content.Length - blockSize * (blocksNeeded - 1));
                int tmp = fat[free];
                fat[free] = -1;
                free = tmp;
                blocksRemain -= blocksNeeded;
                fcb.size = content.Length;
                return true;
            }
            else
            {
                return false;
            }

        }

        // ��ȡ�ļ�����
        public string GetFileContent(FCB fcb)
        {
            if (fcb.start == -1)
            {
                return "";
            }
            else
            {
                string content = "";
                int p = fcb.start;
                int blocksOccupied = GetBlockSize(fcb.size);
                for (int i = 0; i < blocksOccupied; ++i)
                {
                    content += block[p];
                    p = fat[p];
                }
                return content;
            }
        }

        // ɾ���ļ�����
        public void FreeDiskSpace(int start, int sz)
        {
            if (start == -1)
            {
                return;
            }
            int p = start;
            int blocksOccupied = GetBlockSize(sz);
            for(int i = 0; i < blocksOccupied - 1; ++i)
            {
                block[p] = "";
                p = fat[p];
            }
            block[p] = "";
            fat[p] = free;
            free = start;     // �ͷŵĴ��̿����������
            blocksRemain += blocksOccupied;
        }

        // �����ļ�����
        public bool UpdateFile(FCB fcb, string newContent)
        {
            if (this.blocksRemain < GetBlockSize(newContent.Length) - GetBlockSize(fcb.size))
            {
                return false;
            }
            int oldStart = fcb.start, oldSize = fcb.size;
            FreeDiskSpace(oldStart, oldSize);    // ɾ��ԭ���ݲ��ͷŴ��̿�
            AllocateSpace(fcb, newContent);      // �����´��̿鲢д������
            return true;
        }

        // �洢�ļ�������̿�
        private int GetBlockSize(int size)
        {
            return (size + blockSize - 1) / blockSize;
        }

        // ���ļ���д��������
        public void WriteDisk()
        {
            BinaryWriter bw = new BinaryWriter(new FileStream(Application.StartupPath + "\\disk.dat", FileMode.Create));
            for (int i = 0; i < this.blockNum; ++i)
            {
                bw.Write(block[i]);
            }
            bw.Close();
        }

        // ���ļ���дFAT����
        public void WriteFAT()
        {
            BinaryWriter bw = new BinaryWriter(new FileStream(Application.StartupPath + "\\fat.dat", FileMode.Create));
            bw.Write(this.blocksRemain);
            bw.Write(this.free);
            for (int i = 0; i < this.blockNum; ++i)
            {
                bw.Write(fat[i]);
            }
            bw.Close();
        }

        // ���ļ��ж���������
        public void ReadDisk()
        {
            BinaryReader br = new BinaryReader(new FileStream(Application.StartupPath + "\\disk.dat", FileMode.Open));
            for (int i = 0; i < this.blockNum; ++i)
            {
                this.block[i] = br.ReadString();
            }
            br.Close();
        }

        // ���ļ��ж�FAT����
        public void ReadFAT()
        {
            BinaryReader br = new BinaryReader(new FileStream(Application.StartupPath + "\\fat.dat", FileMode.Open));
            this.blocksRemain = br.ReadInt32();
            this.free = br.ReadInt32();
            for (int i = 0; i < this.blockNum; ++i)
            {
                this.fat[i] = br.ReadInt32();
            }
            br.Close();
        }
    }
}
