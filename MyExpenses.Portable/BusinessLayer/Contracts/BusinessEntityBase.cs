using MyExpenses.Portable.DataLayer.SQLite;

namespace MyExpenses.Portable.BusinessLayer.Contracts {
	/// <summary>
	/// Business entity base class. Provides the ID property.
	/// </summary>
	public abstract class BusinessEntityBase : Interfaces.IBusinessEntity {
		public BusinessEntityBase ()
		{
		}
		
		/// <summary>
		/// Gets or sets the Database ID.
		/// </summary>
		[PrimaryKey, AutoIncrement]
    public int ID { get; set; }
	}
}

