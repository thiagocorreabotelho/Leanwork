
namespace Leanwork.Rh.Domain.Validation.Helper;

public static class StateHelper
{
    private static readonly string[] state =
    {
    "AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO",
    "MA", "MT", "MS", "MG", "PA", "PB", "PR", "PE", "PI",
    "RJ", "RN", "RS", "RO", "RR", "SC", "SP", "SE", "TO"
};

    /// <summary>
    /// Method responsible for validation of state.
    /// </summary>
    /// <param name="state">State of register</param>
    /// <returns>Return true for sucess or false for problem.</returns>
    public static bool ValidationState(string state)
    {
        return state.Contains(state);
    }
}
