using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PCIBusiness
{
	public class PdfFile : BaseData
	{
		private Document  doc;
		private Paragraph para;
		private PdfWriter writer;
		private PdfPTable openTable;
		private BaseFont  fontBase;

		private int       k;
		private string    fileName;

//		private const iTextSharp.text.Font.FontFamily PDF_FONT = iTextSharp.text.Font.FontFamily.HELVETICA;
		private const int PDF_FONTSIZE_TABLECELL     = 10;
		private const int PDF_FONTSIZE_TABLEHEADING  = 12;
		private const int PDF_FONTSIZE_MAJORHEADING  = 32;
		private const int PDF_FONTSIZE_MINORHEADING  = 20;
		private const int PDF_FONTSIZE_SUBHEADING    = 16;
		private const int PDF_PARA_SPACING           = 10;
		private const int PDF_CELL_PADDING           =  5;

		public override void LoadData(DBConn dbConn)
		{ } // Just because it has to be here ...

		public string SavedFileNameAndFolder
		{
			get { return fileName; }
		}

		public int Create(string appName,string fileSource,string mainHeading="",string subHeading="")
		{
			try
			{
				StreamWriter fileOut = null;
				fileName = Tools.CreateFile(ref fileOut,fileSource,"pdf");
				if ( fileOut == null )
					return 20;
				fileOut.Close();
				fileOut = null;
				if ( fileName.Length < 1 )
					return 30;

				doc              = new Document();
				writer           = PdfWriter.GetInstance(doc,new FileStream(fileName,FileMode.OpenOrCreate));
				writer.PageEvent = new PdfHeaderFooter();

//	Set up unicode font ... in constructor
//				BaseFont fontBase = BaseFont.CreateFont("C:\\Dev\\Prosperian\\Application\\PCIWebFinAid\\CSS\\raleway-medium-webfont.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
//				Font     fontUse  = new Font(fontBase, 12, Font.NORMAL);

			//	Document attributes
				doc.Open();
				doc.AddCreator (appName + ", Version " + SystemDetails.AppVersion);
				doc.AddProducer();
				doc.AddKeywords("Developed by " + SystemDetails.Developer);
				doc.AddAuthor  (SystemDetails.Owner);
				doc.AddTitle   (mainHeading);
				doc.AddCreationDate();
				doc.SetMargins(25,25,25,50); // Allow for a footer at the bottom
//				doc.SetMargins(doc.LeftMargin,doc.RightMargin,doc.TopMargin,doc.BottomMargin+30); // Allow for footer

//				AddParagraph(theEvent.EventName,PDF_FONTSIZE_MAJORHEADING,2,Element.ALIGN_CENTER,"",iTextSharp.text.Font.BOLD);

				if ( mainHeading.Length < 1 && subHeading.Length < 1 )
					return 0;

				try
				{
					string folder = AppDomain.CurrentDomain.BaseDirectory;
					if ( ! folder.EndsWith("\\") )
						folder = folder + "\\";
					Image logo     = Image.GetInstance(folder+"Images\\FinAid.png");
					logo.Alignment = Element.ALIGN_LEFT;
				//	logo.ScalePercent(100);
				//	logo.ScaleAbsoluteWidth(173);
				//	logo.ScaleAbsoluteHeight(200);
					doc.Add(logo);
				}
				catch
				{ }

				AddParagraph(" ",PDF_FONTSIZE_SUBHEADING,2,Element.ALIGN_LEFT); // 2 blank lines
				AddParagraph(mainHeading,PDF_FONTSIZE_MAJORHEADING,2,Element.ALIGN_CENTER,"",iTextSharp.text.Font.BOLD);
				AddParagraph(subHeading,PDF_FONTSIZE_MINORHEADING ,5,Element.ALIGN_CENTER,"",iTextSharp.text.Font.BOLD);
				AddParagraph("(c) " + SystemDetails.Owner,PDF_FONTSIZE_SUBHEADING,2,Element.ALIGN_CENTER);
				AddParagraph(appName + ", Version " + SystemDetails.AppVersion + " (" + Tools.DateToString(System.DateTime.Now,2,1) + ")",PDF_FONTSIZE_TABLEHEADING,0,Element.ALIGN_LEFT);

				doc.NewPage();
			}
			catch (Exception ex)
			{
				Tools.LogException("PdfFile.Create",mainHeading,ex);
				return 90;
			}
			return 0;
		}

		public int AddParagraph ( string theText,
			                       int    fontSize,
			                       int    blankLines = 0,
			                       int    alignment  = 0,
			                       string foreColor  = "",
			                       int    style      = 0 )
		{
			theText = Tools.NullToString(theText);
			if ( theText.Length < 1 )
				return 0;

			try
			{
				theText   = theText.Replace("<br />  ",Environment.NewLine).Replace("<br /> ",Environment.NewLine).Replace("<br />",Environment.NewLine);
				foreColor = foreColor.Trim().ToUpper();
				if ( foreColor.Length == 0 )
					para = new Paragraph(theText,new iTextSharp.text.Font(fontBase,fontSize,style));
				else
				{
					BaseColor                            textColor = BaseColor.BLACK;
					if      ( foreColor == "WHITE"     ) textColor = BaseColor.WHITE;
					else if ( foreColor == "RED"       ) textColor = BaseColor.RED;
					else if ( foreColor == "GREEN"     ) textColor = BaseColor.GREEN;
					else if ( foreColor == "BLUE"      ) textColor = BaseColor.BLUE;
					else if ( foreColor == "YELLOW"    ) textColor = BaseColor.YELLOW;
					else if ( foreColor == "ORANGE"    ) textColor = BaseColor.ORANGE;
					else if ( foreColor == "PINK"      ) textColor = BaseColor.PINK;
					else if ( foreColor == "CYAN"      ) textColor = BaseColor.CYAN;
					else if ( foreColor == "DARKGRAY"  ) textColor = BaseColor.DARK_GRAY;
					else if ( foreColor == "GRAY"      ) textColor = BaseColor.GRAY;
					else if ( foreColor == "LIGHTGRAY" ) textColor = BaseColor.LIGHT_GRAY;
					else if ( foreColor == "MAGENTA"   ) textColor = BaseColor.MAGENTA;
					para = new Paragraph(theText,new iTextSharp.text.Font(fontBase,fontSize,style,textColor));
//					para = new Paragraph(theText,new iTextSharp.text.Font(PDF_FONT,fontSize,style,textColor));
				}

				if ( alignment > 0 )
					para.Alignment = alignment;

				doc.Add(para);
				if ( blankLines > 0 )
					for ( k = 1 ; k < blankLines ; k++ )
						doc.Add(new Chunk(Environment.NewLine));

				return 0;
			}
			catch (Exception ex)
			{
				Tools.LogException("PdfFile.AddParagraph","theText=" + theText,ex);
			}
			return 20;
		}

		public int TableOpen(byte columns)
		{
			try
			{
				if ( doc == null )
					return 10;
				if ( columns < 2 )
					return 20;
				openTable = new PdfPTable(columns);
				return 0;
			}
			catch (Exception ex)
			{
				Tools.LogException("PdfFile.TableOpen","",ex);
			}
			return 199;
		}

		public int TableClose()
		{
			try
			{
				if ( doc == null )
					return 10;
				if ( openTable == null )
					return 20;
				doc.Add(openTable);
				return 0;
			}
			catch (Exception ex)
			{
				Tools.LogException("PdfFile.TableClose","",ex);
			}
			finally
			{
				openTable = null;
			}
			return 199;
		}

//		Use "AddParagraph" rather
//		public int WriteLine(string data="",Constants.PdfFontSize font=Constants.PdfFontSize.SubHeading,string align="L",byte blankLines=1)
//		{
//			try
//			{
//				if ( doc == null )
//					return 10;
//				data     = Tools.NullToString(data).Replace("<br />",Environment.NewLine);
//	//			int font = ( rowMode == 1 ? PDF_FONTSIZE_SUBHEADING : PDF_FONTSIZE_TABLECELL );
//				if ( align == "R" )
//					AddParagraph(data.Trim(),(int)font,blankLines,Element.ALIGN_RIGHT);
//				else if ( align == "C" )
//					AddParagraph(data.Trim(),(int)font,blankLines,Element.ALIGN_CENTER);
//				else if ( align == "M" )
//					AddParagraph(data.Trim(),(int)font,blankLines,Element.ALIGN_MIDDLE);
//				else
//					AddParagraph(data.Trim(),(int)font,blankLines,Element.ALIGN_LEFT);
//				return 0;
//			}
//			catch (Exception ex)
//			{
//				Tools.LogException("PdfFile.WriteLine","",ex);
//			}
//			return 199;
//		}

		public int TableWriteLine(string data="",byte rowMode=1,byte blankLines=0)
		{
			try
			{
				if ( doc == null )
					return 10;
				if ( openTable == null )
					return 20;

				PdfPCell dtCell;
				data = Tools.NullToString(data).Replace("<br />",Environment.NewLine);

				if ( blankLines > 0 )
					for ( int k = 1 ; k <= blankLines ; k++ )
						data = data + Environment.NewLine;

				if ( data.Length == 0 )
					dtCell = new PdfPCell(new Phrase(" "));
				else if ( rowMode == 1 ) // Heading
					dtCell = new PdfPCell(new Phrase(data,new iTextSharp.text.Font(fontBase,PDF_FONTSIZE_TABLEHEADING,Font.BOLD,BaseColor.ORANGE)));
				else if ( rowMode == 3 ) // Underlined
					dtCell = new PdfPCell(new Phrase(data,new iTextSharp.text.Font(fontBase,PDF_FONTSIZE_TABLECELL,Font.UNDERLINE)));
				else
					dtCell = new PdfPCell(new Phrase(data,new iTextSharp.text.Font(fontBase,PDF_FONTSIZE_TABLECELL)));
				dtCell.Border  = 0;
				dtCell.Colspan = openTable.NumberOfColumns;
				openTable.AddCell(dtCell);
//				openTable.CompleteRow();
				return 0;
			}
			catch (Exception ex)
			{
				Tools.LogException("PdfFile.TableWriteLine","",ex);
			}
			return 199;
		}

		public int TableWriteRow(string[] rowData,byte rowMode=2)
		{
			try
			{
				if ( doc == null )
					return 10;
				if ( openTable == null )
					return 20;
				PdfPCell dtCell;
				string   data;
				for ( int k = 0 ; k < rowData.Length ; k++ )
				{
					data = rowData[k].Replace("<br />",Environment.NewLine).Trim();
					if ( rowMode == 1 ) // Heading
						dtCell = new PdfPCell(new Phrase(data,new iTextSharp.text.Font(fontBase,PDF_FONTSIZE_TABLEHEADING,Font.BOLD)));
					else
						dtCell = new PdfPCell(new Phrase(data,new iTextSharp.text.Font(fontBase,PDF_FONTSIZE_TABLECELL)));
					dtCell.Border = 0;
					openTable.AddCell(dtCell);
//					openTable.CompleteRow();
				}
				return 0;
			}
			catch (Exception ex)
			{
				Tools.LogException("PdfFile.TableWriteRow","",ex);
			}
			return 199;
		}


		public int WriteTable(string sqlRun,string tableOptions)
		{
			PdfPTable dtTable;
			PdfPCell  dtCell;
			string    colValue;
			string    colAttr;
			float[]   colWidths;
			float     colTotal  = 0;
			int       rows      = 0;
			int       extraCols = 0;
			int       p;

			try
			{
				if ( doc == null )
					return 10;
				if ( sqlRun.Length < 7 )
					return 20;
				if ( ! sqlRun.ToUpper().StartsWith("SELECT ") && ! sqlRun.ToUpper().StartsWith("EXEC ") )
					return 30;

				tableOptions = "," + tableOptions.Trim().ToUpper() + ",";
				sql          = sqlRun;

				if ( tableOptions.Contains(",POSITION,") )
					extraCols = 1;

				ExecuteSQL();

				dtTable                 = new PdfPTable(dbConn.ColumnCount+extraCols);
				colWidths               = new float[dbConn.ColumnCount+extraCols];
				dtTable.WidthPercentage = 100;
//				dtTable.SpacingBefore   = 15
//				dtTable.SpacingAfter    = 15

//	Column headings
				for ( k = 0 ; k < dbConn.ColumnCount+extraCols ; k++ )
				{
					if ( k-extraCols < 0 )
						colValue = "Position";
					else
						colValue = dbConn.ColName(k-extraCols);
					colAttr = "";
					p       = colValue.LastIndexOf("]");
					if ( p > 0 )
					{
						colAttr  = colValue.Substring(0,p+1).ToUpper();
						colValue = colValue.Substring(p+1).Trim();
					}
					colWidths[k] = colValue.Length;
		
					dtCell = new PdfPCell(new Phrase(colValue,new iTextSharp.text.Font(fontBase,PDF_FONTSIZE_TABLEHEADING,Font.BOLD)));

					if ( colAttr.Contains("[ALIGN:R]") )
						dtCell.HorizontalAlignment = Element.ALIGN_RIGHT;
					else if ( colAttr.Contains("[ALIGN:M]") )
						dtCell.HorizontalAlignment = Element.ALIGN_MIDDLE;
					else
						dtCell.HorizontalAlignment = Element.ALIGN_LEFT;

					if ( colAttr.Contains("[WRAP:Y]") )
						dtCell.NoWrap         = false;
					else
						dtCell.NoWrap         = true;

					dtCell.VerticalAlignment = Element.ALIGN_MIDDLE;
					dtCell.Border            = 0;
					dtTable.AddCell(dtCell);
					dtTable.HeaderRows = 1;
				}

//	Actual data	
				while ( !dbConn.EOF )
				{
					rows++;
					for ( k = 0 ; k < dbConn.ColumnCount+extraCols ; k++ )
					{
						if ( k-extraCols < 0 )
							colValue  = rows.ToString();
						else
							colValue  = dbConn.ColValue(k-extraCols);
						dtCell        = new PdfPCell(new Phrase(colValue,new iTextSharp.text.Font(fontBase,PDF_FONTSIZE_TABLECELL)));
						dtCell.Border = 0;
						dtTable.AddCell(dtCell);
						if ( colWidths[k] < colValue.Length )
							colWidths[k]  = colValue.Length;
					}
					dbConn.NextRow();
				}

//	Column width calculation
//	First, total of all widths
				for ( k = 0 ; k < dbConn.ColumnCount+extraCols ; k++ )
				{
					colWidths[k] = colWidths[k] + 1;
					colTotal     = colWidths[k] + colTotal;
				}
//	Now a percentage
				for ( k = 0 ; k < dbConn.ColumnCount+extraCols ; k++ )
					colWidths[k] = colWidths[k] * (float)100.00 / colTotal;

				dtTable.SetWidths(colWidths);
				doc.Add(dtTable);
			}
			catch (Exception ex)
			{
				Tools.LogException("PdfFile.WriteTable",sql,ex);
				return 90;
			}
			finally
			{
				Tools.CloseDB(ref dbConn);
				dtCell  = null;
				dtTable = null;
			}
			return 0;
		}

		public override void CleanUp()
		{
			try
			{
				openTable = null;
				para.Clear();
			}
			catch
			{ }
			try
			{
				writer.Close();
			}
			catch
			{ }
			try
			{
				doc.Close();
			}
			catch
			{ }
			para     = null;
			writer   = null;
			doc      = null;
			fontBase = null;

			Tools.DeleteFiles("*.pdf",0,0,15);
		}

		public PdfFile()
		{
//	Set up unicode font
			fileName = Tools.SystemFolder("CSS")+"raleway-medium-webfont.ttf";
			try
			{
				fontBase = BaseFont.CreateFont(fileName, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
			}
			catch (Exception ex)
			{
				fontBase = null;
				Tools.LogException("PdfFile.Constructor","Font file="+fileName,ex);
				Tools.LogInfo     ("PdfFile.Constructor","Failed to load font " + fileName,244);
			}
			finally
			{
				fileName = "";
			}
		}
	}
}
