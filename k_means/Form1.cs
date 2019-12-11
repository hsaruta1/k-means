using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accord.Imaging.Converters;
using Accord.MachineLearning;
using Accord.Math;
using Accord.Statistics.Distributions.DensityKernels;
using Accord.Math.Distances;

namespace k_means
{
    public partial class Form1 : Form
    {
        public Bitmap img = null;
        public string filePath = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void runKMeans()
        {            
            int k = (int)numClusters.Value;
            
            Bitmap image = img;
           
            ImageToArray imageToArray = new ImageToArray(min: -1, max: +1);
            ArrayToImage arrayToImage = new ArrayToImage(image.Width, image.Height, min: -1, max: +1);
            
            double[][] pixels; imageToArray.Convert(image, out pixels);
            
            KMeans kmeans = new KMeans(k, new SquareEuclidean())
            {
                Tolerance = 0.05
            };
          
            int[] idx = kmeans.Learn(pixels).Decide(pixels);
            
            pixels.Apply((x, i) => kmeans.Clusters.Centroids[idx[i]], result: pixels);
           
            Bitmap result; arrayToImage.Convert(pixels, out result);

            pictureBox1.Image = result;
        }

        private void selectImageButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofDialog = new OpenFileDialog();

            // デフォルトのフォルダを指定する
            ofDialog.InitialDirectory = @"C:";

            //ダイアログのタイトルを指定する
            ofDialog.Title = "画像ファイル選択";

            //ダイアログを表示する
            if (ofDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = ofDialog.FileName;
                img = new Bitmap(Image.FromFile(filePath));
                pictureBox1.Image = img;
            }
            else
            {
                //Console.WriteLine("キャンセルされました");
            }

            // オブジェクトを破棄する
            ofDialog.Dispose();
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            img = new Bitmap(Image.FromFile(filePath));
            pictureBox1.Image = img;
            checkBox1.Checked = false;
        }

        private void numClusters_ValueChanged(object sender, EventArgs e)
        {
            runKMeans();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked == true)
            {
                runKMeans();
            }
        }
    }
}
