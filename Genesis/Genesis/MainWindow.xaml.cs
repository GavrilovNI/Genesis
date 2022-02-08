using Genesis.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Genesis
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Map _map;
        private Vector2Int imageSize;

        private int _speed = 1;

        int RGBToInt(int r, int g, int b)
        {
            int colorData = r << 16; // R
            colorData |= g << 8;   // G
            colorData |= b << 0;   // B
            return colorData;
        }

        private unsafe void DrawPixel(WriteableBitmap writeableBitmap, Vector2Int pos, int color)
        {
            IntPtr pBackBuffer = writeableBitmap.BackBuffer;

            // Find the address of the pixel to draw.
            pBackBuffer += pos.Y * writeableBitmap.BackBufferStride;
            pBackBuffer += pos.X * 4;

            // Assign the color data to the pixel.
            *((int*)pBackBuffer) = color;

        }

        private unsafe void DrawSquare(WriteableBitmap writeableBitmap, Vector2Int pos, Vector2Int size, int color)
        {
            for(int y = pos.Y; y < pos.Y + size.Y; y++)
            {
                for (int x = pos.X; x < pos.X + size.X; x++)
                {
                    DrawPixel(writeableBitmap, new Vector2Int(x, y), color);
                }
            }

        }

        private void MarkDirty(WriteableBitmap writeableBitmap, Vector2Int pos)
        {
            writeableBitmap.AddDirtyRect(new Int32Rect(pos.X, pos.Y, 1, 1));
        }

        private void MarkDirty(WriteableBitmap writeableBitmap, Vector2Int pos, Vector2Int size)
        {
            writeableBitmap.AddDirtyRect(new Int32Rect(pos.X, pos.Y, size.X, size.Y));
        }

        private WriteableBitmap CreateBitmap()
        {
            WriteableBitmap writeableBitmap = new WriteableBitmap(
                imageSize.X,
                imageSize.Y,
                96,
                96,
                PixelFormats.Bgr32,
                null);

            Vector2Int entitySize = new Vector2Int((int)(writeableBitmap.Width / _map.Size.X), (int)(writeableBitmap.Height / _map.Size.Y));
            try
            {
                // Reserve the back buffer for updates.
                writeableBitmap.Lock();

                unsafe
                {
                    DrawSquare(writeableBitmap, new Vector2Int(0, 0), new Vector2Int((int)writeableBitmap.Width, (int)writeableBitmap.Height), RGBToInt(255, 255, 255));
                    MarkDirty(writeableBitmap, new Vector2Int(0, 0), new Vector2Int((int)writeableBitmap.Width, (int)writeableBitmap.Height));

                    int i = 0;
                    foreach (var entity in _map)
                    {
                        int color = RGBToInt(255, 255, 255);
                        switch (entity.Value.Type)
                        {
                            case EntityType.Bot:
                                Bot bot = (Bot)entity.Value;
                                float max = MathF.Max(MathF.Max(bot.EnergyFromOrganic, bot.EnergyFromMinerals), bot.EnergyFromSun);
                                if(max == 0)
                                {
                                    color = RGBToInt(0, 255, 0);
                                }
                                else
                                {
                                    int r = (int)(255f * bot.EnergyFromOrganic / max);
                                    int g = (int)(255f * bot.EnergyFromSun / max);
                                    int b = (int)(255f * bot.EnergyFromMinerals / max);
                                    color = RGBToInt(r, g, b);
                                }
                                break;
                            case EntityType.Wall:
                                color = RGBToInt(255, 0, 0);
                                break;
                            case EntityType.Organic:
                                color = RGBToInt(100, 100, 100);
                                break;
                        }

                        Vector2Int pos = new Vector2Int(entity.Key.X * entitySize.X, entity.Key.Y * entitySize.Y);
                        if (pos.X < 0)
                            throw new Exception();
                        DrawSquare(writeableBitmap, pos, entitySize, color);
                        i++;
                    }
                }

            }
            finally
            {
                writeableBitmap.Unlock();
            }
            return writeableBitmap;
        }

        private void DrawMapInOtherThread()
        {
            WriteableBitmap writeableBitmap = CreateBitmap();
            writeableBitmap.Freeze();
            this.Dispatcher.Invoke(() =>
            {
                ImageViewer1.Source = writeableBitmap;
            });
        }
        private void DrawMap()
        {
            WriteableBitmap writeableBitmap = CreateBitmap();
            ImageViewer1.Source = writeableBitmap;
        }

        private readonly System.Timers.Timer _timer;

        public MainWindow()
        {
            //int randomSeed = 100;

            _map = new Map(new Vector2Int(180, 96), new Random());

            Bot firstBot = new Bot(_map, 10);
            firstBot.Direction = BotDirection.Down;
            _map.AddEntity(new Vector2Int(_map.Size.X / 2, 0), firstBot);

            InitializeComponent();

            RoutedPropertyChangedEventHandler<double> sliderValuerChanged = (object sender, RoutedPropertyChangedEventArgs<double> e) =>
            {
                int value = (int)e.NewValue;
                speedSlider.Value = value;
                speedLabel.Content = value.ToString();
                _speed = value;
            };
            sliderValuerChanged.Invoke(null, new RoutedPropertyChangedEventArgs<double>(speedSlider.Value, _speed));
            speedSlider.ValueChanged += sliderValuerChanged;

            imageSize = new Vector2Int((int)ImageViewer1.Width, (int)ImageViewer1.Height);

            DrawMap();

            _timer = new Timer(1); //Updates every quarter second.
            _timer.Elapsed += (object? source, ElapsedEventArgs e) =>
            {
                _timer.Enabled = false;
                for (int i = 0; i < _speed; i++)
                    _map.DoIteration();
                DrawMapInOtherThread();
                _timer.Enabled = true;
            };
            _timer.Enabled = true;
        }
    }
}
