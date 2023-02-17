using System;
using System.Windows;
using TaskManager_Homework1.ViewModels;

namespace TaskManager_Homework1.Views;

public partial class MainView : Window
{
	public MainViewModel MainViewModel { get; set; }

	public MainView()
	{
		InitializeComponent();

		try
		{
			MainViewModel = new MainViewModel(this);
			DataContext = MainViewModel;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Exception occured...", MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}
}