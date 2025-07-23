using System;
using System.Text.RegularExpressions;

//Nícolas Bastos

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
                
            }
        }

        public DateOnly DataNascimento
        {
            get => _dataNascimento;
            set
            {
                
               
            }
        }

        public string NomeCompleto
        {
            get => _nomeCompleto;
            set
            {
               
            }
        }

        public string Telefone
        {
            get => _telefone;
            set
            {
              
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                ;
            }
        }

        public string Senha
        {
            get => _senha;
            set
            {
                
            }
        }

        public string Foto
        {
            get => _foto;
            set => _foto = value; 
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
