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

namespace Minesweeper
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Game game;

        public MainWindow()
        {
            InitializeComponent();

            int numRows = 10;
            int numColumns = 10;

            game = new Game(numRows, numColumns);

            theGrid.HorizontalAlignment = HorizontalAlignment.Left;
            theGrid.VerticalAlignment = VerticalAlignment.Top;
            //theGrid.ShowGridLines = true;

            for (int row = 0; row < numRows; row++)
            {
                RowDefinition rowDef = new RowDefinition();
                theGrid.RowDefinitions.Add(rowDef);

            }
            for (int column = 0; column < numColumns; column++)
            {
                ColumnDefinition colDef = new ColumnDefinition();
                theGrid.ColumnDefinitions.Add(colDef);
            }

            this.renderBoard();

        }

        private void renderBoard()
        {
            theGrid.Children.Clear();
            minesRemainingLabel.Content = "" + game.getMineCount();

            foreach (List<Cell> row in game.getCells())
            {
                foreach (Cell cell in row)
                {
                    MineButton button = new MineButton();
                    button.setRow(cell.row);
                    button.setColumn(cell.column);
                    BitmapImage bmp = new BitmapImage(new Uri(@"pack://application:,,,/minesweeper_tiles.jpg", UriKind.Absolute));
                    ImageBrush brush = new ImageBrush(bmp);
                    brush.TileMode = TileMode.Tile;
                    brush.Stretch = Stretch.Fill;

                    if (cell.touched || game.isGameover())
                    {
                        if (cell.mine)
                        {
                            Label label = new Label();
                            brush.Viewbox = new Rect(0.5, 0, 0.25, 0.333);
                            label.Background = brush;
                            label.HorizontalContentAlignment = HorizontalAlignment.Center;
                            label.VerticalContentAlignment = VerticalAlignment.Center;
                            Grid.SetColumnSpan(label, 1);
                            Grid.SetColumn(label, cell.column);
                            Grid.SetRowSpan(label, 1);
                            Grid.SetRow(label, cell.row);
                            theGrid.Children.Add(label);
                            continue;
                        }
                        else if (cell.n == 0)
                        {
                            Label label = new Label();
                            brush.Viewbox = new Rect(0.75, 0, 0.25, 0.333);
                            label.Background = brush;
                            label.HorizontalContentAlignment = HorizontalAlignment.Center;
                            label.VerticalContentAlignment = VerticalAlignment.Center;
                            Grid.SetColumnSpan(label, 1);
                            Grid.SetColumn(label, cell.column);
                            Grid.SetRowSpan(label, 1);
                            Grid.SetRow(label, cell.row);
                            theGrid.Children.Add(label);
                            continue;
                        }
                        else
                        {
                            switch(cell.n)
                            {
                                case (1):
                                    brush.Viewbox = new Rect(0, 0.333, 0.25, 0.333);
                                    break;
                                case (2):
                                    brush.Viewbox = new Rect(0.25, 0.333, 0.25, 0.333);
                                    break;
                                case (3):
                                    brush.Viewbox = new Rect(0.5, 0.333, 0.25, 0.333);
                                    break;
                                case (4):
                                    brush.Viewbox = new Rect(0.75, 0.333, 0.25, 0.333);
                                    break;
                                case (5):
                                    brush.Viewbox = new Rect(0, 0.333, 0.25, 0.333);
                                    break;
                                case (6):
                                    brush.Viewbox = new Rect(0.25, 0.666, 0.25, 0.333);
                                    break;
                                case (7):
                                    brush.Viewbox = new Rect(0.5, 0.666, 0.25, 0.333);
                                    break;
                                case (8):
                                    brush.Viewbox = new Rect(0.75, 0.666, 0.25, 0.333);
                                    break;
                            }
                            
                            Label label = new Label();
                            label.Background = brush;
                            label.HorizontalContentAlignment = HorizontalAlignment.Center;
                            label.VerticalContentAlignment = VerticalAlignment.Center;
                            Grid.SetColumnSpan(label, 1);
                            Grid.SetColumn(label, cell.column);
                            Grid.SetRowSpan(label, 1);
                            Grid.SetRow(label, cell.row);
                            theGrid.Children.Add(label);
                            continue;
                        }
                    }
                    else if(cell.flagged)
                    {
                        brush.Viewbox = new Rect(0.25, 0, 0.25, 0.333);
                    }
                    else
                    {
                        brush.Viewbox = new Rect(0, 0, 0.25, 0.333);
                    }

                    Grid.SetColumnSpan(button, 1);
                    Grid.SetColumn(button, cell.column);
                    Grid.SetRowSpan(button, 1);
                    Grid.SetRow(button, cell.row);
                    button.Background = brush;
                    theGrid.Children.Add(button);

                    button.MouseDown += new MouseButtonEventHandler(gridButtonRightClicked);
                    button.Click += new RoutedEventHandler(gridButtonClicked);
                }

            }
        }

        private void gridButtonRightClicked(object sender, MouseButtonEventArgs e)
        {
            MineButton button = (MineButton)sender;
            game.flag(button.getRow(), button.getColumn());
            this.renderBoard();
        }
        private void gridButtonClicked(object sender, EventArgs e)
        {
            MineButton button = (MineButton)sender;
            List<List<Cell>> cells = game.getCells();
            game.touch(button.getRow(),button.getColumn());
            this.renderBoard();
        }

        private void newGameButton_Click(object sender, RoutedEventArgs e)
        {
            game = new Game(game.getNumRows(), game.getNumColumns());
            this.renderBoard();
        }
    }
}
