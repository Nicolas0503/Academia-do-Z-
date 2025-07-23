using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Nícolas Bastos

namespace Academia_do_Zé
{
    internal class Endereco
    {
        private int _cep;
        private string _estado;
        private string _cidade;
        private string _bairro;
        private string _logradouro;
        private int _numero;
        private string _complemento;

        public int Cep
        {
            get => _cep;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("CEP deve ser maior que zero.");
                _cep = value;
            }
        }

        public string Estado
        {
            get => _estado;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Estado não pode ser nulo ou vazio.");
                _estado = value;
            }
        }

        public string Cidade
        {
            get => _cidade;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Cidade não pode ser nula ou vazia.");
                _cidade = value;
            }
        }

        public string Bairro
        {
            get => _bairro;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Bairro não pode ser nulo ou vazio.");
                _bairro = value;
            }
        }

        public string Logradouro
        {
            get => _logradouro;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Logradouro não pode ser nulo ou vazio.");
                _logradouro = value;
            }
        }

        public int Numero
        {
            get => _numero;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Número deve ser maior que zero.");
                _numero = value;
            }
        }

        public string Complemento
        {
            get => _complemento;
            set => _complemento = value;
        }

        public Endereco(
            int cep,
            string estado,
            string cidade,
            string bairro,
            string logradouro,
            int numero,
            string complemento)
        {
            Cep = cep;
            Estado = estado;
            Cidade = cidade;
            Bairro = bairro;
            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento;
        }
    }
}
