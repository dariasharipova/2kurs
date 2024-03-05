using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private List<Bitmap> _bitmaps = new List<Bitmap>();
        private List<Bitmap> _bitmaps_dark = new List<Bitmap>();//состояния при скроле
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                pictureBox1.Image = new Bitmap(openFileDialog1.FileName);
                _bitmaps.Clear();
                _bitmaps_dark.Clear();
                var bitmap = new Bitmap(pictureBox1.Image);
                var bitmap_light = new Bitmap(pictureBox1.Image);
                RunProcessing(bitmap, bitmap_light);
            }
        }
        private void RunProcessing(Bitmap bitmap, Bitmap bitmap_light)
        {
            var pixels = GetPixels(bitmap);
            var currentPixelsSet = new List<Pixel>(pixels); // Copy all pixels from the bitmap
            _bitmaps_dark.Add(bitmap);
            int darkenAmount = 5; // Величина затемнения

            for (int i = 0; i < 50; i++)
            {
                for (int j = 0; j < currentPixelsSet.Count; j++)
                {
                    var onePixel = bitmap.GetPixel(currentPixelsSet[j].Point.X, currentPixelsSet[j].Point.Y);
                    var pixelRed = Math.Max(onePixel.R - darkenAmount, 0);
                    var pixelGreen = Math.Max(onePixel.G - darkenAmount, 0);
                    var pixelBlue = Math.Max(onePixel.B - darkenAmount, 0);

                    bitmap.SetPixel(currentPixelsSet[j].Point.X, currentPixelsSet[j].Point.Y, Color.FromArgb(pixelRed, pixelGreen, pixelBlue));
                }
                _bitmaps_dark.Add(new Bitmap(bitmap));
            }


            for (int i = 50; i < trackBar1.Maximum; i++)
            {
                for (int j = 0; j < currentPixelsSet.Count; j++)
                {
                    var onePixel = bitmap_light.GetPixel(currentPixelsSet[j].Point.X, currentPixelsSet[j].Point.Y);
                    var pixelRed = Math.Min(onePixel.R + 2, 255); 
                    var pixelGreen = Math.Min(onePixel.G + 2, 255); 
                    var pixelBlue = Math.Min(onePixel.B + 2, 255);
                    bitmap_light.SetPixel(currentPixelsSet[j].Point.X, currentPixelsSet[j].Point.Y, Color.FromArgb(pixelRed, pixelGreen, pixelBlue));
                }
                _bitmaps.Add(new Bitmap(bitmap_light));
            }
        }

        private List<Pixel> GetPixels(Bitmap bitmap)
        {
            var pixels = new List<Pixel>(bitmap.Width * bitmap.Height);
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    pixels.Add(new Pixel()
                    {
                        /*Color = bitmap.GetPixel(x, y),*/
                        Point = new Point() { X = x, Y = y }
                    });
                }
            }
            return pixels;
        }
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (_bitmaps == null || _bitmaps.Count == 0)
                return;
            if (_bitmaps_dark == null || _bitmaps_dark.Count == 0)
                return;
            if (trackBar1.Value == 50)
                pictureBox1.Image = _bitmaps[0];
            else
            {
                if (trackBar1.Value > 50)
                {
                    pictureBox1.Image = _bitmaps[trackBar1.Value - 51];
                }
                else
                    pictureBox1.Image = _bitmaps_dark[50 - trackBar1.Value];
            }
        }

        private void файлToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void яркостьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            trackBar1.Visible = true;
        }
    }
}
