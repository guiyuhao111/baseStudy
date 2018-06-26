/**M_FileManage.cs*/
using System;
namespace DSMTMALL.DB.Model
{
	/// <summary>
	/// M_FileManage:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_FileManage
	{
		public M_FileManage()
		{}
		#region Model
		private int _filesysid;
		private int _filetype=0;
		private string _filename;
		private string _filepath;
		private string _filelabel;
		private DateTime _creattime= Convert.ToDateTime("1970-01-01 00:00:00");
		/// <summary>
		/// auto_increment
		/// </summary>
		public int FileSysID
		{
			set{ _filesysid=value;}
			get{return _filesysid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int FileType
		{
			set{ _filetype=value;}
			get{return _filetype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string FileName
		{
			set{ _filename=value;}
			get{return _filename;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string FilePath
		{
			set{ _filepath=value;}
			get{return _filepath;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string FileLabel
		{
			set{ _filelabel=value;}
			get{return _filelabel;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime CreatTime
		{
			set{ _creattime=value;}
			get{return _creattime;}
		}
		#endregion Model

	}
}

