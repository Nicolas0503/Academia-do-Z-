using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academia_do_Zé
{
    internal class RegistroPonto
    {
        private Aluno _aluno;
        private Colaborador _colaborador;
        private DateOnly _data;
        private TimeOnly _horaEntrada;
        private TimeOnly _horaSaida;

        public Aluno Aluno
        {
            get => _aluno;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(Aluno), "Aluno não pode ser nulo.");
                _aluno = value;
                _colaborador = null; // Garante que só um dos objetos será usado
            }
        }

        public Colaborador Colaborador
        {
            get => _colaborador;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(Colaborador), "Colaborador não pode ser nulo.");
                _colaborador = value;
                _aluno = null; // Garante que só um dos objetos será usado
            }
        }

        public DateOnly Data
        {
            get => _data;
            set
            {
                if (value > DateOnly.FromDateTime(DateTime.Today))
                    throw new ArgumentException("Data não pode ser no futuro.");
                _data = value;
            }
        }

        public TimeOnly HoraEntrada
        {
            get => _horaEntrada;
            set => _horaEntrada = value;
        }

        public TimeOnly HoraSaida
        {
            get => _horaSaida;
            set
            {
                if (value < _horaEntrada)
                    throw new ArgumentException("Hora de saída não pode ser anterior à hora de entrada.");
                _horaSaida = value;
            }
        }

        // Construtor para Aluno
        public RegistroPonto(Aluno aluno, DateOnly data, TimeOnly horaEntrada, TimeOnly horaSaida)
        {
            Aluno = aluno;
            Data = data;
            HoraEntrada = horaEntrada;
            HoraSaida = horaSaida;
        }

        // Construtor para Colaborador
        public RegistroPonto(Colaborador colaborador, DateOnly data, TimeOnly horaEntrada, TimeOnly horaSaida)
        {
            Colaborador = colaborador;
            Data = data;
            HoraEntrada = horaEntrada;
            HoraSaida = horaSaida;
        }
    }
}
