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
                Font fontHeader = new Font(Font.HELVETICA, 8f, Font.BOLD, BaseColor.Black);
                Font fontData = new Font(Font.HELVETICA, 8f, Font.NORMAL, BaseColor.DarkGray);
                
                
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

                /*Logica para marcar las radiografias del paciente*/

                /*Logica para marcar las evoluciones del paciente*/
                       
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
                infoGeneral.AddCell(cellinfoGeneral);
                document.Add(infoGeneral);

                /*PRIMERA COLUMNA (NUMERO DE HISTORIA CLINICA)*/
                
                PdfPTable tablenumHc = new PdfPTable(1);
                float[] widths = new float[] {60f};
                tablenumHc.SetWidthPercentage(widths, rect);

                foreach (var atribute in clinicHistory){

                PdfPCell cellHeaderHCnum = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(
                    "Historia Clinica No.                                                                                         " //espacio necesario para situar el texto (no permitio por estilos)
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
                Phrase = new Phrase("Fecha de Elaboración                                                                                                                            " //espacio necesario para situar el texto (no permitio por estilos)
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

                PdfPCell cellInfoAntecedentMedical = new PdfPCell(new Phrase("3. ANTECEDENTES MEDICOS Y ODONTOLOGICOS GENERALES", fontHeader )) ;
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
                Phrase = new Phrase( idTableBackgroundMedicals[0].description, fontHeader )};
                tableBackgrounds.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInTrue , fontHeader )};
                tableBackgrounds.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInFalse, fontHeader )};
                tableBackgrounds.AddCell(cellBackgroundFalse);
               
                }

                 PdfPTable tableBackgrounds2 = new PdfPTable(3);
                tableBackgrounds2.HorizontalAlignment = 1;
                tableBackgrounds2.TotalWidth = 175f;

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
                Phrase = new Phrase( idTableBackgroundMedicals[1].description, fontHeader )};
                tableBackgrounds2.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInTrue , fontHeader )};
                tableBackgrounds2.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInFalse , fontHeader )};
                tableBackgrounds2.AddCell(cellBackgroundFalse);
        
                }
                PdfPTable tableBackgrounds3 = new PdfPTable(3);
                tableBackgrounds2.HorizontalAlignment = 1;
                tableBackgrounds2.TotalWidth = 175f;

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
                Phrase = new Phrase( idTableBackgroundMedicals[2].description, fontHeader )};
                tableBackgrounds3.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInTrue , fontHeader )};
                tableBackgrounds3.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInFalse , fontHeader )};
                tableBackgrounds3.AddCell(cellBackgroundFalse);
            
                }
                PdfPTable tableBackgrounds4 = new PdfPTable(3);
                tableBackgrounds4.HorizontalAlignment = 1;
                tableBackgrounds4.TotalWidth = 175f;

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
                Phrase = new Phrase(idTableBackgroundMedicals[3].description, fontHeader )};
                tableBackgrounds4.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInTrue , fontHeader )};
                tableBackgrounds4.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInFalse , fontHeader )};
                tableBackgrounds4.AddCell(cellBackgroundFalse);
               
                }
                PdfPTable tableBackgrounds5 = new PdfPTable(3);
                tableBackgrounds5.HorizontalAlignment = 1;
                tableBackgrounds5.TotalWidth = 175f;

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
                Phrase = new Phrase( idTableBackgroundMedicals[4].description, fontHeader )};
                tableBackgrounds5.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInTrue , fontHeader )};
                tableBackgrounds5.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInFalse , fontHeader )};
                tableBackgrounds5.AddCell(cellBackgroundFalse);
                }

                PdfPTable tableBackgrounds6 = new PdfPTable(3);
                tableBackgrounds5.HorizontalAlignment = 1;
                tableBackgrounds5.TotalWidth = 175f;

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
                Phrase = new Phrase( idTableBackgroundMedicals[5].description, fontHeader )};
                tableBackgrounds6.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInTrue , fontHeader )};
                tableBackgrounds6.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInFalse , fontHeader )};
                tableBackgrounds6.AddCell(cellBackgroundFalse);
                }
                PdfPTable tableBackgrounds7 = new PdfPTable(3);
                tableBackgrounds7.HorizontalAlignment = 1;
                tableBackgrounds7.TotalWidth = 175f;

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
                Phrase = new Phrase( idTableBackgroundMedicals[6].description, fontHeader )};
                tableBackgrounds7.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInTrue.ToString() , fontHeader )};
                tableBackgrounds7.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInFalse.ToString() , fontHeader )};
                tableBackgrounds7.AddCell(cellBackgroundFalse);
                }
                PdfPTable tableBackgrounds8 = new PdfPTable(3);
                tableBackgrounds8.HorizontalAlignment = 1;
                tableBackgrounds8.TotalWidth = 175f;

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
                Phrase = new Phrase( idTableBackgroundMedicals[7].description, fontHeader )};
                tableBackgrounds8.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInTrue , fontHeader )};
                tableBackgrounds8.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInFalse, fontHeader )};
                tableBackgrounds8.AddCell(cellBackgroundFalse);
                }
                PdfPTable tableBackgrounds9 = new PdfPTable(3);
                tableBackgrounds9.HorizontalAlignment = 1;
                tableBackgrounds9.TotalWidth = 175f;

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
                Phrase = new Phrase( idTableBackgroundMedicals[8].description, fontHeader )};
                tableBackgrounds9.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInTrue.ToString() , fontHeader )};
                tableBackgrounds9.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInFalse.ToString() , fontHeader )};
                tableBackgrounds9.AddCell(cellBackgroundFalse);
                }
                PdfPTable tableBackgrounds10 = new PdfPTable(3);
                tableBackgrounds10.HorizontalAlignment = 1;
                tableBackgrounds10.TotalWidth = 175f;

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
                Phrase = new Phrase( idTableBackgroundMedicals[9].description, fontHeader )};
                tableBackgrounds10.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInTrue.ToString() , fontHeader )};
                tableBackgrounds10.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInFalse.ToString() , fontHeader )};
                tableBackgrounds10.AddCell(cellBackgroundFalse);
                }
                PdfPTable tableBackgrounds11 = new PdfPTable(3);
                tableBackgrounds11.HorizontalAlignment = 1;
                tableBackgrounds11.TotalWidth = 175f;

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
                Phrase = new Phrase( idTableBackgroundMedicals[10].description, fontHeader )};
                tableBackgrounds11.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInTrue.ToString() , fontHeader )};
                tableBackgrounds11.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInFalse.ToString() , fontHeader )};
                tableBackgrounds11.AddCell(cellBackgroundFalse);
                }
                PdfPTable tableBackgrounds12 = new PdfPTable(3);
                tableBackgrounds12.HorizontalAlignment = 1;
                tableBackgrounds12.TotalWidth = 175f;

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
                Phrase = new Phrase( idTableBackgroundMedicals[11].description, fontHeader )};
                tableBackgrounds12.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInTrue.ToString() , fontHeader )};
                tableBackgrounds12.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInFalse.ToString() , fontHeader )};
                tableBackgrounds12.AddCell(cellBackgroundFalse);
                }
                PdfPTable tableBackgrounds13 = new PdfPTable(3);
                tableBackgrounds13.HorizontalAlignment = 1;
                tableBackgrounds13.TotalWidth = 175f;

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
                Phrase = new Phrase( idTableBackgroundMedicals[12].description, fontHeader )};
                tableBackgrounds13.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInTrue.ToString() , fontHeader )};
                tableBackgrounds13.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInFalse.ToString() , fontHeader )};
                tableBackgrounds13.AddCell(cellBackgroundFalse);
                }
                 PdfPTable tableBackgrounds14 = new PdfPTable(3);
                tableBackgrounds14.HorizontalAlignment = 1;
                tableBackgrounds14.TotalWidth = 175f;

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
                Phrase = new Phrase( idTableBackgroundMedicals[13].description, fontHeader )};
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
                document.Add(tableAntecedentMedical);
                document.Add(tableBackgrounds);
                document.Add(tableBackgrounds2);
                document.Add(tableBackgrounds3);
                document.Add(tableBackgrounds4);
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

                PdfPTable tableAntecedentOral = new PdfPTable(3);
                // tableAntecedentMedical.HorizontalAlignment = 1;
                tableAntecedentOral.TotalWidth = 50f;
                PdfPCell cellTypeAntecedentOral = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("Tipo de Antecedente" , fontHeader )};

                PdfPCell cellBackgroundObtainTrueOral = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("Si" , fontHeader )};

                 PdfPCell cellBackgroundObtainFalseOral = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase("No" , fontHeader )};
                tableAntecedentOral.AddCell(cellTypeAntecedentOral);
                tableAntecedentOral.AddCell(cellBackgroundObtainTrueOral);
                tableAntecedentOral.AddCell(cellBackgroundObtainFalseOral);

                PdfPTable tableBackgroundsOral = new PdfPTable(3);
                tableBackgroundsOral.HorizontalAlignment = 1;
                tableBackgroundsOral.TotalWidth = 175f;

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
                Phrase = new Phrase( idTableBackgroundOrals[0].description, fontHeader )};
                tableBackgroundsOral.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInTrue , fontHeader )};
                tableBackgroundsOral.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInFalse, fontHeader )};
                tableBackgroundsOral.AddCell(cellBackgroundFalse);
               
                }

                PdfPTable tableBackgroundsOral2 = new PdfPTable(3);
                tableBackgroundsOral2.HorizontalAlignment = 1;
                tableBackgroundsOral2.TotalWidth = 175f;

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
                Phrase = new Phrase( idTableBackgroundOrals[1].description, fontHeader )};
                tableBackgroundsOral2.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInTrue , fontHeader )};
                tableBackgroundsOral2.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInFalse, fontHeader )};
                tableBackgroundsOral2.AddCell(cellBackgroundFalse);
               
                }

                PdfPTable tableBackgroundsOral3 = new PdfPTable(3);
                tableBackgroundsOral3.HorizontalAlignment = 1;
                tableBackgroundsOral3.TotalWidth = 175f;

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
                Phrase = new Phrase( idTableBackgroundOrals[2].description, fontHeader )};
                tableBackgroundsOral3.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInTrue , fontHeader )};
                tableBackgroundsOral3.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInFalse, fontHeader )};
                tableBackgroundsOral3.AddCell(cellBackgroundFalse);
               
                }

                PdfPTable tableBackgroundsOral4 = new PdfPTable(3);
                tableBackgroundsOral4.HorizontalAlignment = 1;
                tableBackgroundsOral4.TotalWidth = 175f;

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
                Phrase = new Phrase( idTableBackgroundOrals[3].description, fontHeader )};
                tableBackgroundsOral4.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInTrue , fontHeader )};
                tableBackgroundsOral4.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInFalse, fontHeader )};
                tableBackgroundsOral4.AddCell(cellBackgroundFalse);
               
                }

                PdfPTable tableBackgroundsOral5 = new PdfPTable(3);
                tableBackgroundsOral5.HorizontalAlignment = 1;
                tableBackgroundsOral5.TotalWidth = 175f;

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
                Phrase = new Phrase( idTableBackgroundOrals[4].description, fontHeader )};
                tableBackgroundsOral5.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInTrue , fontHeader )};
                tableBackgroundsOral5.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInFalse, fontHeader )};
                tableBackgroundsOral5.AddCell(cellBackgroundFalse);
               
                }

                PdfPTable tableBackgroundsOral6 = new PdfPTable(3);
                tableBackgroundsOral6.HorizontalAlignment = 1;
                tableBackgroundsOral6.TotalWidth = 175f;

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
                Phrase = new Phrase( idTableBackgroundOrals[5].description, fontHeader )};
                tableBackgroundsOral6.AddCell(cellTypeBackGround);

                PdfPCell cellBackgroundTrue = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInTrue , fontHeader )};
                tableBackgroundsOral6.AddCell(cellBackgroundTrue);

                 PdfPCell cellBackgroundFalse = new PdfPCell() {CellEvent = roundRectangle, Border = PdfPCell.NO_BORDER, Padding = 3,
                Phrase = new Phrase(backgroundInFalse, fontHeader )};
                tableBackgroundsOral6.AddCell(cellBackgroundFalse);
               
                }
                /*Alineaciòn de tablas de antecedentes orales*/
                tableAntecedentOral.HorizontalAlignment = Element.ALIGN_RIGHT;
                tableBackgroundsOral.HorizontalAlignment = Element.ALIGN_RIGHT;
                tableBackgroundsOral2.HorizontalAlignment = Element.ALIGN_RIGHT;
                tableBackgroundsOral3.HorizontalAlignment = Element.ALIGN_RIGHT;
                tableBackgroundsOral4.HorizontalAlignment = Element.ALIGN_RIGHT;
                tableBackgroundsOral5.HorizontalAlignment = Element.ALIGN_RIGHT;
                tableBackgroundsOral6.HorizontalAlignment = Element.ALIGN_RIGHT;

                tableAntecedentOral.HorizontalAlignment = Element.ALIGN_TOP;
                tableBackgroundsOral.HorizontalAlignment = Element.ALIGN_BASELINE;
                tableBackgroundsOral2.HorizontalAlignment = Element.ALIGN_BASELINE;
                tableBackgroundsOral3.HorizontalAlignment = Element.ALIGN_BASELINE;
                tableBackgroundsOral4.HorizontalAlignment = Element.ALIGN_BASELINE;
                tableBackgroundsOral5.HorizontalAlignment = Element.ALIGN_BASELINE;
                tableBackgroundsOral6.HorizontalAlignment = Element.ALIGN_BASELINE;
                

                float[] widths9 = new float[] { 120f, 50f, 50f};
                tableAntecedentOral.SetWidthPercentage(widths9, rect);
                tableBackgroundsOral.SetWidthPercentage(widths9, rect);
                tableBackgroundsOral2.SetWidthPercentage(widths9, rect);
                tableBackgroundsOral3.SetWidthPercentage(widths9, rect);
                tableBackgroundsOral4.SetWidthPercentage(widths9, rect);
                tableBackgroundsOral5.SetWidthPercentage(widths9, rect);
                tableBackgroundsOral6.SetWidthPercentage(widths9, rect);

                /*CONSTRUCCION DE LAS TABLAS PARA LOS ANTECEDENTES MEDICOS*/     
                document.Add(tableAntecedentOral);
                document.Add(tableBackgroundsOral);
                document.Add(tableBackgroundsOral2);
                document.Add(tableBackgroundsOral3);
                document.Add(tableBackgroundsOral4);
                document.Add(tableBackgroundsOral5);
                document.Add(tableBackgroundsOral6);

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