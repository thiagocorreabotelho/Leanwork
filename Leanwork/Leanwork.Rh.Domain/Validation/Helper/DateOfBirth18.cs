namespace Leanwork.Rh.Domain;

public static class DateOfBirth18
{

    /// <summary>
    /// Calcula se uma pessoa é maior de idade (18 anos ou mais) com base em sua data de nascimento.
    /// </summary>
    /// <param name="dateOfBirth">A data de nascimento da pessoa a ser avaliada.</param>
    /// <returns>
    /// Retorna <c>true</c> se a pessoa for maior de idade (18 anos ou mais); caso contrário, retorna <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método calcula a idade da pessoa subtraindo o ano de nascimento do ano atual. Se a data atual é anterior
    /// ao dia e mês de nascimento no ano corrente, isso significa que a pessoa ainda não completou aniversário no
    /// ano em curso, e portanto, um ano é subtraído da idade calculada. Este método é útil para validações em 
    /// formulários e sistemas onde é necessário confirmar que um usuário é adulto conforme definições legais.
    /// </remarks>
    public static bool BeAtLeast18YearsOld(DateTime dateOfBirth)
    {
        var today = DateTime.Today;
        var age = today.Year - dateOfBirth.Year;
        if (dateOfBirth > today.AddYears(-age)) age--;

        return age >= 18;
    }
}
