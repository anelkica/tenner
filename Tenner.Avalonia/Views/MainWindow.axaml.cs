using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Metadata;
using Avalonia.Platform;
using Lucide.Avalonia;
using System;
using System.Runtime.InteropServices;

namespace Tenner.Avalonia.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SetupTitlebar();
        }

        private void SetupMica()
        {

        }

        private void SetupTitlebar()
        {
            void UpdateMaximizeIcon()
            {
                MaximizeButton.Content = WindowState == WindowState.Maximized 
                    ? new LucideIcon { Kind = LucideIconKind.SquaresUnite, Size = 16 } 
                    : new LucideIcon { Kind = LucideIconKind.Square, Size = 16 };
            }

            Titlebar.PointerPressed += (_, e) => BeginMoveDrag(e);

            MinimizeButton.Click += (_, _) => WindowState = WindowState.Minimized;
            MaximizeButton.Click += (_, _) => WindowState = WindowState == WindowState.Maximized ? WindowState.Normal  : WindowState.Maximized;

            PropertyChanged += (_, e) => { if (e.Property == WindowStateProperty) UpdateMaximizeIcon(); };
            CloseButton.Click += (_, _) => Close();
        }
    }
}