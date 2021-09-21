using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domine;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using persistence;

namespace Aplication.ClinicHistoryApp
{
    public class ExportPdf
    {
        public class getClinicHistoryInPdf : IRequest<Stream>
        {
            public string Id { get; set; }

            public getClinicHistoryInPdf(string id)
            {
                this.Id = id;
            }
        }

        public class Manager : IRequestHandler<getClinicHistoryInPdf, Stream>
        {

            private readonly OntoSoftContext _context;
            private readonly UserManager<User> _userManager;
            public Manager(OntoSoftContext context, UserManager<User> userManager)
            {
                _context = context;
                _userManager = userManager;
            }
            public async Task<Stream> Handle(getClinicHistoryInPdf request, CancellationToken cancellationToken)
            {
                Font fuenteTitulo = new Font(Font.HELVETICA, 8f, Font.BOLD, BaseColor.Blue);
                Font fuenteHeader = new Font(Font.HELVETICA, 7f, Font.BOLD, BaseColor.Black);
                Font fuenteData = new Font(Font.HELVETICA, 7f, Font.NORMAL, BaseColor.Black);
              var clinicHistory = await _context.clinicHistories.Where(a => a.UserId == request.Id)
                .Include(x=>x.user)
                .Include(x=>x.patientEvolutionList)
                .Include(x=>x.oralRadiographyList)
                .Include(x=>x.treamentPlanList)
                .Include(x=>x.Odontogram)
                .ThenInclude(x=>x.toothLink)
                .ThenInclude(x=>x.Tooth)
                .ThenInclude(x=>x.typeProcessLink)
                .ThenInclude(x=>x.typeProcess)
                .Include(x=>x.BackgroundMedicalsLink)
                .ThenInclude(x=>x.BackgroundMedicals)
                .Include(x=>x.BackgroundOralsLink)
                .ThenInclude(x=>x.BackgroundOrals)
                .ToListAsync();

                MemoryStream workStream = new MemoryStream();
                Rectangle rect = new Rectangle(PageSize.A4);

                Document document = new Document(rect, 0, 0, 50, 100);
                PdfWriter writer = PdfWriter.GetInstance(document, workStream);
                writer.CloseStream = false;

                document.Open();
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance("D:\\BackEnd_OntoSoft\\Aplication\\images\\logo.png");
                image.BorderWidth = 0;
                image.Alignment = Element.ALIGN_LEFT;
                image.Alignment = Element.ALIGN_JUSTIFIED;
                float percentage = 0.0f;
                percentage = 150 / image.Width;
                image.ScalePercent(percentage * 100);

                 document.Add(image);
                document.AddTitle("Historia Clinica Generada por OntoSoft");

                PdfPTable tabla = new PdfPTable(1);
                tabla.WidthPercentage = 90;
                PdfPCell celda = new PdfPCell(new Phrase("Lista de Cursos de SQL Server", fuenteTitulo));
                celda.Border = Rectangle.NO_BORDER;
                tabla.AddCell(celda);
                document.Add(tabla);

                PdfPTable tablaCursos = new PdfPTable(2);
                float[] widths = new float[] { 40f, 60f };
                tablaCursos.SetWidthPercentage(widths, rect);

                PdfPCell celdaHeaderTitulo = new PdfPCell(new Phrase("Curso", fuenteHeader));
                tablaCursos.AddCell(celdaHeaderTitulo);

                PdfPCell celdaHeaderDescripcion = new PdfPCell(new Phrase("Descripcion", fuenteHeader));
                tablaCursos.AddCell(celdaHeaderDescripcion);

                tablaCursos.WidthPercentage = 90;



                foreach (var cursoElemento in clinicHistory)
                {
                    PdfPCell celdaDataTitulo = new PdfPCell(new Phrase(cursoElemento.nameCompanion, fuenteData));
                    tablaCursos.AddCell(celdaDataTitulo);

                    PdfPCell celdaDataDescripcion = new PdfPCell(new Phrase(cursoElemento.nameCompanion, fuenteData));
                    tablaCursos.AddCell(celdaDataDescripcion);
                }

                document.Add(tablaCursos);


                document.Close();

                byte[] byteData = workStream.ToArray();
                workStream.Write(byteData, 0, byteData.Length);
                workStream.Position = 0;

                return workStream;
            }
        }
    }
}