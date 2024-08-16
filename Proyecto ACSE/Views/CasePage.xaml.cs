using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Maui.Controls;

namespace Proyecto_ACSE.Views
{
    [QueryProperty(nameof(CaseId), "caseId")]
    [QueryProperty(nameof(IsViewMode), "isViewMode")]
    public partial class CasePage : ContentPage
    {
        private readonly LocalDBService _localDBService;
        private readonly EmailService _emailService;
        private readonly List<Entry> _personEntries;
        private readonly List<Editor> _notesEntries;
        private byte[] _evidenceImageBytes;
        int UserId => UserSessionService.Instance.CurrentUser.Id;
        bool IsAdmin => UserSessionService.Instance.CurrentUser.Role == User.UserRole.Admin;

        public int CaseId { get; set; }
        public bool IsViewMode { get; set; }

        public CasePage()
        {
            InitializeComponent();
            _localDBService = new LocalDBService();
            _emailService = new EmailService();
            _personEntries = new List<Entry>();
            _notesEntries = new List<Editor>();
            BindingContext = this; // Para poder usar propiedades ligadas en XAML
            UpdatePersonsInvolvedStackLayout();
            UpdateNotesStackLayout();

            // Evento para ocultar el mensaje de error de descripción cuando se ingrese texto
            DescriptionEntry.TextChanged += (sender, e) => DescriptionErrorLabel.IsVisible = string.IsNullOrEmpty(e.NewTextValue);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            Title = IsViewMode ? $"Caso #{CaseId}" : "Nuevo Caso";
            if (IsViewMode) SetModeAndLoadData(CaseId);
            else
            {
                SelectUserIDGrid.IsVisible = IsAdmin;
                ToolbarItems.Remove(EmailToolbarItem);
            }
        }

        public async void SetModeAndLoadData(int? caseId)
        {
            if (IsViewMode && caseId.HasValue)
            {
                var caseData = await _localDBService.CaseService.GetCaseById(caseId.Value);
                var userData = await _localDBService.UserService.GetUserById(caseData.UserId);
                if (caseData != null && userData != null)
                {
                    UserIDLabel.Text = CheckNullOrEmpty(userData.Id.ToString());
                    UserNameLabel.Text = CheckNullOrEmpty(userData.Name);
                    RegistrationDateLabel.Text = CheckNullOrEmpty(caseData.RegistrationDate);
                    RegistrationTimeLabel.Text = CheckNullOrEmpty(caseData.RegistrationTime);

                    DatePicker.Date = DateTime.Parse(caseData.Date);
                    TimePicker.Time = TimeSpan.Parse(caseData.Time);
                    LocationEntry.Text = CheckNullOrEmpty(caseData.Location);

                    if (!string.IsNullOrEmpty(caseData.PersonsInvolved))
                    {
                        string[] personsArray = caseData.PersonsInvolved.Split(',');
                        List<string> personsInvolved = personsArray.ToList();
                        PersonsInvolvedEntry.Text = personsInvolved[0];
                        CreatePersonsInvolvedEntries(personsInvolved);
                    }
                    else
                        PersonsInvolvedEntry.Text = CheckNullOrEmpty(string.Join(", ", caseData.PersonsInvolved));

                    DescriptionEntry.Text = CheckNullOrEmpty(caseData.Description);
                    TypePicker.SelectedItem = caseData.Type;
                    AnonymousReportCheckBox.IsChecked = caseData.AnonymousReport;

                    if (!string.IsNullOrEmpty(caseData.notes))
                    {
                        NotesGrid.IsVisible = true;
                        string[] notesArray = caseData.notes.Split(',');
                        List<string> notesInvolved = notesArray.ToList();
                        NotesEntry.Text = notesInvolved[0];
                        CreateNotesEditors(notesInvolved);
                    }

                    ResolveCaseCheckBox.IsChecked = caseData.Resolved;

                    SaveCaseButton.Text = "ACTUALIZAR";

                    if (caseData.Evidence != null && caseData.Evidence.Length > 0)
                    {
                        EvidenceImage.Source = ImageSource.FromStream(() => new MemoryStream(caseData.Evidence));
                        _evidenceImageBytes = caseData.Evidence;
                        EvidenceImage.IsVisible = true;
                    }
                    else
                    {
                        NoEvidenceLabel.IsVisible = !IsAdmin;
                    }

                    ChargeImageBtn.IsVisible = IsAdmin;

                    if (IsAdmin)
                    {
                        ToolbarItems.Remove(EmailToolbarItem);
                        UserIDGrid.IsVisible = true;
                        UserNameGrid.IsVisible = true;
                        RegistrationDateGrid.IsVisible = true;
                        RegistrationTimeGrid.IsVisible = true;
                        AnonymousReportCheckBox.IsEnabled = false;
                        NotesGrid.IsVisible = true;
                        ResolveCaseGrid.IsVisible = true;

                        return;
                    }

                    // Disable interactables
                    DatePicker.IsEnabled = false;
                    TimePicker.IsEnabled = false;
                    LocationEntry.IsEnabled = false;
                    PersonsInvolvedEntry.IsEnabled = false;
                    DescriptionEntry.IsEnabled = false;
                    TypePicker.IsEnabled = false;
                    AnonymousReportCheckBox.IsEnabled = false;
                    SaveCaseButton.IsVisible = false;
                    AddPersonButton.IsVisible = false;
                    RemovePersonButton.IsVisible = false;
                    ChargeImageBtn.IsVisible = false;
                    NotesEntry.IsEnabled = false;
                    AddNoteButton.IsVisible = false;
                    RemoveNoteButton.IsVisible = false;
                }
            }
        }


        private void OnDateSelected(object sender, DateChangedEventArgs e)
        {
            DateErrorLabel.IsVisible = DatePicker.Date > DateTime.Now;
        }

        private void OnTypeSelected(object sender, EventArgs e)
        {
            TypeErrorLabel.IsVisible = TypePicker.SelectedIndex == -1;
        }

        private async void OnSaveCaseClicked(object sender, EventArgs e)
        {
            try
            {
                var userIDTemp = UserId;

                // Validar que la fecha no sea mayor a la fecha actual
                if (DatePicker.Date > DateTime.Now)
                {
                    DateErrorLabel.IsVisible = true;
                }

                // Validar que se seleccione un tipo
                if (TypePicker.SelectedIndex == -1)
                {
                    TypeErrorLabel.IsVisible = true;
                }

                // Validar que la descripción no esté vacía
                DescriptionErrorLabel.IsVisible = string.IsNullOrEmpty(DescriptionEntry.Text);

                if (IsAdmin && !IsViewMode)
                {
                    if (string.IsNullOrEmpty(SelectUserIDEntry.Text))
                    {
                        SelectUserIDErrorLabel.IsVisible = true;
                        SelectUserIDErrorLabel.Text = "Se debe asignar un usuario al caso.";
                    }

                    if (!string.IsNullOrEmpty(SelectUserIDEntry.Text))
                    {
                        var userSelected = await _localDBService.UserService.GetUserById(Int32.Parse(SelectUserIDEntry.Text));

                        if (userSelected == null)
                        {
                            SelectUserIDErrorLabel.IsVisible = true;
                            SelectUserIDErrorLabel.Text = "El usuario seleccionado no existe.";
                        }

                        if (userSelected != null)
                        {
                            if (userSelected.Role == User.UserRole.Admin)
                            {
                                SelectUserIDErrorLabel.IsVisible = true;
                                SelectUserIDErrorLabel.Text = "El usuario no puede ser un administrador.";
                            }
                        }

                        userIDTemp = Int32.Parse(SelectUserIDEntry.Text);
                    }
                }

                if (DateErrorLabel.IsVisible || TypeErrorLabel.IsVisible || DescriptionErrorLabel.IsVisible || SelectUserIDErrorLabel.IsVisible)
                {
                    await DisplayAlert("Error", "El formulario tiene errores.", "Aceptar");
                    return;
                }

                // Si el reporte es anónimo, mostrar el mensaje de confirmación
                if (AnonymousReportCheckBox.IsChecked)
                {
                    bool isConfirmed = await DisplayAlert("Confirmación",
                        "Al crear un reporte anónimo no se ligará el caso a su usuario y por ende no lo podrá revisar luego. ¿Desea continuar?",
                        "Aceptar", "Cancelar");

                    if (!isConfirmed)
                    {
                        return; // No continuar con la creación del caso
                    }

                    var anonymousUser = await _localDBService.UserService.GetUserByName("AnonymousReports");
                    if (anonymousUser != null)
                    {
                        userIDTemp = anonymousUser.Id;
                    }
                }

                var personsInvolved = new List<string> { PersonsInvolvedEntry.Text };
                personsInvolved.AddRange(_personEntries.Select(entry => entry.Text));

                var newCase = new Case
                {
                    Date = DatePicker.Date.ToString("yyyy-MM-dd"),
                    Time = TimePicker.Time.ToString(@"hh\:mm"),
                    Location = LocationEntry.Text,
                    PersonsInvolved = string.Join(",", personsInvolved),
                    Description = DescriptionEntry.Text,
                    Evidence = _evidenceImageBytes, // Guardar la imagen como byte array
                    Type = TypePicker.SelectedItem?.ToString(), // Obtener el valor seleccionado del Picker
                    AnonymousReport = AnonymousReportCheckBox.IsChecked,
                    UserId = userIDTemp,
                    RegistrationDate = DateTime.Now.ToString("yyyy-MM-dd"),
                    RegistrationTime = DateTime.Now.ToString("HH:mm:ss"),
                    notes = "",
                    InProgress = false,
                    Resolved = false
                };

                if (IsAdmin && IsViewMode)
                {
                    var anonymousUser = await _localDBService.UserService.GetUserByName("AnonymousReports");
                    var existingCase = await _localDBService.CaseService.GetCaseById(CaseId);
                    var notes = new List<string> { NotesEntry.Text };
                    notes.AddRange(_notesEntries.Select(entry => entry.Text));

                    //For Email conditions
                    var stringNotes = string.Join(",", notes);
                    var currentNotes = existingCase.notes;
                    var currentResolved = existingCase.Resolved;

                    existingCase.Date = newCase.Date;
                    existingCase.Time = newCase.Time;
                    existingCase.Location = newCase.Location;
                    existingCase.PersonsInvolved = newCase.PersonsInvolved;
                    existingCase.Description = newCase.Description;
                    existingCase.Evidence = newCase.Evidence;
                    existingCase.Type = newCase.Type;
                    existingCase.AnonymousReport = newCase.AnonymousReport;
                    existingCase.UserId = existingCase.AnonymousReport ? anonymousUser.Id : existingCase.UserId;
                    existingCase.notes = stringNotes;
                    existingCase.InProgress = !string.IsNullOrWhiteSpace(NotesEntry.Text) && !ResolveCaseCheckBox.IsChecked;
                    existingCase.Resolved = ResolveCaseCheckBox.IsChecked;

                    if (stringNotes != currentNotes)
                    {
                        SendUpdateCaseEmail(existingCase);
                    }

                    if (currentResolved != existingCase.Resolved && existingCase.Resolved) 
                    {
                        SendResolveCaseEmail(existingCase);
                    }

                    await _localDBService.CaseService.UpdateCase(existingCase);
                    await DisplayAlert("Éxito", "Caso actualizado exitosamente.", "Aceptar");
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await _localDBService.CaseService.AddCase(newCase);

                    var user = await _localDBService.UserService.GetUserById(userIDTemp);

                    SaveCaseButton.IsVisible = false;
                    activityIndicator.IsRunning = true;
                    activityIndicator.IsVisible = true;
                    Shell.SetNavBarIsVisible(this, false);

                    await _emailService.SendNewCaseEmailAsync(newCase, user);

                    activityIndicator.IsRunning = false;
                    activityIndicator.IsVisible = false;
                    SaveCaseButton.IsVisible = true;
                    Shell.SetNavBarIsVisible(this, true);
                    await DisplayAlert("Éxito", "Caso guardado exitosamente.", "Aceptar");
                    await Shell.Current.GoToAsync("..");
                }
            }
            catch (Exception ex) 
            {
                await DisplayAlert("Error", $"Ocurrió un error {ex.Message}, al guardar el caso o enviar el correo.", "Aceptar");
            }
            finally
            {
                activityIndicator.IsRunning = false;
                activityIndicator.IsVisible = false;
                SaveCaseButton.IsVisible = true;
            }
        }

        private async void OnUploadImageClicked(object sender, EventArgs e)
        {
            var pickOptions = new PickOptions
            {
                PickerTitle = "Please select an image",
                FileTypes = FilePickerFileType.Images
            };

            var result = await FilePicker.PickAsync(pickOptions);
            if (result != null)
            {
                var stream = await result.OpenReadAsync();
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                _evidenceImageBytes = memoryStream.ToArray();

                EvidenceImage.Source = ImageSource.FromStream(() => new MemoryStream(_evidenceImageBytes));
                EvidenceImage.IsVisible = true;
            }
        }

        private void OnAddPersonClicked(object sender, EventArgs e)
        {
            var newPersonEntry = new Entry
            {
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            _personEntries.Add(newPersonEntry);
            PersonsInvolvedContainer.Children.Add(newPersonEntry);
            UpdatePersonsInvolvedStackLayout();
        }

        private void OnRemovePersonClicked(object sender, EventArgs e)
        {
            if (_personEntries.Any())
            {
                var lastPersonEntry = _personEntries.Last();
                _personEntries.Remove(lastPersonEntry);
                PersonsInvolvedContainer.Children.Remove(lastPersonEntry);
                UpdatePersonsInvolvedStackLayout();
            }
        }

        private void CreatePersonsInvolvedEntries(List<string> personsInvolved)
        {
            PersonsInvolvedContainer.Children.Clear();
            _personEntries.Clear();

            for (int i = 1; i < personsInvolved.Count; i++)
            {
                var newPersonEntry = new Entry
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Text = personsInvolved[i],
                    IsEnabled = IsAdmin
                };

                _personEntries.Add(newPersonEntry);
                PersonsInvolvedContainer.Children.Add(newPersonEntry);
            }

            UpdatePersonsInvolvedStackLayout();
        }

        private void OnAddNoteClicked(object sender, EventArgs e)
        {
            var newNoteEditor = new Editor
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                AutoSize = EditorAutoSizeOption.TextChanges
            };
            _notesEntries.Add(newNoteEditor);
            NotesContainer.Children.Add(newNoteEditor);
            UpdateNotesStackLayout();
        }

        private void OnRemoveNoteClicked(object sender, EventArgs e)
        {
            if (_notesEntries.Any())
            {
                var lastNoteEntry = _notesEntries.Last();
                _notesEntries.Remove(lastNoteEntry);
                NotesContainer.Children.Remove(lastNoteEntry);
                UpdateNotesStackLayout();
            }
        }

        private void CreateNotesEditors(List<string> notes)
        {
            NotesContainer.Children.Clear();
            _notesEntries.Clear();

            for (int i = 1; i < notes.Count; i++)
            {
                var newNoteEditor = new Editor
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Text = notes[i],
                    IsEnabled = IsAdmin,
                    AutoSize = EditorAutoSizeOption.TextChanges
                };

                _notesEntries.Add(newNoteEditor);
                NotesContainer.Children.Add(newNoteEditor);
            }

            UpdateNotesStackLayout();
        }

        private void UpdatePersonsInvolvedStackLayout()
        {
            RemovePersonButton.IsVisible = _personEntries.Any();
        }

        void UpdateNotesStackLayout()
        {
            RemoveNoteButton.IsVisible = _notesEntries.Any();
        }

        protected override bool OnBackButtonPressed()
        {
            // Si el indicador de actividad está visible, bloquear el retroceso
            if (activityIndicator.IsVisible || EmailActivityIndicator.IsVisible)
            {
                return true;
            }

            return base.OnBackButtonPressed();
        }

        private async void OnEmailToolbarItemClicked(object sender, EventArgs e)
        {
            string lastEmailSentKey = $"LastEmailSent_{CaseId}";
            DateTime lastEmailSentDate = Preferences.Get(lastEmailSentKey, DateTime.MinValue);

            if (lastEmailSentDate.Date == DateTime.Now.Date)
            {
                await DisplayAlert("Aviso", "Ya has enviado un correo electrónico hoy para este caso.", "Aceptar");
                return;
            }

            bool answer = await DisplayAlert("Enviar Correo", "¿Quieres enviar un correo para pedir información respecto al caso?", "Aceptar", "Cancelar");
            if (answer)
            {
                try
                {
                    EmailToolbarItem.IsEnabled = false;
                    LoadingOverlay.IsVisible = true;
                    EmailActivityIndicator.IsVisible = true;
                    EmailActivityIndicator.IsRunning = true;
                    Shell.SetNavBarIsVisible(this, false);

                    var caseDetails = await _localDBService.CaseService.GetCaseById(CaseId);
                    var user = await _localDBService.UserService.GetUserById(caseDetails.UserId);

                    await _emailService.SendCaseInformationRequestEmailAsync(caseDetails, user);

                    EmailToolbarItem.IsEnabled = true;
                    LoadingOverlay.IsVisible = false;
                    EmailActivityIndicator.IsVisible = false;
                    EmailActivityIndicator.IsRunning = false;
                    Shell.SetNavBarIsVisible(this, true);
                    Preferences.Set(lastEmailSentKey, DateTime.Now);

                    await DisplayAlert("Éxito", "Correo enviado exitosamente.", "Aceptar");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Ocurrió un error {ex.Message} al enviar el correo.", "Aceptar");
                }
                finally
                {
                    EmailToolbarItem.IsEnabled = true;
                    LoadingOverlay.IsVisible = false;
                    EmailActivityIndicator.IsVisible = false;
                    EmailActivityIndicator.IsRunning = false;
                    Shell.SetNavBarIsVisible(this, true);
                }
            }
        }

        private async void SendUpdateCaseEmail(Case caseUpdate)
        {
            try
            {
                var user = await _localDBService.UserService.GetUserById(caseUpdate.UserId);
                await _emailService.SendUpdateCaseEmailAsync(caseUpdate, user);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error {ex.Message} al enviar el correo.", "Aceptar");
            }
        }

        private async void SendResolveCaseEmail(Case caseUpdate)
        {
            try
            {
                var user = await _localDBService.UserService.GetUserById(caseUpdate.UserId);
                await _emailService.SendResolveCaseEmailAsync(caseUpdate, user);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error {ex.Message} al enviar el correo.", "Aceptar");
            }
        }

        private void OnSelectUserIDEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            SelectUserIDErrorLabel.IsVisible = false;
        }

        string CheckNullOrEmpty(string value)
        {
            return string.IsNullOrEmpty(value) ? "No se proporcionó información." : value;
        }
    }
}