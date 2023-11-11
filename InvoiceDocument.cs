using System;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.PortableExecutable;

using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDF.ExampleInvoice
{
    public class InvoiceDocument : IDocument
    {
        public static Image LogoImage { get; } = Image.FromFile("logo.png");
        public static Image LogoCertified { get; } = Image.FromFile("logocertified.jpg");
        public static Image Signature { get; } = Image.FromFile("signature.jpg");

        public InvoiceModel Model { get; }

        public InvoiceDocument(InvoiceModel model)
        {
            Model = model;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container

                .Page(page =>
                {
                    page.Margin(25);

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(ComposeContent);

                    page.Footer().Element(container =>
                    {
                        container.Row(row =>
                        {
                            row.RelativeItem(3).Column(column =>
                            {
                                column.Item().AlignCenter().Text($@" L'employeur / De Werkgever").FontSize(8).SemiBold();
                                column.Item().AlignCenter().Image(Signature);
                            });
                            row.RelativeItem(4).Column(column =>
                            {
                                column.Item().AlignCenter().Text($@"L'employé / De Werknemer").FontSize(8).SemiBold();
                            });
                            row.RelativeItem(5).Column(column =>
                            {
                                column.Item().AlignCenter().Text($@"Responsable sur event / Eventverantwoordelijk  Signature + Nom / Handeteking + Naam").FontSize(8).SemiBold();
                            });
                            row.RelativeItem(3).Column(column =>
                            {
                                column.Item().AlignCenter().Image(LogoCertified);
                            });
                        });

                    });

                })
                .Page(page =>
                {
                    page.Margin(25);
                    page.MarginTop(50);
                    page.Content().Element(container =>
                    {
                        container.Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();

                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("CONDITIONS GENERAL DE TRAVAIL").FontSize(10).Bold();
                                header.Cell().Text(" ALGEMENE WERKVOORWAARDEN").FontSize(10).Bold();

                            });

                            table.Cell().Text("Le travailleur s engage à lire attentivement le règlement de travail dont il a reçu un exemplaire lors de son premier engagement et lors de chaque modification, et engage à en respecter les conditions.").FontSize(8);
                            table.Cell().Text("De werknemer verbindt er zich toe het arbeidsreglement grondig te lezen. Er wordt hem hiervan een exemplaar verschaft op het ogenblik van zijn eerste aanwerving en bij elke wijziging van het arbeidsreglement. Hij verbindt er zich toe de voorwaarden ervan na te leven").FontSize(8);
                            table.Cell().Text("Le travailleur s engage à respecter scrupuleusement l horaire assigné. Tout retard pourra être sanctionné par une perte de rémunération correspondant au temps perdu au cas ou celui-ci ne serait pas justifié.").FontSize(8);
                            table.Cell().Text("De werknemer verbindt er zich toe het aangegeven uurrooster nauwgezet te volgen. Elke laattijdigheid kan bestraft worden met een loonverlies in verhouding met de verloren tijd, voorzover deze laattijdigheid niet gerechtvaardigd is.").FontSize(8);
                            table.Cell().Text(" Le travailleur s à l engage à consacrer tout son temps dans le cadre de l horaire prévu, exercice de sa mission. Tout absence de son poste pourra être sanctionné au prorata du temps perdu.").FontSize(8);
                            table.Cell().Text("De werknemer verbindt er zich toe zijn tijd binnen de perken van het voorziene uurrooster te besteden aan de volbrenging van zijn taak. Elke afwezigheid op zijn werk kan bestraft worden in verhouding met de verloren tijd.").FontSize(8);
                            table.Cell().Text(" Le travailleur s engage à respecter strictement toutes les instructions de travail contenue dans le présent contrat (dates, heures, lieux, dépenses, etc En cas de défaillance de la personne, celle-ci s engage à ne réclamer des rémunérations que pour des prestations effectivement prestées.").FontSize(8);
                            table.Cell().Text("De werknemer verbindt er zich toe alle werkinstructies van dit kontrakt strikt te respecteren (data, uren, plaatsen, uitgaven, e.d.). In geval van het in gebreke blijven van de werknemer, verbindt deze er zich toe enkel de wedde te eisen voor de effectief gepresteerde uren").FontSize(8);
                            table.Cell().Text(" La rémunération est fixée sur la base suivante : montant brut à déduire les cotisations du travailleur à l ONSS et éventuellement le précompte professionnel.").FontSize(8);
                            table.Cell().Text("De bezoldiging wordt vastgesteld op grond van volgende gegevens: Het bruto loon waarop de werknemersbijdragen voor de sociale zekerheid en de bedrijfsvoorheffing in mindering worden gebracht").FontSize(8);
                            table.Cell().Text("Le travailleur s engage à prévenir immédiatement et au plus tard 48 heures avant le début de la prestation l employeur du fait susceptible d présent contrat de travail.").FontSize(8);
                            table.Cell().Text(" De werknemer verbindt er zich toe de werkgever onmiddellijk en ten minste 48 uren voor de prestatie op de hoogte te brengen in geval van afwezigheid, zoniet kan dit de schorsing van deze overeenkomst tot gevolg hebben.").FontSize(8);
                            table.Cell().Text("Le travailleur s'engage à renvoyer à l employer la fiche de prestation dûment complétée et signée dés la fin de ses prestations.").FontSize(8);
                            table.Cell().Text(" De werknemer verbindt er zich toe de volledige en ingevulde en ondertekende prestatiefiche terug te sturen naar de werkgever na de beëindiging van de prestatie.").FontSize(8);
                            table.Cell().Text(" Le travailleur s où l engage à avoir une tenue soignée, seule compatible avec un travail on est en contact constant avec le public.").FontSize(8);
                            table.Cell().Text(" De werknemer verbindt er zich toe een verzorgd voorkomen te hebben, hetgeen noodzakelijk is wanneer men voortdurend in kontakt is met het publiek.").FontSize(8);
                            table.Cell().Text(" Le travailleur accepte de prendre l entière responsabilité de la tenue et/ou du matériel qui lui est confié et accepte d en rembourser la valeur en cas de perte, de vol ou de détérioration par sa faute grave. Le travailleur s engage à restituer la tenue et/ou le matériel confié endéans les 3 jours suivant la fin de sa prestation.").FontSize(8);
                            table.Cell().Text(" De werknemer verklaart de volle verantwoordelijkheid te dragen voor de kledij en/of het materiaal dat hem wordt toevertrouwd en aanvaardt de waarde te vergoeden in geval van diefstal, verlies of beschadiging door zijn eigen fout. De werknemer verbindt er zich toe binnen 3 dagen na het einde van de prestatie de kledij en/of het materiaal terug te brengen").FontSize(8);
                            table.Cell().Text(" Le travailleur s abstiendra tant au cours du contrat qu application de l après sa cessation, en article 17 de la loi relative au contrat de travail : - de divulguer les secrets d affaires ou de toutes affaires à caractère personnel ou confidentiel dont il aurait eu connaissance dans l exercice de son activité professionnelle;- de se livrer ou de coopérer à toute acte e concurrence déloyale").FontSize(8);
                            table.Cell().Text("Zowal tijdens de duur van de overeenkomst als na de beëindiging ervan, zal de werknemer, overeenkomstig artikel 17 van de wet over de arbeidsovereenkomst, zich ervan onthouden: - zakengeheimen te onthullen of zaken bekend te maken, die van persoonlijke of vertrouwelijke aard zijn en waarvan hij tijdens zijn professionele aktiviteit kennis zou gekregen hebben.- Elke daad van oneerlijke mededinging of medewerking aan zodanige handelingen te stellen").FontSize(8);
                            table.Cell().Text(" Tout travail effectué pour le compte d qu un client direct tant au cours du contrat après la cessation de celui-ci (max 2 ans) sera considéré comme un acte de concurrence déloyale.").FontSize(8);
                            table.Cell().Text(" Alle arbeid voor rekening van een rechtstreekse klant tijdens de duur van het kontrakt of na de beëindiging daarvan (max. 2 jaar) zal beschouwd worden als een daad van oneerlijke concurrentie").FontSize(8);
                            table.Cell().Text(" Le travailleur s engage à respecter le règlement de travail du lieu de travail où il doit exercer sa mission.").FontSize(8);
                            table.Cell().Text(" De werknemer verbindt er zich toe het arbeidsreglement te eerbiedigen dat van kracht is op de plaats waar hij zijn arbeidstaak volbrengt").FontSize(8);
                            table.Cell().Text(" Le travailleur s engage à ne réclamer aucun jour férié, ni aucun pécule de vacances complémentaire").FontSize(8);
                            table.Cell().Text("De werknemer verbindt er zich toe noch enige feestdag, noch aanvullend vakantiegeld te eisen").FontSize(8);


                        });
                    });
                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.CurrentPageNumber();
                        text.Span(" / ");
                        text.TotalPages();
                    });
                });
        }

        void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem(7).DebugArea().Column(column =>
                {
                    column.Item().MinHeight(15);
                    column
                        .Item().Text($"CONTRAT DE TRAVAIL À DUREE DÉTERMINÉE")
                        .FontSize(11).SemiBold();
                    column
                        .Item().Text($"CONTRAT DE TRAVAIL À DUREE DÉTERMINÉE")
                        .FontSize(11).SemiBold();
                    column.Item().MinHeight(15);
                    column.Item()
                        .Text($@"Exemplaire à renvoyer signé sous 48h après la prestation à UPANDUPS sous réserve de retard de paiement et de non remboursement des frais  de déplacement.")
                        .FontSize(8).SemiBold();
                    column.Item().MinHeight(15);
                    column.Item()
                         .Text($@"Exemplaire à renvoyer signé sous 48h après la prestation à UPANDUPS sous réserve de retard de paiement et de non remboursement des frais  de déplacement.")
                         .FontSize(8).SemiBold();
                });

                row.RelativeItem(2).DebugArea("Logo").Image(LogoImage);
            });
        }

        void ComposeContent(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Element(ComposeTable);
                    column.Item().Element(container =>
                    {

                        container.Row(row =>
                        {
                            row.RelativeItem(8).Column(column =>
                            {
                                column.Spacing(5);
                                column.Item().Text($@"L'employeur engage le travailleur pour une durée determinée.").FontSize(8).SemiBold();
                                column.Item().Text($@"De werkgever neemt de werknemen in dienst voor een bepaalde duur.").FontSize(8).SemiBold();
                                column.Item().Text(text =>
                                {
                                    text.Span("En qualité de: / In functie van : ").FontSize(8).SemiBold();
                                    text.Span("Hotesse").FontSize(8).SemiBold();
                                });

                                column.Item().Text(text =>
                                {
                                    text.Span("Montant de la rémunération: / Bedrag van het loon: ").FontSize(8).SemiBold();
                                    text.Span("10.2499 €").FontSize(8).SemiBold();
                                });
                            });

                            row.RelativeItem(6).Column(column =>
                            {
                                column.Spacing(5);
                                column.Item().Text(text =>
                                    {
                                        text.Span("du/van: ").FontSize(8).SemiBold();
                                        text.Span("01/11/2023").FontSize(8).SemiBold();
                                    });
                                column.Item().Text(text =>
                                {
                                    text.Span("au/tot: ").FontSize(8).SemiBold();
                                    text.Span("01/11/2023").FontSize(8).SemiBold();
                                });
                                column.Item().Text(text =>
                                {
                                    text.Span("Comm: ").FontSize(8).SemiBold();
                                    text.Span("Par. N°:/ Par. Com. N°: 200- Employé").FontSize(8).SemiBold();
                                });

                            });
                        });
                    });
                    column.Item().Row(row =>
                    {
                        row.RelativeItem().PaddingTop(5).Column(column2 =>
                        {
                            column2.Spacing(2);
                            column2.Item().Text("IL A ÉTÉ CONVENU ET ACCEPTÉ CE QUI SUIT:").FontSize(8).Bold();
                            column2.Item().Text("Article 1 : Objet du contrat le droit d affecter l L employeur engage l employé(e) à d employé(e), qui accepte, au qualité mentionnés ci-dessus. L autres\r\n fonctions compatibles avec ses capacités, sans que cette modification ne soit constitutive d une modification substantielle du contrat.").FontSize(4);
                            column2.Item().Text("Article 2 : Le présent contrat est conclu pour la prestation de services aux dates mentionnés ci-dessus et aux heures et endroit décrits à la rubrique WORK\r\n INFORMATIONS, sauf contrordre ou avis de changement confirmé par écrit ou fax.").FontSize(4);
                            column2.Item().Text("Article 3 : La rémunération brute horaire est celle mentionné ci-dessus et sera payée sur le compte bancaire de l ouvrable après la fin du mois de la\r\n prestation.").FontSize(4);
                            column2.Item().Text("Article 4 : Les frais de déplacement sont remboursés à concurrence de 0.20€/km  à l unique personne du chauffeur et calculé, sauf contrordre, sur base d'un\r\n aller/retour du domicile l employé(e) à son lieu de travail.").FontSize(4);    
                            column2.Item().Text("Article 5 : L employé(e) reconnaît avoir pris note des conditions générales de travail détaillées au verso, avoir reçu et pris connaissance du règlement de\r\n travail et en accepter les clauses et conditions").FontSize(4);
                            column2.Item().Text("Tout travail effectué pour le compte d un client direct tant au cours du contrat qu après la cessation de celui-ci (max 2 ans) sera considéré comme un acte de\r\n concurrence déloyale.").FontSize(4);

                        });
                        row.RelativeItem().PaddingTop(5).Column(column2 =>
                        {
                            column2.Spacing(2);
                            column2.Item().Text("DE OVEREENKOMST LUIDT ALS VOLGT:").FontSize(8).Bold();
                            column2.Item().Text("Artikel 1 : Contractbeschrijving : De werkgever neemt de werknemer aan, die aanvaardt, in de bovenstaande hoedanigheid. De werkgever behoudt zich het recht om de werknemer voor andere funkties te bestemmen die verenigbaar zijn met zijn/haar bekwaamheden en dit zonder dat deze wijziging dit kontrakt in hoofdzaak zou wijzigen.").FontSize(4);
                            column2.Item().Text(" Artikel 2 : Onderhavig kontrakt wordt afgesloten voor de prestatie op bovenstaande datum en uren en plaatsen beschreven in de rubriek WORK INFORMATIONS, uitgezonderd schriftelijk tegenbericht van wijziging of per fax bevestigt.").FontSize(4);
                            column2.Item().Text("Artikel 3 : De bruto bezoldiging per uur is hierboven vermeld en zal aan de werknemer worden uitbetaald op zijn bankrekening 10 werkdagen na het einde van de maand van de prestatie.").FontSize(4);
                            column2.Item().Text(" Artikel 4 : Verplaatsingkosten worden terugbetaald tegen 0.20€ /km enkel en alleen aan de chauffeur, en berekend uitgezonderd tegenbericht, op basis van een verplaatsing van de woonplaats van de werknemer tot de hieronder voorziene arbeidsplaats.").FontSize(4);
                            column2.Item().Text(" Artikel 5 : De werknemer erkent nota te hebben genomen van de algemene voorwaarden gedetailleerd op de achterzijde, en kennis te hebben van het arbeidsreglement en het te hebben ontvange en onze voorwaarden en condities te aanvaarden.").FontSize(4);
                            column2.Item().Text("Alle arbeid voor rekening van een rechtstreekse klant tijdens de duur van het kontrakt of na de beëindiging daarvan (max. 2 jaar) zal beschouwd worden als een daad van oneerlijke concurrentie").FontSize(4);

                        });

                    });
                   
                    column.Item().PaddingTop(5).Element(ComposeWorkInfo);

                    column.Item().Element(container =>
                    {
                        container.Border(1).Text("Fiche de prestation: à compléter par le travailleur après sa prestation / Werkfiche: door de werknemer in te vullen na de prestatie").Bold().FontSize(8);
                    });

                    column.Item().Element(container =>
                    {
                        
                        var textStyle = TextStyle.Default.SemiBold().FontSize(6);
                        container
                        .Border(1)
                        .Table(table =>
                        {
                            
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();

                            });

                            
                            table.Header(header =>
                            {
                                header.Cell().AlignCenter().Text("Jours / Dagen").Style(textStyle);
                                header.Cell().Border(1).AlignCenter().Text("Mercredi / Woensdag").Style(textStyle);
                                header.Cell().Border(1).AlignCenter().Text("Jeudi / Donderdag").Style(textStyle);
                                header.Cell().Border(1).AlignCenter().Text("Vendredi / Vrijdag").Style(textStyle);
                                header.Cell().Border(1).AlignCenter().Text("Samedi / Zaterdag").Style(textStyle );
                                header.Cell().Border(1).AlignCenter().Text("Dimanche / Zondag").Style(textStyle);
                                header.Cell().Border(1).AlignCenter().Text("Lundi / Maandag").Style(textStyle);
                                header.Cell().Border(1).AlignCenter().Text("Mardi / Dinsdag").Style(textStyle);
                            });
                            
                            table.Cell().Row(1).Border(1).Text("UpandUps  Sprl").Style(textStyle);
                            table.Cell().Row(2).Column(1).Border(1).Text("Date / Datum").Style(textStyle);
                            table.Cell().Row(3).Column(1).Border(1).Text("Heure de début prestation / Beginuur prestatie").Style(textStyle);
                            table.Cell().Row(4).Column(1).Border(1).Text("Heure de fin de prestation / Einduur prestatie").Style(textStyle);
                            table.Cell().Row(5).Column(1).Border(1).Text("Frais divers, de transport (ou nbrs de kms) avec justificatif/ Diverse onkosten, vervoerkosten (of aantal kms) met bewijsstuk").Style(textStyle);



                        });
                    });
                    column.Item().PaddingTop(5).Text(text =>
                    {
                        
                        text.Span("FAIT EN DOUBLE EXEMPLAIRE A NEUPRE LE: / IN DUBBEL OPGEMAAKT TE NEUPRE OP:    ").FontSize(8).Bold();
                        text.Span(" 01/11/2023").FontSize(8).Bold();
                    });
                });

            });



        }

        void ComposeTable(IContainer container)
        {
            var headerStyle = TextStyle.Default.SemiBold().FontSize(8);

            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();

                });

                table.Header(header =>
                {
                    header.Cell().Border(1).Column(column =>
                    {
                        column.Item().Text("Entre - Tussen").FontSize(8);
                        column.Item().Text("La partie ci-après nommée « l  employeur » / Enerzijds « de werkgever ").FontSize(8);
                    });
                    header.Cell().Border(1).Column(column =>
                    {

                        column.Item().Text(" Et - En").FontSize(8);
                        column.Item().Text("La partie dénommée « l employé(e) » / Anderzijds « de werknemer »").FontSize(8);
                    });

                });

                table.Cell().Border(1).Column(column =>
                {

                    column.Item().Text("UpandUps  Sprl").FontSize(8).Bold();
                    column.Item().Text("Drève du bois de Neuville, 23 4121 Neupré (Liège) - Belgique").FontSize(8);
                    column.Item().Text("T +32 (0)494 14 06 78 // www.upandups.be").FontSize(8);
                });
                table.Cell().Border(1).Column(column =>
                {

                    column.Item().Text("shulga oksana").FontSize(8).Bold();
                    column.Item().Text("Drève du bois de Neuville, 23 4121 Neupré (Liège) - Belgique").FontSize(8);
                    column.Item().Text("T +32 (0)494 14 06 78 // www.upandups.be").FontSize(8);
                });

            });
        }

        void ComposeWorkInfo(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Spacing(2);
                    column.Item().Padding(5).PaddingLeft(0).Text("Work informations:").FontSize(11).Underline();
                    column.Item().Text(text =>
                    {

                        text.Span("Customer:  ").FontSize(8);
                        text.Span("Customer").FontSize(8);
                    });
                    column.Item().Text(text =>
                    {

                        text.Span("Address:  ").FontSize(8);
                        text.Span(" asdfsd 3798 asdfsa").FontSize(8);
                    });
                    column.Item().Text(text =>
                    {

                        text.Span("Timing:  ").FontSize(8);
                        text.Span(" 01/11 08:00-18:00 ").FontSize(8);
                    });
                    column.Item().Text(text =>
                    {

                        text.Span("Pause:  ").FontSize(8);
                        text.Span("au moment creu, une à la foi ").FontSize(8);
                    });
                });
                row.RelativeItem().Column(column =>
                {
                    column.Spacing(5);
                    column.Item().Text("");
                    column.Item().Text(text =>
                    {

                        text.Span("Event:  ").FontSize(8);
                        text.Span("asadf").FontSize(8);
                    });
                });
            });
            //container.ShowEntire().Padding(10).Column(column =>
            //{
            //    column.Item().Text(" Work informations:").FontSize(11).Underline();
            //    column.Item().Text(text =>
            //    {

            //        text.Span("Customer:  ");
            //        text.Span("Customer");
            //    });
            //    column.Item().Text(Model.Comments);
            //});

        }
    }

    public class AddressComponent : IComponent
    {
        private string Title { get; }
        private Address Address { get; }

        public AddressComponent(string title, Address address)
        {
            Title = title;
            Address = address;
        }

        public void Compose(IContainer container)
        {
            container.ShowEntire().Column(column =>
            {
                column.Spacing(2);

                column.Item().Text(Title).SemiBold();
                column.Item().PaddingBottom(5).LineHorizontal(1);

                column.Item().Text(Address.CompanyName);
                column.Item().Text(Address.Street);
                column.Item().Text($"{Address.City}, {Address.State}");
                column.Item().Text(Address.Email);
                column.Item().Text(Address.Phone);
            });
        }
    }
}