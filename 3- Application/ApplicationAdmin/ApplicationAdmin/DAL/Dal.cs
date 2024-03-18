using ApplicationAdmin.DAL.Factories;

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationAdmin.DAL
{
    public class Dal
    {
        private ClientFactory? clientFactory;
        private EvenementFactory? evenementFactory;
        private AdminFactory? adminFactory;
        private BilletFactory? billetFactory;
        private PanierFactory? panierFactory;
        private RapportFactory? rapportFactory;

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

        public AdminFactory AdminFactory
        {
            get
            {
                adminFactory ??= new AdminFactory();
                return adminFactory;
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

        public PanierFactory PanierFactory
        {
            get
            {
                panierFactory ??= new PanierFactory();
                return panierFactory;
            }
        }
        public RapportFactory RapportFactory
        {
            get
            {
                rapportFactory ??= new RapportFactory();
                return rapportFactory;
            }
        }
    }
}
