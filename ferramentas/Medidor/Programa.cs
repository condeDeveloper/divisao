using Conde.Divisao;

var comando = args.Length > 0 ? args[0] : "tudo";

if (comando is "existe" or "tudo") Existe();
if (comando is "bolos" or "tudo") Bolos();
if (comando is "protocolos" or "tudo") Protocolos();
if (comando is "relacao" or "tudo") Relacao();

static void Existe()
{
    Console.WriteLine();
    Console.WriteLine("EM QUANTOS BOLOS EXISTE DIVISAO SEM INVEJA. Com fatia indivisivel, nem sempre.");
    Console.WriteLine();
    Console.WriteLine($"  {"pessoas",8} {"fatias",7} {"valores ate",12} {"bolos",9} {"tem sem inveja",15} {"tem proporcional",17} {"fracao",8}");

    foreach (var l in Medidas.Existe())
        Console.WriteLine($"  {l.Pessoas,8} {l.Fatias,7} {l.MaiorValor,12} {l.Bolos,9:n0} {l.ComSemInveja,15:n0} {l.ComProporcional,17:n0} {l.Fracao,7:0.0%}");

    Console.WriteLine();
}

static void Bolos()
{
    Console.WriteLine();
    Console.WriteLine("QUATRO BOLOS, varridos inteiros.");
    Console.WriteLine();
    Console.WriteLine($"  {"bolo",-28} {"divisoes",9} {"proporcionais",14} {"sem inveja",11} {"eficientes",11} {"menor inveja",13}");

    foreach (var l in Medidas.BolosDeExemplo())
        Console.WriteLine($"  {l.Bolo,-28} {l.Divisoes,9:n0} {l.Proporcionais,14:n0} {l.SemInveja,11:n0} {l.Eficientes,11:n0} {l.MenorInveja,13}");

    Console.WriteLine();
}

static void Protocolos()
{
    Console.WriteLine();
    Console.WriteLine("OS PROTOCOLOS em todos os bolos de 2 pessoas e 4 fatias com valores de 0 a 2.");
    Console.WriteLine();
    Console.WriteLine($"  {"protocolo",-18} {"bolos",9} {"proporcionais",14} {"sem inveja",11} {"com a menor inveja",19} {"fracao",8}");

    foreach (var l in Medidas.Protocolos())
        Console.WriteLine($"  {l.Protocolo,-18} {l.Bolos,9:n0} {l.Proporcionais,14:n0} {l.SemInveja,11:n0} {l.Otimas,19:n0} {l.Fracao,7:0.0%}");

    Console.WriteLine();
}

static void Relacao()
{
    Console.WriteLine();
    Console.WriteLine("SEM INVEJA contra PROPORCIONAL, divisao por divisao.");
    Console.WriteLine();
    Console.WriteLine($"  {"pessoas",8} {"divisoes",12} {"so proporcional",16} {"so sem inveja",14} {"as duas",9} {"sem inveja implica",19}");

    foreach (var l in Medidas.Relacao())
        Console.WriteLine($"  {l.Pessoas,8} {l.Divisoes,12:n0} {l.SoProporcional,16:n0} {l.SoSemInveja,14:n0} {l.Ambas,9:n0} {(l.UmaImplicaAOutra ? "sim" : "NAO"),19}");

    Console.WriteLine();
}
