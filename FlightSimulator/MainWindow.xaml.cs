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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlightSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FlyViewModel vm;
        public MainWindow()
        {
            InitializeComponent();
            vm = new FlyViewModel(new MyFlyModel(new MyTelnetClient()));
            DataContext = vm;
            vm.model.connect("127.0.0.1", 5402);
            vm.model.start();
        }
    }
}
