using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Nícolas Bastos

namespace Academia_do_Zé
{
    internal class Permissao
    {
        public TipoPermissao Tipo { get; set; }

        public bool PodeAcessarTotal { get; }
        public bool PodeCadastrarAlunos { get; }
        public bool PodeMatricular { get; }
        public bool PodeRegistrarEntradaSaidaAluno { get; }
        public bool PodeRegistrarPropriaEntradaSaida { get; }
        public bool PodeVisualizarEstatisticas { get; }

        public Permissao(TipoPermissao tipo)
        {
            Tipo = tipo;

            switch (tipo)
            {
                case TipoPermissao.Administrador:
                    PodeAcessarTotal = true;
                    PodeCadastrarAlunos = true;
                    PodeMatricular = true;
                    PodeRegistrarEntradaSaidaAluno = true;
                    PodeRegistrarPropriaEntradaSaida = true;
                    PodeVisualizarEstatisticas = true;
                    break;
                case TipoPermissao.Atendente:
                    PodeAcessarTotal = false;
                    PodeCadastrarAlunos = true;
                    PodeMatricular = true;
                    PodeRegistrarEntradaSaidaAluno = true;
                    PodeRegistrarPropriaEntradaSaida = true;
                    PodeVisualizarEstatisticas = false;
                    break;
                case TipoPermissao.Instrutor:
                    PodeAcessarTotal = false;
                    PodeCadastrarAlunos = false;
                    PodeMatricular = false;
                    PodeRegistrarEntradaSaidaAluno = true;
                    PodeRegistrarPropriaEntradaSaida = true;
                    PodeVisualizarEstatisticas = false;
                    break;
                case TipoPermissao.Aluno:
                    PodeAcessarTotal = false;
                    PodeCadastrarAlunos = false;
                    PodeMatricular = false;
                    PodeRegistrarEntradaSaidaAluno = false;
                    PodeRegistrarPropriaEntradaSaida = true;
                    PodeVisualizarEstatisticas = true;
                    break;
            }
        }
    }
}

