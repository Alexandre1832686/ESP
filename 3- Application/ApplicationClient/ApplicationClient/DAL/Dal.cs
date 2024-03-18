 using ApplicationClient.DAL.Factories;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationClient.DAL
{
    public class Dal
    {
        private ClientFactory? clientFactory;
        private EvenementFactory? evenementFactory;
        private PanierFactory? panierFactory;
        private BilletFactory? billetFactory;


        public string? ConectionString
        {
            get
            {
                return "Server=sql.decinfo-cchic.ca;Port=33306;Database=a23_déploiement_a23_1832686;Uid=dev-1832686;Pwd=Info2020";
            }
        }

        public ClientFactory ClientFactory
        {
            get
            {
                clientFactory ??= new ClientFactory();
                return clientFactory;
            }
        }

        public EvenementFactory EvenementFactory
        {
            get
            {
                evenementFactory ??= new EvenementFactory();
                return evenementFactory;
            }
        }
        public PanierFactory PanierFactory
        {
            get
            {
                panierFactory ??= new PanierFactory();
                return panierFactory;
            }
        }
        public BilletFactory BilletFactory
        {
            get
            {
                billetFactory ??= new BilletFactory();
                return billetFactory;
            }
        }
    }
}
