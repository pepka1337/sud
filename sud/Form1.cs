using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sud
{
    public partial class Form1 : Form
    {
        private const int N = 3;
        private const int SizeButton = 40;
        private int[,] _map = new int[N * N, N * N];
        private readonly Button[,] _buttons = new Button[N * N, N * N];

        public Form1()
        {
            InitializeComponent();
            GenerateMap();
        }

        private void GenerateMap()
        {
            for (var i = 0; i < N * N; i++)
            {
                for (var j = 0; j < N * N; j++)
                {
                    _map[i, j] = (i * N + i / N + j) % (N * N) + 1;
                    _buttons[i, j] = new Button();
                }
            }

            MatrixTransposition();
            SwapRowsInBlock();
            SwapColumnsInBlock();
            SwapBlocksInRow();
            SwapBlocksInColumn();
            var r = new Random();
            for (var i = 0; i < 40; i++)
            {
                ShuffleMap(r.Next(0, 5));
            }

            CreateMap();
            HideCells();
        }

        public void HideCells()
        {
            var N = 40;
            var r = new Random();
            while (N > 0)
            {
                for (var i = 0; i < Form1.N * Form1.N; i++)
                {
                    for (var j = 0; j < Form1.N * Form1.N; j++)
                    {
                        if (!string.IsNullOrEmpty(_buttons[i, j].Text))
                        {
                            var a = r.Next(0, 3);
                            _buttons[i, j].Text = a == 0 ? "" : _buttons[i, j].Text;
                            _buttons[i, j].Enabled = a == 0 ? true : false;

                            if (a == 0)
                                N--;
                            if (N <= 0)
                                break;
                        }
                    }

                    if (N <= 0)
                        break;
                }
            }
        }

        private void ShuffleMap(int i)
        {
            switch (i)
            {
                case 0:
                    MatrixTransposition();
                    break;
                case 1:
                    SwapRowsInBlock();
                    break;
                case 2:
                    SwapColumnsInBlock();
                    break;
                case 3:
                    SwapBlocksInRow();
                    break;
                case 4:
                    SwapBlocksInColumn();
                    break;
                default:
                    MatrixTransposition();
                    break;
            }
        }

        private void SwapBlocksInColumn()
        {
            var r = new Random();
            var block1 = r.Next(0, N);
            var block2 = r.Next(0, N);
            while (block1 == block2)
                block2 = r.Next(0, N);
            block1 *= N;
            block2 *= N;
            for (var i = 0; i < N * N; i++)
            {
                var k = block2;
                for (var j = block1; j < block1 + N; j++)
                {
                    (_map[i, j], _map[i, k]) = (_map[i, k], _map[i, j]);
                    k++;
                }
            }
        }

        private void SwapBlocksInRow()
        {
            var r = new Random();
            var block1 = r.Next(0, N);
            var block2 = r.Next(0, N);
            while (block1 == block2)
                block2 = r.Next(0, N);
            block1 *= N;
            block2 *= N;
            for (var i = 0; i < N * N; i++)
            {
                var k = block2;
                for (var j = block1; j < block1 + N; j++)
                {
                    (_map[j, i], _map[k, i]) = (_map[k, i], _map[j, i]);
                    k++;
                }
            }
        }
        public void SwapRowsInBlock()
        {
            var r = new Random();
            var block = r.Next(0, N);
            var row1 = r.Next(0, N);
            var line1 = block * N + row1;
            var row2 = r.Next(0, N);
            while (row1 == row2)
                row2 = r.Next(0, N);
            var line2 = block * N + row2;
            for (var i = 0; i < N * N; i++)
            {
                (_map[line1, i], _map[line2, i]) = (_map[line2, i], _map[line1, i]);
            }
        }
        public string PrintMap(int[,] sudoku)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < sudoku.GetLength(0); i++)
            {
                for (int j = 0; j < sudoku.GetLength(1); j++)
                {
                    sb.Append(sudoku[i, j] + " ");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public void SwapColumnsInBlock()
        {
            var r = new Random();
            var block = r.Next(0, N);
            var row1 = r.Next(0, N);
            var line1 = block * N + row1;
            var row2 = r.Next(0, N);
            while (row1 == row2)
                row2 = r.Next(0, N);
            var line2 = block * N + row2;
            for (var i = 0; i < N * N; i++)
            {
                (_map[i, line1], _map[i, line2]) = (_map[i, line2], _map[i, line1]);
            }
        }

        public void MatrixTransposition()
        {
            var tMap = new int[N * N, N * N];
            for (var i = 0; i < N * N; i++)
            {
                for (var j = 0; j < N * N; j++)
                {
                    tMap[i, j] = _map[j, i];
                }
            }

            _map = tMap;
        }

        private void CreateMap()
        {
            for (var i = 0; i < N * N; i++)
            {
                for (var j = 0; j < N * N; j++)
                {
                    var button = new Button();
                    _buttons[i, j] = button;
                    button.Size = new Size(SizeButton, SizeButton);
                    button.Text = _map[i, j].ToString();
                    button.BackColor = Color.Coral;
                    button.Click += OnCellPressed;
                    button.Location = new Point(j * SizeButton, i * SizeButton);
                    this.Controls.Add(button);
                }
            }
        }

        private void OnCellPressed(object sender, EventArgs e)
        {
            var pressedButton = sender as Button;
            var buttonText = pressedButton?.Text;
            if (string.IsNullOrEmpty(buttonText))
            {
                pressedButton.Text = "1";
            }
            else
            {
                var num = int.Parse(buttonText);
                num++;
                if (num == 10)
                    num = 1;
                pressedButton.Text = num.ToString();
            }
        }


       
        public bool SolveSudoku(int[,] sudoku)
        {
            int row, col;

            // Поиск пустой клетки
            if (!FindEmptyCell(sudoku, out row, out col))
            {
                return true; // Если нет пустых клеток, то судоку уже решено
            }

            // Попробовать разместить числа от 1 до 9 в пустую клетку
            for (int num = 1; num <= 9; num++)
            {
                // Проверяем, можно ли разместить число в клетке
                if (CanPlaceNumber(sudoku, row, col, num))
                {
                    // Если можно, размещаем число в клетке
                    sudoku[row, col] = num;

                    // Рекурсивно вызываем SolveSudoku для следующей клетки
                    if (SolveSudoku(sudoku))
                        return true; // Если решение найдено, возвращаем true

                    // Если решение не найдено, отменяем размещение и пытаемся с другим числом
                    sudoku[row, col] = 0;
                }
            }

            // Если ни одно число не подходит, возвращаем false
            return false;
        }

        public bool CanPlaceNumber(int[,] sudoku, int row, int col, int num)
        {
            // Отладочный вывод
            MessageBox.Show($"Пытаемся разместить число {num} в клетке [{row}, {col}]");

            // Проверяем, что число не встречается в строке и столбце
            for (int i = 0; i < sudoku.GetLength(0); i++)
            {
                if (sudoku[row, i] == num || sudoku[i, col] == num)
                    return false;
            }

            // Проверяем, что число не встречается в квадрате 3x3
            int startRow = row - row % 3;
            int startCol = col - col % 3;
            for (int i = startRow; i < startRow + 3; i++)
            {
                for (int j = startCol; j < startCol + 3; j++)
                {
                    if (sudoku[i, j] == num)
                        return false;
                }
            }

            // Если число не встречается ни в строке, ни в столбце, ни в квадрате 3x3, можно его разместить
            return true;
        }

        private static bool FindEmptyCell(int[,] sudoku, out int row, out int col)
        {
            row = -1;
            col = -1;
            for (int i = 0; i < sudoku.GetLength(0); i++)
            {
                for (int j = 0; j < sudoku.GetLength(1); j++)
                {
                    if (sudoku[i, j] == 0)
                    {
                        row = i;
                        col = j;
                        return true;
                    }
                }
            }
            return false;
        }

        

        private void button1_Click_1(object sender, EventArgs e)
        {
            for (var i = 0; i < N * N; i++)
            {
                for (var j = 0; j < N * N; j++)
                {
                    var btnText = _buttons[i, j].Text;
                    if (btnText == _map[i, j].ToString()) continue;
                    MessageBox.Show("Неверно!");
                    return;
                }
            }

            MessageBox.Show("Верно!");
            for (var i = 0; i < N * N; i++)
            {
                for (var j = 0; j < N * N; j++)
                {
                    this.Controls.Remove(_buttons[i, j]);
                }
            }

            GenerateMap();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            {
                if (SolveSudoku(_map))
                {
                    MessageBox.Show("Подсказка!\n\nРешение:\n" + PrintMap(_map));
                }
                else
                {
                    MessageBox.Show("Не удалось найти решение для судоку.");
                }

            }
        }

        
    }
}

    

