using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Interfaces;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Dispatching;
using System.ComponentModel;

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
        public ObservableCollection<AlunoDTO> Alunos
        {
            get => _alunos;
            set => SetProperty(ref _alunos, value);
        }
        public AlunoListViewModel(IAlunoService alunoService)
        {
            _alunoService = alunoService;
            Title = "Alunos";
        }

        [RelayCommand]
        private async Task AddAlunoAsync()
        {
            await Shell.Current.GoToAsync("aluno");
        }

        [RelayCommand]
        private async Task EditAlunoAsync(AlunoDTO aluno)
        {
            if (aluno == null) return;
            await Shell.Current.GoToAsync($"aluno?Id={aluno.Id}");
        }

        [RelayCommand]
        private async Task RefreshAsync()
        {
            IsRefreshing = true;
            await LoadAlunosAsync();
        }

        [RelayCommand]
        private async Task SearchAlunosAsync()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                await Microsoft.Maui.ApplicationModel.MainThread.InvokeOnMainThreadAsync(() => Alunos.Clear());
                IEnumerable<AlunoDTO> resultados = Enumerable.Empty<AlunoDTO>();
                if (string.IsNullOrWhiteSpace(SearchText))
                {
                    resultados = await _alunoService.ObterTodosAsync() ?? Enumerable.Empty<AlunoDTO>();
                }
                else if (SelectedFilterType == "Id" && int.TryParse(SearchText, out int id))
                {
                    var aluno = await _alunoService.ObterPorIdAsync(id);
                    if (aluno != null) resultados = new[] { aluno };
                }
                else if (SelectedFilterType == "CPF")
                {
                    var aluno = await _alunoService.ObterPorCpfAsync(SearchText);
                    if (aluno != null) resultados = new[] { aluno };
                }
                await Microsoft.Maui.ApplicationModel.MainThread.InvokeOnMainThreadAsync(() =>
                {
                    foreach (var item in resultados)
                        Alunos.Add(item);
                    OnPropertyChanged(nameof(Alunos));
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
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                await Microsoft.Maui.ApplicationModel.MainThread.InvokeOnMainThreadAsync(() => { Alunos.Clear(); OnPropertyChanged(nameof(Alunos)); });
                var list = await _alunoService.ObterTodosAsync();
                if (list != null)
                {
                    await Microsoft.Maui.ApplicationModel.MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        foreach (var a in list) Alunos.Add(a);
                        OnPropertyChanged(nameof(Alunos));
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
        private async Task DeleteAlunoAsync(AlunoDTO aluno)
        {
            if (aluno == null) return;
            var confirm = await Shell.Current.DisplayAlert("Confirmar Exclusão", $"Deseja realmente excluir o aluno {aluno.Nome}?", "Sim", "Não");
            if (!confirm) return;
            try
            {
                IsBusy = true;
                var success = await _alunoService.RemoverAsync(aluno.Id);
                if (success)
                {
                    Alunos.Remove(aluno);
                    await Shell.Current.DisplayAlert("Sucesso", "Aluno excluído com sucesso!", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Erro", "Não foi possível excluir o aluno.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Erro ao excluir aluno: {ex.Message}", "OK");
            }
            finally { IsBusy = false; }
        }
    }
}
