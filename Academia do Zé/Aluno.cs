using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academia_do_Zé
{
    internal class Aluno : Pessoa
    {
        private string _matricula;
        
        private DateTime _dataMatricula;
        

        public string Matricula
        {
            get => _matricula;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Matrícula não pode ser nula.");
                _matricula = value;
            }
        }
        
        public DateTime DataMatricula
        {
            get => _dataMatricula;
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("Data de matrícula não pode ser no futuro.");
                _dataMatricula = value;
            }
        }

        public Aluno(
                 string cpf,
                 DateOnly dataNascimento,
                 string nomeCompleto,
                 string telefone,
                 string email,
                 string senha,
                 string foto,
                 string matricula,
                 
     DateTime dataMatricula)
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
            Matricula = matricula;
           
            DataMatricula = dataMatricula;
        }

    }
}
