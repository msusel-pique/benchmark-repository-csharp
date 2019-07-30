using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace WpfApp1
{
	public class MainWindowViewModel
			: INotifyPropertyChanged
	{
		private int tilePadding = 6;


		/// <summary>
		/// Property to define the Tile Padding.
		/// </summary>
		public int TilePadding
		{
			get => tilePadding;
			set {
				tilePadding = value;
				RaisePropertyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
