using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.Lexico
{
    public static class LexicoClass
    {
        private const bool IGNORAR_ESPACOS = true;

        public static List<TokenClass> ProcessaEntrada(string[] entrada)
        {
            int estado = 0;
            string lexema = String.Empty;
            List<TokenClass> listaTokens = new List<TokenClass>();

            // Percorre as linhas:
            for (int i = 0; i < entrada.Length; i++)
            {
                // Converte a linha em um vetor de caracteres.
                char[] chrs = entrada[i].ToCharArray();

                // Percorre os caracteres da linha buscando por tokens:
                int j = 0;
                while (j < chrs.Length)
                {
                    if (TabelaEstados.AFD[estado] != null && TabelaEstados.AFD[estado].ContainsKey(chrs[j]))
                    {
                        estado = TabelaEstados.AFD[estado][chrs[j]];
                        lexema += chrs[j];
                        j++;
                    }
                    else
                    {
                        if (TabelaEstados.Finais.ContainsKey(estado))
                        {
                            if (!IGNORAR_ESPACOS || (IGNORAR_ESPACOS && !lexema.Equals(" ")))
                                listaTokens.Add(new TokenClass(lexema, TabelaEstados.Finais[estado], i + 1, j));
                            estado = 0;
                            lexema = String.Empty;
                        }
                        else
                        {
                            throw new Exception($"Erro léxico no lexema: '{lexema}{chrs[j]}'");
                        }
                    }
                }

                // Necessário para tokens não processados devido ao final de linha
                if (lexema.Length > 0)
                {
                    if (TabelaEstados.Finais.ContainsKey(estado))
                    {
                        if (!IGNORAR_ESPACOS || (IGNORAR_ESPACOS && !lexema.Equals(" ")))
                            listaTokens.Add(new TokenClass(lexema, TabelaEstados.Finais[estado], i + 1, j));
                        estado = 0;
                        lexema = String.Empty;
                    }
                    else
                    {
                        throw new Exception($"Erro léxico no lexema: '{lexema}'");
                    }
                }
            }

            return listaTokens;
        }
    }
}
