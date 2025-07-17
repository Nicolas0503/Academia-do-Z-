using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academia_do_Zé
{
    internal class Colaborador : Pessoa
    {
        private string _tipo { get; set; }
        private DateOnly _datadeEmissao { get; set; }
        private string _vinculo { get; set; }

        public string Tipo
        {
            get => _tipo;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Tipo não pode ser nulo.");
                _tipo = value;
            }
        }
        public DateOnly DataDeEmissao
        {
            get => _datadeEmissao;
            set
            {
                if (value > DateOnly.FromDateTime(DateTime.Today))
                    throw new ArgumentException("Data de emissão não pode ser no futuro.");
                _datadeEmissao = value;
            }
        }
        public string Vinculo
        {
            get => _vinculo;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Vínculo não pode ser nulo.");
                _vinculo = value;
            }
        }
        public Colaborador(
    string cpf,
    DateOnly dataNascimento,
    string nomeCompleto,
    string telefone,
    string email,
    string senha,
    string foto,
    string tipo,
    DateOnly datadeEmissao,
    string vinculo)
    : base(
        cpf,
        dataNascimento,
        nomeCompleto,
        telefone,
        email,
        senha,
        foto
      )
        {
            Tipo = tipo;
            DataDeEmissao = datadeEmissao;
            Vinculo = vinculo;
        }

    }
}

    

