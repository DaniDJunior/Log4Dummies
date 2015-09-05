using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Configuration;

/// <summary>
/// Log para idiotas
/// </summary>
public class Log4Dummies
{

    /// <summary>
    /// Caminho do Log a ser configurado em um web.config ou em um app.config na parte de appSettings com o nome de "CaminhoLog", não esquecer o "\" no final do caminho, como "C:\Aquivos\Logs\"
    /// </summary>
    private static string CaminhoLog
    {
        get
        {
            if (ConfigurationManager.AppSettings["L4D.Caminho"] != null)
                return ConfigurationManager.AppSettings["L4D.Caminho"];
            else
                return System.Environment.CurrentDirectory + "\\";
        }
    }

    private static int TamanoArquivo
    {
        get
        {
            if (ConfigurationManager.AppSettings["L4D.Tamanho"] != null)
                return int.Parse(ConfigurationManager.AppSettings["L4D.Tamanho"]);
            else
                return 1000000;
        }
    }

    private static string NomeArquivo
    {
        get
        {
            if (ConfigurationManager.AppSettings["L4D.Nome"] != null)
                return ConfigurationManager.AppSettings["L4D.Nome"];
            else
                return "log";
        }
    }

    private static string ExtencaoArquivo
    {
        get
        {
            if (ConfigurationManager.AppSettings["L4D.Extencao"] != null)
                return ConfigurationManager.AppSettings["L4D.Extencao"];
            else
                return "txt";
        }
    }

    private static string FormatoArquivo
    {
        get
        {
            if (ConfigurationManager.AppSettings["L4D.Formato"] != null)
                return ConfigurationManager.AppSettings["L4D.Formato"];
            else
                return "{0}{1}";
        }
    }

    private static string FormatoDataArquivo
    {
        get
        {
            if (ConfigurationManager.AppSettings["L4D.FormatoData"] != null)
                return ConfigurationManager.AppSettings["L4D.FormatoData"];
            else
                return "yyyyMMddHHmmss";
        }
    }

    private static string FormatoLog
    {
        get
        {
            if (ConfigurationManager.AppSettings["L4D.FormatoLog"] != null)
                return ConfigurationManager.AppSettings["L4D.FormatoLog"];
            else
                return "{0} - {1}";
        }
    }

    private static string FormatoDataLog
    {
        get
        {
            if (ConfigurationManager.AppSettings["L4D.FormatoDataLog"] != null)
                return ConfigurationManager.AppSettings["L4D.FormatoDataLog"];
            else
                return "dd/MM/yyyy HH:mm:ss";
        }
    }

    /// <summary>
    /// Objeto para travar a insersão do arquivo em multiplas threads
    /// </summary>
    private static object thisLock = new object();

    /// <summary>
    /// Salva o texto do log diretamente no aquivo de log com a data atual
    /// </summary>
    /// <param name="texto">Texto a ser salvo no log</param>
    public static void SalvaLog(string texto)
    {
        SalvaLog(DateTime.Now, texto);
    }

    /// <summary>
    /// Salva o texto do log diretamente no aquivo do log com a data fornecida
    /// </summary>
    /// <param name="data">Data a ser salva no log</param>
    /// <param name="texto">Texto a ser salvo no log</param>
    public static void SalvaLog(DateTime data, string texto)
    {
        lock (thisLock)
        {
            if (File.Exists(CaminhoLog + NomeArquivo + "." + ExtencaoArquivo))
            {
                StreamReader Aux = new StreamReader(CaminhoLog + NomeArquivo + "." + ExtencaoArquivo);
                if (Aux.ReadToEnd().Length > TamanoArquivo)
                {
                    Aux.Close();
                    File.Copy(CaminhoLog + NomeArquivo + "." + ExtencaoArquivo, CaminhoLog + string.Format(FormatoArquivo, NomeArquivo, DateTime.Now.ToString(FormatoDataArquivo)) + "." + ExtencaoArquivo);
                    File.Delete(CaminhoLog + NomeArquivo + "." + ExtencaoArquivo);
                }
                else
                    Aux.Close();
            }
            StreamWriter Arq = new StreamWriter(CaminhoLog + NomeArquivo + "." + ExtencaoArquivo, true);
            Arq.WriteLine(string.Format(FormatoLog, data.ToString(FormatoDataLog), texto));
            Arq.Close();
        }
    }

    /// <summary>
    /// Salva o conteudo do objeto log diretamente no arquivo
    /// </summary>
    /// <param name="log"></param>
    public static void SalvaLog(Log log)
    {
        log.Salvar();
    }

    /// <summary>
    /// Salva exeçao no arquivo do log
    /// </summary>
    /// <param name="ex">Exeção a ser salva</param>
    public static void SalvaLog(Exception ex)
    {
        SalvaLog(RetornaExecao(ex, false));
    }

    /// <summary>
    /// Retorna a mensagem completa da exeção
    /// </summary>
    /// <param name="ex">Exeção a retornar o texto de mensagem</param>
    /// <param name="HTML">Verdadeiro retorna o separador como <br>, falso retorna com separador de linha</param>
    /// <returns>Texto completo do erro</returns>
    public static string RetornaExecao(Exception ex, bool HTML)
    {
        string separador;
        if(HTML)
        {
            separador = "<br>";
        }
        else
        {
            separador = "\r\n";
        }
        if(ex.InnerException != null)
        {
            return ex.Message + separador + RetornaExecao(ex.InnerException, HTML);
        }
        else
        {
            return ex.Message;
        }
    }

    /// <summary>
    /// Objeto de Log
    /// </summary>
    public class Log
    {

        /// <summary>
        /// Data do Log
        /// </summary>
        public DateTime Data { get; set; }

        /// <summary>
        /// Texto do Log
        /// </summary>
        public string Texto { get; set; }

        /// <summary>
        /// Construtor do Log vazio
        /// </summary>
        public Log()
        {
            Data = DateTime.Now;
            Texto = string.Empty;
        }

        /// <summary>
        /// Construtor do Log com o texto e data atual
        /// </summary>
        /// <param name="texto">Texto do Log</param>
        public Log(string texto)
        {
            Data = DateTime.Now;
            Texto = texto;
        }

        /// <summary>
        /// Construtor do Log o texto e data fornecidas
        /// </summary>
        /// <param name="data">Data do Log</param>
        /// <param name="texto">Texto do Log</param>
        public Log(DateTime data, string texto)
        {
            Data = data;
            Texto = texto;
        }

        /// <summary>
        /// Salva o objeto no arquivo
        /// </summary>
        public void Salvar()
        {
            SalvaLog(Data, Texto);
        }

        /// <summary>
        /// Abre uma linha expecifica do log e carrega seus dados no campos certos
        /// </summary>
        /// <param name="linha">Linha expecifica do Log</param>
        public void Abrir(string linha)
        {
            Data = new DateTime(int.Parse(linha.Substring(6, 4)), int.Parse(linha.Substring(3, 2)), int.Parse(linha.Substring(0, 2)), int.Parse(linha.Substring(11, 2)), int.Parse(linha.Substring(14, 2)), int.Parse(linha.Substring(17, 2)));
            Texto = linha.Remove(0, 22);
        }

        /// <summary>
        /// Override do metodo ToString para fornecer a linha do log
        /// </summary>
        /// <returns>linha do log salva ou a ser salva no arquivo</returns>
        public override string ToString()
        {
            return string.Format(FormatoLog, Data.ToString(FormatoDataLog), Texto);
        }

        /// <summary>
        /// Recupera um arquivo de log e o converte em uma lista de objetos log
        /// </summary>
        /// <param name="ArquivoLog">Caminho do arquivo de log a ser aberto</param>
        /// <returns>Lista de logs do arquivo</returns>
        public static List<Log> RecuperaLogs(string ArquivoLog)
        {
            List<Log> aux = new List<Log>();
            StreamReader Arquivo = new StreamReader(ArquivoLog);
            while (!Arquivo.EndOfStream)
            {
                Log auxLog = new Log();
                auxLog.Abrir(Arquivo.ReadLine());
                aux.Add(auxLog);
            }
            Arquivo.Close();
            return aux;
        }
        
    }
}
