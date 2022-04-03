using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Variant9
{
    class Agent
    {
        private int id;
        private string title;
        private int agentTypeId;
        private string address;
        private string inn;
        private string kpp;
        private string directorName;
        private string phone;
        private string email;
        private string logo;
        private int priority;

        public Agent() { }
        public Agent(int id, string title, int agentTypeId, string address, string inn, string kpp, string directorName, string phone, string email, string logo, int priority)
        {
            this.Id = id;
            this.Title = title;
            this.AgentTypeId = agentTypeId;
            this.Address = address;
            this.Inn = inn;
            this.Kpp = kpp;
            this.DirectorName = directorName;
            this.Phone = phone;
            this.Email = email;
            this.Logo = logo;
            this.Priority = priority;
        }

        public int Id { get => id; set => id = value; }
        public string Title { get => title; set => title = value; }
        public int AgentTypeId { get => agentTypeId; set => agentTypeId = value; }
        public string Address { get => address; set => address = value; }
        public string Inn { get => inn; set => inn = value; }
        public string Kpp { get => kpp; set => kpp = value; }
        public string DirectorName { get => directorName; set => directorName = value; }
        public string Phone { get => phone; set => phone = value; }
        public string Email { get => email; set => email = value; }
        public string Logo { get => logo; set => logo = value; }
        public int Priority { get => priority; set => priority = value; }

    }
} 
