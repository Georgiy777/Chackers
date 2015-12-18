using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Checkers
{
    public partial class MainWindow : Window
    {
        public int[,] fieldArr = new int[8,8]{
                                    {0,21,0,22,0,23,0,24},
                                    {17,0,18,0,19,0,20,0},
                                    {0,13,0,14,0,15,0,16},
                                    {0,0,0,0,0,0,0,0},
                                    {0,0,0,0,0,0,0,0},
                                    {9,0,10,0,11,0,12,0},
                                    {0,5,0,6,0,7,0,8},
                                    {1,0,2,0,3,0,4,0}}; // поле , заполненное ИД шашки

        Point oldPosition;
        int x = -865, y = 550; // начальные координаты для расстановки шашек
        Checker1 check1, damagedChecker1; // шашки и шашки , находящиеся под ударом
        Checker2 check2, damagedChecker2;
        SpecialChecker1 special_checker1, special_damagedChecker1; // дамки и дамки , находящиеся под ударом
        SpecialChecker2 special_checker2, special_damagedChecker2;
        turnShow turn_Show; // переменная для создания на поле подсказок в виде подсветок
        List<turnShow> turnShow_list = new List<turnShow>(); // список всех текущих подсветок
        List<Checker1> checker1_list = new List<Checker1>(); // список всех черных шашек
        List<Checker2> checker2_list = new List<Checker2>(); // список всех белых шашек
        List<SpecialChecker1> special_checker1_list = new List<SpecialChecker1>(); // список всех черных дамок
        List<SpecialChecker2> special_checker2_list = new List<SpecialChecker2>(); // список всех белых дамок
        int curIndexOfCheck_I, curIndexOfCheck_J; // идексы текущей шашки 
        int curIndexOfDamagedCheck_I, curIndexOfDamagedCheck_J; // идексы текущей шашки, которая идет под сруб 
        bool white_turn = true, // ходят белые - если true , черные - если false
            isLocked = false, // запрещает выбирать шашки , если атака не окончена , обязуем продолжать атаку
            isSpecialChecker = false; // проверяем ходим ли мы дамкой или обычной

        public MainWindow()
        {
            InitializeComponent();
            createField(true); // создаем поле
        }

        private void mainChecker1_method()
        {
            if (!white_turn) return;

            for (int i = 0; i < Math.Sqrt(fieldArr.Length); i++)
            {
                for (int j = 0; j < Math.Sqrt(fieldArr.Length); j++)
                {
                    if (fieldArr[i, j] == Convert.ToInt16(check1.Name.Split('w')[1]))
                    {
                        curIndexOfCheck_I = i;
                        curIndexOfCheck_J = j;

                        if (isLocked) return;
                        if (turnShow_list.Count > 0)
                        {
                            for (int k = 0; k < turnShow_list.Count; k++)
                            {
                                grid.Children.Remove(turnShow_list[k]);
                            }
                        }
                        try
                        {
                            if (fieldArr[i - 1, j - 1] == 0)
                            {
                                turn_Show = new turnShow();
                                turn_Show.Name = "w" + (i - 1).ToString() + (j - 1).ToString();
                                turn_Show.Margin = new Thickness(check1.Margin.Left - 160, check1.Margin.Top - 160, 0, 0);
                                turn_Show.MouseLeftButtonDown += new MouseButtonEventHandler(turn_Show_MouseLeftButtonDown);

                                turnShow_list.Add(turn_Show);
                                grid.Children.Add(turn_Show);
                            }
                        }
                        catch { }
                        try
                        {
                            if (fieldArr[i - 1, j + 1] == 0)
                            {
                                turn_Show = new turnShow();
                                turn_Show.Name = "w" + (i - 1).ToString() + (j + 1).ToString();
                                turn_Show.Margin = new Thickness(check1.Margin.Left + 160, check1.Margin.Top - 160, 0, 0);
                                turn_Show.MouseLeftButtonDown += new MouseButtonEventHandler(turn_Show_MouseLeftButtonDown);

                                turnShow_list.Add(turn_Show);
                                grid.Children.Add(turn_Show);
                            }
                        }
                        catch { }
                        secondChecker1_method(i, j);
                    }
                }
            }
        } // основной метод для работы с черными

        private bool secondChecker1_method(int i, int j)
        {
            bool inBattle = false;
            try
            {
                if (fieldArr[i - 2, j - 2] == 0 && isBlackCheck(fieldArr[i - 1, j - 1]))
                {
                    for (int l = 0; l < checker2_list.Count; l++)
                    {
                        if (fieldArr[i - 1, j - 1] == Convert.ToInt16(checker2_list[l].Name.Split('b')[1]))
                            damagedChecker2 = checker2_list[l];
                        curIndexOfDamagedCheck_I = i - 1;
                        curIndexOfDamagedCheck_J = j - 1;
                    }
                    for (int l = 0; l < special_checker2_list.Count; l++)
                    {
                        if (fieldArr[i - 1, j - 1] == Convert.ToInt16(special_checker2_list[l].Name.Split('b')[1]))
                            special_damagedChecker2 = special_checker2_list[l];
                        curIndexOfDamagedCheck_I = i - 1;
                        curIndexOfDamagedCheck_J = j - 1;
                    }

                    turn_Show = new turnShow();
                    turn_Show.Name = "w" + (i - 2).ToString() + (j - 2).ToString();
                    turn_Show.Margin = new Thickness(check1.Margin.Left - 320, check1.Margin.Top - 320, 0, 0);
                    turn_Show.MouseLeftButtonDown += new MouseButtonEventHandler(turn_Show_MouseLeftButtonDown);

                    turnShow_list.Add(turn_Show);
                    grid.Children.Add(turn_Show);

                    inBattle = true;
                }
            }
            catch { }
            try
            {
                if (fieldArr[i - 2, j + 2] == 0 && isBlackCheck(fieldArr[i - 1, j + 1]))
                {
                    for (int l = 0; l < checker2_list.Count; l++)
                    {
                        if (fieldArr[i - 1, j + 1] == Convert.ToInt16(checker2_list[l].Name.Split('b')[1]))
                            damagedChecker2 = checker2_list[l];
                        curIndexOfDamagedCheck_I = i - 1;
                        curIndexOfDamagedCheck_J = j + 1;
                    }
                    for (int l = 0; l < special_checker2_list.Count; l++)
                    {
                        if (fieldArr[i - 1, j + 1] == Convert.ToInt16(special_checker2_list[l].Name.Split('b')[1]))
                            special_damagedChecker2 = special_checker2_list[l];
                        curIndexOfDamagedCheck_I = i - 1;
                        curIndexOfDamagedCheck_J = j + 1;
                    }

                    turn_Show = new turnShow();
                    turn_Show.Name = "w" + (i - 2).ToString() + (j + 2).ToString();
                    turn_Show.Margin = new Thickness(check1.Margin.Left + 320, check1.Margin.Top - 320, 0, 0);
                    turn_Show.MouseLeftButtonDown += new MouseButtonEventHandler(turn_Show_MouseLeftButtonDown);

                    turnShow_list.Add(turn_Show);
                    grid.Children.Add(turn_Show);

                    inBattle = true;
                }
            }
            catch { }
            try
            {
                if (fieldArr[i + 2, j + 2] == 0 && isBlackCheck(fieldArr[i + 1, j + 1]))
                {
                    for (int l = 0; l < checker2_list.Count; l++)
                    {
                        if (fieldArr[i + 1, j + 1] == Convert.ToInt16(checker2_list[l].Name.Split('b')[1]))
                            damagedChecker2 = checker2_list[l];
                        curIndexOfDamagedCheck_I = i + 1;
                        curIndexOfDamagedCheck_J = j + 1;
                    }
                    for (int l = 0; l < special_checker2_list.Count; l++)
                    {
                        if (fieldArr[i + 1, j + 1] == Convert.ToInt16(special_checker2_list[l].Name.Split('b')[1]))
                            special_damagedChecker2 = special_checker2_list[l];
                        curIndexOfDamagedCheck_I = i + 1;
                        curIndexOfDamagedCheck_J = j + 1;
                    }

                    turn_Show = new turnShow();
                    turn_Show.Name = "w" + (i + 2).ToString() + (j + 2).ToString();
                    turn_Show.Margin = new Thickness(check1.Margin.Left + 320, check1.Margin.Top + 320, 0, 0);
                    turn_Show.MouseLeftButtonDown += new MouseButtonEventHandler(turn_Show_MouseLeftButtonDown);

                    turnShow_list.Add(turn_Show);
                    grid.Children.Add(turn_Show);

                    inBattle = true;
                }
            }
            catch { }
            try
            {
                if (fieldArr[i + 2, j - 2] == 0 && isBlackCheck(fieldArr[i + 1, j - 1]))
                {
                    for (int l = 0; l < checker2_list.Count; l++)
                    {
                        if (fieldArr[i + 1, j - 1] == Convert.ToInt16(checker2_list[l].Name.Split('b')[1]))
                            damagedChecker2 = checker2_list[l];
                        curIndexOfDamagedCheck_I = i + 1;
                        curIndexOfDamagedCheck_J = j - 1;
                    }
                    for (int l = 0; l < special_checker2_list.Count; l++)
                    {
                        if (fieldArr[i + 1, j + 1] == Convert.ToInt16(special_checker2_list[l].Name.Split('b')[1]))
                            special_damagedChecker2 = special_checker2_list[l];
                        curIndexOfDamagedCheck_I = i + 1;
                        curIndexOfDamagedCheck_J = j + 1;
                    }

                    turn_Show = new turnShow();
                    turn_Show.Name = "w" + (i + 2).ToString() + (j - 2).ToString();
                    turn_Show.Margin = new Thickness(check1.Margin.Left - 320, check1.Margin.Top + 320, 0, 0);
                    turn_Show.MouseLeftButtonDown += new MouseButtonEventHandler(turn_Show_MouseLeftButtonDown);

                    turnShow_list.Add(turn_Show);
                    grid.Children.Add(turn_Show);

                    inBattle = true;
                }
            }
            catch { }
            isSpecialChecker = false;

            if (inBattle) return true;
            else return false;
        }  // вторичный метод для работы с черными

        private void mainChecker2_method()
        {
            if (white_turn) return;

            for (int i = 0; i < Math.Sqrt(fieldArr.Length); i++)
            {
                for (int j = 0; j < Math.Sqrt(fieldArr.Length); j++)
                {
                    if (fieldArr[i, j] == Convert.ToInt16(check2.Name.Split('b')[1]))
                    {
                        curIndexOfCheck_I = i;
                        curIndexOfCheck_J = j;

                        if (isLocked) return;
                        if (turnShow_list.Count > 0)
                        {
                            for (int k = 0; k < turnShow_list.Count; k++)
                            {
                                grid.Children.Remove(turnShow_list[k]);
                            }
                        }
                        try
                        {
                            if (fieldArr[i + 1, j + 1] == 0)
                            {
                                turn_Show = new turnShow();
                                turn_Show.Name = "b" + (i + 1).ToString() + (j + 1).ToString();
                                turn_Show.Margin = new Thickness(check2.Margin.Left + 160, check2.Margin.Top + 160, 0, 0);
                                turn_Show.MouseLeftButtonDown += new MouseButtonEventHandler(turn_Show_MouseLeftButtonDown);

                                turnShow_list.Add(turn_Show);
                                grid.Children.Add(turn_Show);
                            }
                        }
                        catch { }
                        try
                        {
                            if (fieldArr[i + 1, j - 1] == 0)
                            {
                                turn_Show = new turnShow();
                                turn_Show.Name = "b" + (i + 1).ToString() + (j - 1).ToString();
                                turn_Show.Margin = new Thickness(check2.Margin.Left - 160, check2.Margin.Top + 160, 0, 0);
                                turn_Show.MouseLeftButtonDown += new MouseButtonEventHandler(turn_Show_MouseLeftButtonDown);

                                turnShow_list.Add(turn_Show);
                                grid.Children.Add(turn_Show);
                            }
                        }
                        catch { }

                        secondChecker2_method(i, j);
                    }
                }
            }
        } // основной метод для работы с белыми

        private bool secondChecker2_method(int i, int j)
        {
            bool inBattle = false;
            try
            {
                if (fieldArr[i + 2, j + 2] == 0 && isWhiteCheck(fieldArr[i + 1, j + 1]))
                {
                    for (int l = 0; l < checker1_list.Count; l++)
                    {
                        if (fieldArr[i + 1, j + 1] == Convert.ToInt16(checker1_list[l].Name.Split('w')[1]))
                            damagedChecker1 = checker1_list[l];
                        curIndexOfDamagedCheck_I = i + 1;
                        curIndexOfDamagedCheck_J = j + 1;
                    }
                    for (int l = 0; l < special_checker1_list.Count; l++)
                    {
                        if (fieldArr[i + 1, j + 1] == Convert.ToInt16(special_checker1_list[l].Name.Split('w')[1]))
                            special_damagedChecker1 = special_checker1_list[l];
                        curIndexOfDamagedCheck_I = i + 1;
                        curIndexOfDamagedCheck_J = j + 1;
                    }

                    turn_Show = new turnShow();
                    turn_Show.Name = "b" + (i + 2).ToString() + (j + 2).ToString();
                    turn_Show.Margin = new Thickness(check2.Margin.Left + 320, check2.Margin.Top + 320, 0, 0);
                    turn_Show.MouseLeftButtonDown += new MouseButtonEventHandler(turn_Show_MouseLeftButtonDown);

                    turnShow_list.Add(turn_Show);
                    grid.Children.Add(turn_Show);
                    inBattle = true;
                }
            }
            catch { }
            try
            {
                if (fieldArr[i + 2, j - 2] == 0 && isWhiteCheck(fieldArr[i + 1, j - 1]))
                {
                    for (int l = 0; l < checker1_list.Count; l++)
                    {
                        if (fieldArr[i + 1, j - 1] == Convert.ToInt16(checker1_list[l].Name.Split('w')[1]))
                            damagedChecker1 = checker1_list[l];
                        curIndexOfDamagedCheck_I = i + 1;
                        curIndexOfDamagedCheck_J = j - 1;
                    }
                    for (int l = 0; l < special_checker1_list.Count; l++)
                    {
                        if (fieldArr[i + 1, j - 1] == Convert.ToInt16(special_checker1_list[l].Name.Split('w')[1]))
                            special_damagedChecker1 = special_checker1_list[l];
                        curIndexOfDamagedCheck_I = i + 1;
                        curIndexOfDamagedCheck_J = j - 1;
                    }

                    turn_Show = new turnShow();
                    turn_Show.Name = "b" + (i + 2).ToString() + (j - 2).ToString();
                    turn_Show.Margin = new Thickness(check2.Margin.Left - 320, check2.Margin.Top + 320, 0, 0);
                    turn_Show.MouseLeftButtonDown += new MouseButtonEventHandler(turn_Show_MouseLeftButtonDown);

                    turnShow_list.Add(turn_Show);
                    grid.Children.Add(turn_Show);
                    inBattle = true;
                }
            }
            catch { }
            try
            {
                if (fieldArr[i - 2, j - 2] == 0 && isWhiteCheck(fieldArr[i - 1, j - 1]))
                {
                    for (int l = 0; l < checker1_list.Count; l++)
                    {
                        if (fieldArr[i - 1, j - 1] == Convert.ToInt16(checker1_list[l].Name.Split('w')[1]))
                            damagedChecker1 = checker1_list[l];
                        curIndexOfDamagedCheck_I = i - 1;
                        curIndexOfDamagedCheck_J = j - 1;
                    }
                    for (int l = 0; l < special_checker1_list.Count; l++)
                    {
                        if (fieldArr[i - 1, j - 1] == Convert.ToInt16(special_checker1_list[l].Name.Split('w')[1]))
                            special_damagedChecker1 = special_checker1_list[l];
                        curIndexOfDamagedCheck_I = i - 1;
                        curIndexOfDamagedCheck_J = j - 1;
                    }

                    turn_Show = new turnShow();
                    turn_Show.Name = "b" + (i - 2).ToString() + (j - 2).ToString();
                    turn_Show.Margin = new Thickness(check2.Margin.Left - 320, check2.Margin.Top - 320, 0, 0);
                    turn_Show.MouseLeftButtonDown += new MouseButtonEventHandler(turn_Show_MouseLeftButtonDown);

                    turnShow_list.Add(turn_Show);
                    grid.Children.Add(turn_Show);
                    inBattle = true;
                }
            }
            catch { }
            try
            {
                if (fieldArr[i - 2, j + 2] == 0 && isWhiteCheck(fieldArr[i - 1, j + 1]))
                {
                    for (int l = 0; l < checker1_list.Count; l++)
                    {
                        if (fieldArr[i - 1, j + 1] == Convert.ToInt16(checker1_list[l].Name.Split('w')[1]))
                            damagedChecker1 = checker1_list[l];
                        curIndexOfDamagedCheck_I = i - 1;
                        curIndexOfDamagedCheck_J = j + 1;
                    }
                    for (int l = 0; l < special_checker1_list.Count; l++)
                    {
                        if (fieldArr[i - 1, j + 1] == Convert.ToInt16(special_checker1_list[l].Name.Split('w')[1]))
                            special_damagedChecker1 = special_checker1_list[l];
                        curIndexOfDamagedCheck_I = i - 1;
                        curIndexOfDamagedCheck_J = j + 1;
                    }

                    turn_Show = new turnShow();
                    turn_Show.Name = "b" + (i - 2).ToString() + (j + 2).ToString();
                    turn_Show.Margin = new Thickness(check2.Margin.Left + 320, check2.Margin.Top - 320, 0, 0);
                    turn_Show.MouseLeftButtonDown += new MouseButtonEventHandler(turn_Show_MouseLeftButtonDown);

                    turnShow_list.Add(turn_Show);
                    grid.Children.Add(turn_Show);
                    inBattle = true;
                }
            }
            catch { }
            isSpecialChecker = false;

            if (inBattle) return true;
            else return false;
        } // вторичный метод для работы с белыми 

        private void special_mainChecker1_method()
        {
            if (!white_turn) return;

            for (int i = 0; i < Math.Sqrt(fieldArr.Length); i++)
            {
                for (int j = 0; j < Math.Sqrt(fieldArr.Length); j++)
                {
                    if (fieldArr[i, j] == Convert.ToInt16(special_checker1.Name.Split('w')[1]))
                    {
                        curIndexOfCheck_I = i;
                        curIndexOfCheck_J = j;

                        if (isLocked) return;
                        if (turnShow_list.Count > 0)
                        {
                            for (int k = 0; k < turnShow_list.Count; k++)
                            {
                                grid.Children.Remove(turnShow_list[k]);
                            }
                        }
                        try
                        {
                            if (fieldArr[i - 1, j - 1] == 0)
                            {
                                turn_Show = new turnShow();
                                turn_Show.Name = "w" + (i - 1).ToString() + (j - 1).ToString();
                                turn_Show.Margin = new Thickness(special_checker1.Margin.Left - 160, special_checker1.Margin.Top - 160, 0, 0);
                                turn_Show.MouseLeftButtonDown += new MouseButtonEventHandler(turn_Show_MouseLeftButtonDown);

                                turnShow_list.Add(turn_Show);
                                grid.Children.Add(turn_Show);
                            }
                        }
                        catch { }
                        try
                        {
                            if (fieldArr[i - 1, j + 1] == 0)
                            {
                                turn_Show = new turnShow();
                                turn_Show.Name = "w" + (i - 1).ToString() + (j + 1).ToString();
                                turn_Show.Margin = new Thickness(special_checker1.Margin.Left + 160, special_checker1.Margin.Top - 160, 0, 0);
                                turn_Show.MouseLeftButtonDown += new MouseButtonEventHandler(turn_Show_MouseLeftButtonDown);

                                turnShow_list.Add(turn_Show);
                                grid.Children.Add(turn_Show);
                            }
                        }
                        catch { }
                        try
                        {
                            if (fieldArr[i + 1, j + 1] == 0)
                            {
                                turn_Show = new turnShow();
                                turn_Show.Name = "w" + (i + 1).ToString() + (j + 1).ToString();
                                turn_Show.Margin = new Thickness(special_checker1.Margin.Left + 160, special_checker1.Margin.Top + 160, 0, 0);
                                turn_Show.MouseLeftButtonDown += new MouseButtonEventHandler(turn_Show_MouseLeftButtonDown);

                                turnShow_list.Add(turn_Show);
                                grid.Children.Add(turn_Show);
                            }
                        }
                        catch { }
                        try
                        {
                            if (fieldArr[i + 1, j - 1] == 0)
                            {
                                turn_Show = new turnShow();
                                turn_Show.Name = "w" + (i + 1).ToString() + (j - 1).ToString();
                                turn_Show.Margin = new Thickness(special_checker1.Margin.Left - 160, special_checker1.Margin.Top + 160, 0, 0);
                                turn_Show.MouseLeftButtonDown += new MouseButtonEventHandler(turn_Show_MouseLeftButtonDown);

                                turnShow_list.Add(turn_Show);
                                grid.Children.Add(turn_Show);
                            }
                        }
                        catch { }
                        special_secondChecker1_method(i, j);
                    }
                }
            }
        } // основной метод для работы с дамками белых

        private bool special_secondChecker1_method(int i,int j)
        {
            bool inBattle = false;
            try
            {
                if (fieldArr[i - 2, j - 2] == 0 && isBlackCheck(fieldArr[i - 1, j - 1]))
                {
                    for (int l = 0; l < checker2_list.Count; l++)
                    {
                        if (fieldArr[i - 1, j - 1] == Convert.ToInt16(checker2_list[l].Name.Split('b')[1]))
                            damagedChecker2 = checker2_list[l];
                        curIndexOfDamagedCheck_I = i - 1;
                        curIndexOfDamagedCheck_J = j - 1;
                    }
                    for (int l = 0; l < special_checker2_list.Count; l++)
                    {
                        if (fieldArr[i - 1, j - 1] == Convert.ToInt16(special_checker2_list[l].Name.Split('b')[1]))
                            special_damagedChecker2 = special_checker2_list[l];
                        curIndexOfDamagedCheck_I = i - 1;
                        curIndexOfDamagedCheck_J = j - 1;
                    }

                    turn_Show = new turnShow();
                    turn_Show.Name = "w" + (i - 2).ToString() + (j - 2).ToString();
                    turn_Show.Margin = new Thickness(special_checker1.Margin.Left - 320, special_checker1.Margin.Top - 320, 0, 0);
                    turn_Show.MouseLeftButtonDown += new MouseButtonEventHandler(turn_Show_MouseLeftButtonDown);

                    turnShow_list.Add(turn_Show);
                    grid.Children.Add(turn_Show);

                    inBattle = true;
                }
            }
            catch { }
            try
            {
                if (fieldArr[i - 2, j + 2] == 0 && isBlackCheck(fieldArr[i - 1, j + 1]))
                {
                    for (int l = 0; l < checker2_list.Count; l++)
                    {
                        if (fieldArr[i - 1, j + 1] == Convert.ToInt16(checker2_list[l].Name.Split('b')[1]))
                            damagedChecker2 = checker2_list[l];
                        curIndexOfDamagedCheck_I = i - 1;
                        curIndexOfDamagedCheck_J = j + 1;
                    }
                    for (int l = 0; l < special_checker2_list.Count; l++)
                    {
                        if (fieldArr[i - 1, j + 1] == Convert.ToInt16(special_checker2_list[l].Name.Split('b')[1]))
                            special_damagedChecker2 = special_checker2_list[l];
                        curIndexOfDamagedCheck_I = i - 1;
                        curIndexOfDamagedCheck_J = j + 1;
                    }

                    turn_Show = new turnShow();
                    turn_Show.Name = "w" + (i - 2).ToString() + (j + 2).ToString();
                    turn_Show.Margin = new Thickness(special_checker1.Margin.Left + 320, special_checker1.Margin.Top - 320, 0, 0);
                    turn_Show.MouseLeftButtonDown += new MouseButtonEventHandler(turn_Show_MouseLeftButtonDown);

                    turnShow_list.Add(turn_Show);
                    grid.Children.Add(turn_Show);

                    inBattle = true;
                }
            }
            catch { }
            try
            {
                if (fieldArr[i + 2, j + 2] == 0 && isBlackCheck(fieldArr[i + 1, j + 1]))
                {
                    for (int l = 0; l < checker2_list.Count; l++)
                    {
                        if (fieldArr[i + 1, j + 1] == Convert.ToInt16(checker2_list[l].Name.Split('b')[1]))
                            damagedChecker2 = checker2_list[l];
                        curIndexOfDamagedCheck_I = i + 1;
                        curIndexOfDamagedCheck_J = j + 1;
                    }
                    for (int l = 0; l < special_checker2_list.Count; l++)
                    {
                        if (fieldArr[i + 1, j + 1] == Convert.ToInt16(special_checker2_list[l].Name.Split('b')[1]))
                            special_damagedChecker2 = special_checker2_list[l];
                        curIndexOfDamagedCheck_I = i + 1;
                        curIndexOfDamagedCheck_J = j + 1;
                    }

                    turn_Show = new turnShow();
                    turn_Show.Name = "w" + (i + 2).ToString() + (j + 2).ToString();
                    turn_Show.Margin = new Thickness(special_checker1.Margin.Left + 320, special_checker1.Margin.Top + 320, 0, 0);
                    turn_Show.MouseLeftButtonDown += new MouseButtonEventHandler(turn_Show_MouseLeftButtonDown);

                    turnShow_list.Add(turn_Show);
                    grid.Children.Add(turn_Show);

                    inBattle = true;
                }
            }
            catch { }
            try
            {
                if (fieldArr[i + 2, j - 2] == 0 && isBlackCheck(fieldArr[i + 1, j - 1]))
                {
                    for (int l = 0; l < checker2_list.Count; l++)
                    {
                        if (fieldArr[i + 1, j - 1] == Convert.ToInt16(checker2_list[l].Name.Split('b')[1]))
                            damagedChecker2 = checker2_list[l];
                        curIndexOfDamagedCheck_I = i + 1;
                        curIndexOfDamagedCheck_J = j - 1;
                    }
                    for (int l = 0; l < special_checker2_list.Count; l++)
                    {
                        if (fieldArr[i + 1, j - 1] == Convert.ToInt16(special_checker2_list[l].Name.Split('b')[1]))
                            special_damagedChecker2 = special_checker2_list[l];
                        curIndexOfDamagedCheck_I = i + 1;
                        curIndexOfDamagedCheck_J = j - 1;
                    }

                    turn_Show = new turnShow();
                    turn_Show.Name = "w" + (i + 2).ToString() + (j - 2).ToString();
                    turn_Show.Margin = new Thickness(special_checker1.Margin.Left - 320, special_checker1.Margin.Top + 320, 0, 0);
                    turn_Show.MouseLeftButtonDown += new MouseButtonEventHandler(turn_Show_MouseLeftButtonDown);

                    turnShow_list.Add(turn_Show);
                    grid.Children.Add(turn_Show);

                    inBattle = true;
                }
            }
            catch { }
            isSpecialChecker = true;

            if (inBattle) return true;
            else return false;
        }

        private void special_mainChecker2_method()
        {
            if (white_turn) return;

            for (int i = 0; i < Math.Sqrt(fieldArr.Length); i++)
            {
                for (int j = 0; j < Math.Sqrt(fieldArr.Length); j++)
                {
                    if (fieldArr[i, j] == Convert.ToInt16(special_checker2.Name.Split('b')[1]))
                    {
                        curIndexOfCheck_I = i;
                        curIndexOfCheck_J = j;

                        if (isLocked) return;
                        if (turnShow_list.Count > 0)
                        {
                            for (int k = 0; k < turnShow_list.Count; k++)
                            {
                                grid.Children.Remove(turnShow_list[k]);
                            }
                        }
                        try
                        {
                            if (fieldArr[i + 1, j + 1] == 0)
                            {
                                turn_Show = new turnShow();
                                turn_Show.Name = "b" + (i + 1).ToString() + (j + 1).ToString();
                                turn_Show.Margin = new Thickness(special_checker2.Margin.Left + 160, special_checker2.Margin.Top + 160, 0, 0);
                                turn_Show.MouseLeftButtonDown += new MouseButtonEventHandler(turn_Show_MouseLeftButtonDown);

                                turnShow_list.Add(turn_Show);
                                grid.Children.Add(turn_Show);
                            }
                        }
                        catch { }
                        try
                        {
                            if (fieldArr[i + 1, j - 1] == 0)
                            {
                                turn_Show = new turnShow();
                                turn_Show.Name = "b" + (i + 1).ToString() + (j - 1).ToString();
                                turn_Show.Margin = new Thickness(special_checker2.Margin.Left - 160, special_checker2.Margin.Top + 160, 0, 0);
                                turn_Show.MouseLeftButtonDown += new MouseButtonEventHandler(turn_Show_MouseLeftButtonDown);

                                turnShow_list.Add(turn_Show);
                                grid.Children.Add(turn_Show);
                            }
                        }
                        catch { }
                        try
                        {
                            if (fieldArr[i - 1, j - 1] == 0)
                            {
                                turn_Show = new turnShow();
                                turn_Show.Name = "b" + (i - 1).ToString() + (j - 1).ToString();
                                turn_Show.Margin = new Thickness(special_checker2.Margin.Left - 160, special_checker2.Margin.Top - 160, 0, 0);
                                turn_Show.MouseLeftButtonDown += new MouseButtonEventHandler(turn_Show_MouseLeftButtonDown);

                                turnShow_list.Add(turn_Show);
                                grid.Children.Add(turn_Show);
                            }
                        }
                        catch { }
                        try
                        {
                            if (fieldArr[i - 1, j + 1] == 0)
                            {
                                turn_Show = new turnShow();
                                turn_Show.Name = "b" + (i - 1).ToString() + (j + 1).ToString();
                                turn_Show.Margin = new Thickness(special_checker2.Margin.Left + 160, special_checker2.Margin.Top - 160, 0, 0);
                                turn_Show.MouseLeftButtonDown += new MouseButtonEventHandler(turn_Show_MouseLeftButtonDown);

                                turnShow_list.Add(turn_Show);
                                grid.Children.Add(turn_Show);
                            }
                        }
                        catch { }
                        special_secondChecker2_method(i, j);
                    }
                }
            }
        } // основной метод для работы с дамками черных

        private bool special_secondChecker2_method(int i, int j)
        {
            bool inBattle = false;
            try
            {
                if (fieldArr[i + 2, j + 2] == 0 && isWhiteCheck(fieldArr[i + 1, j + 1]))
                {
                    for (int l = 0; l < checker1_list.Count; l++)
                    {
                        if (fieldArr[i + 1, j + 1] == Convert.ToInt16(checker1_list[l].Name.Split('w')[1]))
                            damagedChecker1 = checker1_list[l];
                        curIndexOfDamagedCheck_I = i + 1;
                        curIndexOfDamagedCheck_J = j + 1;
                    }
                    for (int l = 0; l < special_checker1_list.Count; l++)
                    {
                        if (fieldArr[i + 1, j + 1] == Convert.ToInt16(special_checker1_list[l].Name.Split('w')[1]))
                            special_damagedChecker1 = special_checker1_list[l];
                        curIndexOfDamagedCheck_I = i + 1;
                        curIndexOfDamagedCheck_J = j + 1;
                    }

                    turn_Show = new turnShow();
                    turn_Show.Name = "b" + (i + 2).ToString() + (j + 2).ToString();
                    turn_Show.Margin = new Thickness(special_checker2.Margin.Left + 320, special_checker2.Margin.Top + 320, 0, 0);
                    turn_Show.MouseLeftButtonDown += new MouseButtonEventHandler(turn_Show_MouseLeftButtonDown);

                    turnShow_list.Add(turn_Show);
                    grid.Children.Add(turn_Show);

                    inBattle = true;
                }
            }
            catch { }
            try
            {
                if (fieldArr[i + 2, j - 2] == 0 && isWhiteCheck(fieldArr[i + 1, j - 1]))
                {
                    for (int l = 0; l < checker1_list.Count; l++)
                    {
                        if (fieldArr[i + 1, j - 1] == Convert.ToInt16(checker1_list[l].Name.Split('w')[1]))
                            damagedChecker1 = checker1_list[l];
                        curIndexOfDamagedCheck_I = i + 1;
                        curIndexOfDamagedCheck_J = j - 1;
                    }
                    for (int l = 0; l < special_checker1_list.Count; l++)
                    {
                        if (fieldArr[i + 1, j - 1] == Convert.ToInt16(special_checker1_list[l].Name.Split('w')[1]))
                            special_damagedChecker1 = special_checker1_list[l];
                        curIndexOfDamagedCheck_I = i + 1;
                        curIndexOfDamagedCheck_J = j - 1;
                    }

                    turn_Show = new turnShow();
                    turn_Show.Name = "b" + (i + 2).ToString() + (j - 2).ToString();
                    turn_Show.Margin = new Thickness(special_checker2.Margin.Left - 320, special_checker2.Margin.Top + 320, 0, 0);
                    turn_Show.MouseLeftButtonDown += new MouseButtonEventHandler(turn_Show_MouseLeftButtonDown);

                    turnShow_list.Add(turn_Show);
                    grid.Children.Add(turn_Show);

                    inBattle = true;
                }
            }
            catch { }
            try
            {
                if (fieldArr[i - 2, j - 2] == 0 && isWhiteCheck(fieldArr[i - 1, j - 1]))
                {
                    for (int l = 0; l < checker1_list.Count; l++)
                    {
                        if (fieldArr[i - 1, j - 1] == Convert.ToInt16(checker1_list[l].Name.Split('w')[1]))
                            damagedChecker1 = checker1_list[l];
                        curIndexOfDamagedCheck_I = i - 1;
                        curIndexOfDamagedCheck_J = j - 1;
                    }
                    for (int l = 0; l < special_checker1_list.Count; l++)
                    {
                        if (fieldArr[i - 1, j - 1] == Convert.ToInt16(special_checker1_list[l].Name.Split('w')[1]))
                            special_damagedChecker1 = special_checker1_list[l];
                        curIndexOfDamagedCheck_I = i - 1;
                        curIndexOfDamagedCheck_J = j - 1;
                    }

                    turn_Show = new turnShow();
                    turn_Show.Name = "b" + (i - 2).ToString() + (j - 2).ToString();
                    turn_Show.Margin = new Thickness(special_checker2.Margin.Left - 320, special_checker2.Margin.Top - 320, 0, 0);
                    turn_Show.MouseLeftButtonDown += new MouseButtonEventHandler(turn_Show_MouseLeftButtonDown);

                    turnShow_list.Add(turn_Show);
                    grid.Children.Add(turn_Show);

                    inBattle = true;
                }
            }
            catch { }
            try
            {
                if (fieldArr[i - 2, j + 2] == 0 && isWhiteCheck(fieldArr[i - 1, j + 1]))
                {
                    for (int l = 0; l < checker1_list.Count; l++)
                    {
                        if (fieldArr[i - 1, j + 1] == Convert.ToInt16(checker1_list[l].Name.Split('w')[1]))
                            damagedChecker1 = checker1_list[l];
                        curIndexOfDamagedCheck_I = i - 1;
                        curIndexOfDamagedCheck_J = j + 1;
                    }
                    for (int l = 0; l < special_checker1_list.Count; l++)
                    {
                        if (fieldArr[i - 1, j + 1] == Convert.ToInt16(special_checker1_list[l].Name.Split('w')[1]))
                            special_damagedChecker1 = special_checker1_list[l];
                        curIndexOfDamagedCheck_I = i - 1;
                        curIndexOfDamagedCheck_J = j + 1;
                    }

                    turn_Show = new turnShow();
                    turn_Show.Name = "b" + (i - 2).ToString() + (j + 2).ToString();
                    turn_Show.Margin = new Thickness(special_checker2.Margin.Left + 320, special_checker2.Margin.Top - 320, 0, 0);
                    turn_Show.MouseLeftButtonDown += new MouseButtonEventHandler(turn_Show_MouseLeftButtonDown);

                    turnShow_list.Add(turn_Show);
                    grid.Children.Add(turn_Show);

                    inBattle = true;
                }
            }
            catch { }
            isSpecialChecker = true;

            if (inBattle) return true;
            else return false;
        }

        private void createField(bool whiteChoice)
        {
            Field field = new Field();
            grid.Children.Add(field);

            for (int i = 1; i <= 24; i++) // создаем на поле все 24 шашки
            {
                if (i <= 12)
                {
                    if (whiteChoice) add_white_checker(i);
                    else add_black_checker(i);
                }
                else
                {
                    if (whiteChoice) add_black_checker(i);
                    else  add_white_checker(i);
                }
            }
        } // создаем поле

        private void add_white_checker(int i)
        {
            Checker1 check = new Checker1();
            check.MouseLeftButtonDown += new MouseButtonEventHandler(check_MouseLeftButtonDown);
            check.Margin = new Thickness(x += 315, y, 0, 0);
            check.Name = "w"+i.ToString();

            checker1_list.Add(check);
            grid.Children.Add(check);

            if (i % 8 == 0)
            {
                x = -865;
                y -= 157;
            }
            else if (i % 4 == 0)
            {
                x = -707;
                y -= 157;
            }
            if (i == 12) y -= 315;
        } // добавляем белую шашку

        private void add_black_checker(int i)
        {
            Checker2 check = new Checker2();
            check.MouseLeftButtonDown += new MouseButtonEventHandler(check_MouseLeftButtonDown);
            check.Margin = new Thickness(x += 315, y, 0, 0);
            check.Name = "b" + i.ToString();

            checker2_list.Add(check);
            grid.Children.Add(check);

            if (i % 8 == 0)
            {
                x = -865;
                y -= 157;
            }
            else if (i % 4 == 0)
            {
                x = -707;
                y -= 157;
            }
            if (i == 12) y -= 315;
        } // добавляем черную шашку

        private bool isBlackCheck(int cell)
        {
            for (int i = 13; i <= 24; i++)
            {
                if (cell == i) return true;
            }
            return false;
        } // проверка на цвет шашки в ячейке

        private bool isWhiteCheck(int cell)
        {
            for (int i = 1; i <= 12; i++)
            {
                if (cell == i) return true;
            }
            return false;
        } // проверка на цвет шашки в ячейке

        private void isWin()
        {
            if (checker1_list.Count == 0 && special_checker1_list.Count == 0) MessageBox.Show("Второй игрок победил", "Win");
            else if (checker2_list.Count == 0 && special_checker2_list.Count == 0) MessageBox.Show("Первый игрок победил", "Win");
        } // проверка на выйгрыш

        private void isSpecial()
        {
            if (!white_turn)
            {
                try
                {
                    if (!isSpecialChecker)
                    {
                        if (fieldArr[0, 1].Equals(Convert.ToInt16(check1.Name.Split('w')[1])) ||
                            fieldArr[0, 3].Equals(Convert.ToInt16(check1.Name.Split('w')[1])) ||
                            fieldArr[0, 5].Equals(Convert.ToInt16(check1.Name.Split('w')[1])) ||
                            fieldArr[0, 7].Equals(Convert.ToInt16(check1.Name.Split('w')[1])))
                        {
                            special_checker1 = new SpecialChecker1();
                            special_checker1.Margin = check1.Margin;
                            special_checker1.Name = check1.Name;
                            special_checker1.MouseLeftButtonDown += new MouseButtonEventHandler(special_checker1_MouseLeftButtonDown);

                            grid.Children.Remove(check1);
                            checker1_list.Remove(check1);

                            grid.Children.Add(special_checker1);
                            special_checker1_list.Add(special_checker1);
                        }
                    }
                }
                catch { }
            }
            else
            {
                try
                {
                    if (!isSpecialChecker)
                    {
                        if (fieldArr[7, 0].Equals(Convert.ToInt16(check2.Name.Split('b')[1])) ||
                            fieldArr[7, 2].Equals(Convert.ToInt16(check2.Name.Split('b')[1])) ||
                            fieldArr[7, 4].Equals(Convert.ToInt16(check2.Name.Split('b')[1])) ||
                            fieldArr[7, 6].Equals(Convert.ToInt16(check2.Name.Split('b')[1])))
                        {
                            special_checker2 = new SpecialChecker2();
                            special_checker2.Margin = check2.Margin;
                            special_checker2.Name = check2.Name;
                            special_checker2.MouseLeftButtonDown += new MouseButtonEventHandler(special_checker1_MouseLeftButtonDown);

                            grid.Children.Remove(check2);
                            checker2_list.Remove(check2);

                            grid.Children.Add(special_checker2);
                            special_checker2_list.Add(special_checker2);
                        }
                    }
                }
                catch { }
            }
        } // проверка на появление дамки

        private void check_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!isLocked)
                {
                    check1 = (Checker1)sender;
                    mainChecker1_method();
                }
            }
            catch
            {
                if (!isLocked)
                {
                    check2 = (Checker2)sender;
                    mainChecker2_method();
                }
            }
        } // событие нажатия на шашку 

        private void turn_Show_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(white_turn)Console.Beep(1000, 40); // звуковое сопровождение нажатия на белую шашку
            else Console.Beep(1000, 23); // звуковое сопровождение нажатия на черную шашку

            turnShow turn__show=(turnShow)sender;

            for (int k = 0; k < turnShow_list.Count; k++)
            {
                grid.Children.Remove(turnShow_list[k]);  // удаляем все подсветы на поле
            }

            if (white_turn) // ход белых
            {
                bool isContinue = false;

                if (!isSpecialChecker) // если не дамка
                {
                    if (check1.Margin.Left - 320 == turn__show.Margin.Left || check1.Margin.Left + 320 == turn__show.Margin.Left)
                    {
                        for (int l = 0; l < checker2_list.Count; l++)
                        {
                            if (checker2_list[l] == damagedChecker2)
                            {
                                grid.Children.Remove(checker2_list[l]); // удаляем шашку с поля 
                                checker2_list.Remove(damagedChecker2); // удаляем шашку из списка

                                fieldArr[curIndexOfDamagedCheck_I, curIndexOfDamagedCheck_J] = 0;

                                isContinue = true;
                            }
                        }

                        for (int l = 0; l < special_checker2_list.Count; l++)
                        {
                            if (special_checker2_list[l] == special_damagedChecker2)
                            {
                                grid.Children.Remove(special_checker2_list[l]); // удаляем шашку с поля 
                                special_checker2_list.Remove(special_damagedChecker2); // удаляем шашку из списка

                                fieldArr[curIndexOfDamagedCheck_I, curIndexOfDamagedCheck_J] = 0;

                                isContinue = true;
                            }
                        }
                    }

                    check1.Margin = new Thickness(turn__show.Margin.Left, turn__show.Margin.Top, 0, 0);

                    fieldArr[Convert.ToInt16(turn__show.Name.Split('w')[1][0]) - 48, Convert.ToInt16(turn__show.Name.Split('w')[1][1]) - 48]
                        = fieldArr[curIndexOfCheck_I, curIndexOfCheck_J];

                    fieldArr[curIndexOfCheck_I, curIndexOfCheck_J] = 0;
                }
                else // если дамка
                {
                    if (special_checker1.Margin.Left - 320 == turn__show.Margin.Left || special_checker1.Margin.Left + 320 == turn__show.Margin.Left)
                    {
                        for (int l = 0; l < checker2_list.Count; l++)
                        {
                            if (checker2_list[l] == damagedChecker2)
                            {
                                grid.Children.Remove(checker2_list[l]); // удаляем шашку с поля 
                                checker2_list.Remove(damagedChecker2); // удаляем шашку из списка

                                fieldArr[curIndexOfDamagedCheck_I, curIndexOfDamagedCheck_J] = 0;

                                isContinue = true;
                            }
                        }
                        
                        for (int l = 0; l < special_checker2_list.Count; l++)
                        {
                            if (special_checker2_list[l] == special_damagedChecker2)
                            {
                                grid.Children.Remove(special_checker2_list[l]); // удаляем шашку с поля 
                                special_checker2_list.Remove(special_damagedChecker2); // удаляем шашку из списка

                                fieldArr[curIndexOfDamagedCheck_I, curIndexOfDamagedCheck_J] = 0;

                                isContinue = true;
                            }
                        }
                    }

                    special_checker1.Margin = new Thickness(turn__show.Margin.Left, turn__show.Margin.Top, 0, 0);

                    fieldArr[Convert.ToInt16(turn__show.Name.Split('w')[1][0]) - 48, Convert.ToInt16(turn__show.Name.Split('w')[1][1]) - 48]
                        = fieldArr[curIndexOfCheck_I, curIndexOfCheck_J];

                    fieldArr[curIndexOfCheck_I, curIndexOfCheck_J] = 0;
                }

                if (isContinue)
                {
                    if (!isSpecialChecker)
                    {
                        if (!secondChecker1_method(Convert.ToInt16(turn__show.Name.Split('w')[1][0]) - 48, Convert.ToInt16(turn__show.Name.Split('w')[1][1]) - 48))
                        {
                            white_turn = false;
                            isLocked = false;
                        }
                        else
                        {
                            isLocked = true;
                        }
                        mainChecker1_method();
                    }
                    else
                    {
                        if (!special_secondChecker1_method(Convert.ToInt16(turn__show.Name.Split('w')[1][0]) - 48, Convert.ToInt16(turn__show.Name.Split('w')[1][1]) - 48))
                        {
                            white_turn = false;
                            isLocked = false;
                        }
                        else
                        {
                            isLocked = true;
                        }
                        special_mainChecker1_method();
                    }
                }
                else
                {
                    white_turn = false;
                    isLocked = false;
                }
                isWin();
                isSpecial();
            }
            else  // ход черных
            {
                bool isContinue = false;

                if (!isSpecialChecker) // если не дамка
                {
                    if (check2.Margin.Left - 320 == turn__show.Margin.Left || check2.Margin.Left + 320 == turn__show.Margin.Left)
                    {
                        for (int l = 0; l < checker1_list.Count; l++)
                        {
                            if (checker1_list[l] == damagedChecker1)
                            {
                                grid.Children.Remove(checker1_list[l]); // удаляем шашку с поля
                                checker1_list.Remove(damagedChecker1); // удаляем шашку из списка

                                fieldArr[curIndexOfDamagedCheck_I, curIndexOfDamagedCheck_J] = 0;

                                isContinue = true;
                            }
                        }

                        for (int l = 0; l < special_checker1_list.Count; l++)
                        {
                            if (special_checker1_list[l] == special_damagedChecker1)
                            {
                                grid.Children.Remove(special_checker1_list[l]); // удаляем шашку с поля
                                special_checker1_list.Remove(special_damagedChecker1); // удаляем шашку из списка

                                fieldArr[curIndexOfDamagedCheck_I, curIndexOfDamagedCheck_J] = 0;

                                isContinue = true;
                            }
                        }
                    }

                    check2.Margin = new Thickness(turn__show.Margin.Left, turn__show.Margin.Top, 0, 0);

                    fieldArr[Convert.ToInt16(turn__show.Name.Split('b')[1][0]) - 48, Convert.ToInt16(turn__show.Name.Split('b')[1][1]) - 48]
                        = fieldArr[curIndexOfCheck_I, curIndexOfCheck_J];

                    fieldArr[curIndexOfCheck_I, curIndexOfCheck_J] = 0;
                }
                else // если дамка
                {
                    if (special_checker2.Margin.Left - 320 == turn__show.Margin.Left || special_checker2.Margin.Left + 320 == turn__show.Margin.Left)
                    {
                        for (int l = 0; l < checker1_list.Count; l++)
                        {
                            if (checker1_list[l] == damagedChecker1)
                            {
                                grid.Children.Remove(checker1_list[l]); // удаляем шашку с поля
                                checker1_list.Remove(damagedChecker1); // удаляем шашку из списка

                                fieldArr[curIndexOfDamagedCheck_I, curIndexOfDamagedCheck_J] = 0;

                                isContinue = true;
                            }
                        }

                        for (int l = 0; l < special_checker1_list.Count; l++)
                        {
                            if (special_checker1_list[l] == special_damagedChecker1)
                            {
                                grid.Children.Remove(special_checker1_list[l]); // удаляем шашку с поля
                                special_checker1_list.Remove(special_damagedChecker1); // удаляем шашку из списка

                                fieldArr[curIndexOfDamagedCheck_I, curIndexOfDamagedCheck_J] = 0;

                                isContinue = true;
                            }
                        }
                    }

                    special_checker2.Margin = new Thickness(turn__show.Margin.Left, turn__show.Margin.Top, 0, 0);

                    fieldArr[Convert.ToInt16(turn__show.Name.Split('b')[1][0]) - 48, Convert.ToInt16(turn__show.Name.Split('b')[1][1]) - 48]
                        = fieldArr[curIndexOfCheck_I, curIndexOfCheck_J];

                    fieldArr[curIndexOfCheck_I, curIndexOfCheck_J] = 0;
                }
                if (isContinue)
                {
                    if (!isSpecialChecker)
                    {
                        if (!secondChecker2_method(Convert.ToInt16(turn__show.Name.Split('b')[1][0]) - 48, Convert.ToInt16(turn__show.Name.Split('b')[1][1]) - 48))
                        {
                            white_turn = true;
                            isLocked = false;
                        }
                        else
                        {
                            isLocked = true;
                        }
                        mainChecker2_method();
                    }
                    else
                    {
                        if (!special_secondChecker2_method(Convert.ToInt16(turn__show.Name.Split('b')[1][0]) - 48, Convert.ToInt16(turn__show.Name.Split('b')[1][1]) - 48))
                        {
                            white_turn = true;
                            isLocked = false;
                        }
                        else
                        {
                            isLocked = true;
                        }
                        special_mainChecker2_method();
                    }
                }
                else
                {
                    white_turn = true;
                    isLocked = false;
                }
                isWin();
                isSpecial();
            }
        } // событие нажатия на подсветку ходов

        private void special_checker1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (!isLocked)
                {
                    special_checker1 = (SpecialChecker1)sender;

                    special_mainChecker1_method();
                }
            }
            catch
            {
                if (!isLocked)
                {
                    special_checker2 = (SpecialChecker2)sender;

                    special_mainChecker2_method();
                }
            }
        } // событие нажатия на дамку
    }
}
