using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Minesweeper
{
    public class Game
    {
        private int numRows;
        private int numColumns;
        private int mineCount = 0;
        private bool gameover = false;
        private bool won = false;
        private int flaggedMines = 0;

        List<List<Cell>> cells;

        public Game(int numRows, int numColumns)
        {
            this.numRows = numRows;
            this.numColumns = numColumns;
            this.cells = this.generateGrid();
        }

        private List<List<Cell>> generateGrid()
        {
            List<List<Cell>> cells = new List<List<Cell>>();
            Random random = new Random();
            for (int ri = 0; ri < numRows; ri++)
            {
                List<Cell> row = new List<Cell>();
                for (var ci = 0; ci < numColumns; ci++)
                {

                    bool isMine = random.Next(0, 1000) < 100;

                    if (isMine)
                    {
                        this.mineCount += 1;
                    }
                    Cell cell = new Cell();
                    cell.row = ri;
                    cell.column = ci;
                    cell.mine = isMine;
                    row.Add(cell);
                }
                cells.Add(row);
            }

            foreach (List<Cell> row in cells)
            {
                foreach (Cell cell in row)
                {
                    if (!cell.mine)
                    {
                        continue;
                    }
                    foreach (Cell surrounding_cell in this.getSurroundingCells(cell.row, cell.column, cells))
                    {
                        surrounding_cell.n += 1;
                    }
                }
            }

            return cells;

        }
        private List<Cell> getSurroundingCells(int row, int column, List<List<Cell>> cells)
        {
            List<Cell> surrounding = new List<Cell>();
            // N
            if (row - 1 >= 0)
            {
                surrounding.Add(cells[row - 1][column]);
                // NW
                if (column - 1 >= 0)
                {
                    surrounding.Add(cells[row - 1][column - 1]);
                }
                // NE
                if (column + 2 <= numColumns)
                {
                    surrounding.Add(cells[row - 1][column + 1]);
                }
            }

            // S
            if (row + 2 <= numRows)
            {
                surrounding.Add(cells[row + 1][column]);
                // SE
                if (column + 2 <= numColumns)
                {
                    surrounding.Add(cells[row + 1][column + 1]);
                }
                // SW
                if (column - 1 >= 0)
                {
                    surrounding.Add(cells[row + 1][column - 1]);
                }
            }
            // E
            if (column + 2 <= numColumns)
            {
                surrounding.Add(cells[row][column + 1]);
            }
            // W
            if (column - 1 >= 0)
            {
                surrounding.Add(cells[row][column - 1]);
            }
            return surrounding;
        }

        public List<Cell> getConnectedBlankCells(int row, int column, List<List<Cell>> cells, List<Cell> visited)
        {
            List<Cell> connected = new List<Cell>();
            foreach (Cell cell in this.getSurroundingCells(row, column, cells))
            {
                if (visited.Contains(cell))
                {
                    continue;
                }
                visited.Add(cell);
                if (cell.n > 0)
                {
                    // include the boundary cells
                    connected.Add(cell);
                    continue;
                }
                connected.Add(cell);

                foreach (Cell other_cell in this.getConnectedBlankCells(cell.row, cell.column, cells, visited))
                {
                    connected.Add(other_cell);
                }
            }
            return connected;
        }

        public void touch(int row, int column)
        {
            Cell cell = cells[row][column];

            if (cell.flagged)
            {
                return;
            }

            cell.touched = true;
            if (cell.mine)
            {
                this.gameover = true;
                this.won = false;
                MessageBox.Show("Game over!");
            }
            else if (cell.n == 0)
            {
                foreach (Cell connected_cell in this.getConnectedBlankCells(cell.row, cell.column, this.cells, new List<Cell>()))
                {
                    if (!connected_cell.flagged)
                    {
                        connected_cell.touched = true;
                    }
                }
            }
            bool has_won = true;
            foreach (List<Cell> rrow in this.cells)
            {
                foreach (Cell ccell in rrow)
                {
                    if (!ccell.touched && !ccell.mine)
                    {
                        has_won = false;
                    }
                }
            }
            this.won = has_won;
            if(has_won)
            {
                MessageBox.Show("Yay! You win!");
            }
        }

        public void flag(int row,int column)
        {
            Cell cell = this.cells[row][column];
            this.flaggedMines += 1 * (cell.flagged ? -1 : 1);
            cell.flagged = !cell.flagged;
        }

        public bool didWin()
        {
            return this.won;
        }


        public bool isGameover()
        {
            return this.gameover;
        }

        public int getNumRows()
        {
            return this.numRows;
        }

        public int getNumColumns()
        {
            return this.numColumns;
        }

        public List<List<Cell>> getCells()
        {
            return this.cells;
        }

        public int getMineCount()
        {
            return this.mineCount - this.flaggedMines;
        }
    }
}
