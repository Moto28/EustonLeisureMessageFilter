﻿using System;
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
using System.Windows.Shapes;

namespace EustonLeisureMessageFilter
{
    /// <summary>
    /// Interaction logic for CreateMessage.xaml
    /// </summary>
    public partial class CreateMessage : Window
    {
        public CreateMessage()
        {
            InitializeComponent();
            subjectLbl.Visibility = Visibility.Hidden;
            subjectTxtBox.Visibility = Visibility.Hidden;
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //allows the window to move
            DragMove();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow newWindow = new MainWindow();
            newWindow.Show();
            this.Close();
        }

        private void messageTypeComboBox_GotFocus(object sender, RoutedEventArgs e)
        {

            if (messageTypeComboBox.Text.ToString() == "E")
            {
                subjectLbl.Visibility = Visibility.Visible;
                subjectTxtBox.Visibility = Visibility.Visible;
            }
            else
            {
                subjectLbl.Visibility = Visibility.Hidden;
                subjectTxtBox.Visibility = Visibility.Hidden;
            }

        }
    }
}
