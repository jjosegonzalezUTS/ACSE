using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_ACSE
{
    public class EmailService
    {
        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly int _smtpPort = 587;
        private readonly string _smtpUser = "acsemaui@gmail.com";
        private readonly string _smtpPass = "myqm edwo xrpy xdzm";

        async Task SendEmailAsync(string subject, string body, string toEmail = "acsemaui@gmail.com")
        {
            var fromAddress = new MailAddress(_smtpUser, "ACSE");
            var toAddress = new MailAddress(toEmail, "Recipient");

            using (var smtp = new SmtpClient
            {
                Host = _smtpServer,
                Port = _smtpPort,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUser, _smtpPass)
            })
            {
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
        }

        public async Task SendNewCaseEmailAsync(Case newCase, User user)
        {
            string subject = $"Se creó un nuevo caso #{newCase.Id}";

            // Construir el cuerpo del correo
            var bodyBuilder = new StringBuilder();
            bodyBuilder.AppendLine($"El usuario {CheckNullOrEmpty(user.Name)} proporcionó la siguiente información:");

            bodyBuilder.AppendLine($"\nID del Caso: {CheckNullOrEmpty(newCase.Id.ToString())}");
            bodyBuilder.AppendLine($"Fecha: {CheckNullOrEmpty(newCase.Date)}");
            bodyBuilder.AppendLine($"Hora: {CheckNullOrEmpty(newCase.Time)}");
            bodyBuilder.AppendLine($"Ubicación: {CheckNullOrEmpty(newCase.Location)}");
            bodyBuilder.AppendLine($"Personas Involucradas: {CheckNullOrEmpty(newCase.PersonsInvolved)}");
            bodyBuilder.AppendLine($"Descripción: {CheckNullOrEmpty(newCase.Description)}");
            bodyBuilder.AppendLine($"Evidencía: {(newCase.Evidence != null && newCase.Evidence.Length > 0 ? "Se registró archivo multimedia." : "No se proporcionó información.")}");
            bodyBuilder.AppendLine($"Tipo: {CheckNullOrEmpty(newCase.Type)}");
            bodyBuilder.AppendLine($"Reporte Anónimo: {newCase.AnonymousReport}");
            bodyBuilder.AppendLine($"ID del Usuario: {newCase.UserId}");
            bodyBuilder.AppendLine($"Fecha de Registro: {CheckNullOrEmpty(newCase.RegistrationDate)}");
            bodyBuilder.AppendLine($"Hora de Registro: {CheckNullOrEmpty(newCase.RegistrationTime)}");

            string body = bodyBuilder.ToString();

            await SendEmailAsync(subject, body);
        }

        public async Task SendCaseInformationRequestEmailAsync(Case caseDetails, User user)
        {
            var subject = $"Solicitud de información caso #{caseDetails.Id}";
            var body = $"El usuario {user.Name} con el ID #{user.Id} y el correo {user.Email}, solicita información del proceso en el caso {caseDetails.Id} registrado el {caseDetails.RegistrationDate} a las {caseDetails.RegistrationTime}.";
            await SendEmailAsync(subject, body);
        }

        public async Task SendUpdateCaseEmailAsync(Case caseDetails, User user)
        {
            var subject = $"El caso #{caseDetails.Id} ha sido actualizado";

            // Separar las notas por comas y crear una lista con viñetas
            var notesList = caseDetails.notes.Split(',')
                .Select(note => $"- {note.Trim()}")
                .ToList();
            var notesFormatted = string.Join("\n", notesList);

            var body = $"Se ha actualizado la información referente al caso #{caseDetails.Id} con la siguiente información:\n\nNOTAS:\n{notesFormatted}";
            await SendEmailAsync(subject, body, user.Email);
        }

        public async Task SendResolveCaseEmailAsync(Case caseDetails, User user)
        {
            var subject = $"El caso #{caseDetails.Id} ha sido finalizado";

            var currentDate = DateTime.Now.ToString("dd/MM/yyyy");
            var currentTime = DateTime.Now.ToString("HH:mm");

            var body = $"El caso #{caseDetails.Id} ha sido finalizado el {currentDate}, a las {currentTime}.";
            await SendEmailAsync(subject, body, user.Email);
        }

        public async Task SendCaseDeletionEmailAsync(Case deletedCase, User user)
        {
            string subject = $"Eliminación del caso #{deletedCase.Id}";

            var bodyBuilder = new StringBuilder();
            bodyBuilder.AppendLine($"El usuario {CheckNullOrEmpty(user.Name)} ha eliminado el caso #{deletedCase.Id} el día {DateTime.Now.ToShortDateString()} a las {DateTime.Now.ToShortTimeString()}.");

            string body = bodyBuilder.ToString();

            await SendEmailAsync(subject, body);
        }

        string CheckNullOrEmpty(string value)
        {
            return string.IsNullOrEmpty(value) ? "No se proporcionó información." : value;
        }
    }
}