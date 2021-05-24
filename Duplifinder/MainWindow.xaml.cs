using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Daramee.Winston.Dialogs;
using Daramee.Winston.File;

namespace Duplifinder
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly ObservableCollection<FindSet> findSets = new ObservableCollection<FindSet>();

		public MainWindow ()
		{
			InitializeComponent ();

			TreeViewFound.ItemsSource = findSets;
		}

		private void ButtonBrowse_Click (object sender, RoutedEventArgs e)
		{
			var ofd = new OpenFolderDialog();
			if (ofd.ShowDialog(this) == false)
				return;

			TextBoxPath.Text = ofd.FileName;
		}

		private async void ButtonDoFind_Click (object sender, RoutedEventArgs e)
		{
			const double SENSITIVITY = 0.75;

			var path = TextBoxPath.Text;
			var filenameSimilarity = CheckBoxFilenameSimilarity.IsChecked == true;
			var hashValueSameness = CheckBoxHashValueSameness.IsChecked == true;

			findSets.Clear();

			if (filenameSimilarity)
			{
				await Task.Run(() =>
				{
					var files = FilesEnumerator.EnumerateFiles(path, "*", false)
						.Where((filename) => !File.GetAttributes(filename).HasFlag(FileAttributes.Directory))
						.Distinct()
						.ToArray();
					var count = 0;
					var unfinds = new List<string>();
					foreach (var filename in files)
					{
						try
						{
							if (!File.Exists(filename))
								continue;

							var found = false;
							foreach (var findSet in findSets)
							{
								foreach (var f in findSet.Filenames)
								{
									if (!(filename.FilenameSimilarity(f) >= SENSITIVITY))
										continue;

									Dispatcher.BeginInvoke(new Action(
										() =>
										{
											if (!findSet.Filenames.Contains(f))
												findSet.Filenames.Add(f);
										}
									)).Wait();
									found = true;

									break;
								}

								if (found)
									break;
							}

							if (found)
								continue;

							foreach (var unfind in unfinds.Where(unfind => filename.FilenameSimilarity(unfind) >= SENSITIVITY))
							{
								unfinds.Remove (unfind);
								Dispatcher.BeginInvoke(new Action(() =>
								{
									var newFindSet = new FindSet(FindingMethod.FilenameSimiliarity);
									newFindSet.Filenames.Add(unfind);
									newFindSet.Filenames.Add(filename);
									findSets.Add(newFindSet);
								})).Wait();
								found = true;
								break;
							}

							if (!found)
								unfinds.Add(filename);
						}
						finally
						{
							Interlocked.Increment(ref count);
							Dispatcher.BeginInvoke(new Action(() =>
								ProgressBarProgress.Value = count / (double) files.Length)).Wait();
						}
					}
				});
			}

			if (hashValueSameness)
			{
				await Task.Run(() =>
				{

				});
			}

			MessageBox.Show("Done!");
		}

		private void TreeViewItem_DoubleClick(object sender, MouseButtonEventArgs e)
		{
			if ((sender as TreeViewItem)?.DataContext is string filename)
				Process.Start("explorer", $"/select, \"{filename}\"");
		}
	}
}
