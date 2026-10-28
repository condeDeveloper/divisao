namespace Conde.Divisao;

/// <summary>
/// O bolo já cortado em fatias, e quanto cada pessoa acha que cada fatia vale.
/// As fatias são indivisíveis: é esse detalhe que faz metade das garantias do
/// problema clássico deixarem de valer.
/// </summary>
public sealed class Bolo
{
    private readonly int[,] _valor;

    public Bolo(int[,] valores)
    {
        ArgumentNullException.ThrowIfNull(valores);
        Pessoas = valores.GetLength(0);
        Fatias = valores.GetLength(1);
        if (Pessoas < 1 || Fatias < 1) throw new ArgumentException("bolo sem pessoa ou sem fatia", nameof(valores));

        for (var p = 0; p < Pessoas; p++)
            for (var f = 0; f < Fatias; f++)
                if (valores[p, f] < 0)
                    throw new ArgumentException("fatia de valor negativo", nameof(valores));

        _valor = (int[,])valores.Clone();
    }

    public int Pessoas { get; }

    public int Fatias { get; }

    /// <summary>Quanto a pessoa acha que a fatia vale.</summary>
    public int Valor(int pessoa, int fatia) => _valor[pessoa, fatia];

    /// <summary>Quanto a pessoa acha que o bolo inteiro vale.</summary>
    public int Total(int pessoa)
    {
        var total = 0;
        for (var f = 0; f < Fatias; f++) total += _valor[pessoa, f];
        return total;
    }

    /// <summary>Quanto a pessoa acha que o lote dela vale.</summary>
    public int ValorDoLote(int pessoa, IReadOnlyList<int> donoDaFatia)
    {
        ArgumentNullException.ThrowIfNull(donoDaFatia);
        var total = 0;
        for (var f = 0; f < Fatias; f++)
            if (donoDaFatia[f] == pessoa)
                total += _valor[pessoa, f];
        return total;
    }

    /// <summary>
    /// Quanto a pessoa acha que o lote de OUTRA pessoa vale. É com isto que a
    /// inveja é medida: cada um olha o prato do vizinho com os próprios olhos.
    /// </summary>
    public int ValorDoLoteAlheio(int pessoa, int dono, IReadOnlyList<int> donoDaFatia)
    {
        ArgumentNullException.ThrowIfNull(donoDaFatia);
        var total = 0;
        for (var f = 0; f < Fatias; f++)
            if (donoDaFatia[f] == dono)
                total += _valor[pessoa, f];
        return total;
    }

    public override string ToString()
    {
        var linhas = new string[Pessoas];
        for (var p = 0; p < Pessoas; p++)
        {
            var valores = new int[Fatias];
            for (var f = 0; f < Fatias; f++) valores[f] = _valor[p, f];
            linhas[p] = string.Join(",", valores);
        }
        return string.Join(" / ", linhas);
    }
}
