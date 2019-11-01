using System;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PCIBusiness
{
	public partial class PdfHeaderFooter : PdfPageEventHelper
	{
		public override void OnEndPage(PdfWriter writer, Document doc)
		{
			if ( writer.PageNumber > 1 )
				try
				{
					PdfPTable footTable = new PdfPTable(3);
					PdfPCell  cell;
//					Image     logo;
//					string    dir;

//					dir = AppDomain.CurrentDomain.BaseDirectory;
//					if ( ! dir.EndsWith("\\") ) dir = dir + "\\";
//					dir = dir + "Images\\";

					Rectangle page = doc.PageSize;
					footTable.TotalWidth = page.Width;

//	Put this in if you want a logo on the footer page
//					logo                     = Image.GetInstance(dir + "Prosperian-Small.jpg");
//					logo.Alignment           = Element.ALIGN_LEFT;
//					cell                     = new PdfPCell();
//					cell.Border              = PdfPCell.NO_BORDER;
//					cell.HorizontalAlignment = Element.ALIGN_LEFT;
//					cell.VerticalAlignment   = Element.ALIGN_BOTTOM;
//					cell.AddElement(new Chunk(logo,0,0));
//					footTable.AddCell(cell);

					cell                     = new PdfPCell();
					cell.Border              = PdfPCell.NO_BORDER;
					cell.HorizontalAlignment = Element.ALIGN_CENTER;
					cell.VerticalAlignment   = Element.ALIGN_CENTER;
					cell.Phrase              = new Phrase("Page " + (writer.PageNumber-1).ToString(),new Font(Font.FontFamily.HELVETICA,10));
					footTable.AddCell(cell);

					footTable.WriteSelectedRows(0, -1, doc.LeftMargin, footTable.TotalHeight+10, writer.DirectContent);
					cell      = null;
					footTable = null;
				}
				catch (Exception ex)
				{
					Tools.LogException("PdfHeaderFooter.OnEndPage","",ex);
				}
		}
	}
}