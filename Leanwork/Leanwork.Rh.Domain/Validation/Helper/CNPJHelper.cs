namespace Leanwork.Rh.Domain;


public static class CNPJValidation
{

    /// <summary>
    /// Valida um CNPJ para verificar se é um número válido conforme as regras da receita federal brasileira.
    /// </summary>
    /// <param name="cnpj">O CNPJ a ser validado.</param>
    /// <returns>
    /// Retorna <c>true</c> se o CNPJ for válido; caso contrário, retorna <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método realiza várias verificações para determinar a validade de um CNPJ:
    /// 1. Verifica se o CNPJ está vazio ou é nulo.
    /// 2. Remove caracteres não numéricos e verifica se o resultado tem exatamente 14 dígitos.
    /// 3. Checa se o CNPJ não está em uma sequência de números repetidos que são conhecidos por serem inválidos.
    /// 4. Calcula e compara os dígitos verificadores usando um algoritmo específico para garantir que o CNPJ está corretamente formatado.
    /// Este método é essencial para garantir que registros de empresas brasileiras sejam representados por números válidos.
    /// </remarks>
    public static bool ValidateCNPJ(string cnpj)
    {
        if (IsNullOrEmpty(cnpj))
            return false;

        cnpj = CleanCNPJ(cnpj);

        if (!CheckLength(cnpj) || CheckInvalidSequence(cnpj))
            return false;

        string firstPart = cnpj.Substring(0, 12);
        string calculatedDigit = CalculateDigit(firstPart);

        return cnpj.EndsWith(calculatedDigit);
    }

    private static bool IsNullOrEmpty(string cnpj)
    {
        return string.IsNullOrWhiteSpace(cnpj);
    }

    public static string CleanCNPJ(string cnpj)
    {
        return new string(cnpj.Where(char.IsDigit).ToArray());
    }

    private static bool CheckLength(string cnpj)
    {
        return cnpj.Length == 14;
    }

    private static bool CheckInvalidSequence(string cnpj)
    {
        return cnpj == "00000000000000" ||
            cnpj == "11111111111111" ||
            cnpj == "22222222222222" ||
            cnpj == "33333333333333" ||
            cnpj == "44444444444444" ||
            cnpj == "55555555555555" ||
            cnpj == "66666666666666" ||
            cnpj == "77777777777777" ||
            cnpj == "88888888888888" ||
            cnpj == "99999999999999";
    }

    private static string CalculateDigit(string cnpj)
    {
        int[] multiplier1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplier2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int sum;
        string digit;
        string tempCnpj;

        sum = 0;
        tempCnpj = cnpj;

        for (int i = 0; i < 12; i++)
            sum += int.Parse(tempCnpj[i].ToString()) * multiplier1[i];

        digit = (sum % 11 < 2) ? "0" : (11 - sum % 11).ToString();
        tempCnpj = tempCnpj + digit;
        sum = 0;

        for (int i = 0; i < 13; i++)
            sum += int.Parse(tempCnpj[i].ToString()) * multiplier2[i];

        digit = digit + ((sum % 11 < 2) ? "0" : (11 - sum % 11).ToString());

        return digit;
    }
}

