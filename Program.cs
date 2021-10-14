using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;

namespace JeuDeLaViePDF
{
    public struct Coords
    {
        private int _x;
        private int _y;

        public int X { get { return _x; } set { _x = value; } }
        public int Y { get { return _y; } set { _y = value; } }

        public Coords(int x_, int y_)
        {
            _x = x_;
            _y = y_;
        }
    }

    public class Cell
    {
        private bool _isAlive;
        public bool isAlive { get { return _isAlive; } set { _isAlive = value; } }

        private bool _nextState = false;
        public bool nextState { get { return _nextState; } set { _nextState = value; } }

        public Cell(bool state)
        {
            _isAlive = state;
        }

        public void ComeAlive()
        {
            _nextState = true;
        }

        public void PassAway()
        {
            _nextState = false;
        }

        public void Update()
        {
            _isAlive = _nextState;
        }
    }


    public class Grid
    {
        private int _n; // taille de la grille
        public int n { get { return _n; } set { _n = value; } }
        public Cell[,] TabCells;

        public Grid(int nbCells, List<Coords> AliveCellsCoords)
        {
            n = nbCells;
            TabCells = new Cell[n, n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    TabCells[i, j] = new Cell(false);
                }
            }
            foreach(Coords c in AliveCellsCoords)
            {
                TabCells[c.X, c.Y].isAlive = true;
            }
        }

        public int getNbAliveNeighboor(int i, int j)
        {
            int cpt = 0;

            List<Coords> neighboor = getCoordsNeighboors(i, j);
            foreach(Coords c in neighboor)
            {
                if (TabCells[c.X, c.Y].isAlive)
                    cpt++;
            }

            return cpt;
        }

        public List<Coords> getCoordsNeighboors(int i, int j)
        {
            List<Coords> neighboors = new List<Coords>();

            // haut
            if (i > 0)
            {
                neighboors.Add(new Coords(i - 1, j));
                // gauche
                if (j > 0)
                    neighboors.Add(new Coords(i - 1, j - 1));
                // droite
                if (j < n)
                    neighboors.Add(new Coords(i - 1, j + 1));
            }
            // droite
            if (j < n)
                neighboors.Add(new Coords(i, j + 1));
            // bas
            if (i < n)
            {
                neighboors.Add(new Coords(i + 1, j));
                // gauche
                if (j > 0)
                    neighboors.Add(new Coords(i + 1, j - 1));
                // droite
                if (j < n)
                    neighboors.Add(new Coords(i + 1, j + 1));
            }
            // gauche
            if (j > 0)
                neighboors.Add(new Coords(i, j - 1));

            return neighboors;
        }

        public List<Coords> getCoordsCellAlive()
        {
            List<Coords> cellsAlive = new List<Coords>();

            for(int i = 0; i < n; i++)
            {
                for(int j = 0; j < n; j++)
                {
                    if (TabCells[i, j].isAlive)
                        cellsAlive.Add(new Coords(i, j));
                }
            }

            return cellsAlive;
        }

        public void DisplayGrid()
        {
            // on crée la ligne de base
            string ligne = "+";
            for (int i = 0; i < _n; ++i)
            {
                ligne += "---+";
            }

            for (int i = 0; i < _n; ++i)
            {
                Console.WriteLine(ligne);

                string colonne = "|";
                for (int j = 0; j < _n; ++j)
                {
                    if (TabCells[i, j].isAlive)
                        colonne += " X |";
                    else
                        colonne += "   |";
                }

                Console.WriteLine(colonne);
            }
            Console.WriteLine(ligne);
        }

        public void UpdateGrid()
        {
            List<Coords> alive = getCoordsCellAlive();
            foreach(Coords cAlive in alive)
            {
                if (getNbAliveNeighboor(cAlive.X, cAlive.Y) < 2 || getNbAliveNeighboor(cAlive.X, cAlive.Y) > 3)
                    TabCells[cAlive.X, cAlive.Y].PassAway();
                else
                    TabCells[cAlive.X, cAlive.Y].ComeAlive();

                List<Coords> cNeiboors = getCoordsNeighboors(cAlive.X, cAlive.Y);
                foreach(Coords neigh in cNeiboors)
                {
                    if (getNbAliveNeighboor(neigh.X, neigh.Y) == 3)
                        TabCells[neigh.X, neigh.Y].ComeAlive();
                }
            }

            // mise à jour du tableau
            foreach (Cell c in TabCells)
                c.Update();
        }
    }
    public class Game
    {
        private int n; // taille de la grille
        private int iter;
        public Grid grid;
        List<Coords> AliveCellsCoords;

        public Game(int nbCells)
        {
            AliveCellsCoords = new List<Coords>
            {
                new Coords(1, 1),
                new Coords(1, 2),
                new Coords(1, 3)
            };


            n = nbCells;
            grid = new Grid(n, AliveCellsCoords);
        }
        public Game(int nbCells, int nbIterations) : this(nbCells)
        {
            iter = nbIterations;
        }
        

        public void RunGameConsole()
        {
            grid.DisplayGrid();
            for (int i = 0; i < iter; i++)
            {
                grid.UpdateGrid();
                grid.DisplayGrid();
                Thread.Sleep(1000);
            }
        }

        public void Paint(Graphics g)
        {
            SolidBrush whiteBrush = new SolidBrush(Color.White);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (grid.TabCells[i, j].isAlive)
                    {
                        g.FillRectangle(whiteBrush, j * 10, i * 10, 10, 10);
                    }
                }
            }
        }
    }


    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
