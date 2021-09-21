using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Aplication.ClinicHistoryApp
{
    public class RoundRectangle : IPdfPCellEvent {
  public void CellLayout(
    PdfPCell cell, Rectangle rect, PdfContentByte[] canvas
  ) 
  {
    PdfContentByte cb = canvas[PdfPTable.LINECANVAS];
    cb.RoundRectangle(
      rect.Left,
      rect.Bottom,
      rect.Width,
      rect.Height,
      2 // change to adjust how "round" corner is displayed
    );
    cb.SetLineWidth(1f);
    cb.SetCmykColorStrokeF(0f, 0f, 0f, 1f);
    cb.Stroke();
  }
}
}