using System;
using System.Collections.Generic;
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
            public  bool canRead {get;set;}

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
                Font fontHeader = new Font(Font.HELVETICA, 9f, Font.BOLD, BaseColor.Black);
                Font fontData = new Font(Font.HELVETICA, 8f, Font.NORMAL, BaseColor.DarkGray);
                 Font fontTitleAntecedent = new Font(Font.HELVETICA, 9f, Font.BOLD, BaseColor.Black);
                Font fontDataAntecedent = new Font(Font.HELVETICA, 9f, Font.NORMAL, BaseColor.Black);
                
                
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
                
                //CONFIGURACIÓN DEL PDF
                MemoryStream workStream = new MemoryStream();
                Rectangle rect = new Rectangle(PageSize.A4);
                RoundRectangle roundRectangle = new RoundRectangle(); 
                Document document = new Document(rect, 50, 50, 50, 100);
                PdfWriter writer = PdfWriter.GetInstance(document, workStream);
                writer.CloseStream = false;
                // writer.SetEncryption(PdfWriter.STRENGTH40BITS, user.document, user.fullName, PdfWriter.ALLOW_COPY); //Linea para agregarle contraseña al documento    

                var User = _context.User.FirstOrDefault(x=>x.Id == request.Id);
                 
                 /*Logica para calcular la edad por años y meses*/
                int age = DateTime.Today.AddTicks(-clinicHistory[0].user.dateBirth.Ticks).Year - 1;
                double ageperMonth = DateTime.Now.Subtract(clinicHistory[0].user.dateBirth).Days / (365 / 12);
                string message = "";

                if(age > ageperMonth){
                    message = (age + " años");
                    
                }
                else{
                     message= (ageperMonth + " meses"); 
                }

                /*Logica para marcar los antecedentes medicos que tiene el paciente*/
                var idTableBackgroundMedicals = _context.backgroundMedicals.ToList();           

                /*Logica para marcar los antecedentes orales que tiene el paciente*/
                var idTableBackgroundOrals = _context.backgroundOrals.ToList();  
                /*Logica para marcar los tratamientos del paciente*/
                var treamentData = _context.treamentPlan.ToList();
                /*Logica para marcar las radiografias del paciente*/
                var radiographyData = _context.oralRadiography.ToList();
                /*Logica para marcar las evoluciones del paciente*/
                var patientEvolutionData = _context.patientEvolution.ToList();
                       
                //AQUI ES DONDE INICIA LA CONSTRUCCION DE LOS ESTILOS DEL PDF.
                document.Open();
                Image image = iTextSharp.text.Image.GetInstance("E:\\logo.png");
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
                infoGeneral.SpacingAfter = 3;
                infoGeneral.AddCell(cellinfoGeneral);
                document.Add(infoGeneral);

                /*PRIMERA COLUMNA (NUMERO DE HISTORIA CLINICA)*/
                
                PdfPTable tablenumHc = new PdfPTable(1);
                float[] widths = new float[] {60f};
                tablenumHc.SetWidthPercentage(widths, rect);

                foreach (var atribute in clinicHistory){

                PdfPCell cellHeaderHCnum = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(
                    "Historia Clinica No.                                                                       " //espacio necesario para situar el texto (no permitio por estilos)
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
                Phrase = new Phrase("Fecha de Elaboración                                                                                                       " //espacio necesario para situar el texto (no permitio por estilos)
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
                infoGeneralUser.SpacingAfter = 3;
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
                float[] widths4 = new float[] { 60f, 35f, 60f, 60f, 60f, 40f};
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
                float[] widths5 = new float[] { 60f, 45f, 60f, 60f, 60f, 40f, 60f};
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

                PdfPCell cellInfoAntecedentMedical = new PdfPCell(new Phrase("3. ANTECEDENTES MEDICOS Y ODONTOLOGICOS GENERALES", fontHeader )) ;
                cellInfoAntecedentMedical.Border = Rectangle.NO_BORDER;
                infoAntecedentMedical.AddCell(cellInfoAntecedentMedical);
                infoAntecedentMedical.SpacingAfter = 5;
                document.Add(infoAntecedentMedical);

                /*Tabla que contiene las dos tablas de antecedentes, medicos y orales*/
                PdfPTable mtable = new PdfPTable(2);
                mtable.WidthPercentage = 90;
                mtable.SpacingAfter = -3;
                mtable.DefaultCell.Border = Rectangle.NO_BORDER;

                PdfPTable m2table = new PdfPTable(2);
                m2table.WidthPercentage = 90;
                m2table.SpacingAfter = -3;
                m2table.DefaultCell.Border = Rectangle.NO_BORDER;

                PdfPTable m3table = new PdfPTable(2);
                m3table.WidthPercentage = 90;
                m3table.SpacingAfter = -3;
                m3table.DefaultCell.Border = Rectangle.NO_BORDER;

                PdfPTable m4table = new PdfPTable(2);
                m4table.WidthPercentage = 90;
                m4table.SpacingAfter = -3;
                m4table.DefaultCell.Border = Rectangle.NO_BORDER;

                PdfPTable m5table = new PdfPTable(2);
                m5table.WidthPercentage = 90;
                m5table.SpacingAfter = -3;
                m5table.DefaultCell.Border = Rectangle.NO_BORDER;

                PdfPTable m6table = new PdfPTable(2);
                m6table.WidthPercentage = 90;
                m6table.SpacingAfter = -3;
                m6table.DefaultCell.Border = Rectangle.NO_BORDER;

                PdfPTable m7table = new PdfPTable(2);
                m7table.WidthPercentage = 90;
                m7table.SpacingAfter = -3;
                m7table.DefaultCell.Border = Rectangle.NO_BORDER;

                PdfPTable m8table = new PdfPTable(2);
                m8table.WidthPercentage = 90;
                m8table.SpacingAfter = -3;
                m8table.DefaultCell.Border = Rectangle.NO_BORDER;

                PdfPTable m9table = new PdfPTable(2);
                m9table.WidthPercentage = 90;
                m9table.SpacingAfter = -3;
                m9table.DefaultCell.Border = Rectangle.NO_BORDER;

                PdfPTable m10table = new PdfPTable(2);
                m10table.WidthPercentage = 90;
                m10table.SpacingAfter = -3;
                m10table.DefaultCell.Border = Rectangle.NO_BORDER;

                PdfPTable m11table = new PdfPTable(2);
                m11table.WidthPercentage = 90;
                m11table.SpacingAfter = -3;
                m11table.DefaultCell.Border = Rectangle.NO_BORDER;

                PdfPTable m12table = new PdfPTable(2);
                m12table.WidthPercentage = 90;
                m12table.SpacingAfter = -3;
                m12table.DefaultCell.Border = Rectangle.NO_BORDER;

                PdfPTable m13table = new PdfPTable(2);
                m13table.WidthPercentage = 90;
                m13table.SpacingAfter = -3;
                m13table.DefaultCell.Border = Rectangle.NO_BORDER;

                PdfPTable m14table = new PdfPTable(2);
                m14table.WidthPercentage = 90;
                m14table.SpacingAfter = -3;
                m14table.DefaultCell.Border = Rectangle.NO_BORDER;

                PdfPTable m15table = new PdfPTable(2);
                m15table.WidthPercentage = 90;
                m15table.SpacingAfter = -3;
                m15table.DefaultCell.Border = Rectangle.NO_BORDER;

                PdfPTable tableAntecedentMedical = new PdfPTable(3);
                PdfPCell cellTypeAntecedent = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("Nombre Antecedente" , fontTitleAntecedent )};

                PdfPCell cellBackgroundObtainTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("Si" , fontTitleAntecedent )};

                 PdfPCell cellBackgroundObtainFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("No" , fontTitleAntecedent )};
                tableAntecedentMedical.AddCell(cellTypeAntecedent);
                tableAntecedentMedical.AddCell(cellBackgroundObtainTrue);
                tableAntecedentMedical.AddCell(cellBackgroundObtainFalse);


                PdfPTable tableBackgrounds = new PdfPTable(3);
                tableBackgrounds.TotalWidth = 50f;
                foreach (var atribute in clinicHistory)
                { 
                var backgroundInTrue = "";
                var backgroundInFalse = "";
                    var exist = atribute.BackgroundMedicalsLink.Where( x => x.BackgroundMedicalsId == idTableBackgroundMedicals[0].Id).Any();
                    if(exist){
                    var backgroundObtainTrue = string.Format("{0:x;0; }", exist.GetHashCode());
                    backgroundInTrue = backgroundObtainTrue;
                   }else {
                        var backgroundObtainFalse = string.Format("{0: ;0;x}", exist.GetHashCode());
                        backgroundInFalse = backgroundObtainFalse;
                    }  
                   
                PdfPCell cellTypeBackGround = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase( "1. "+idTableBackgroundMedicals[0].description, fontDataAntecedent )};
                tableBackgrounds.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInTrue , fontHeader )};
                tableBackgrounds.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInFalse, fontHeader )};
                tableBackgrounds.AddCell(cellBackgroundFalse);
               
                }

                 PdfPTable tableBackgrounds2 = new PdfPTable(3);

                foreach (var atribute in clinicHistory)
                {
                var backgroundInTrue = "";
                var backgroundInFalse = "";
                var exist = atribute.BackgroundMedicalsLink.Where( x => x.BackgroundMedicalsId == idTableBackgroundMedicals[1].Id).Any();
                 if(exist){
                var backgroundObtainTrue = string.Format("{0:x;0; }", exist.GetHashCode());
                    backgroundInTrue = backgroundObtainTrue;
                   }else {
                        var backgroundObtainFalse = string.Format("{0: ;0;x}", exist.GetHashCode());
                        backgroundInFalse = backgroundObtainFalse;
                    }  
                    
                PdfPCell cellTypeBackGround = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase( "2. "+idTableBackgroundMedicals[1].description, fontDataAntecedent )};
                tableBackgrounds2.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInTrue , fontHeader )};
                tableBackgrounds2.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInFalse , fontHeader )};
                tableBackgrounds2.AddCell(cellBackgroundFalse);
        
                }
                PdfPTable tableBackgrounds3 = new PdfPTable(3);

                foreach (var atribute in clinicHistory)
                {
                var backgroundInTrue = "";
                var backgroundInFalse = "";
                var exist = atribute.BackgroundMedicalsLink.Where( x => x.BackgroundMedicalsId == idTableBackgroundMedicals[2].Id).Any();
                 if(exist){
                var backgroundObtainTrue = string.Format("{0:x;0; }", exist.GetHashCode());
                    backgroundInTrue = backgroundObtainTrue;
                   }else {
                        var backgroundObtainFalse = string.Format("{0: ;0;x}", exist.GetHashCode());
                        backgroundInFalse = backgroundObtainFalse;
                    }   
                    
                PdfPCell cellTypeBackGround = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("3. "+ idTableBackgroundMedicals[2].description, fontDataAntecedent )};
                tableBackgrounds3.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInTrue , fontHeader )};
                tableBackgrounds3.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInFalse , fontHeader )};
                tableBackgrounds3.AddCell(cellBackgroundFalse);
            
                }
                PdfPTable tableBackgrounds4 = new PdfPTable(3);

                foreach (var atribute in clinicHistory)
                {
                var backgroundInTrue = "";
                var backgroundInFalse = "";
                var exist = atribute.BackgroundMedicalsLink.Where( x => x.BackgroundMedicalsId == idTableBackgroundMedicals[3].Id).Any();
                 if(exist){
                var backgroundObtainTrue = string.Format("{0:x;0; }", exist.GetHashCode());
                    backgroundInTrue = backgroundObtainTrue;
                   }else {
                        var backgroundObtainFalse = string.Format("{0: ;0;x}", exist.GetHashCode());
                        backgroundInFalse = backgroundObtainFalse;
                    }   
                   
                PdfPCell cellTypeBackGround = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("4. "+idTableBackgroundMedicals[3].description, fontDataAntecedent )};
                tableBackgrounds4.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInTrue , fontHeader )};
                tableBackgrounds4.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInFalse , fontHeader )};
                tableBackgrounds4.AddCell(cellBackgroundFalse);
               
                }
                PdfPTable tableBackgrounds5 = new PdfPTable(3);

                foreach (var atribute in clinicHistory)
                {
                var backgroundInTrue = "";
                var backgroundInFalse = "";
                var exist = atribute.BackgroundMedicalsLink.Where( x => x.BackgroundMedicalsId == idTableBackgroundMedicals[4].Id).Any();
                    if(exist){
                    var backgroundObtainTrue = string.Format("{0:x;0; }", exist.GetHashCode());
                    backgroundInTrue = backgroundObtainTrue;
                   }else {
                        var backgroundObtainFalse = string.Format("{0: ;0;x}", exist.GetHashCode());
                        backgroundInFalse = backgroundObtainFalse;
                    }  
                PdfPCell cellTypeBackGround = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase( "5. "+idTableBackgroundMedicals[4].description, fontDataAntecedent )};
                tableBackgrounds5.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInTrue , fontHeader )};
                tableBackgrounds5.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInFalse , fontHeader )};
                tableBackgrounds5.AddCell(cellBackgroundFalse);
                }

                PdfPTable tableBackgrounds6 = new PdfPTable(3);

                foreach (var atribute in clinicHistory)
                {
                var backgroundInTrue = "";
                var backgroundInFalse = "";
                var exist = atribute.BackgroundMedicalsLink.Where( x => x.BackgroundMedicalsId == idTableBackgroundMedicals[5].Id).Any();
                    if(exist){
                    var backgroundObtainTrue = string.Format("{0:x;0; }", exist.GetHashCode());
                    backgroundInTrue = backgroundObtainTrue;
                   }else {
                        var backgroundObtainFalse = string.Format("{0: ;0;x}", exist.GetHashCode());
                        backgroundInFalse = backgroundObtainFalse;
                    }  
                PdfPCell cellTypeBackGround = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase( "6. "+idTableBackgroundMedicals[5].description, fontDataAntecedent )};
                tableBackgrounds6.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInTrue , fontHeader )};
                tableBackgrounds6.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInFalse , fontHeader )};
                tableBackgrounds6.AddCell(cellBackgroundFalse);
                }
                PdfPTable tableBackgrounds7 = new PdfPTable(3);
                tableBackgrounds7.TotalWidth = 50f;

                foreach (var atribute in clinicHistory)
                {
                var backgroundInTrue = "";
                var backgroundInFalse = "";
                var exist = atribute.BackgroundMedicalsLink.Where( x => x.BackgroundMedicalsId == idTableBackgroundMedicals[6].Id).Any();
                    if(exist){
                    var backgroundObtainTrue = string.Format("{0:x;0; }", exist.GetHashCode());
                    backgroundInTrue = backgroundObtainTrue;
                   }else {
                        var backgroundObtainFalse = string.Format("{0: ;0;x}", exist.GetHashCode());
                        backgroundInFalse = backgroundObtainFalse;
                    } 
                

                PdfPCell cellTypeBackGround = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase( "7. "+idTableBackgroundMedicals[6].description, fontDataAntecedent )};
                tableBackgrounds7.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInTrue.ToString() , fontHeader )};
                tableBackgrounds7.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInFalse.ToString() , fontHeader )};
                tableBackgrounds7.AddCell(cellBackgroundFalse);
                }
                PdfPTable tableBackgrounds8 = new PdfPTable(3);

                foreach (var atribute in clinicHistory)
                {
                var backgroundInTrue = "";
                var backgroundInFalse = "";
                var exist = atribute.BackgroundMedicalsLink.Where( x => x.BackgroundMedicalsId == idTableBackgroundMedicals[7].Id).Any();
                    if(exist){
                    var backgroundObtainTrue = string.Format("{0:x;0; }", exist.GetHashCode());
                    backgroundInTrue = backgroundObtainTrue;
                   }else {
                        var backgroundObtainFalse = string.Format("{0: ;0;x}", exist.GetHashCode());
                        backgroundInFalse = backgroundObtainFalse;
                    } 

                PdfPCell cellTypeBackGround = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase( "8. "+idTableBackgroundMedicals[7].description, fontDataAntecedent )};
                tableBackgrounds8.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInTrue , fontHeader )};
                tableBackgrounds8.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInFalse, fontHeader )};
                tableBackgrounds8.AddCell(cellBackgroundFalse);
                }
                PdfPTable tableBackgrounds9 = new PdfPTable(3);

                foreach (var atribute in clinicHistory)
                {
                var backgroundInTrue = "";
                var backgroundInFalse = "";
                var exist = atribute.BackgroundMedicalsLink.Where( x => x.BackgroundMedicalsId == idTableBackgroundMedicals[8].Id).Any();
                    if(exist){
                    var backgroundObtainTrue = string.Format("{0:x;0; }", exist.GetHashCode());
                    backgroundInTrue = backgroundObtainTrue;
                   }else {
                        var backgroundObtainFalse = string.Format("{0: ;0;x}", exist.GetHashCode());
                        backgroundInFalse = backgroundObtainFalse;
                    } 

                PdfPCell cellTypeBackGround = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase( "9. "+idTableBackgroundMedicals[8].description, fontDataAntecedent )};
                tableBackgrounds9.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInTrue.ToString() , fontHeader )};
                tableBackgrounds9.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInFalse.ToString() , fontHeader )};
                tableBackgrounds9.AddCell(cellBackgroundFalse);
                }
                PdfPTable tableBackgrounds10 = new PdfPTable(3);

                foreach (var atribute in clinicHistory)
                {
                var backgroundInTrue = "";
                var backgroundInFalse = "";
                var exist = atribute.BackgroundMedicalsLink.Where( x => x.BackgroundMedicalsId == idTableBackgroundMedicals[9].Id).Any();
                    if(exist){
                    var backgroundObtainTrue = string.Format("{0:x;0; }", exist.GetHashCode());
                    backgroundInTrue = backgroundObtainTrue;
                   }else {
                        var backgroundObtainFalse = string.Format("{0: ;0;x}", exist.GetHashCode());
                        backgroundInFalse = backgroundObtainFalse;
                    } 

                PdfPCell cellTypeBackGround = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase( "10. "+idTableBackgroundMedicals[9].description, fontDataAntecedent )};
                tableBackgrounds10.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInTrue.ToString() , fontHeader )};
                tableBackgrounds10.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInFalse.ToString() , fontHeader )};
                tableBackgrounds10.AddCell(cellBackgroundFalse);
                }
                PdfPTable tableBackgrounds11 = new PdfPTable(3);

                foreach (var atribute in clinicHistory)
                {
                var backgroundInTrue = "";
                var backgroundInFalse = "";
                var exist = atribute.BackgroundMedicalsLink.Where( x => x.BackgroundMedicalsId == idTableBackgroundMedicals[10].Id).Any();
                    if(exist){
                    var backgroundObtainTrue = string.Format("{0:x;0; }", exist.GetHashCode());
                    backgroundInTrue = backgroundObtainTrue;
                   }else {
                        var backgroundObtainFalse = string.Format("{0: ;0;x}", exist.GetHashCode());
                        backgroundInFalse = backgroundObtainFalse;
                    } 

                PdfPCell cellTypeBackGround = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase( "11. "+idTableBackgroundMedicals[10].description, fontDataAntecedent )};
                tableBackgrounds11.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInTrue.ToString() , fontHeader )};
                tableBackgrounds11.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInFalse.ToString() , fontHeader )};
                tableBackgrounds11.AddCell(cellBackgroundFalse);
                }
                PdfPTable tableBackgrounds12 = new PdfPTable(3);

                foreach (var atribute in clinicHistory)
                {
                var backgroundInTrue = "";
                var backgroundInFalse = "";
                var exist = atribute.BackgroundMedicalsLink.Where( x => x.BackgroundMedicalsId == idTableBackgroundMedicals[11].Id).Any();
                    if(exist){
                    var backgroundObtainTrue = string.Format("{0:x;0; }", exist.GetHashCode());
                    backgroundInTrue = backgroundObtainTrue;
                   }else {
                        var backgroundObtainFalse = string.Format("{0: ;0;x}", exist.GetHashCode());
                        backgroundInFalse = backgroundObtainFalse;
                    } 

                PdfPCell cellTypeBackGround = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase( "12. "+idTableBackgroundMedicals[11].description, fontDataAntecedent )};
                tableBackgrounds12.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInTrue.ToString() , fontHeader )};
                tableBackgrounds12.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInFalse.ToString() , fontHeader )};
                tableBackgrounds12.AddCell(cellBackgroundFalse);
                }
                PdfPTable tableBackgrounds13 = new PdfPTable(3);

                foreach (var atribute in clinicHistory)
                {
                var backgroundInTrue = "";
                var backgroundInFalse = "";
                var exist = atribute.BackgroundMedicalsLink.Where( x => x.BackgroundMedicalsId == idTableBackgroundMedicals[12].Id).Any();
                    if(exist){
                    var backgroundObtainTrue = string.Format("{0:x;0; }", exist.GetHashCode());
                    backgroundInTrue = backgroundObtainTrue;
                   }else {
                        var backgroundObtainFalse = string.Format("{0: ;0;x}", exist.GetHashCode());
                        backgroundInFalse = backgroundObtainFalse;
                    } 

                PdfPCell cellTypeBackGround = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase( "13. "+idTableBackgroundMedicals[12].description, fontDataAntecedent )};
                tableBackgrounds13.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInTrue.ToString() , fontHeader )};
                tableBackgrounds13.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInFalse.ToString() , fontHeader )};
                tableBackgrounds13.AddCell(cellBackgroundFalse);
                }
                 PdfPTable tableBackgrounds14 = new PdfPTable(3);

                foreach (var atribute in clinicHistory)
                {
                var backgroundInTrue = "";
                var backgroundInFalse = "";
                var exist = atribute.BackgroundMedicalsLink.Where( x => x.BackgroundMedicalsId == idTableBackgroundMedicals[13].Id).Any();
                    if(exist){
                    var backgroundObtainTrue = string.Format("{0:x;0; }", exist.GetHashCode());
                    backgroundInTrue = backgroundObtainTrue;
                   }else {
                        var backgroundObtainFalse = string.Format("{0: ;0;x}", exist.GetHashCode());
                        backgroundInFalse = backgroundObtainFalse;
                    } 

                PdfPCell cellTypeBackGround = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase( "14. "+idTableBackgroundMedicals[13].description, fontDataAntecedent )};
                tableBackgrounds14.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInTrue.ToString() , fontHeader )};
                tableBackgrounds14.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInFalse.ToString() , fontHeader )};
                tableBackgrounds14.AddCell(cellBackgroundFalse);
                }
                /*Alineaciòn de tablas hacia la izquierda*/
                tableAntecedentMedical.HorizontalAlignment = Element.ALIGN_LEFT;
                tableBackgrounds.HorizontalAlignment = Element.ALIGN_LEFT;
                tableBackgrounds2.HorizontalAlignment = Element.ALIGN_LEFT;
                tableBackgrounds3.HorizontalAlignment = Element.ALIGN_LEFT;
                tableBackgrounds4.HorizontalAlignment = Element.ALIGN_LEFT;
                tableBackgrounds5.HorizontalAlignment = Element.ALIGN_LEFT;
                tableBackgrounds6.HorizontalAlignment = Element.ALIGN_LEFT;
                tableBackgrounds7.HorizontalAlignment = Element.ALIGN_LEFT;
                tableBackgrounds8.HorizontalAlignment = Element.ALIGN_LEFT;
                tableBackgrounds9.HorizontalAlignment = Element.ALIGN_LEFT;
                tableBackgrounds10.HorizontalAlignment = Element.ALIGN_LEFT;
                tableBackgrounds11.HorizontalAlignment = Element.ALIGN_LEFT;
                tableBackgrounds12.HorizontalAlignment = Element.ALIGN_LEFT;
                tableBackgrounds13.HorizontalAlignment = Element.ALIGN_LEFT;
                tableBackgrounds14.HorizontalAlignment = Element.ALIGN_LEFT;


                float[] widths8 = new float[] { 120f, 50f, 50f};
                tableAntecedentMedical.SetWidthPercentage(widths8, rect);
                tableBackgrounds.SetWidthPercentage(widths8, rect);
                tableBackgrounds2.SetWidthPercentage(widths8, rect);
                tableBackgrounds3.SetWidthPercentage(widths8, rect);
                tableBackgrounds4.SetWidthPercentage(widths8, rect);
                tableBackgrounds5.SetWidthPercentage(widths8, rect);
                tableBackgrounds6.SetWidthPercentage(widths8, rect);
                tableBackgrounds7.SetWidthPercentage(widths8, rect);
                tableBackgrounds8.SetWidthPercentage(widths8, rect);
                tableBackgrounds9.SetWidthPercentage(widths8, rect);
                tableBackgrounds10.SetWidthPercentage(widths8, rect);
                tableBackgrounds11.SetWidthPercentage(widths8, rect);
                tableBackgrounds12.SetWidthPercentage(widths8, rect);
                tableBackgrounds13.SetWidthPercentage(widths8, rect);
                tableBackgrounds14.SetWidthPercentage(widths8, rect);

                           


                /*CONSTRUCCION DE LAS TABLAS PARA LOS ANTECEDENTES MEDICOS*/   
                
                PdfPTable tableAntecedentOral = new PdfPTable(3);
                tableAntecedentOral.TotalWidth = 50f;
                PdfPCell cellTypeAntecedentOral = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("Nombre Antecedente" , fontTitleAntecedent )};

                PdfPCell cellBackgroundObtainTrueOral = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("Si" , fontTitleAntecedent )};

                 PdfPCell cellBackgroundObtainFalseOral = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("No" , fontTitleAntecedent )};
                tableAntecedentOral.AddCell(cellTypeAntecedentOral);
                tableAntecedentOral.AddCell(cellBackgroundObtainTrueOral);
                tableAntecedentOral.AddCell(cellBackgroundObtainFalseOral);

                PdfPTable tableBackgroundsOral = new PdfPTable(3);
                tableBackgroundsOral.TotalWidth = 50f;

                foreach (var atribute in clinicHistory)
                { 
                var backgroundInTrue = "";
                var backgroundInFalse = "";
                    var exist = atribute.BackgroundOralsLink.Where( x => x.BackgroundOralsId == idTableBackgroundOrals[0].Id).Any();
                    if(exist){
                    var backgroundObtainTrue = string.Format("{0:x;0; }", exist.GetHashCode());
                    backgroundInTrue = backgroundObtainTrue;
                   }else {
                        var backgroundObtainFalse = string.Format("{0: ;0;x}", exist.GetHashCode());
                        backgroundInFalse = backgroundObtainFalse;
                    }  
                   
                PdfPCell cellTypeBackGround = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase( "1. " + idTableBackgroundOrals[0].description, fontDataAntecedent )};
                tableBackgroundsOral.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInTrue , fontHeader )};
                tableBackgroundsOral.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInFalse, fontHeader )};
                tableBackgroundsOral.AddCell(cellBackgroundFalse);
               
                }

                PdfPTable tableBackgroundsOral2 = new PdfPTable(3);
                tableBackgroundsOral2.TotalWidth = 50f;

                foreach (var atribute in clinicHistory)
                { 
                var backgroundInTrue = "";
                var backgroundInFalse = "";
                    var exist = atribute.BackgroundOralsLink.Where( x => x.BackgroundOralsId == idTableBackgroundOrals[1].Id).Any();
                    if(exist){
                    var backgroundObtainTrue = string.Format("{0:x;0; }", exist.GetHashCode());
                    backgroundInTrue = backgroundObtainTrue;
                   }else {
                        var backgroundObtainFalse = string.Format("{0: ;0;x}", exist.GetHashCode());
                        backgroundInFalse = backgroundObtainFalse;
                    }  
                   
                PdfPCell cellTypeBackGround = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase( "2. "+ idTableBackgroundOrals[1].description, fontDataAntecedent )};
                tableBackgroundsOral2.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInTrue , fontHeader )};
                tableBackgroundsOral2.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInFalse, fontHeader )};
                tableBackgroundsOral2.AddCell(cellBackgroundFalse);
               
                }

                PdfPTable tableBackgroundsOral3 = new PdfPTable(3);
                tableBackgroundsOral3.TotalWidth = 50f;

                foreach (var atribute in clinicHistory)
                { 
                var backgroundInTrue = "";
                var backgroundInFalse = "";
                    var exist = atribute.BackgroundOralsLink.Where( x => x.BackgroundOralsId == idTableBackgroundOrals[2].Id).Any();
                    if(exist){
                    var backgroundObtainTrue = string.Format("{0:x;0; }", exist.GetHashCode());
                    backgroundInTrue = backgroundObtainTrue;
                   }else {
                        var backgroundObtainFalse = string.Format("{0: ;0;x}", exist.GetHashCode());
                        backgroundInFalse = backgroundObtainFalse;
                    }  
                   
                PdfPCell cellTypeBackGround = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("3. "+ idTableBackgroundOrals[2].description, fontDataAntecedent )};
                tableBackgroundsOral3.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInTrue , fontHeader )};
                tableBackgroundsOral3.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInFalse, fontHeader )};
                tableBackgroundsOral3.AddCell(cellBackgroundFalse);
               
                }

                PdfPTable tableBackgroundsOral4 = new PdfPTable(3);
                tableBackgroundsOral4.TotalWidth = 50f;

                foreach (var atribute in clinicHistory)
                { 
                var backgroundInTrue = "";
                var backgroundInFalse = "";
                    var exist = atribute.BackgroundOralsLink.Where( x => x.BackgroundOralsId == idTableBackgroundOrals[3].Id).Any();
                    if(exist){
                    var backgroundObtainTrue = string.Format("{0:x;0; }", exist.GetHashCode());
                    backgroundInTrue = backgroundObtainTrue;
                   }else {
                        var backgroundObtainFalse = string.Format("{0: ;0;x}", exist.GetHashCode());
                        backgroundInFalse = backgroundObtainFalse;
                    }  
                   
                PdfPCell cellTypeBackGround = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase( "4. "+idTableBackgroundOrals[3].description, fontDataAntecedent )};
                tableBackgroundsOral4.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInTrue , fontHeader )};
                tableBackgroundsOral4.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInFalse, fontHeader )};
                tableBackgroundsOral4.AddCell(cellBackgroundFalse);
               
                }

                PdfPTable tableBackgroundsOral5 = new PdfPTable(3);

                foreach (var atribute in clinicHistory)
                { 
                var backgroundInTrue = "";
                var backgroundInFalse = "";
                    var exist = atribute.BackgroundOralsLink.Where( x => x.BackgroundOralsId == idTableBackgroundOrals[4].Id).Any();
                    if(exist){
                    var backgroundObtainTrue = string.Format("{0:x;0; }", exist.GetHashCode());
                    backgroundInTrue = backgroundObtainTrue;
                   }else {
                        var backgroundObtainFalse = string.Format("{0: ;0;x}", exist.GetHashCode());
                        backgroundInFalse = backgroundObtainFalse;
                    }  
                   
                PdfPCell cellTypeBackGround = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase( "5. "+idTableBackgroundOrals[4].description, fontDataAntecedent )};
                tableBackgroundsOral5.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInTrue , fontHeader )};
                tableBackgroundsOral5.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInFalse, fontHeader )};
                tableBackgroundsOral5.AddCell(cellBackgroundFalse);
               
                }

                PdfPTable tableBackgroundsOral6 = new PdfPTable(3);
                tableBackgroundsOral6.TotalWidth = 50f;

                foreach (var atribute in clinicHistory)
                { 
                var backgroundInTrue = "";
                var backgroundInFalse = "";
                    var exist = atribute.BackgroundOralsLink.Where( x => x.BackgroundOralsId == idTableBackgroundOrals[5].Id).Any();
                    if(exist){
                    var backgroundObtainTrue = string.Format("{0:x;0; }", exist.GetHashCode());
                    backgroundInTrue = backgroundObtainTrue;
                   }else {
                        var backgroundObtainFalse = string.Format("{0: ;0;x}", exist.GetHashCode());
                        backgroundInFalse = backgroundObtainFalse;
                    }  
                   
                PdfPCell cellTypeBackGround = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("6. "+ idTableBackgroundOrals[5].description, fontDataAntecedent )};
                tableBackgroundsOral6.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInTrue , fontHeader )};
                tableBackgroundsOral6.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInFalse, fontHeader )};
                tableBackgroundsOral6.AddCell(cellBackgroundFalse);
               
                }

                PdfPTable tableBackgroundsOral7 = new PdfPTable(3);
                tableBackgroundsOral7.TotalWidth = 50f;

                foreach (var atribute in clinicHistory)
                { 
                // var backgroundInTrue = "";
                // var backgroundInFalse = "";
                //     var exist = atribute.BackgroundOralsLink.Where( x => x.BackgroundOralsId == idTableBackgroundOrals[5].Id).Any();
                //     if(exist){
                //     var backgroundObtainTrue = string.Format("{0:x;0; }", exist.GetHashCode());
                //     backgroundInTrue = backgroundObtainTrue;
                //    }else {
                //         var backgroundObtainFalse = string.Format("{0: ;0;x}", exist.GetHashCode());
                //         backgroundInFalse = backgroundObtainFalse;
                //     }  
                   
                PdfPCell cellTypeBackGround = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("", fontDataAntecedent )};
                tableBackgroundsOral7.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("", fontHeader )};
                tableBackgroundsOral7.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("", fontHeader )};
                tableBackgroundsOral7.AddCell(cellBackgroundFalse);
               
                }

                PdfPTable tableBackgroundsOral8 = new PdfPTable(3);
                tableBackgroundsOral8.TotalWidth = 50f;

                foreach (var atribute in clinicHistory)
                { 
                // var backgroundInTrue = "";
                // var backgroundInFalse = "";
                //     var exist = atribute.BackgroundOralsLink.Where( x => x.BackgroundOralsId == idTableBackgroundOrals[5].Id).Any();
                //     if(exist){
                //     var backgroundObtainTrue = string.Format("{0:x;0; }", exist.GetHashCode());
                //     backgroundInTrue = backgroundObtainTrue;
                //    }else {
                //         var backgroundObtainFalse = string.Format("{0: ;0;x}", exist.GetHashCode());
                //         backgroundInFalse = backgroundObtainFalse;
                //     }  
                   
                PdfPCell cellTypeBackGround = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("", fontDataAntecedent )};
                tableBackgroundsOral8.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("", fontHeader )};
                tableBackgroundsOral8.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("", fontHeader )};
                tableBackgroundsOral8.AddCell(cellBackgroundFalse);
               
                }
                
                PdfPTable tableBackgroundsOral9 = new PdfPTable(3);
                tableBackgroundsOral9.TotalWidth = 50f;

                foreach (var atribute in clinicHistory)
                { 
                // var backgroundInTrue = "";
                // var backgroundInFalse = "";
                //     var exist = atribute.BackgroundOralsLink.Where( x => x.BackgroundOralsId == idTableBackgroundOrals[5].Id).Any();
                //     if(exist){
                //     var backgroundObtainTrue = string.Format("{0:x;0; }", exist.GetHashCode());
                //     backgroundInTrue = backgroundObtainTrue;
                //    }else {
                //         var backgroundObtainFalse = string.Format("{0: ;0;x}", exist.GetHashCode());
                //         backgroundInFalse = backgroundObtainFalse;
                //     }  
                   
                PdfPCell cellTypeBackGround = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("", fontDataAntecedent )};
                tableBackgroundsOral9.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("", fontHeader )};
                tableBackgroundsOral9.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("", fontHeader )};
                tableBackgroundsOral9.AddCell(cellBackgroundFalse);
               
                }

                PdfPTable tableBackgroundsOral10 = new PdfPTable(3);
                tableBackgroundsOral10.TotalWidth = 50f;

                foreach (var atribute in clinicHistory)
                { 
                // var backgroundInTrue = "";
                // var backgroundInFalse = "";
                //     var exist = atribute.BackgroundOralsLink.Where( x => x.BackgroundOralsId == idTableBackgroundOrals[5].Id).Any();
                //     if(exist){
                //     var backgroundObtainTrue = string.Format("{0:x;0; }", exist.GetHashCode());
                //     backgroundInTrue = backgroundObtainTrue;
                //    }else {
                //         var backgroundObtainFalse = string.Format("{0: ;0;x}", exist.GetHashCode());
                //         backgroundInFalse = backgroundObtainFalse;
                //     }  
                   
                PdfPCell cellTypeBackGround = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("", fontDataAntecedent )};
                tableBackgroundsOral10.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("", fontHeader )};
                tableBackgroundsOral10.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("", fontHeader )};
                tableBackgroundsOral10.AddCell(cellBackgroundFalse);
               
                }

                 PdfPTable tableBackgroundsOral11 = new PdfPTable(3);
                tableBackgroundsOral11.TotalWidth = 50f;

                foreach (var atribute in clinicHistory)
                { 
                // var backgroundInTrue = "";
                // var backgroundInFalse = "";
                //     var exist = atribute.BackgroundOralsLink.Where( x => x.BackgroundOralsId == idTableBackgroundOrals[5].Id).Any();
                //     if(exist){
                //     var backgroundObtainTrue = string.Format("{0:x;0; }", exist.GetHashCode());
                //     backgroundInTrue = backgroundObtainTrue;
                //    }else {
                //         var backgroundObtainFalse = string.Format("{0: ;0;x}", exist.GetHashCode());
                //         backgroundInFalse = backgroundObtainFalse;
                //     }  
                   
                PdfPCell cellTypeBackGround = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("", fontDataAntecedent )};
                tableBackgroundsOral11.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("", fontHeader )};
                tableBackgroundsOral11.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("", fontHeader )};
                tableBackgroundsOral11.AddCell(cellBackgroundFalse);
               
                }

                PdfPTable tableBackgroundsOral12 = new PdfPTable(3);
                tableBackgroundsOral12.TotalWidth = 50f;

                foreach (var atribute in clinicHistory)
                { 
                // var backgroundInTrue = "";
                // var backgroundInFalse = "";
                //     var exist = atribute.BackgroundOralsLink.Where( x => x.BackgroundOralsId == idTableBackgroundOrals[5].Id).Any();
                //     if(exist){
                //     var backgroundObtainTrue = string.Format("{0:x;0; }", exist.GetHashCode());
                //     backgroundInTrue = backgroundObtainTrue;
                //    }else {
                //         var backgroundObtainFalse = string.Format("{0: ;0;x}", exist.GetHashCode());
                //         backgroundInFalse = backgroundObtainFalse;
                //     }  
                   
                PdfPCell cellTypeBackGround = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("", fontDataAntecedent )};
                tableBackgroundsOral12.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("", fontHeader )};
                tableBackgroundsOral12.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("", fontHeader )};
                tableBackgroundsOral12.AddCell(cellBackgroundFalse);
               
                }

                PdfPTable tableBackgroundsOral13 = new PdfPTable(3);
                tableBackgroundsOral13.TotalWidth = 50f;

                foreach (var atribute in clinicHistory)
                { 
                // var backgroundInTrue = "";
                // var backgroundInFalse = "";
                //     var exist = atribute.BackgroundOralsLink.Where( x => x.BackgroundOralsId == idTableBackgroundOrals[5].Id).Any();
                //     if(exist){
                //     var backgroundObtainTrue = string.Format("{0:x;0; }", exist.GetHashCode());
                //     backgroundInTrue = backgroundObtainTrue;
                //    }else {
                //         var backgroundObtainFalse = string.Format("{0: ;0;x}", exist.GetHashCode());
                //         backgroundInFalse = backgroundObtainFalse;
                //     }  
                   
                PdfPCell cellTypeBackGround = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("", fontDataAntecedent )};
                tableBackgroundsOral13.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("", fontHeader )};
                tableBackgroundsOral13.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("", fontHeader )};
                tableBackgroundsOral13.AddCell(cellBackgroundFalse);
               
                }

                PdfPTable tableBackgroundsOral14 = new PdfPTable(3);
                tableBackgroundsOral14.TotalWidth = 50f;

                foreach (var atribute in clinicHistory)
                { 
                // var backgroundInTrue = "";
                // var backgroundInFalse = "";
                //     var exist = atribute.BackgroundOralsLink.Where( x => x.BackgroundOralsId == idTableBackgroundOrals[5].Id).Any();
                //     if(exist){
                //     var backgroundObtainTrue = string.Format("{0:x;0; }", exist.GetHashCode());
                //     backgroundInTrue = backgroundObtainTrue;
                //    }else {
                //         var backgroundObtainFalse = string.Format("{0: ;0;x}", exist.GetHashCode());
                //         backgroundInFalse = backgroundObtainFalse;
                //     }  
                   
                PdfPCell cellTypeBackGround = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("", fontDataAntecedent )};
                tableBackgroundsOral14.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("", fontHeader )};
                tableBackgroundsOral14.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("", fontHeader )};
                tableBackgroundsOral14.AddCell(cellBackgroundFalse);
               
                }
                
                /*Alineaciòn de tablas de antecedentes orales*/
                tableAntecedentOral.HorizontalAlignment = Element.ALIGN_RIGHT;
                tableBackgroundsOral.HorizontalAlignment = Element.ALIGN_RIGHT;
                tableBackgroundsOral2.HorizontalAlignment = Element.ALIGN_RIGHT;
                tableBackgroundsOral3.HorizontalAlignment = Element.ALIGN_RIGHT;
                tableBackgroundsOral4.HorizontalAlignment = Element.ALIGN_RIGHT;
                tableBackgroundsOral5.HorizontalAlignment = Element.ALIGN_RIGHT;
                tableBackgroundsOral6.HorizontalAlignment = Element.ALIGN_RIGHT;
                tableBackgroundsOral7.HorizontalAlignment = Element.ALIGN_RIGHT;
                tableBackgroundsOral8.HorizontalAlignment = Element.ALIGN_RIGHT;
                tableBackgroundsOral9.HorizontalAlignment = Element.ALIGN_RIGHT;
                tableBackgroundsOral10.HorizontalAlignment = Element.ALIGN_RIGHT;
                tableBackgroundsOral11.HorizontalAlignment = Element.ALIGN_RIGHT;
                tableBackgroundsOral12.HorizontalAlignment = Element.ALIGN_RIGHT;
                tableBackgroundsOral13.HorizontalAlignment = Element.ALIGN_RIGHT;
                tableBackgroundsOral14.HorizontalAlignment = Element.ALIGN_RIGHT;

                float[] widths9 = new float[] { 120f, 50f, 50f};
                tableAntecedentOral.SetWidthPercentage(widths9, rect);
                tableBackgroundsOral.SetWidthPercentage(widths9, rect);
                tableBackgroundsOral2.SetWidthPercentage(widths9, rect);
                tableBackgroundsOral3.SetWidthPercentage(widths9, rect);
                tableBackgroundsOral4.SetWidthPercentage(widths9, rect);
                tableBackgroundsOral5.SetWidthPercentage(widths9, rect);
                tableBackgroundsOral6.SetWidthPercentage(widths9, rect);
                tableBackgroundsOral7.SetWidthPercentage(widths9, rect);
                tableBackgroundsOral8.SetWidthPercentage(widths9, rect);
                tableBackgroundsOral9.SetWidthPercentage(widths9, rect);
                tableBackgroundsOral10.SetWidthPercentage(widths9, rect);
                tableBackgroundsOral11.SetWidthPercentage(widths9, rect);
                tableBackgroundsOral12.SetWidthPercentage(widths9, rect);
                tableBackgroundsOral13.SetWidthPercentage(widths9, rect);
                tableBackgroundsOral14.SetWidthPercentage(widths9, rect);

                /*Se añaden por creaciones de tablas 2x2 para agregar las tablas ya creadas como celda y poderlas meter en una mismo contenedor*/
                mtable.AddCell(tableAntecedentMedical);
                mtable.AddCell(tableAntecedentOral);

                m2table.AddCell(tableBackgrounds);
                m2table.AddCell(tableBackgroundsOral);

                m3table.AddCell(tableBackgrounds2);
                m3table.AddCell(tableBackgroundsOral2);

                m4table.AddCell(tableBackgrounds3);
                m4table.AddCell(tableBackgroundsOral3);

                m5table.AddCell(tableBackgrounds4);
                m5table.AddCell(tableBackgroundsOral4);

                m6table.AddCell(tableBackgrounds5);
                m6table.AddCell(tableBackgroundsOral5);

                m7table.AddCell(tableBackgrounds6);
                m7table.AddCell(tableBackgroundsOral6);

                m8table.AddCell(tableBackgrounds7);
                m8table.AddCell(tableBackgroundsOral7); 
            
                m9table.AddCell(tableBackgrounds8);
                m9table.AddCell(tableBackgroundsOral8);

                m10table.AddCell(tableBackgrounds9);
                m10table.AddCell(tableBackgroundsOral9);

                m11table.AddCell(tableBackgrounds10);
                m11table.AddCell(tableBackgroundsOral10);

                m12table.AddCell(tableBackgrounds11);
                m12table.AddCell(tableBackgroundsOral11);

                m13table.AddCell(tableBackgrounds12);
                m13table.AddCell(tableBackgroundsOral12);

                m14table.AddCell(tableBackgrounds13);
                m14table.AddCell(tableBackgroundsOral13);

                m15table.AddCell(tableBackgrounds14);
                m15table.AddCell(tableBackgroundsOral14);

                m15table.SpacingAfter = 5;
                
                /*CONSTRUCCION DE LAS TABLAS PARA LOS ANTECEDENTES MEDICOS*/
                document.Add(mtable);
                document.Add(m2table);    
                document.Add(m3table); 
                document.Add(m4table); 
                document.Add(m5table);
                document.Add(m6table);
                document.Add(m7table);
                document.Add(m8table); 
                document.Add(m9table); 
                document.Add(m10table); 
                document.Add(m11table); 
                document.Add(m12table); 
                document.Add(m13table);
                document.Add(m14table);
                document.Add(m15table);  

                PdfPTable ObservationsAntecedents = new PdfPTable(1);
                ObservationsAntecedents.WidthPercentage = 90f;
                PdfPCell cellObservationsAntecedents = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("OBSERVACIONES (Segùn numero de antecedente)", fontHeader )};
                cellObservationsAntecedents.Border = Rectangle.NO_BORDER;
                ObservationsAntecedents.AddCell(cellObservationsAntecedents);

                PdfPTable ObservationsAntecedentsText = new PdfPTable(1);
                ObservationsAntecedentsText.WidthPercentage = 90f;
                PdfPCell cellObservationsAntecedentsText = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 43,
                Phrase = new Phrase("", fontHeader )};
                cellObservationsAntecedents.Border = Rectangle.NO_BORDER;
                ObservationsAntecedentsText.AddCell(cellObservationsAntecedentsText);

                document.Add(ObservationsAntecedents);
                document.Add(ObservationsAntecedentsText);

                PdfPTable tableoralRadiograhies = new PdfPTable(1);
                tableoralRadiograhies.WidthPercentage = 90f;
                PdfPCell celloralRadiographies = new PdfPCell() {Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("4. RADIOGRAFIAS", fontHeader )};
                tableoralRadiograhies.SpacingAfter = 3;
                celloralRadiographies.Border = Rectangle.NO_BORDER;
                tableoralRadiograhies.AddCell(celloralRadiographies);

                document.Add(tableoralRadiograhies);

                  PdfPTable tableInfoRadiographies = new PdfPTable(3);
                float[] widths11 = new float[] {60f, 60f, 60f};
                tableInfoRadiographies.SetWidthPercentage(widths11, rect);

                PdfPCell cellheaderIdRadiographies = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("Radiografia no." , fontHeader )};
                tableInfoRadiographies.AddCell(cellheaderIdRadiographies);

                PdfPCell cellheaderdateRegister = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("Fecha de revisiòn" , fontHeader )};
                tableInfoRadiographies.AddCell(cellheaderdateRegister);

                PdfPCell cellheaderObservation = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("Observaciòn" , fontHeader )};
                tableInfoRadiographies.AddCell(cellheaderObservation);
            
                tableInfoRadiographies.WidthPercentage = 90;

                PdfPTable tableRadiographiesText = new PdfPTable(3);
                float[] widths12 = new float[] {60f, 60f, 60f};
                tableRadiographiesText.SetWidthPercentage(widths12, rect);

               
               for(int i = 0; i < radiographyData.Count; i++){
                foreach (var atribute in clinicHistory)
                {
                var name = atribute.oralRadiographyList.ToList();
                
                PdfPCell cellId = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase( atribute.oralRadiographyList.ToList()[i].Id.ToString() , fontData )};
                tableRadiographiesText.AddCell(cellId);

                PdfPCell celldateRegister = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(atribute.oralRadiographyList.ToList()[i].dateRegister.ToShortDateString().ToString(), fontData )};
                tableRadiographiesText.AddCell(celldateRegister);

                PdfPCell cellObservation = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(atribute.oralRadiographyList.ToList()[i].observation.ToString(), fontData )};
                tableRadiographiesText.AddCell(cellObservation);
                }
               }
                tableRadiographiesText.WidthPercentage = 90;

                tableRadiographiesText.SpacingAfter = 10;

                document.Add(tableInfoRadiographies);
                document.Add(tableRadiographiesText);

                PdfPTable tabletreamentPlan = new PdfPTable(1);
                tabletreamentPlan.WidthPercentage = 90f;
                PdfPCell celltreamentPlan = new PdfPCell() {Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("5. PLAN DE TRATAMIENTOS REALIZADOS", fontHeader )};
                tabletreamentPlan.SpacingAfter = 3;
                celltreamentPlan.Border = Rectangle.NO_BORDER;
                tabletreamentPlan.AddCell(celltreamentPlan);

                document.Add(tabletreamentPlan);

                  PdfPTable tableInfotreamentPlan = new PdfPTable(3);
                float[] widths13 = new float[] {60f, 60f, 60f};
                tableInfotreamentPlan.SetWidthPercentage(widths13, rect);

                PdfPCell cellheaderIdPlan = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("Tratamiento no." , fontHeader )};
                tableInfotreamentPlan.AddCell(cellheaderIdPlan);

                PdfPCell cellheaderName = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("Nombre del Plan" , fontHeader )};
                tableInfotreamentPlan.AddCell(cellheaderName);

                PdfPCell cellheaderObservationPlan = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("Observaciòn" , fontHeader )};
                tableInfotreamentPlan.AddCell(cellheaderObservationPlan);
            
                tableInfotreamentPlan.WidthPercentage = 90;

                PdfPTable tabletramentPlanText = new PdfPTable(3);
                float[] widths14 = new float[] {60f, 60f, 60f};
                tabletramentPlanText.SetWidthPercentage(widths14, rect);

               
               for(int i = 0; i < treamentData.Count; i++){
                foreach (var atribute in clinicHistory)
                {
                var name = atribute.treamentPlanList.ToList();
                
                PdfPCell cellId = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase( atribute.treamentPlanList.ToList()[i].Id.ToString() , fontData )};
                tabletramentPlanText.AddCell(cellId);

                PdfPCell cellNamePlan = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(atribute.treamentPlanList.ToList()[i].Name, fontData )};
                tabletramentPlanText.AddCell(cellNamePlan);

                PdfPCell cellObservation = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(atribute.treamentPlanList.ToList()[i].observation.ToString(), fontData )};
                tabletramentPlanText.AddCell(cellObservation);
                }
               }
                tabletramentPlanText.WidthPercentage = 90;

                tabletramentPlanText.SpacingAfter = 10;

                document.Add(tableInfotreamentPlan);
                document.Add(tabletramentPlanText);

                /*ESPACIO PARA TRABAJAR EL ODONTOGRAMA*/

                 PdfPTable tableOdontogram = new PdfPTable(1);
                tableOdontogram.WidthPercentage = 90f;
                PdfPCell cellOdontogram = new PdfPCell() {Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("6. DIAGNOSTICO ODONTOGRAMA", fontHeader )};
                tableOdontogram.SpacingAfter = 3;
                cellOdontogram.Border = Rectangle.NO_BORDER;
                tableOdontogram.AddCell(cellOdontogram);

                 document.Add(tableOdontogram);

                /*CONSTRUCCIÒN DE LOGICA Y TABLA, CELDAS PARA ODONTOGRAMA*/

                /*AQUI ACABA LA CONFIGURACIÒN PARA EL ODONTOGRAMA*/
                 PdfPTable tablepatienteEvolution = new PdfPTable(1);
                tablepatienteEvolution.WidthPercentage = 90f;
                PdfPCell cellpatientEvolution = new PdfPCell() {Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("7. EVOLUCIÒN DEL TRATAMIENTO", fontHeader )};
                tablepatienteEvolution.SpacingAfter = 3;
                cellpatientEvolution.Border = Rectangle.NO_BORDER;
                tablepatienteEvolution.AddCell(cellpatientEvolution);

                document.Add(tablepatienteEvolution);

                  PdfPTable tableInfopatientEvolution = new PdfPTable(4);
                float[] widths15 = new float[] {60f, 60f, 60f, 60f};
                tableInfopatientEvolution.SetWidthPercentage(widths15, rect);

                PdfPCell cellheaderIdpatientEvolution = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("Evoluciòn no." , fontHeader )};
                tableInfopatientEvolution.AddCell(cellheaderIdpatientEvolution);

                PdfPCell cellheaderFecha = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("Fecha" , fontHeader )};
                tableInfopatientEvolution.AddCell(cellheaderFecha);

                PdfPCell cellheaderEvolution = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("Evoluciòn" , fontHeader )};
                tableInfopatientEvolution.AddCell(cellheaderEvolution);

                PdfPCell cellheaderSignature = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("Firma del Paciente" , fontHeader )};
                tableInfopatientEvolution.AddCell(cellheaderSignature);
            
                tableInfopatientEvolution.WidthPercentage = 90;

                PdfPTable tablepatientEvolutionText = new PdfPTable(4);
                float[] widths16 = new float[] {60f, 60f, 60f, 60f};
                tablepatientEvolutionText.SetWidthPercentage(widths16, rect);

               
               for(int i = 0; i < patientEvolutionData.Count; i++){
                foreach (var atribute in clinicHistory)
                {       
                PdfPCell cellId = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase( atribute.patientEvolutionList.ToList()[i].Id.ToString() , fontData )};
                tablepatientEvolutionText.AddCell(cellId);

                PdfPCell cellDate = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(atribute.patientEvolutionList.ToList()[i].dateCreate.ToShortDateString().ToString(), fontData )};
                tablepatientEvolutionText.AddCell(cellDate);

                PdfPCell cellEvolution = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(atribute.patientEvolutionList.ToList()[i].observation.ToString(), fontData )};
                tablepatientEvolutionText.AddCell(cellEvolution);

                PdfPCell cellSignature = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("", fontData )};
                tablepatientEvolutionText.AddCell(cellSignature);
                }
               }
                tablepatientEvolutionText.WidthPercentage = 90;

                document.Add(tableInfopatientEvolution);
                document.Add(tablepatientEvolutionText);

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