using Academia_do_Zé.Entities;
using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Interfaces;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
namespace AcademiaDoZe.Presentation.AppMaui.ViewModels
{
    public partial class AlunoListViewModel : BaseViewModel
    {
        public ObservableCollection<string> FilterTypes { get; } = new() { "Id", "CPF" };
        private readonly IAlunoService _alunoService;
        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }
        private string _selectedFilterType = "CPF";
        public string SelectedFilterType
        {
            get => _selectedFilterType;
            set => SetProperty(ref _selectedFilterType, value);
        }
        private ObservableCollection<AlunoDTO> _alunos = new();
        public ObservableCollection<AlunoDTO> alunos
        {
            get => _alunos;
            set => SetProperty(ref _alunos, value);
        }
        private AlunoDTO? _selectedAluno;
        public AlunoDTO? SelectedAluno
        {
            get => _selectedAluno;
            set => SetProperty(ref _selectedAluno, value);
        }
        public AlunoListViewModel(IAlunoService AlunoService)
        {
            _alunoService = AlunoService;
            Title = "alunos";
        }
        // métodos de comando


        [RelayCommand]
        private async Task AddAlunoAsync()
        {
            try
            {
                await Shell.Current.GoToAsync("aluno");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Erro ao navegar para tela de cadastro: {ex.Message}", "OK");
            }
        }
        [RelayCommand]
        private async Task EditAlunoAsync(AlunoDTO aluno)
        {
            try
            {
                if (aluno == null)
                    return;
                await Shell.Current.GoToAsync($"aluno?Id={aluno.Id}");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Erro ao navegar para tela de edição: {ex.Message}", "OK");
            }
        }
        [RelayCommand]
        private async Task RefreshAsync()
        {
            IsRefreshing = true;
            await LoadAlunosAsync();
        }

        [RelayCommand]
        private async Task SearchalunosAsync()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                // Limpa a lista atual

                await MainThread.InvokeOnMainThreadAsync(() =>

                {
                    alunos.Clear();
                });
                IEnumerable<AlunoDTO> resultados = Enumerable.Empty<AlunoDTO>();
                // Busca os alunos de acordo com o filtro
                if (string.IsNullOrWhiteSpace(SearchText))

                {
                    resultados = await _alunoService.ObterTodosAsync() ?? Enumerable.Empty<AlunoDTO>();
                }
                else if (SelectedFilterType == "Id" && int.TryParse(SearchText, out int id))
                {
                    var aluno = await _alunoService.ObterPorIdAsync(id);

                    if (aluno != null)

                        resultados = new[] { aluno };
                }
                else if (SelectedFilterType == "CPF")
                {
                    // ObterPorCpfAsync agora retorna IEnumerable<AlunoDTO>

                    var alunos = await _alunoService.ObterPorCpfAsync(SearchText) ?? Enumerable.Empty<AlunoDTO>();

                    resultados = alunos;
                }
                // Atualiza a coleção na thread principal

                await MainThread.InvokeOnMainThreadAsync(() =>

                {
                    foreach (var item in resultados)
                    {
                        alunos.Add(item);
                    }
                    OnPropertyChanged(nameof(alunos));
                });
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Erro ao buscar alunos: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task LoadAlunosAsync()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                // Limpa a lista atual antes de carregar novos dados
                await MainThread.InvokeOnMainThreadAsync(() =>

                {
                    alunos.Clear();
                    OnPropertyChanged(nameof(alunos));
                });
                var alunosList = await _alunoService.ObterTodosAsync();
                if (alunosList != null)
                {
                    // Garantir que a atualização da UI aconteça na thread principal

                    await MainThread.InvokeOnMainThreadAsync(() =>

                    {
                        foreach (var Aluno in alunosList)
                        {
                            alunos.Add(Aluno);
                        }
                        OnPropertyChanged(nameof(alunos));
                    });
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Erro ao carregar alunos: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }
        }

        [RelayCommand]
        private async Task DeleteAlunoAsync(AlunoDTO Aluno)
        {
            if (Aluno == null)
                return;
            bool confirm = await Shell.Current.DisplayAlert(
            "Confirmar Exclusão",

            $"Deseja realmente excluir o Aluno {Aluno.Nome}?",
            "Sim", "Não");
            if (!confirm)
                return;
            try
            {
                IsBusy = true;
                bool success = await _alunoService.RemoverAsync(Aluno.Id);
                if (success)
                {
                    alunos.Remove(Aluno);
                    await Shell.Current.DisplayAlert("Sucesso", "Aluno excluído com sucesso!", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Erro", "Não foi possível excluir o Aluno.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Erro ao excluir Aluno: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
        [RelayCommand]
        public async Task SearchByCpfAsync(AlunoDTO Aluno)
        {
            if (string.IsNullOrWhiteSpace(Aluno.Cpf))
                return;
            try
            {
                IsBusy = true;
                // normaliza para apenas dígitos (o repositório espera dígitos)

                var cpfNormalized = new string(Aluno.Cpf.Where(char.IsDigit).ToArray());

                var resultados = (await _alunoService.ObterPorCpfAsync(cpfNormalized))?.ToList() ?? new List<AlunoDTO>();
                if (!resultados.Any())
                {
                    await Shell.Current.DisplayAlert("Aviso", "CPF não encontrado.", "OK"); return;
                }
                if (resultados.Count == 1)
                {
                    Aluno = resultados.First();
                    await Shell.Current.DisplayAlert("Aviso", "Aluno já cadastrado! Dados carregados para edição.", "OK"); return;
                }
                // múltiplos resultados -> perguntar ao usuário qual selecionar

                var options = resultados.Select(c => $"{c.Id} - {c.Nome} ({c.Cpf})").ToArray();

                var escolha = await Shell.Current.DisplayActionSheet("Vários alunos encontrados", "Cancelar", null, options);
                if (string.IsNullOrWhiteSpace(escolha) || escolha == "Cancelar")
                    return;
                // extrai ID a partir da string selecionada ("{Id} - ...")
                var idStr = escolha.Split('-', 2).FirstOrDefault()?.Trim();
                if (int.TryParse(idStr, out var selId))

                {
                    var selecionado = resultados.FirstOrDefault(c => c.Id == selId);

                    if (selecionado != null)

                    {
                        Aluno = selecionado;
                        await Shell.Current.DisplayAlert("Aviso", "Aluno selecionado: dados carregados para edição.", "OK");
                    }
                }
            }
            catch (Exception ex) { await Shell.Current.DisplayAlert("Erro", $"Erro ao buscar CPF: {ex.Message}", "OK"); }
            finally { IsBusy = false; }
        }
    }
}