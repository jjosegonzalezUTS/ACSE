using Microsoft.Maui.ApplicationModel.Communication;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Proyecto_ACSE.Views
{
    [QueryProperty(nameof(UserId), "userId")]
    [QueryProperty(nameof(IsEditMode), "isEditMode")]
    public partial class RegisterPage : ContentPage
    {
        private static readonly Style ErrorEntryStyle = new Style(typeof(Entry))
        {
            Setters = {
                new Setter { Property = Entry.TextColorProperty, Value = Colors.Red },
                new Setter { Property = Label.TextColorProperty, Value = Colors.Red },
            }
        };
        private static readonly Style NormalEntryStyle = new Style(typeof(Entry))
        {
            Setters = {
                new Setter { Property = Entry.TextColorProperty, Value = Colors.Black },
                new Setter { Property = Label.TextColorProperty, Value = Colors.Black }
            }
        };

        LocalDBService _localDBService;
        User _userToEdit;
        bool _isAdmin = false;
        bool _isAnonymousReports = false;

        public int UserId { get; set; }
        public bool IsEditMode { get; set; }

        public RegisterPage()
        {
            InitializeComponent();
            _localDBService = new LocalDBService();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var currentUserOnSesion = UserSessionService.Instance.CurrentUser;
            if (currentUserOnSesion != null)
            {
                _isAdmin = UserSessionService.Instance.CurrentUser.Role == User.UserRole.Admin;
                if (_isAdmin && !IsEditMode) RolePicker.SelectedIndex = 0;
            }

            if (IsEditMode)
            {
                _userToEdit = await _localDBService.UserService.GetUserById(UserId);
                _isAnonymousReports = _userToEdit.Name == "AnonymousReports";
                RolePicker.SelectedIndex = _userToEdit.Role == User.UserRole.Admin ? 1 : 0;

                nameEntryField.Text = _userToEdit.Name;
                emailEntryField.Text = _userToEdit.Email;
                passwordEntryField.Text = _userToEdit.Password;

                nameEntryField.IsEnabled = !_userToEdit.Anonymous;
                nameEntryField.IsEnabled = !_isAnonymousReports;
                emailEntryField.IsEnabled = !_isAnonymousReports;
                passwordEntryField.IsEnabled = !_isAnonymousReports;
            }

            RoleStack.IsVisible = _isAdmin;
            RolePicker.IsEnabled = !IsEditMode;

            saveButton.Text = IsEditMode ? "ACTUALIZAR" : "REGISTRAR";
            FormTitleLabel.Text = IsEditMode ? $"ACTUALIZAR USUARIO #{UserId}" : "REGISTRO";
        }

        private async void OnSaveUser(object sender, EventArgs e)
        {
            bool isValid = ValidateEntries();

            if (!isValid)
            {
                return;
            }

            if (await ValidateFieldsAsync())
            {
                var userName = string.IsNullOrWhiteSpace(nameEntryField.Text) ? "Anonimo" : nameEntryField.Text;
                var isAnonymous = string.IsNullOrWhiteSpace(nameEntryField.Text);
                var role = User.UserRole.User;

                if (_isAdmin) 
                {
                    role = RolePicker.SelectedIndex == 1 ? User.UserRole.Admin : User.UserRole.User;
                }

                var newUser = new User
                {
                    Name = userName,
                    Email = emailEntryField.Text,
                    Password = passwordEntryField.Text,
                    Role = role,
                    Anonymous = isAnonymous
                };

                if (IsEditMode)
                {
                    var existingUser = await _localDBService.UserService.GetUserById(UserId);
                    existingUser.Name = userName;
                    existingUser.Email = emailEntryField.Text;
                    existingUser.Password = passwordEntryField.Text;
                    await _localDBService.UserService.UpdateUser(existingUser);
                }
                else
                {
                    await _localDBService.UserService.AddUser(newUser);
                }

                await DisplayAlert("Éxito", "El usuario ha sido registrado con éxito.", "Aceptar");
                await Navigation.PopAsync();
            }
        }

        bool ValidateEntries()
        {
            bool isEmailValid = !string.IsNullOrWhiteSpace(emailEntryField.Text);
            bool isPasswordValid = !string.IsNullOrWhiteSpace(passwordEntryField.Text);
            return isEmailValid && isPasswordValid;
        }

        private async Task<bool> ValidateFieldsAsync()
        {
            bool isNameValid = ValidateName();
            bool isEmailValid = await ValidateEmailAsync();
            bool isPasswordValid = ValidatePassword();

            return isNameValid && isEmailValid && isPasswordValid;
        }

        private bool ValidateName()
        {
            string name = nameEntryField.Text?.Trim();
            bool isValid = true;

            if (_isAdmin)
            {
                if (RolePicker.SelectedIndex == 1)
                {
                    if (string.IsNullOrEmpty(name))
                    {
                        nameErrorLabel.Text = "Un usuario administrador debe contener la palabra admin.";
                        isValid = false;
                    }
                    else
                    {
                        if (!ContainsAdminKeywords(name))
                        {
                            nameErrorLabel.Text = "Un usuario administrador debe contener la palabra admin.";
                            isValid = false;
                        }

                        if (name.Contains("AnonymousReports"))
                        {
                            nameErrorLabel.Text = "No se permiten palabras claves.";
                            isValid = false;
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        nameErrorLabel.IsVisible = false;
                        return isValid;
                    }

                    if (ContainsForbiddenKeywords(name))
                    {
                        nameErrorLabel.Text = "No se permiten palabras claves.";
                        isValid = false;
                    }
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(name)) return isValid;

                if (ContainsForbiddenKeywords(name))
                {
                    nameErrorLabel.Text = "No se permiten palabras claves.";
                    isValid = false;
                }
            }
            
            if (_isAnonymousReports) isValid = true;

            nameErrorLabel.IsVisible = !isValid;
            nameEntryField.Style = isValid ? NormalEntryStyle : ErrorEntryStyle;
            return isValid;
        }

        private async Task<bool> ValidateEmailAsync()
        {
            string email = emailEntryField.Text?.Trim();
            bool isValid = false;

            if (_isAdmin)
            {
                if (RolePicker.SelectedIndex == 1)
                {
                    isValid = true;

                    if (IsEditMode)
                    {
                        if (_userToEdit != null && _userToEdit.Email != email)
                        {
                            if (email != null && await _localDBService.UserService.IsEmailRegisteredAsync(email))
                            {
                                emailErrorLabel.Text = "El correo electrónico ya está registrado.";
                                isValid = false;
                            }
                        }
                    }
                    else
                    {
                        if (email != null && await _localDBService.UserService.IsEmailRegisteredAsync(email))
                        {
                            emailErrorLabel.Text = "El correo electrónico ya está registrado.";
                            isValid = false;
                        }
                    }

                    if (email != null && !ContainsAdminKeywords(email))
                    {
                        emailErrorLabel.Text = "Un usuario administrador debe contener la palabra admin.";
                        isValid = false;
                    }

                    if (email != null && email.Contains("AnonymousReports"))
                    {
                        nameErrorLabel.Text = "No se permiten palabras claves.";
                        isValid = false;
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(email))
                    {
                        emailErrorLabel.Text = "El correo electrónico es obligatorio.";
                    }
                    else if (ContainsForbiddenKeywords(email))
                    {
                        emailErrorLabel.Text = "No se permiten palabras claves.";
                    }
                    else if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    {
                        emailErrorLabel.Text = "El correo electrónico no es válido.";
                    }
                    else if (await _localDBService.UserService.IsEmailRegisteredAsync(email))
                    {
                        emailErrorLabel.Text = "El correo electrónico ya está registrado.";

                        if (IsEditMode)
                        {
                            if (_userToEdit.Email == email)
                            {
                                isValid = true;
                            }
                        }
                    }
                    else
                    {
                        isValid = true;
                    }
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    emailErrorLabel.Text = "El correo electrónico es obligatorio.";
                }
                else if (ContainsForbiddenKeywords(email))
                {
                    emailErrorLabel.Text = "No se permiten palabras claves.";
                }
                else if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    emailErrorLabel.Text = "El correo electrónico no es válido.";
                }
                else if (await _localDBService.UserService.IsEmailRegisteredAsync(email))
                {
                    emailErrorLabel.Text = "El correo electrónico ya está registrado.";
                }
                else
                {
                    isValid = true;
                }
            }

            if (_isAnonymousReports) isValid = true;

            emailErrorLabel.IsVisible = !isValid;
            emailEntryField.Style = isValid ? NormalEntryStyle : ErrorEntryStyle;
            return isValid;
        }

        private bool ValidatePassword()
        {
            string password = passwordEntryField.Text ?? string.Empty;
            bool isLowerUpperRuleMet = Regex.IsMatch(password, @"(?=.*[a-z])(?=.*[A-Z])");
            bool isNumberRuleMet = Regex.IsMatch(password, @"(?=.*\d)");
            bool isSpecialCharRuleMet = Regex.IsMatch(password, @"(?=.*[!@#$%*\.])");
            bool isMinLengthRuleMet = password.Length >= 8;

            if (_isAdmin && RolePicker.SelectedIndex == 1)
            {
                var adminValid = true;
                if (String.IsNullOrEmpty(password))
                {
                    adminValid = false;
                }

                passwordErrorLabel.Text = adminValid ? string.Empty : "Tu contraseña no cumple con los criterios de seguridad.\nVuelve a intentarlo.";
                passwordErrorLabel.IsVisible = !adminValid;
                passwordEntryField.Style = adminValid ? NormalEntryStyle : ErrorEntryStyle;

                return adminValid;
            }

            if (_isAnonymousReports) return true;

            lowerUpperRule.Style = isLowerUpperRuleMet ? Resources["PasswordRuleStyleMet"] as Style : Resources["PasswordRuleStyle"] as Style;
            numberRule.Style = isNumberRuleMet ? Resources["PasswordRuleStyleMet"] as Style : Resources["PasswordRuleStyle"] as Style;
            specialCharRule.Style = isSpecialCharRuleMet ? Resources["PasswordRuleStyleMet"] as Style : Resources["PasswordRuleStyle"] as Style;
            minLengthRule.Style = isMinLengthRuleMet ? Resources["PasswordRuleStyleMet"] as Style : Resources["PasswordRuleStyle"] as Style;

            bool isValid = isLowerUpperRuleMet && isNumberRuleMet && isSpecialCharRuleMet && isMinLengthRuleMet;
            passwordErrorLabel.Text = isValid ? string.Empty : "Tu contraseña no cumple con los criterios de seguridad.\nVuelve a intentarlo.";
            passwordErrorLabel.IsVisible = !isValid;
            passwordEntryField.Style = isValid ? NormalEntryStyle : ErrorEntryStyle;
            return isValid;
        }

        private bool ContainsForbiddenKeywords(string input)
        {
            string[] forbiddenKeywords = { "admin", "anonymous" };
            return forbiddenKeywords.Any(keyword => input.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private bool ContainsAdminKeywords(string input)
        {
            string[] forbiddenKeywords = { "admin"};
            return forbiddenKeywords.Any(keyword => input.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void OnNameEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateName();
        }

        private async void OnEmailEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            await ValidateEmailAsync();
        }

        private void OnPasswordEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidatePassword();
        }

        private async void OnRoleSelected(object sender, EventArgs e)
        {
            ValidateName();

            if (!string.IsNullOrWhiteSpace(emailEntryField.Text))
            {
                await ValidateEmailAsync();
            }

            if (!string.IsNullOrWhiteSpace(passwordEntryField.Text))
            {
                ValidatePassword();
            }
        }

        private async void OnShowRegistrationRules(object sender, EventArgs e)
        {
            await DisplayAlert("Registro", "Si optas por no proporcionar tu nombre y dejas el campo correspondiente vacío, el sistema autocompletará dicho campo con 'Anónimo' para completar el registro.", "Aceptar");
        }
    }
}
