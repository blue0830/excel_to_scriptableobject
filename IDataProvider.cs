using System.Collections.Generic;

public interface IDataProvider<T>
	{

		bool Init();

		void Reset();
		
		T GetById(int id);

		IReadOnlyCollection<T> GetItems();
	}