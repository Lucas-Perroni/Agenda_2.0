using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Text;

namespace Agenda_Lib.DAO.CEP
{
    public class Dados_cep
    {

        /// <summary>
        /// Chave de conexao ao banco de dados
        /// </summary>
        string Chaveconexao = "Data Source=10.39.45.44;Initial Catalog=Senac2022;User ID=Turma2022;Password=Turma2022@2022";


        /// <summary>
        /// Processo de consulta (SELECT )de CEP no banco de dados
        /// </summary>
        /// <param name="p_CEP"></param>
        /// <returns></returns>
        public DataSet List_CEP(string p_CEP)
        {
            DataSet DataSetCEP = new DataSet();
            try
            {
                SqlConnection Conexao = new SqlConnection(Chaveconexao);
                Conexao.Open();
                string wQuery = $"select * from CEP where CEP = '{p_CEP}'";
                SqlDataAdapter adapter = new SqlDataAdapter(wQuery, Conexao);
                adapter.Fill(DataSetCEP);
                Conexao.Open();
            }
            catch (Exception)
            {

                throw;
            }
            return DataSetCEP;
        }

        /// <summary>
        /// Apagar registro de CEP na tabela CEP 
        /// </summary>
        /// <param name="p_CEP"></param>
        public void Apagar_CEP(string p_CEP)
        {
            try
            {
                SqlConnection Conexao = new SqlConnection(Chaveconexao);
                Conexao.Open();
                string oQueryDelete = $"delete from CEP where CEP = '{p_CEP}'";
                SqlCommand cmd = new SqlCommand(oQueryDelete, Conexao);
                cmd.ExecuteNonQuery();


            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// Alterar registro de CEP na tebela CEP
        /// </summary>
        /// <param name="p_CEP"></param>
        public void Alterar_CEP(
            string p_cep,
            string p_logradouro,
            string p_complemento,
            string p_bairro,
            string p_localidade,
            string p_uf,
            string p_ibge,
            string p_gia,
            string p_ddd,
            string p_siafi )

            {
                try
                {
                SqlConnection Conexao = new SqlConnection(Chaveconexao);
                Conexao.Open();
                string oQueryDelete = "UPDATE cep " +
                           $"  SET logradouro      = '{p_logradouro}' " +
                           $"      ,complemento    = '{p_complemento}' " +
                           $"      ,bairro         = '{p_bairro}' " +
                           $"      ,localidade     = '{p_localidade}' " +
                           $"      ,uf             = '{p_uf}' " +
                           $"      ,ibge           = '{p_ibge}' " +
                           $"      ,gia            = '{p_gia}' " +
                           $"      ,ddd            = '{p_ddd}' " +
                           $"      ,siafi          = '{p_siafi}' " +
                           $"where cep             = '{p_cep} ' ";

                 SqlCommand cmd = new SqlCommand(oQueryDelete, Conexao);
                cmd.ExecuteNonQuery();
                Conexao.Close();
                }
                catch (Exception)
                {

                    throw;
                }

        }

        public void Adicionar_CEP(
            string p_cep,
            string p_logradouro,
            string p_complemento,
            string p_bairro,
            string p_localidade,
            string p_uf,
            string p_ibge,
            string p_gia,
            string p_ddd,
            string p_siafi)
        {
            try
            {

                SqlConnection conexao = new SqlConnection(Chaveconexao);
                conexao.Open();
                string oQueryUpdate = "INSERT INTO CEP " +
                           $"      ([cep]            " +
                           $"      ,[logradouro  ]   " +
                           $"      ,[complemento ]   " +
                           $"      ,[bairro      ]   " +
                           $"      ,[localidade  ]   " +
                           $"      ,[uf          ]   " +
                           $"      ,[ibge        ]   " +
                           $"      ,[gia         ]   " +
                           $"      ,[ddd         ]   " +
                           $"      ,[siafi       ] ) " +
                           $"   VALUES " +
                           $"       (   '{p_cep          }' " +
                           $"          ,'{p_logradouro   }' " +
                           $"          ,'{p_complemento  }' " +
                           $"          ,'{p_bairro       }' " +
                           $"          ,'{p_localidade   }' " +
                           $"          ,'{p_uf           }' " +
                           $"          ,'{p_ibge         }' " +
                           $"          ,'{p_gia          }' " +
                           $"          ,'{p_ddd          }' " +
                           $"          ,'{p_siafi        }' )"  ;


                SqlCommand cmd = new SqlCommand(oQueryUpdate, conexao);
                cmd.ExecuteNonQuery();
                conexao.Close();
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Consulta do CEP no Site Via CEP
        /// </summary>
        /// <param name="p_CEP"></param>
        /// <returns></returns>
        public ViaCep PesquisaCEP (string p_CEP)
        {
            ViaCep oviaCEP = new ViaCep();
            try
            {
                string oCEP = p_CEP;
                string oURL = "https://viacep.com.br/ws/" + oCEP + "/json/";

                HttpClient _httpClient = new HttpClient();
                HttpResponseMessage result = _httpClient.GetAsync(oURL).Result;
                String JsonRetorno =    result.Content.ReadAsStringAsync().Result;
                oviaCEP = JsonConvert.DeserializeObject<ViaCep>(JsonRetorno);

                add_cep(oviaCEP);
            }
            catch (Exception)
            {

                throw;
            }
            return oviaCEP;
           
        }

        public void  add_cep (ViaCep oviaCep) 
        {
            DataSet DataSetPesquisa = new DataSet();
            DataSetPesquisa = List_CEP(oviaCep.cep);

            if (DataSetPesquisa.Tables[0].Rows.Count == 0 )
            {
                Adicionar_CEP(
                oviaCep.cep,
                oviaCep.logradouro,
                oviaCep.complemento,
                oviaCep.bairro,
                oviaCep.logradouro,
                oviaCep.uf,
                oviaCep.ibge,
                oviaCep.gia,
                oviaCep.ddd,
                oviaCep.siafi);
            }
            else
            {
                Console.WriteLine($"Ja existe dados para este CEP {oviaCep.cep} quantidade de registro {DataSetPesquisa.Tables[0].Rows.Count.ToString()}");
            }


        
                
        }
    }
}
