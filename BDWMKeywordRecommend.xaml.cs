using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Runtime.InteropServices;//引用命名空间
using System.Data;

using WpfQuery.BDWMKRService;
//using WpfQuery.BDWMAccountService;


namespace WpfQuery
{
    /// <summary>
    /// BDWMKeywordRecommend.xaml 的交互逻辑
    /// </summary>
    public partial class BDWMKeywordRecommend : UserControl
    {
        #region "声明变量"
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retval, int size, string filePath);
        private string strFilePath = "config\\account.ini";//获取INI文件路径
        private string strSec = ""; //INI文件名
        #endregion
        DataTable dt = new DataTable();


        public BDWMKeywordRecommend()
        {
            InitializeComponent();


            dt.Columns.Add("id", typeof(Int32));
            dt.Columns.Add("key", typeof(string));
            dt.Columns.Add("day", typeof(string));
            dt.Columns.Add("vie", typeof(string));
            dgQueryResult.ItemsSource = dt.DefaultView;
        }

        private void btGet_Click(object sender, RoutedEventArgs e)
        {
            //AuthHeader h = new AuthHeader();
            ////读取
            //if (File.Exists(strFilePath))//读取时先要判读INI文件是否存在
            //{
            //    strSec = System.IO.Path.GetFileNameWithoutExtension(strFilePath);
            //    h.username = ContentValue(strSec, "Username");
            //    h.password = Common.DecryptDES(ContentValue(strSec, "Password"), "WpfQuery");//ContentValue(strSec, "Password");
            //    h.token = ContentValue(strSec, "Token");
            //}

            //AccountServiceClient client = new AccountServiceClient();
            //AccountInfoType ait;
            //ResHeader resHeader = client.getAccountInfo(h, out ait);
            //MessageBox.Show(resHeader.desc);


            if (txtKeyword.Text.Trim() == "")
            {
                MessageBox.Show("请输入关键词！");
            }
            else
            {
                dt.Rows.Clear();
                string key = txtKeyword.Text.Trim();//赋值
                AuthHeader h = new AuthHeader();
                //读取
                if (File.Exists(strFilePath))//读取时先要判读INI文件是否存在
                {
                    strSec = System.IO.Path.GetFileNameWithoutExtension(strFilePath);
                    h.username = ContentValue(strSec, "Username");
                    h.password = Common.DecryptDES(ContentValue(strSec, "Password"), "WpfQuery"); //ContentValue(strSec, "Password");
                    h.token = ContentValue(strSec, "Token");
                }
                else
                {
                    MessageBox.Show("account.ini文件不存在");
                    return;
                }
                KRServiceClient client = new KRServiceClient();

                KRResultType[] response = getKRbySeedWord(client, h, txtKeyword.Text.Trim());



                int num = 1;
                if (response != null)
                {
                    foreach (var ba in response)
                    {
                        dt.Rows.Add(num, ba.word, ba.avgShowCnt.ToString(), ba.cmpDegree.ToString());
                        num++;
                    }
                }
            }
        }

        public static KRResultType[] getKRbySeedWord(KRServiceClient client, AuthHeader h, string keyword)
        {
            KRResultType[] response;
            int[] packids = new int[] { };
            int[] regionlist = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 99, 101, 102, 103, 104, 105, 106, 107, 110, 111, 112, 113, 115, 116, 118, 119, 120, 121, 124, 126, 127, 128, 129, 130, 131, 132, 138, 139, 140, 144, 145, 146, 147, 148, 150, 151, 152, 153, 154, 156, 157, 158, 160, 162, 163, 164, 166, 167, 169, 170, 172, 173, 174, 175, 176, 180, 182, 184, 185, 186, 187, 188, 189, 191, 193, 194, 195, 196, 197, 198, 199, 200, 201, 203, 204, 205, 208, 210, 211, 212, 213, 215, 217, 218, 219, 220, 221, 223, 225, 226, 228, 229, 230, 231, 234, 236, 239, 240, 241, 242, 243, 246, 249, 250, 252, 253, 254, 255, 256, 257, 258, 261, 262, 263, 266, 267, 268, 272, 273, 276, 278, 279, 282, 284, 287, 289, 290, 291, 293, 298, 304, 305, 307, 308, 309, 310, 311, 312, 313, 314, 315, 317, 319, 320, 321, 323, 324, 328, 329, 330, 331, 334, 335, 337, 338, 339, 340, 341, 342, 343, 344, 345, 346, 351, 352, 355, 356, 358, 359, 361, 363, 367, 375, 376, 377, 381, 383, 386, 391, 392, 393, 395, 399, 401, 402, 403, 404, 406, 407, 408, 409, 410, 411, 412, 413, 414, 415, 416, 417, 418, 419, 421, 422, 423, 424, 425, 426, 427, 428, 429, 430, 431, 432, 434, 435, 437, 438, 439, 442, 444, 446, 447, 448, 449, 450, 454, 456, 458, 459, 461, 462, 463, 466, 467, 468, 470, 472, 473, 474, 476, 477, 479, 480, 481, 482, 485, 486, 491, 492, 493, 494, 495, 496, 497, 498, 501, 502, 503, 504, 506, 508, 509, 510, 511, 512, 513, 515, 516, 517, 518, 519, 520, 523, 524, 526, 528, 529, 530, 531, 532, 534, 535, 536, 538, 540, 541, 542, 543, 546, 547, 548, 549, 551, 554, 556, 557, 560, 563, 564, 565, 566, 570, 571, 572, 573, 576, 578, 579, 580, 581, 585, 587, 589, 590, 593, 594, 595, 597, 598, 604, 605, 606, 608, 611, 615, 617, 619, 621, 624, 630, 742, 743, 744, 745, 746, 747, 748, 749, 750, 751, 752, 753, 754, 755, 756, 757, 758, 759, 760, 761, 763, 765, 766, 767, 768, 769, 770, 771, 772, 773, 774, 776, 777, 778, 779, 780, 781, 782, 783, 784, 785, 786, 787, 788, 789, 790, 791, 792, 793, 794, 795, 796, 797, 798, 799, 800, 801, 802, 803, 804, 805, 806, 807, 808, 809, 810, 811, 812, 813, 814, 815, 816, 817, 818, 819, 820, 821, 822, 823, 824, 825, 826, 827, 830, 831, 832, 833, 834, 835, 836, 837, 867, 868, 869, 870, 900, 901 };
            long groupId = 9279682;

            ResHeader resHeader = client.getKRBySeed(h, keyword, groupId, regionlist, 7, 30, packids, "reserved", out response);

            MessageBox.Show(resHeader.desc);
            return response;

        }

        private string ContentValue(string Section, string key)
        {
            StringBuilder temp = new StringBuilder(1024);
            GetPrivateProfileString(Section, key, "", temp, 1024, strFilePath);
            return temp.ToString();
        }

    }
}
