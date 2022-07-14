using System;
using System.IO;
using System.Web;

namespace PCIWebFinAid
{
	public class DownLoad : IHttpHandler
	{
		public void ProcessRequest(HttpContext context)
		{
			string folder   = "";
			string fileName = "";
			string saveAs   = "";
			byte   err      = 10;
			int    k;

			try
			{
				fileName = PCIBusiness.Tools.NullToString(context.Request["File"]);
				fileName = fileName.Replace("/","\\");

				err      = 20;
				saveAs   = PCIBusiness.Tools.NullToString(context.Request["SaveAs"]);
				saveAs   = saveAs.Replace(" ","");
				saveAs   = saveAs.Replace(",","");
				saveAs   = saveAs.Replace("/","");
				saveAs   = saveAs.Replace("\\","");
				saveAs   = saveAs.Replace(":","");

				err      = 30;
				k        = fileName.LastIndexOf("\\");

				if ( k > 0 )
				{
					err      = 40;
					folder   = fileName.Substring(0,k);
					fileName = fileName.Substring(k+1);
					folder   = PCIBusiness.Tools.FixFolderName(folder);
				}
				else if ( k == 0 )
					fileName = fileName.Substring(k+1);

				if ( saveAs.Length == 0 )
					saveAs = fileName;

				if ( ! File.Exists(folder + fileName) )
					folder = PCIBusiness.Tools.FixFolderName(context.Request.PhysicalApplicationPath+folder);

				if ( File.Exists(folder + fileName) )
				{
					err = 80;
					if ( fileName.ToUpper().EndsWith(".PDF") )
						context.Response.ContentType = "application/pdf";
					else if ( fileName.ToUpper().EndsWith(".ZIP") )
						context.Response.ContentType = "application/zip";
					else
						context.Response.ContentType = "text/csv";
					err                             = 90;
					context.Response.BufferOutput   = true;
					err                             = 92;
					context.Response.Clear();
					context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + saveAs);
					err                             = 94;
					context.Response.TransmitFile(folder + fileName);
					try
					{
						context.Response.Flush(); // No problem if this fails
					}
					catch
					{ }
					return;
				}
				PCIBusiness.Tools.LogException("DownLoad.ProcessRequest/1","File=" + folder + fileName);
			}
			catch (Exception ex)
			{
				PCIBusiness.Tools.LogException("DownLoad.ProcessRequest/2","Err=" + err.ToString() + ", File=" + folder + fileName,ex);
			}
			context.Response.Redirect("ErrorStd.aspx?ErrorType=41");
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}
