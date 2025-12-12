namespace Domain.Enums
{
    public enum ConsultaStatus
    {
        Agendada = 1, // Post Agendamento
        Triagem = 2, // Iniciar Triagem
        AguardandoConsulta = 3, // Finalizar Triagem
        EmAndamento = 4, //Iniciar Consulta
        Concluida = 5, //Finalizar Consulta
        Cancelada = 6
    }
}
