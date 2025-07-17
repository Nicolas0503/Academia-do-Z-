using System;
using System.Text.RegularExpressions;

namespace Academia_do_Zé
{
    abstract class Pessoa
    {
        private string _cpf;
        private DateOnly _dataNascimento;
        private string _nomeCompleto;
        private string _telefone;
        private string _email;
        private string _senha;
        private string _foto;
       
        public string Cpf
        {
            get => _cpf;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("CPF não pode ser nulo.");
                _cpf = value;
            }
        }

        public DateOnly DataNascimento
        {
            get => _dataNascimento;
            set
            {
                
                if (value > DateOnly.FromDateTime(DateTime.Today))
                    throw new ArgumentException("Data de nascimento não pode ser no futuro.");
                if (value < DateOnly.FromDateTime(new DateTime(1900, 1, 1)))
                    throw new ArgumentException("Data de nascimento muito antiga.");
                _dataNascimento = value;
            }
        }

        public string NomeCompleto
        {
            get => _nomeCompleto;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Nome completo não pode ser vazio.");
                _nomeCompleto = value;
            }
        }

        public string Telefone
        {
            get => _telefone;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Telefone inválido.");
                _telefone = value;
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || !Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    throw new ArgumentException("Email inválido.");
                _email = value;
            }
        }

        public string Senha
        {
            get => _senha;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length < 6)
                    throw new ArgumentException("Senha deve ter pelo menos 6 caracteres.");
                _senha = value;
            }
        }

        public string Foto
        {
            get => _foto;
            set => _foto = value; // Adicione validação se necessário
        }

        


        public Pessoa(string cpf, DateOnly dataNascimento, string nomeCompleto, string telefone, string email, string senha, string foto)
        {
            Cpf = cpf;
            DataNascimento = dataNascimento;
            NomeCompleto = nomeCompleto;
            Telefone = telefone;
            Email = email;
            Senha = senha;
            Foto = foto;
            
            
        }
    }
}
