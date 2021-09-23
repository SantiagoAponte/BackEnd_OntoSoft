using System;
using System.IO;
using System.Linq;
using System.Text;
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
                Font fontTitle = new Font(Font.HELVETICA, 20f, Font.BOLD, BaseColor.Black);
                Font fontHeader = new Font(Font.HELVETICA, 7f, Font.BOLD, BaseColor.Black);
                Font fontData = new Font(Font.HELVETICA, 7f, Font.NORMAL, BaseColor.DarkGray);
                
                
              var clinicHistory = await _context.clinicHistories.Where(a => a.UserId == request.Id)
                .Include(x=>x.user)
                .ThenInclude(x=>x.typeDocument)
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

                Console.WriteLine(clinicHistory);
                //CONFIGURACIÓN DEL PDF
                MemoryStream workStream = new MemoryStream();
                Rectangle rect = new Rectangle(PageSize.A4);
                RoundRectangle roundRectangle = new RoundRectangle(); 
                Document document = new Document(rect, 50, 50, 50, 100);
                PdfWriter writer = PdfWriter.GetInstance(document, workStream);
                writer.CloseStream = false;
                // writer.SetEncryption(PdfWriter.STRENGTH40BITS, user.document, user.fullName, PdfWriter.ALLOW_COPY); //Linea para agregarle contraseña al documento    

                var User = _context.User.FirstOrDefault(x=>x.Id == request.Id);
                 
                 
                int age = DateTime.Today.AddTicks(-clinicHistory[0].user.dateBirth.Ticks).Year - 1;
                double ageperMonth = DateTime.Now.Subtract(clinicHistory[0].user.dateBirth).Days / (365 / 12);
                string message = "";

                if(age > ageperMonth){
                    message = (age + " años");
                    
                }
                else{
                     message= (ageperMonth + " meses"); 
                }

                //AQUI ES DONDE INICIA LA CONSTRUCCION DE LOS ESTILOS DEL PDF.
                document.Open();
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance("D:\\BackEnd_OntoSoft\\Aplication\\images\\logo.png");
                image.BorderWidth = 0;
                image.Alignment = Element.ALIGN_LEFT;
                float percentage = 0.0f;
                percentage = 150 / image.Width;
                image.ScalePercent(percentage * 100);

                document.Add(image);
                document.AddTitle("Historia Clinica_" + User.fullName + "_OntoSoft");
                document.AddSubject("Esta es la historia clinica generada por el software odontologico OntoSoft de manera automatizada");
                document.AddKeywords("OntoSoft, Odontologia, Dientes, Oral");
                document.AddCreator("OntoSoft");
                document.AddAuthor("Santiago Aponte, Fernando Martinez");
                document.AddHeader("OntoSoft", "No Header");

                Paragraph cellTitle = new Paragraph("HISTORIA CLINICA", fontTitle);
                cellTitle.Alignment = Element.ALIGN_CENTER;
                cellTitle.SpacingAfter = 10f;
                document.Add(cellTitle);

                /*CASILLA PARA DEFINIR LA INFO GENERAL */
                PdfPTable infoGeneral = new PdfPTable(1);
                infoGeneral.WidthPercentage = 90f;
                PdfPCell cellinfoGeneral = new PdfPCell(new Phrase("1. INFORMACIÓN GENERAL", fontHeader )) ;
                cellinfoGeneral.Border = Rectangle.NO_BORDER;
                infoGeneral.AddCell(cellinfoGeneral);
                document.Add(infoGeneral);

                /*PRIMERA COLUMNA (NUMERO DE HISTORIA CLINICA)*/
                
                PdfPTable tablenumHc = new PdfPTable(1);
                float[] widths = new float[] {60f};
                tablenumHc.SetWidthPercentage(widths, rect);

                foreach (var atribute in clinicHistory){

                PdfPCell cellHeaderHCnum = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(
                    "Historia Clinica No.                                                                                                                         " //espacio necesario para situar el texto (no permitio por estilos)
                    + atribute.Id.ToString(), fontHeader)};

                tablenumHc.AddCell(cellHeaderHCnum);
                tablenumHc.WidthPercentage = 90;
                }
                document.Add(tablenumHc); //llamado para añadir la mini tabla.

                 PdfPTable tabledateCreate = new PdfPTable(1);
                float[] widths2 = new float[] {60f};
                tabledateCreate.SetWidthPercentage(widths2, rect);

                foreach (var atribute in clinicHistory){

                PdfPCell cellDateCreate = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("Fecha de Elaboración                                                                                                                                                " //espacio necesario para situar el texto (no permitio por estilos)
                + atribute.dateRegister.ToShortDateString(),fontHeader )};

                tabledateCreate.AddCell(cellDateCreate);
                tabledateCreate.SpacingAfter = 10f;
                tabledateCreate.WidthPercentage = 90;
                }
                document.Add(tabledateCreate);
                
                 /*CASILLA PARA DEFINIR LA INFO GENERAL DEL PACIENTE */
                PdfPTable infoGeneralUser = new PdfPTable(1);
                infoGeneralUser.WidthPercentage = 90f;
                PdfPCell cellinfoGeneralUser = new PdfPCell(new Phrase("2. INFORMACIÓN GENERAL DEL PACIENTE", fontHeader )) ;
                cellinfoGeneralUser.Border = Rectangle.NO_BORDER;
                infoGeneralUser.AddCell(cellinfoGeneralUser);
                document.Add(infoGeneralUser);

                /*SEGUNDA COLUMNA (INFORMACIÓN PACIENTE GENERAL)*/
                
               PdfPTable tableInfogeneric = new PdfPTable(3);
                float[] widths3 = new float[] { 40f, 60f, 40f};
                tableInfogeneric.SetWidthPercentage(widths3, rect);

                PdfPCell cellheaderFullname = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("Nombre Completo" , fontHeader )};
                tableInfogeneric.AddCell(cellheaderFullname);

                PdfPCell cellheadertypeDocument = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("Tipo de Documento" , fontHeader )};
                tableInfogeneric.AddCell(cellheadertypeDocument);

                PdfPCell cellheaderNoDocument = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("Numero de Documento" , fontHeader )};
                tableInfogeneric.AddCell(cellheaderNoDocument);

                tableInfogeneric.WidthPercentage = 90;

                foreach (var atribute in clinicHistory)
                {
                PdfPCell cellFullname = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(atribute.user.fullName , fontData )};
                tableInfogeneric.AddCell(cellFullname);

                PdfPCell celltypeDocument = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(atribute.user.typeDocument.description , fontData )};
                tableInfogeneric.AddCell(celltypeDocument);

                PdfPCell cellNoDocument = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(atribute.user.document , fontData )};
                tableInfogeneric.AddCell(cellNoDocument);
                }

                document.Add(tableInfogeneric);

                /* SECCION DONDE INGRESA SU INFO COMO TIPO DE SANGRE, PESO,  ETC*/
                 PdfPTable tableInfo = new PdfPTable(6);
                float[] widths4 = new float[] { 40f, 60f, 40f, 40f, 40f,40f};
                tableInfo.SetWidthPercentage(widths4, rect);

                PdfPCell cellheaderGender = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("Sexo" , fontHeader )};
                tableInfo.AddCell(cellheaderGender);

                PdfPCell cellheaderAge = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("Edad" , fontHeader )};
                tableInfo.AddCell(cellheaderAge);

                PdfPCell cellheaderDateBirth = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("Fecha de Nacimiento" , fontHeader )};
                tableInfo.AddCell(cellheaderDateBirth);

                PdfPCell cellheadertypeBlood = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("Tipo de Sangre" , fontHeader )};
                tableInfo.AddCell(cellheadertypeBlood);

                PdfPCell cellheaderHeigth = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("Altura" , fontHeader )};
                tableInfo.AddCell(cellheaderHeigth);

                PdfPCell cellheaderWeidth = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("Peso", fontHeader )};
                tableInfo.AddCell(cellheaderWeidth);

            
                tableInfo.WidthPercentage = 90;

                foreach (var atribute in clinicHistory)
                {

                PdfPCell cellGender = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(atribute.user.gender , fontData )};
                tableInfo.AddCell(cellGender);

                PdfPCell cellAge = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(message, fontData )};
                tableInfo.AddCell(cellAge);

                PdfPCell celldateBirth = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(atribute.user.dateBirth.ToShortDateString().ToString() , fontData )};
                tableInfo.AddCell(celldateBirth);

                PdfPCell cellBloodTypeAndRh = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("GS: " + atribute.user.bloodType + "  " +"RH: " + atribute.user.rh , fontData )};
                tableInfo.AddCell(cellBloodTypeAndRh);

                PdfPCell cellHeigth = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(atribute.user.height , fontData )};
                tableInfo.AddCell(cellHeigth);

                PdfPCell cellWeigth = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(atribute.user.weight , fontData )};
                tableInfo.AddCell(cellWeigth);
                }
                document.Add(tableInfo);

                /*Tabla que incluye datos como direccion propios y de contacto de emergencia*/
                 PdfPTable tableInfoSummary = new PdfPTable(7);
                float[] widths5 = new float[] { 40f, 60f, 40f, 40f, 40f, 40f, 40f};
                tableInfoSummary.SetWidthPercentage(widths5, rect);

                PdfPCell cellheaderAddress = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("Dirección y Ciudad de Residencia" , fontHeader )};
                tableInfoSummary.AddCell(cellheaderAddress);

                PdfPCell cellheaderPhone = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("Teléfono" , fontHeader )};
                tableInfoSummary.AddCell(cellheaderPhone);

                PdfPCell cellheaderNameCompanion = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("Nombre de Acompañante" , fontHeader )};
                tableInfoSummary.AddCell(cellheaderNameCompanion);

                PdfPCell cellheaderPhoneCompanion = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("Teléfono Acompañante" , fontHeader )};
                tableInfoSummary.AddCell(cellheaderPhoneCompanion);

                PdfPCell cellheaderAddressEmergency = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("Dirección del acompañante" , fontHeader )};
                tableInfoSummary.AddCell(cellheaderAddressEmergency);

                PdfPCell cellheaderEps = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("EPS" , fontHeader )};
                tableInfoSummary.AddCell(cellheaderEps);

                PdfPCell cellheaderCenterEmergency = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("Nombre de Centro Asistencial" , fontHeader )};
                tableInfoSummary.AddCell(cellheaderCenterEmergency);

            
                tableInfoSummary.WidthPercentage = 90;

                foreach (var atribute in clinicHistory)
                {

                PdfPCell cellAddress = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("Dirección: " + atribute.user.address + "  " +"Ciudad: " + atribute.user.city , fontData )};
                tableInfoSummary.AddCell(cellAddress);

                PdfPCell cellPhone = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(atribute.user.PhoneNumber , fontData )};
                tableInfoSummary.AddCell(cellPhone);

                PdfPCell cellnameCompanion = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase( atribute.nameCompanion , fontData )};
                tableInfoSummary.AddCell(cellnameCompanion);

                PdfPCell cellPhoneCompanion = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(atribute.phoneCompanion, fontData )};
                tableInfoSummary.AddCell(cellPhoneCompanion);

                PdfPCell cellAddressEmergency = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(atribute.user.addresContact , fontData )};
                tableInfoSummary.AddCell(cellAddressEmergency);

                PdfPCell cellEps = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(atribute.user.eps , fontData )};
                tableInfoSummary.AddCell(cellEps);

                PdfPCell cellCenterEmergency = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(atribute.user.centerEmergency , fontData )};
                tableInfoSummary.AddCell(cellCenterEmergency);
                }
                tableInfoSummary.SpacingAfter = 10f;
                document.Add(tableInfoSummary);

                /*CASILLA PARA DEFINIR LA CASILLA DE ANTECEDENTES MEDICOS */
                PdfPTable infoAntecedentMedical = new PdfPTable(1);
                infoAntecedentMedical.WidthPercentage = 90f;

                PdfPCell cellInfoAntecedentMedical = new PdfPCell(new Phrase("3. ANTECEDENTES MEDICOS", fontHeader )) ;
                cellInfoAntecedentMedical.Border = Rectangle.NO_BORDER;
                infoAntecedentMedical.AddCell(cellInfoAntecedentMedical);
                document.Add(infoAntecedentMedical);

                PdfPTable tableAntecedentMedical = new PdfPTable(3);
                // tableAntecedentMedical.HorizontalAlignment = 1;
                tableAntecedentMedical.TotalWidth = 50f;
                PdfPCell cellTypeAntecedent = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("Tipo de Antecedente" , fontHeader )};

                PdfPCell cellBackgroundObtainTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("Si" , fontHeader )};

                 PdfPCell cellBackgroundObtainFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("No" , fontHeader )};
                tableAntecedentMedical.AddCell(cellTypeAntecedent);
                tableAntecedentMedical.AddCell(cellBackgroundObtainTrue);
                tableAntecedentMedical.AddCell(cellBackgroundObtainFalse);

                PdfPTable tableBackgrounds = new PdfPTable(3);
                tableBackgrounds.HorizontalAlignment = 1;
                tableBackgrounds.TotalWidth = 175f;

                foreach (var atribute in clinicHistory)
                {
                var backgroundObtainTrue = string.Format("{0:x;0; }", atribute.backgroundMedical.GetHashCode());
                var backgroundObtainFalse = string.Format("{0: ;0;x}", atribute.backgroundMedical.GetHashCode());

                PdfPCell cellTypeBackGround = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase( "hola", fontHeader )};
                tableBackgrounds.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundObtainTrue.ToString() , fontHeader )};
                tableBackgrounds.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundObtainFalse.ToString() , fontHeader )};
                tableBackgrounds.AddCell(cellBackgroundFalse);
                }

                 PdfPTable tableBackgrounds2 = new PdfPTable(3);
                tableBackgrounds2.HorizontalAlignment = 1;
                tableBackgrounds2.TotalWidth = 175f;

                foreach (var atribute in clinicHistory)
                {
                var backgroundObtainTrue = string.Format("{0:x;0; }", atribute.backgroundMedical.GetHashCode());
                var backgroundObtainFalse = string.Format("{0: ;0;x}", atribute.backgroundMedical.GetHashCode());

                PdfPCell cellTypeBackGround = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase( "hola", fontHeader )};
                tableBackgrounds2.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundObtainTrue.ToString() , fontHeader )};
                tableBackgrounds2.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundObtainFalse.ToString() , fontHeader )};
                tableBackgrounds2.AddCell(cellBackgroundFalse);
                }
                PdfPTable tableBackgrounds3 = new PdfPTable(3);
                tableBackgrounds2.HorizontalAlignment = 1;
                tableBackgrounds2.TotalWidth = 175f;

                foreach (var atribute in clinicHistory)
                {
                var backgroundObtainTrue = string.Format("{0:x;0; }", atribute.backgroundMedical.GetHashCode());
                var backgroundObtainFalse = string.Format("{0: ;0;x}", atribute.backgroundMedical.GetHashCode());

                PdfPCell cellTypeBackGround = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase( "hola", fontHeader )};
                tableBackgrounds3.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundObtainTrue.ToString() , fontHeader )};
                tableBackgrounds3.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundObtainFalse.ToString() , fontHeader )};
                tableBackgrounds3.AddCell(cellBackgroundFalse);
                }
                PdfPTable tableBackgrounds4 = new PdfPTable(3);
                tableBackgrounds4.HorizontalAlignment = 1;
                tableBackgrounds4.TotalWidth = 175f;

                foreach (var atribute in clinicHistory)
                {
                var backgroundObtainTrue = string.Format("{0:x;0; }", atribute.backgroundMedical.GetHashCode());
                var backgroundObtainFalse = string.Format("{0: ;0;x}", atribute.backgroundMedical.GetHashCode());

                PdfPCell cellTypeBackGround = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase( "hola", fontHeader )};
                tableBackgrounds4.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundObtainTrue.ToString() , fontHeader )};
                tableBackgrounds4.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundObtainFalse.ToString() , fontHeader )};
                tableBackgrounds4.AddCell(cellBackgroundFalse);
                }
                PdfPTable tableBackgrounds5 = new PdfPTable(3);
                tableBackgrounds5.HorizontalAlignment = 1;
                tableBackgrounds5.TotalWidth = 175f;

                foreach (var atribute in clinicHistory)
                {
                var backgroundObtainTrue = string.Format("{0:x;0; }", atribute.backgroundMedical.GetHashCode());
                var backgroundObtainFalse = string.Format("{0: ;0;x}", atribute.backgroundMedical.GetHashCode());

                PdfPCell cellTypeBackGround = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase( "hola", fontHeader )};
                tableBackgrounds5.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundObtainTrue.ToString() , fontHeader )};
                tableBackgrounds5.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundObtainFalse.ToString() , fontHeader )};
                tableBackgrounds5.AddCell(cellBackgroundFalse);
                }

                PdfPTable tableBackgrounds6 = new PdfPTable(3);
                tableBackgrounds5.HorizontalAlignment = 1;
                tableBackgrounds5.TotalWidth = 175f;

                foreach (var atribute in clinicHistory)
                {
                var backgroundObtainTrue = string.Format("{0:x;0; }", atribute.backgroundMedical.GetHashCode());
                var backgroundObtainFalse = string.Format("{0: ;0;x}", atribute.backgroundMedical.GetHashCode());

                PdfPCell cellTypeBackGround = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase( "hola", fontHeader )};
                tableBackgrounds6.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundObtainTrue.ToString() , fontHeader )};
                tableBackgrounds6.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundObtainFalse.ToString() , fontHeader )};
                tableBackgrounds6.AddCell(cellBackgroundFalse);
                }
                PdfPTable tableBackgrounds7 = new PdfPTable(3);
                tableBackgrounds7.HorizontalAlignment = 1;
                tableBackgrounds7.TotalWidth = 175f;

                foreach (var atribute in clinicHistory)
                {
                var backgroundObtainTrue = string.Format("{0:x;0; }", atribute.backgroundMedical.GetHashCode());
                var backgroundObtainFalse = string.Format("{0: ;0;x}", atribute.backgroundMedical.GetHashCode());

                PdfPCell cellTypeBackGround = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase( "hola", fontHeader )};
                tableBackgrounds7.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundObtainTrue.ToString() , fontHeader )};
                tableBackgrounds7.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundObtainFalse.ToString() , fontHeader )};
                tableBackgrounds7.AddCell(cellBackgroundFalse);
                }
                PdfPTable tableBackgrounds8 = new PdfPTable(3);
                tableBackgrounds8.HorizontalAlignment = 1;
                tableBackgrounds8.TotalWidth = 175f;

                foreach (var atribute in clinicHistory)
                {
                var backgroundObtainTrue = string.Format("{0:x;0; }", atribute.backgroundMedical.GetHashCode());
                var backgroundObtainFalse = string.Format("{0: ;0;x}", atribute.backgroundMedical.GetHashCode());

                PdfPCell cellTypeBackGround = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase( "hola", fontHeader )};
                tableBackgrounds8.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundObtainTrue.ToString() , fontHeader )};
                tableBackgrounds8.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundObtainFalse.ToString() , fontHeader )};
                tableBackgrounds8.AddCell(cellBackgroundFalse);
                }
                PdfPTable tableBackgrounds9 = new PdfPTable(3);
                tableBackgrounds9.HorizontalAlignment = 1;
                tableBackgrounds9.TotalWidth = 175f;

                foreach (var atribute in clinicHistory)
                {
                var backgroundObtainTrue = string.Format("{0:x;0; }", atribute.backgroundMedical.GetHashCode());
                var backgroundObtainFalse = string.Format("{0: ;0;x}", atribute.backgroundMedical.GetHashCode());

                PdfPCell cellTypeBackGround = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase( "hola", fontHeader )};
                tableBackgrounds9.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundObtainTrue.ToString() , fontHeader )};
                tableBackgrounds9.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundObtainFalse.ToString() , fontHeader )};
                tableBackgrounds9.AddCell(cellBackgroundFalse);
                }
                PdfPTable tableBackgrounds10 = new PdfPTable(3);
                tableBackgrounds10.HorizontalAlignment = 1;
                tableBackgrounds10.TotalWidth = 175f;

                foreach (var atribute in clinicHistory)
                {
                var backgroundObtainTrue = string.Format("{0:x;0; }", atribute.backgroundMedical.GetHashCode());
                var backgroundObtainFalse = string.Format("{0: ;0;x}", atribute.backgroundMedical.GetHashCode());

                PdfPCell cellTypeBackGround = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase( "hola", fontHeader )};
                tableBackgrounds10.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundObtainTrue.ToString() , fontHeader )};
                tableBackgrounds10.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundObtainFalse.ToString() , fontHeader )};
                tableBackgrounds10.AddCell(cellBackgroundFalse);
                }
                PdfPTable tableBackgrounds11 = new PdfPTable(3);
                tableBackgrounds11.HorizontalAlignment = 1;
                tableBackgrounds11.TotalWidth = 175f;

                foreach (var atribute in clinicHistory)
                {
                var backgroundObtainTrue = string.Format("{0:x;0; }", atribute.backgroundMedical.GetHashCode());
                var backgroundObtainFalse = string.Format("{0: ;0;x}", atribute.backgroundMedical.GetHashCode());

                PdfPCell cellTypeBackGround = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase( "hola", fontHeader )};
                tableBackgrounds11.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundObtainTrue.ToString() , fontHeader )};
                tableBackgrounds11.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundObtainFalse.ToString() , fontHeader )};
                tableBackgrounds11.AddCell(cellBackgroundFalse);
                }
                PdfPTable tableBackgrounds12 = new PdfPTable(3);
                tableBackgrounds12.HorizontalAlignment = 1;
                tableBackgrounds12.TotalWidth = 175f;

                foreach (var atribute in clinicHistory)
                {
                var backgroundObtainTrue = string.Format("{0:x;0; }", atribute.backgroundMedical.GetHashCode());
                var backgroundObtainFalse = string.Format("{0: ;0;x}", atribute.backgroundMedical.GetHashCode());

                PdfPCell cellTypeBackGround = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase( "hola", fontHeader )};
                tableBackgrounds12.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundObtainTrue.ToString() , fontHeader )};
                tableBackgrounds12.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundObtainFalse.ToString() , fontHeader )};
                tableBackgrounds12.AddCell(cellBackgroundFalse);
                }
                PdfPTable tableBackgrounds13 = new PdfPTable(3);
                tableBackgrounds13.HorizontalAlignment = 1;
                tableBackgrounds13.TotalWidth = 175f;

                foreach (var atribute in clinicHistory)
                {
                var backgroundObtainTrue = string.Format("{0:x;0; }", atribute.backgroundMedical.GetHashCode());
                var backgroundObtainFalse = string.Format("{0: ;0;x}", atribute.backgroundMedical.GetHashCode());

                PdfPCell cellTypeBackGround = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase( "hola", fontHeader )};
                tableBackgrounds13.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundObtainTrue.ToString() , fontHeader )};
                tableBackgrounds13.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundObtainFalse.ToString() , fontHeader )};
                tableBackgrounds13.AddCell(cellBackgroundFalse);
                }
                 PdfPTable tableBackgrounds14 = new PdfPTable(3);
                tableBackgrounds14.HorizontalAlignment = 1;
                tableBackgrounds14.TotalWidth = 175f;

                foreach (var atribute in clinicHistory)
                {
                var backgroundObtainTrue = string.Format("{0:x;0; }", atribute.backgroundMedical.GetHashCode());
                var backgroundObtainFalse = string.Format("{0: ;0;x}", atribute.backgroundMedical.GetHashCode());

                PdfPCell cellTypeBackGround = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase( "hola", fontHeader )};
                tableBackgrounds14.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundObtainTrue.ToString() , fontHeader )};
                tableBackgrounds14.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundObtainFalse.ToString() , fontHeader )};
                tableBackgrounds14.AddCell(cellBackgroundFalse);
                }

                PdfPTable tableBackgrounds15 = new PdfPTable(3);
                tableBackgrounds15.HorizontalAlignment = 1;
                tableBackgrounds15.TotalWidth = 175f;

                foreach (var atribute in clinicHistory)
                {
                var backgroundObtainTrue = string.Format("{0:x;0; }", atribute.backgroundMedical.GetHashCode());
                var backgroundObtainFalse = string.Format("{0: ;0;x}", atribute.backgroundMedical.GetHashCode());

                PdfPCell cellTypeBackGround = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase( "hola", fontHeader )};
                tableBackgrounds15.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundObtainTrue.ToString() , fontHeader )};
                tableBackgrounds15.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundObtainFalse.ToString() , fontHeader )};
                tableBackgrounds15.AddCell(cellBackgroundFalse);
                }

                /*CONSTRUCCION DE LAS TABLAS PARA LOS ANTECEDENTES MEDICOS*/     
                document.Add(tableAntecedentMedical);
                document.Add(tableBackgrounds);
                document.Add(tableBackgrounds2);
                document.Add(tableBackgrounds3);
                document.Add(tableBackgrounds5);
                document.Add(tableBackgrounds6);
                document.Add(tableBackgrounds7);
                document.Add(tableBackgrounds8);
                document.Add(tableBackgrounds9);
                document.Add(tableBackgrounds10);
                document.Add(tableBackgrounds11);
                document.Add(tableBackgrounds12);
                document.Add(tableBackgrounds13);
                document.Add(tableBackgrounds14);
                document.Add(tableBackgrounds15);


                // AQUI ES DONDE SE ACABA EL DISEÑO DEL PDF, NO TOCAR
                document.Close();

                byte[] byteData = workStream.ToArray();
                workStream.Write(byteData, 0, byteData.Length);
                workStream.Position = 0;
                return workStream;
            }
        }
    }
}