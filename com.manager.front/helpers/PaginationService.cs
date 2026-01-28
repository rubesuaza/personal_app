namespace com.manager.front.helpers
{
	public class PaginationService<T>
	{
		private IEnumerable<T> _items;
		public int CurrentPage { get; private set; }
		public int PageSize { get; }

		public PaginationService(IEnumerable<T> items, int pageSize = 5)
		{
			_items = items;
			PageSize = pageSize;
			CurrentPage = 1;
		}

		public IEnumerable<T> GetCurrentPageItems()
		{
			return _items.Skip((CurrentPage - 1) * PageSize).Take(PageSize);
		}

		public int CountItems => _items.Count();

		public int TotalPages => (int)Math.Ceiling((double)_items.Count() / PageSize);

		public void NextPage()
		{
			if (CurrentPage < TotalPages)
			{
				CurrentPage++;
			}
		}

		public void PreviousPage()
		{
			if (CurrentPage > 1)
			{
				CurrentPage--;
			}
		}

		public void UpdateItems(IEnumerable<T> items)
		{
			_items = items;
			CurrentPage = 1; // Reinicia la página actual
		}

	}
}
