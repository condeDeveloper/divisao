namespace Conde.Divisao;

/// <summary>Quantas divisões de cada tipo existem num bolo.</summary>
public readonly record struct Resumo(
    long Divisoes, long Proporcionais, long SemInveja, long Eficientes, int MenorInveja, int[]? MelhorDivisao);

/// <summary>
/// Todas as divisões possíveis: cada fatia vai para uma das pessoas, então são
/// pessoas elevado a fatias. Com 3 pessoas e 8 fatias são 6.561, e é nesse
/// tamanho que as perguntas do juiz ainda podem ser feitas a todas elas.
/// </summary>
public static class Varredura
{
    /// <summary>Toda divisão, uma por uma.</summary>
    public static IEnumerable<int[]> Todas(Bolo bolo)
    {
        ArgumentNullException.ThrowIfNull(bolo);
        var dono = new int[bolo.Fatias];

        while (true)
        {
            yield return dono;

            var f = 0;
            while (f < bolo.Fatias)
            {
                dono[f]++;
                if (dono[f] < bolo.Pessoas) break;
                dono[f] = 0;
                f++;
            }

            if (f == bolo.Fatias) yield break;
        }
    }

    /// <summary>Quantas divisões existem: pessoas elevado a fatias.</summary>
    public static long Quantas(Bolo bolo)
    {
        ArgumentNullException.ThrowIfNull(bolo);
        long total = 1;
        for (var f = 0; f < bolo.Fatias; f++) total *= bolo.Pessoas;
        return total;
    }

    /// <summary>
    /// A conta completa de um bolo: quantas divisões são proporcionais, quantas
    /// são sem inveja, quantas são eficientes, e qual a menor inveja possível
    /// quando o zero não é alcançável.
    /// </summary>
    public static Resumo Resume(Bolo bolo, bool comEficiencia = true)
    {
        ArgumentNullException.ThrowIfNull(bolo);

        long divisoes = 0, proporcionais = 0, semInveja = 0, eficientes = 0;
        var menorInveja = int.MaxValue;
        int[]? melhor = null;

        foreach (var dono in Todas(bolo))
        {
            divisoes++;
            if (Juiz.Proporcional(bolo, dono)) proporcionais++;

            var inveja = Juiz.Inveja(bolo, dono);
            if (inveja == 0) semInveja++;
            if (inveja < menorInveja)
            {
                menorInveja = inveja;
                melhor = (int[])dono.Clone();
            }

            if (comEficiencia && Juiz.Eficiente(bolo, dono)) eficientes++;
        }

        return new Resumo(divisoes, proporcionais, semInveja, comEficiencia ? eficientes : -1, menorInveja, melhor);
    }

    /// <summary>
    /// Todos os bolos possíveis em que cada pessoa dá a cada fatia um valor de 0
    /// a <paramref name="maiorValor"/>. É a varredura de dentro da varredura: não
    /// um bolo, mas todos os bolos daquele tamanho.
    /// </summary>
    public static IEnumerable<Bolo> TodosOsBolos(int pessoas, int fatias, int maiorValor)
    {
        if (pessoas < 1 || fatias < 1) throw new ArgumentOutOfRangeException(nameof(pessoas));
        if (maiorValor < 1) throw new ArgumentOutOfRangeException(nameof(maiorValor));

        var celulas = pessoas * fatias;
        var valores = new int[celulas];

        while (true)
        {
            var matriz = new int[pessoas, fatias];
            for (var i = 0; i < celulas; i++) matriz[i / fatias, i % fatias] = valores[i];
            yield return new Bolo(matriz);

            var k = 0;
            while (k < celulas)
            {
                valores[k]++;
                if (valores[k] <= maiorValor) break;
                valores[k] = 0;
                k++;
            }

            if (k == celulas) yield break;
        }
    }
}
