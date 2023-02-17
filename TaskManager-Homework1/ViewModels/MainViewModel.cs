using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Threading;
using System.Windows;
using TaskManager_Homework1.Commands;
using TaskManager_Homework1.Views;

namespace TaskManager_Homework1.ViewModels;

public class MainViewModel
{
	public ObservableCollection<Process> Processes { get; set; }
	public ObservableCollection<Process> BlackList { get; set; } = new ObservableCollection<Process>();

	MainView mainView;
	DispatcherTimer timer;

	public RelayCommand CreateTaskCommand { get; set; }
	public RelayCommand EndTaskCommand { get; set; }
	public RelayCommand AddBlackListCommand { get; set; }

	public MainViewModel(MainView _mainView)
	{
		mainView = _mainView;

		timer = new DispatcherTimer();
		timer.Interval = TimeSpan.FromSeconds(3);
		timer.Tick += timer_Tick;
		timer.Start();

		CreateTaskCommand = new RelayCommand
		(
			a => BtnCreateTask_Click(),
			p => true
		);

		EndTaskCommand = new RelayCommand
		(
			a => BtnEndTask_Click(),
			p => true
		);

		AddBlackListCommand = new RelayCommand
		(
			a => BtnAddBlackList_Click(),
			p => true
		);
	}

	public void BtnAddBlackList_Click()
	{
		try
		{
			if (mainView.listviewProcess.SelectedIndex == -1)
				MessageBox.Show("Task not selected!", "Task Manager", MessageBoxButton.OK, MessageBoxImage.Warning);
			else
			{
				Process process = mainView.listviewProcess.SelectedItem as Process;
				BlackList.Add(process);
				Thread.Sleep(3000);
				process.Kill();
				Processes.Remove(process);
				mainView.txbProcess.Text = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void timer_Tick(object sender, EventArgs e)
	{
		Processes = new ObservableCollection<Process>(Process.GetProcesses());
		mainView.listviewProcess.ItemsSource = Processes;
	}

	public void BtnEndTask_Click()
	{
		try
		{
			if (mainView.listviewProcess.SelectedIndex == -1)
				MessageBox.Show("Task not selected!", "Task Manager", MessageBoxButton.OK, MessageBoxImage.Warning);
			else
			{
				var p = mainView.listviewProcess.SelectedItem as Process;
				p.Kill();
				Process process = mainView.listviewProcess.SelectedItem as Process;
				Processes.Remove(process);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void BtnCreateTask_Click()
	{
		try
		{
			if (!string.IsNullOrEmpty(mainView.txbProcess.Text))
			{
				if (!BlackList.Any(pro => pro.ProcessName == mainView.txbProcess.Text))
				{
					Process p = new Process();
					p.StartInfo.FileName = $"{mainView.txbProcess.Text}";
					p.StartInfo.Arguments = " ";
					p.Start();
					Processes.Add(p);
					mainView.txbProcess.Text = null;
				}
				else
					MessageBox.Show("This task is already on the Black List!", "Task Manager", MessageBoxButton.OK, MessageBoxImage.Warning);
			}
			else
				MessageBox.Show("Task name cannot be empty!", "Task Manager", MessageBoxButton.OK, MessageBoxImage.Warning);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}
}