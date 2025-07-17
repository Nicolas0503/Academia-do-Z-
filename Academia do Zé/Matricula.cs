using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academia_do_Zé
{
    internal class Matricula
    {
        private string _nome;
        private string _plano;
        private DateOnly _dataInicio;
        private DateOnly _dataFim;
        private string _restricoes;
        private string _laudoMedico;
        private string _observacoes;
        private Aluno _aluno;

        public string Nome
        {
            get => _nome;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Nome não pode ser nulo ou vazio.");
                _nome = value;
            }
        }

        public string Plano
        {
            get => _plano;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Plano não pode ser nulo ou vazio.");
                _plano = value;
            }
        }

        public DateOnly DataInicio
        {
            get => _dataInicio;
            set
            {
                if (value > DateOnly.FromDateTime(DateTime.Today))
                    throw new ArgumentException("Data de início não pode ser no futuro.");
                _dataInicio = value;
            }
        }

        public DateOnly DataFim
        {
            get => _dataFim;
            set
            {
                if (value < _dataInicio)
                    throw new ArgumentException("Data de fim não pode ser anterior à data de início.");
                _dataFim = value;
            }
        }

        public string Restricoes
        {
            get => _restricoes;
            set => _restricoes = value;
        }

        public string LaudoMedico
        {
            get => _laudoMedico;
            set => _laudoMedico = value;
        }

        public string Observacoes
        {
            get => _observacoes;
            set => _observacoes = value;
        }

        public Aluno Aluno
        {
            get => _aluno;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(Aluno), "Aluno não pode ser nulo.");
                _aluno = value;
            }
        }

        public Matricula(
            string nome,
            string plano,
            DateOnly dataInicio,
            DateOnly dataFim,
            string restricoes,
            string laudoMedico,
            string observacoes,
            Aluno aluno)
        {
            Nome = nome;
            Plano = plano;
            DataInicio = dataInicio;
            DataFim = dataFim;
            Restricoes = restricoes;
            Observacoes = observacoes;
            Aluno = aluno;

            // Cálculo da idade do aluno
            int idade = DateTime.Today.Year - aluno.DataNascimento.Year;
            if (aluno.DataNascimento > DateOnly.FromDateTime(DateTime.Today.AddYears(-idade)))
                idade--;

            // Validação do laudo médico para alunos de 12 a 16 anos
            if (idade >= 12 && idade <= 16 && string.IsNullOrWhiteSpace(laudoMedico))
                throw new ArgumentException("Alunos de 12 a 16 anos devem apresentar um laudo médico.");

            LaudoMedico = laudoMedico;
        }
    }
}
