using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace PagSeguro
{
    public class sPagSeguro
    {
        // Gerar pagamento do PagSeguro
        public static Dados GerarPagamento(Dados dados = null)
        {
            if (dados == null) return null;
            dados.stringConexao = "";
            try
            {
                //URI de checkout.                

                string uri = @"https://ws.pagseguro.uol.com.br/v2/checkout";
                //Conjunto de parâmetros/formData.
                System.Collections.Specialized.NameValueCollection postData =
                    new System.Collections.Specialized.NameValueCollection
                    {
                        {"email", dados.MeuEmail},
                        {"token", dados.MeuToken},
                        {"currency", "BRL"},
                        {"itemId1", "0001"},
                        {"itemDescription1", dados.TituloPagamento},
                        {"itemAmount1", dados.Valor.Replace(",",".")},
                        {"itemQuantity1", "1"},
                        {"itemWeight1", "000"},
                        {"reference", "Ref " + dados.Referencia},
                        {"senderName", dados.Nome},
                        {"senderAreaCode", dados.DDD},
                        {"senderPhone", dados.NumeroTelefone},
                        {"senderEmail", dados.Email},
                        {"shippingAddressRequired", "false"}
                    };
                //String que receberá o XML de retorno.
                string xmlString = null;
                //Webclient faz o post para o servidor de pagseguro.
                using (WebClient wc = new WebClient())
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                    //Informa header sobre URL.
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    //Faz o POST e retorna o XML contendo resposta do servidor do pagseguro.
                    var result = wc.UploadValues(uri, postData);
                    //Obtém string do XML.
                    xmlString = Encoding.ASCII.GetString(result);
                }
                //Cria documento XML.
                XmlDocument xmlDoc = new XmlDocument();
                //Carrega documento XML por string.
                xmlDoc.LoadXml(xmlString);
                //Obtém código de transação (Checkout).
                var code = xmlDoc.GetElementsByTagName("code")[0];
                //Monta a URL para pagamento.                
                if (!code.InnerText.Equals(""))
                {
                    dados.CodigoAcesso = code.InnerText;
                    dados.stringConexao = string.Concat("https://pagseguro.uol.com.br/v2/checkout/payment.html?code=", code.InnerText);
                }
            }
            catch
            {
                dados.CodigoAcesso = "";
                dados.stringConexao = "";
            }
            // Retorna com a URL para carregar na tela
            return dados;
        }

        // Validar situação do pagamento
        public static Dados ValidarPagamento(Dados dados = null)
        {
            if (dados == null) return null;
            Dados retorno = new Dados();
            retorno.CodigoAcesso = dados.CodigoAcesso;
            try
            {
                //uri de consulta da transação.
                string uri = "https://ws.pagseguro.uol.com.br/v3/transactions/" + dados.CodigoAcesso +
                             "?email=" + dados.MeuEmail + "&token=" + dados.MeuToken;
                //Classe que irá fazer a requisição GET.
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
                //Método do webrequest.
                request.Method = "GET";
                //String que vai armazenar o xml de retorno.
                string xmlString = null;
                //Obtém resposta do servidor.
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    //Cria stream para obter retorno.
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        //Lê stream.
                        using (StreamReader reader = new StreamReader(dataStream))
                        {
                            //Xml convertido para string.
                            xmlString = reader.ReadToEnd();
                            //Cria xml document para facilitar acesso ao xml.
                            XmlDocument xmlDoc = new XmlDocument();
                            //Carrega xml document através da string com XML.
                            xmlDoc.LoadXml(xmlString);
                            //Busca elemento status do XML.
                            var status = xmlDoc.GetElementsByTagName("status")[0];
                            //Fecha reader.
                            reader.Close();
                            //Fecha stream.
                            dataStream.Close();
                            //Verifica status de retorno.
                            //3 = Pago. Outas Tags verificar na documentação no site do PagSeguro
                            retorno.Status = status.InnerText;
                        }
                    }
                    return retorno;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
