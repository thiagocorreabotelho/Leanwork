namespace Leanwork.Rh.Domain.Validation.Helper;

public static class CPFHelper
{
    /// <summary>
    /// Valida se uma string representa um número de CPF válido.
    /// </summary>
    /// <param name="cpf">A string contendo o número de CPF a ser validado.</param>
    /// <returns>
    /// Retorna <c>true</c> se o número de CPF for válido; caso contrário, retorna <c>false</c>.
    /// </returns>
    /// <remarks>
    /// O método verifica inicialmente se a string do CPF não está vazia e remove espaços, pontos e traços.
    /// Então, checa se a string resultante possui exatamente 11 caracteres e se todos eles são dígitos.
    /// Após essas validações iniciais, o método utiliza dois arrays de multiplicadores para calcular os dígitos
    /// verificadores do CPF, seguindo a fórmula padrão da receita federal brasileira. O CPF é considerado válido se
    /// os dígitos verificadores calculados correspondem aos dois últimos dígitos da string original.
    /// </remarks>
    public static bool ValidateCPF(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return false;

        cpf = cpf.Trim().Replace(".", "").Replace("-", "");

        if (cpf.Length != 11)
            return false;

        if (!cpf.All(char.IsDigit))
            return false;

        int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        string tempCpf;
        string digito;
        int soma;
        int resto;

        tempCpf = cpf.Substring(0, 9);
        soma = 0;

        for (int i = 0; i < 9; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

        resto = soma % 11;
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;

        digito = resto.ToString();
        tempCpf = tempCpf + digito;
        soma = 0;
        for (int i = 0; i < 10; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

        resto = soma % 11;
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;

        digito = digito + resto.ToString();

        return cpf.EndsWith(digito);
    }
}
