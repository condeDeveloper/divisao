using Conde.Divisao;
using Xunit;

namespace Conde.Divisao.Testes;

/// <summary>Todo número do README está preso aqui.</summary>
public class MedidasTestes
{
    private static readonly List<Medidas.LinhaExiste> Existencia = Medidas.Existe();

    [Theory]
    [InlineData(2, 1, 1, 4L, 3L, 3L)]
    [InlineData(2, 2, 1, 16L, 14L, 14L)]
    [InlineData(2, 3, 1, 64L, 60L, 60L)]
    [InlineData(2, 4, 1, 256L, 248L, 248L)]
    [InlineData(2, 2, 2, 81L, 63L, 63L)]
    [InlineData(2, 3, 2, 729L, 656L, 656L)]
    [InlineData(3, 3, 1, 512L, 407L, 407L)]
    [InlineData(3, 4, 1, 4_096L, 3_767L, 3_791L)]
    public void CadaLinhaDaExistencia(int pessoas, int fatias, int maior, long bolos, long semInveja, long proporcional)
    {
        var linha = Existencia.Single(l => l.Pessoas == pessoas && l.Fatias == fatias && l.MaiorValor == maior);
        Assert.Equal(bolos, linha.Bolos);
        Assert.Equal(semInveja, linha.ComSemInveja);
        Assert.Equal(proporcional, linha.ComProporcional);
    }

    [Fact]
    public void ComFatiaIndivisivelADivisaoSemInvejaNemSempreExiste()
    {
        foreach (var linha in Existencia)
            Assert.True(linha.ComSemInveja < linha.Bolos, $"{linha.Pessoas} por {linha.Fatias}");
    }

    [Fact]
    public void ComTresPessoasExisteBoloComProporcionalESemNenhumaSemInveja()
    {
        var tres = Existencia.Single(l => l is { Pessoas: 3, Fatias: 4 });
        Assert.Equal(24, tres.ComProporcional - tres.ComSemInveja);

        // Com duas pessoas isso nunca acontece.
        foreach (var linha in Existencia.Where(l => l.Pessoas == 2))
            Assert.Equal(linha.ComProporcional, linha.ComSemInveja);
    }

    [Theory]
    [InlineData("gostos iguais", 8L, 2L, 2L, 8L, 0)]
    [InlineData("gostos opostos", 8L, 2L, 2L, 4L, 0)]
    [InlineData("uma fatia so interessa", 8L, 0L, 0L, 8L, 5)]
    [InlineData("tres pessoas, quatro fatias", 81L, 3L, 1L, 27L, 0)]
    public void CadaLinhaDosBolos(string nome, long divisoes, long proporcionais, long semInveja, long eficientes, int menorInveja)
    {
        var linha = Medidas.BolosDeExemplo().Single(l => l.Bolo == nome);
        Assert.Equal(divisoes, linha.Divisoes);
        Assert.Equal(proporcionais, linha.Proporcionais);
        Assert.Equal(semInveja, linha.SemInveja);
        Assert.Equal(eficientes, linha.Eficientes);
        Assert.Equal(menorInveja, linha.MenorInveja);
    }

    [Fact]
    public void QuandoSoUmaFatiaInteressaNadaSalva()
    {
        var linha = Medidas.BolosDeExemplo().Single(l => l.Bolo == "uma fatia so interessa");
        Assert.Equal(0, linha.SemInveja);
        Assert.Equal(0, linha.Proporcionais);
        Assert.Equal(5, linha.MenorInveja);
        // E todas as oito divisões são eficientes: não há o que melhorar.
        Assert.Equal(linha.Divisoes, linha.Eficientes);
    }

    [Theory]
    [InlineData("rodizio", 5_272L, 5_484L)]
    [InlineData("guloso", 3_909L, 4_033L)]
    [InlineData("corta e escolhe", 3_759L, 3_971L)]
    public void CadaLinhaDosProtocolos(string protocolo, long semInveja, long otimas)
    {
        var linha = Medidas.Protocolos().Single(l => l.Protocolo == protocolo);
        Assert.Equal(6_561, linha.Bolos);
        Assert.Equal(semInveja, linha.SemInveja);
        Assert.Equal(semInveja, linha.Proporcionais);
        Assert.Equal(otimas, linha.Otimas);
    }

    [Fact]
    public void OCortaEEscolheEhOPiorDosTres()
    {
        var linhas = Medidas.Protocolos();
        Assert.Equal("rodizio", linhas[0].Protocolo);
        Assert.Equal("corta e escolhe", linhas[^1].Protocolo);
        Assert.Equal(0.804, linhas[0].Fracao, 3);
        Assert.Equal(0.573, linhas[^1].Fracao, 3);
    }

    [Theory]
    [InlineData(2, 5_832L, 0L, 1_434L)]
    [InlineData(3, 13_824L, 126L, 1_278L)]
    public void CadaLinhaDaRelacao(int pessoas, long divisoes, long soProporcional, long ambas)
    {
        var linha = Medidas.Relacao().First(l => l.Pessoas == pessoas && l.Divisoes == divisoes);
        Assert.Equal(soProporcional, linha.SoProporcional);
        Assert.Equal(ambas, linha.Ambas);
        Assert.Equal(0, linha.SoSemInveja);
    }

    [Fact]
    public void SemInvejaSempreImplicaProporcionalEONaoContrario()
    {
        foreach (var linha in Medidas.Relacao())
        {
            Assert.Equal(0, linha.SoSemInveja);
            Assert.True(linha.UmaImplicaAOutra);
        }

        // Com três pessoas aparecem divisões proporcionais e invejosas.
        Assert.Equal(8_964, Medidas.Relacao().Single(l => l.Divisoes == 331_776).SoProporcional);
    }
}
