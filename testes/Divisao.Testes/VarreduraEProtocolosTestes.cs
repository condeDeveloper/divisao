using Conde.Divisao;
using Xunit;

namespace Conde.Divisao.Testes;

public class VarreduraEProtocolosTestes
{
    [Theory]
    [InlineData(2, 3, 8L)]
    [InlineData(2, 4, 16L)]
    [InlineData(3, 3, 27L)]
    [InlineData(3, 4, 81L)]
    public void QuantasDivisoesExistem(int pessoas, int fatias, long quantas)
    {
        var bolo = new Bolo(new int[pessoas, fatias]);
        Assert.Equal(quantas, Varredura.Quantas(bolo));
        Assert.Equal(quantas, Varredura.Todas(bolo).Count());
    }

    [Fact]
    public void AVarreduraNaoRepeteDivisao()
    {
        var bolo = new Bolo(new int[3, 3]);
        var vistas = Varredura.Todas(bolo).Select(d => string.Join(",", d)).ToList();
        Assert.Equal(27, vistas.Distinct().Count());
    }

    [Fact]
    public void TodaDivisaoVarridaEhValida()
    {
        var bolo = new Bolo(new[,] { { 1, 2, 3 }, { 3, 2, 1 } });
        foreach (var dono in Varredura.Todas(bolo))
            Assert.True(Juiz.EhDivisao(bolo, dono));
    }

    [Theory]
    [InlineData(2, 2, 1, 16)]
    [InlineData(2, 2, 2, 81)]
    [InlineData(3, 2, 1, 64)]
    public void QuantosBolosExistemComValoresAteUmTeto(int pessoas, int fatias, int maior, int bolos)
    {
        Assert.Equal(bolos, Varredura.TodosOsBolos(pessoas, fatias, maior).Count());
    }

    [Fact]
    public void OResumoContaOQueOJuizConta()
    {
        var bolo = new Bolo(new[,] { { 3, 2, 1 }, { 1, 2, 3 } });
        var resumo = Varredura.Resume(bolo);

        Assert.Equal(8, resumo.Divisoes);
        Assert.Equal(Varredura.Todas(bolo).Count(d => Juiz.Proporcional(bolo, d)), resumo.Proporcionais);
        Assert.Equal(Varredura.Todas(bolo).Count(d => Juiz.SemInveja(bolo, d)), resumo.SemInveja);
    }

    [Fact]
    public void CortaEEscolheSoAceitaDuasPessoas()
    {
        Assert.Throws<ArgumentException>(() => Protocolos.CortaEEscolhe(new Bolo(new int[3, 3])));
    }

    [Fact]
    public void OEscolhedorNuncaFicaComInveja()
    {
        // A segunda pessoa escolhe o lote que mais vale para ela, então ela nunca
        // prefere o outro. A inveja, quando existe, é do cortador.
        foreach (var bolo in Varredura.TodosOsBolos(2, 3, 2))
        {
            var divisao = Protocolos.CortaEEscolhe(bolo);
            var meu = bolo.ValorDoLote(1, divisao);
            var outro = bolo.ValorDoLoteAlheio(1, 0, divisao);
            Assert.True(meu >= outro, bolo.ToString());
        }
    }

    [Fact]
    public void OGulosoDaCadaFatiaAQuemMaisQuer()
    {
        var bolo = new Bolo(new[,] { { 5, 1, 2 }, { 1, 5, 2 } });
        Assert.Equal(new[] { 0, 1, 0 }, Protocolos.Guloso(bolo));
    }

    [Fact]
    public void OGulosoMaximizaASomaDosValores()
    {
        foreach (var bolo in Varredura.TodosOsBolos(2, 3, 2))
        {
            var guloso = Protocolos.Guloso(bolo);
            var somaGulosa = bolo.ValorDoLote(0, guloso) + bolo.ValorDoLote(1, guloso);
            foreach (var dono in Varredura.Todas(bolo))
                Assert.True(bolo.ValorDoLote(0, dono) + bolo.ValorDoLote(1, dono) <= somaGulosa);
        }
    }

    [Fact]
    public void ORodizioDaFatiasAlternadamente()
    {
        var bolo = new Bolo(new[,] { { 4, 3, 2, 1 }, { 4, 3, 2, 1 } });
        var divisao = Protocolos.Rodizio(bolo);
        Assert.Equal(new[] { 0, 1, 0, 1 }, divisao);
    }

    [Fact]
    public void TodoProtocoloDevolveUmaDivisaoValida()
    {
        foreach (var bolo in Varredura.TodosOsBolos(2, 3, 2))
        {
            Assert.True(Juiz.EhDivisao(bolo, Protocolos.CortaEEscolhe(bolo)));
            Assert.True(Juiz.EhDivisao(bolo, Protocolos.Guloso(bolo)));
            Assert.True(Juiz.EhDivisao(bolo, Protocolos.Rodizio(bolo)));
        }
    }

    [Fact]
    public void AMenorInvejaAchadaEhMesmoAMenor()
    {
        foreach (var bolo in Varredura.TodosOsBolos(2, 3, 2))
        {
            var melhor = Juiz.Inveja(bolo, Protocolos.MenorInveja(bolo));
            foreach (var dono in Varredura.Todas(bolo))
                Assert.True(Juiz.Inveja(bolo, dono) >= melhor);
        }
    }
}
