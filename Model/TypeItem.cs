using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace comdata_activiterDetector
{
    public class TypeItem
    {
        private int id;
        private String nom;

        public TypeItem()
        {

        }

        public TypeItem(int id, string nom)
        {
            this.Id = id;
            this.Nom = nom;
        }

        public int Id { get => id; set => id = value; }
        public string Nom { get => nom; set => nom = value; }
    }
}
