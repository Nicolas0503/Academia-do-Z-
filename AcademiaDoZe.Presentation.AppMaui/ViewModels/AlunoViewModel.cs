using System;
using System.Linq;
using System.Threading.Tasks;
using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Interfaces;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;

namespace AcademiaDoZe.Presentation.AppMaui.ViewModels
{
    [QueryProperty(nameof(AlunoId), "Id")]
    public partial class AlunoViewModel : BaseViewModel
    {
        private readonly IAlunoService _alunoService;
        private readonly ILogradouroService _logradouroService;
        private AlunoDTO _aluno = new()
        {
            Nome = string.Empty,
            Cpf = string.Empty,
            DataNascimento = DateOnly.FromDateTime(DateTime.Today.AddYears(-18)),
            Telefone = string.Empty,
            Endereco = new LogradouroDTO { Cep = string.Empty, Nome = string.Empty, Bairro = string.Empty, Cidade = string.Empty, Estado = string.Empty, Pais = string.Empty },
            Numero = string.Empty
        };
        public AlunoDTO Aluno
        {
            get => _aluno;
            set => SetProperty(ref _aluno, value);
        }
        private int _alunoId;
        public int AlunoId
        {
            get => _alunoId;
            set => SetProperty(ref _alunoId, value);
        }
        private bool _isEditMode;
        public bool IsEditMode
        {
            get => _isEditMode;
            set => SetProperty(ref _isEditMode, value);
        }

        public AlunoViewModel(IAlunoService alunoService, ILogradouroService logradouroService)
        {
            _alunoService = alunoService;
            _logradouroService = logradouroService;
            Title = "Detalhes do Aluno";
        }

        [RelayCommand]
        private async Task CancelAsync()
        {
            await Shell.Current.GoToAsync("..");
        }

        public async Task InitializeAsync()
        {
            if (AlunoId > 0)
            {
                IsEditMode = true;
                Title = "Editar Aluno";
                await LoadAlunoAsync();
            }
            else
            {
                IsEditMode = false;
                Title = "Novo Aluno";
            }
        }

        [RelayCommand]
        public async Task LoadAlunoAsync()
        {
            if (AlunoId <= 0) return;
            try
            {
                IsBusy = true;
                var data = await _alunoService.ObterPorIdAsync(AlunoId);
                if (data != null) Aluno = data;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Erro ao carregar aluno: {ex.Message}", "OK");
            }
            finally { IsBusy = false; }
        }

        [RelayCommand]
        public async Task SaveAlunoAsync()
        {
            if (IsBusy) return;
            if (!ValidateAluno(Aluno)) return;
            try
            {
                IsBusy = true;
                var logradouroData = await _logradouroService.ObterPorCepAsync(Aluno.Endereco.Cep);
                if (logradouroData == null)
                {
                    await Shell.Current.DisplayAlert("Erro", "O CEP informado não existe. O cadastro não pode continuar.", "OK");
                    return;
                }
                Aluno.Endereco = logradouroData;
                if (IsEditMode)
                {
                    await _alunoService.AtualizarAsync(Aluno);
                    await Shell.Current.DisplayAlert("Sucesso", "Aluno atualizado com sucesso!", "OK");
                }
                else
                {
                    await _alunoService.AdicionarAsync(Aluno);
                    await Shell.Current.DisplayAlert("Sucesso", "Aluno criado com sucesso!", "OK");
                }
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Erro ao salvar aluno: {ex.Message}", "OK");
            }
            finally { IsBusy = false; }
        }

        [RelayCommand]
        public async Task SearchByCpfAsync()
        {
            if (string.IsNullOrWhiteSpace(Aluno.Cpf)) return;
            try
            {
                IsBusy = true;
                var cpfNormalized = new string(Aluno.Cpf.Where(char.IsDigit).ToArray());
                var resultado = await _alunoService.ObterPorCpfAsync(cpfNormalized);
                if (resultado == null)
                {
                    await Shell.Current.DisplayAlert("Aviso", "CPF não encontrado.", "OK");
                    return;
                }
                Aluno = resultado;
                IsEditMode = true;
                await Shell.Current.DisplayAlert("Aviso", "Aluno encontrado: dados carregados para edição.", "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Erro ao buscar CPF: {ex.Message}", "OK");
            }
            finally { IsBusy = false; }
        }

        [RelayCommand]
        public async Task SearchByCepAsync()
        {
            if (string.IsNullOrWhiteSpace(Aluno.Endereco.Cep)) return;
            try
            {
                IsBusy = true;
                var logradouroData = await _logradouroService.ObterPorCepAsync(Aluno.Endereco.Cep);
                if (logradouroData != null)
                {
                    Aluno.Endereco = logradouroData;
                    OnPropertyChanged(nameof(Aluno));
                    await Shell.Current.DisplayAlert("Aviso", "CEP encontrado! Endereço preenchido automaticamente.", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Aviso", "CEP não encontrado.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Erro ao buscar CEP: {ex.Message}", "OK");
            }
            finally { IsBusy = false; }
        }

        public static bool ValidateAluno(AlunoDTO aluno)
        {
            const string validationTitle = "Validação";
            if (string.IsNullOrWhiteSpace(aluno.Nome))
            {
                Shell.Current.DisplayAlert(validationTitle, "Nome é obrigatório.", "OK");
                return false;
            }
            if (string.IsNullOrWhiteSpace(aluno.Cpf) || aluno.Cpf.Length != 11)
            {
                Shell.Current.DisplayAlert(validationTitle, "CPF deve ter 11 dígitos.", "OK");
                return false;
            }
            if (aluno.DataNascimento == default)
            {
                Shell.Current.DisplayAlert(validationTitle, "Data de nascimento é obrigatória.", "OK");
                return false;
            }
            if (string.IsNullOrWhiteSpace(aluno.Telefone) || aluno.Telefone.Length != 11)
            {
                Shell.Current.DisplayAlert(validationTitle, "Telefone deve ter 11 dígitos.", "OK");
                return false;
            }
            if (aluno.Endereco == null)
            {
                Shell.Current.DisplayAlert(validationTitle, "Endereço é obrigatório.", "OK");
                return false;
            }
            return true;
        }
    }
}
