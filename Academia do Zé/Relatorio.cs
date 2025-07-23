using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Nícolas Bastos

namespace Academia_do_Zé
{
    using System;
    using System.Collections.Generic;

    namespace Academia_do_Zé
    {
        internal class Relatorio
        {
            public int HorasTrabalhadas { get; set; }
            public Dictionary<DateOnly, int> HorasTrabalhadasPorDia { get; set; }
            public TimeOnly HorarioMaiorProcura { get; set; }
            public int TotalAlunos { get; set; }
            public int NovasMatriculas { get; set; }
            public DateOnly DataInicioPlano { get; set; }
            public DateOnly DataFimPlano { get; set; }
            public double PercentualEvasao { get; set; }
            public double PermanenciaMedia { get; set; }
            public string TendenciaEvasao { get; set; }
            public double PercentualRetencao
            {
                get
                {
                    // Retenção é o inverso da evasão
                    return 100 - PercentualEvasao;
                }
            }

            public Relatorio(
                int horasTrabalhadas,
                Dictionary<DateOnly, int> horasTrabalhadasPorDia,
                TimeOnly horarioMaiorProcura,
                int totalAlunos,
                int novasMatriculas,
                DateOnly dataInicioPlano,
                DateOnly dataFimPlano,
                double percentualEvasao,
                double permanenciaMedia,
                string tendenciaEvasao)
            {
                if (horasTrabalhadas < 0)
                    throw new ArgumentException("Horas trabalhadas não podem ser negativas.");
                if (totalAlunos < 0)
                    throw new ArgumentException("Total de alunos não pode ser negativo.");
                if (novasMatriculas < 0)
                    throw new ArgumentException("Novas matrículas não podem ser negativas.");
                if (dataInicioPlano > DateOnly.FromDateTime(DateTime.Today))
                    throw new ArgumentException("Data de início do plano não pode ser no futuro.");
                if (dataFimPlano < dataInicioPlano)
                    throw new ArgumentException("Data de fim do plano não pode ser anterior à data de início.");
                if (percentualEvasao < 0 || percentualEvasao > 100)
                    throw new ArgumentException("Percentual de evasão deve estar entre 0 e 100.");
                if (permanenciaMedia < 0)
                    throw new ArgumentException("Permanência média não pode ser negativa.");

                HorasTrabalhadas = horasTrabalhadas;
                HorasTrabalhadasPorDia = horasTrabalhadasPorDia ?? new Dictionary<DateOnly, int>();
                HorarioMaiorProcura = horarioMaiorProcura;
                TotalAlunos = totalAlunos;
                NovasMatriculas = novasMatriculas;
                DataInicioPlano = dataInicioPlano;
                DataFimPlano = dataFimPlano;
                PercentualEvasao = percentualEvasao;
                PermanenciaMedia = permanenciaMedia;
                TendenciaEvasao = tendenciaEvasao;
            }
        }
    }
