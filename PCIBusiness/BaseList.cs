using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PCIBusiness
{
	public abstract class BaseList : StdDisposable, IEnumerable
	{
		protected string         sqlSelect;
		protected string         sqlOrder;
		protected string         sql;
		protected List<BaseData> objList;
		protected BaseData       objItem;
		protected DBConn         dbConn;
		protected int            rowsTotal;
		protected int            rowsPerPage;

//		Constructor/Destructor
//		----------------------

		public BaseList() : base()
		{
			objList = new List<BaseData>();
		}

//		No longer needed - this is handled by the base class (StdDisposable).

//		~BaseList()
//		{
//			Clear();
//		}

      public override void Close()
      {
      // This will automatically be called by the base class destructor (StdDisposable).

		//	Clean up the derived class (may or may not exist)
         CleanUp();

		//	Clean up the base class
			try
			{
				Tools.CloseDB(ref dbConn);
				objList.Clear();
			}
			catch
			{ }
			objItem = null;
			objList = null;
			dbConn  = null;
      }

		public virtual void CleanUp()
		{
		//	This method can be overridden in the derived class to CLEAN UP stuff - not to initialize in the beginning
		//	Nothing here, so can completely override it in the derived class
		}

//		IEnumerable interface
//		---------------------

//		IEnumerator IEnumerable.GetEnumerator()
		public IEnumerator GetEnumerator()
		{
			return objList.GetEnumerator();
		}

//		Generic item creation ... must override
//		---------------------------------------

		public abstract BaseData NewItem();

//		General list methods
//		--------------------

		public int Count
		{
			get { return objList.Count; }
		}
		public BaseData Item(int index)
		{
			try
			{
				return objList[index];
			}
			catch { }
			return null;
		}

//		SQL Stuff
//		---------

		public int RowsTotal
		{
			get {  return rowsTotal; }
		}
		public int RowsPerPage
		{
			get {  return rowsPerPage; }
		}

		protected int LoadDataFromSQL(int maxRows=0,string sourceInfo="")
		{
			if ( maxRows < 1 )
				maxRows = Constants.MaxRowsSQL;
			return LoadDataFromSQL(null,maxRows,null,0,sourceInfo);
		}
		protected int LoadDataFromSQL(object[][] parms,int maxRows,System.Type classType=null,byte pagingMode=0,string sourceInfo="")
		{
			objList.Clear();

			if ( Tools.OpenDB(ref dbConn) )
			{
				int    q          = 0;
				dbConn.SourceInfo = sourceInfo;

				if ( maxRows > short.MaxValue )
					maxRows   = short.MaxValue;
				else if ( maxRows < 1 )
					maxRows   = Constants.MaxRowsSQL;

				if ( dbConn.Execute(sql,true,parms) )
				{
					if ( pagingMode == (byte)PCIBusiness.Constants.PagingMode.AllowScreenPaging && ! dbConn.EOF && dbConn.ColStatus("RowsPerPage") == Constants.DBColumnStatus.ColumnOK )
					{
						rowsTotal   = dbConn.ColLong("RowsTotal");
						rowsPerPage = dbConn.ColLong("RowsPerPage");
						dbConn.NextResultSet();
					}
					while ( ! dbConn.EOF && q < maxRows )
					{
						q++;
						if ( classType == null )
							objItem = NewItem();
						else
							objItem = (PCIBusiness.BaseData)Activator.CreateInstance(classType);
						objItem.RowNumber = q;
						objItem.LoadData(dbConn);
						objList.Add(objItem);
						if ( pagingMode != (byte)PCIBusiness.Constants.PagingMode.DoNotReadNextRow )
							dbConn.NextRow();
					}
				}
			}
			Tools.CloseDB(ref dbConn);
			return objList.Count;
		}
		protected int ExecuteSQL(object[][] parms,bool alwaysClose=false,bool noRowsIsError=true,string connectionName="")
		{
			int ret = 0;

//			Tools.LogInfo("BaseList.ExecuteSQL",sql,199);

			if ( ! Tools.OpenDB(ref dbConn,connectionName) )
				ret = 1;
			else if ( ! dbConn.Execute(sql,true,parms) )
				ret = 2;
			else if ( dbConn.EOF && noRowsIsError )
				ret = 3;

			if ( ret > 0 || alwaysClose )
				Tools.CloseDB(ref dbConn);

			return ret;
		}
	}
}
