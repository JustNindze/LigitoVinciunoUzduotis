using System;
using System.Collections.Generic;
using Excel.FinancialFunctions;

namespace LigitoVinciunoUzduotis
{
    class MainClass
    {
        // paskolos suma nurodoma eurais
        private const double paskolosSuma = 3000;
        // terminas nurodomas menesiais
        private const int terminas = 60;
        // metine palukanu norma nurodoma procentais
        private const double metinePalukanuNorma = 5;
        //mokejimo diena nurodoma skaiciumi nuo 1 iki 31
        private const int mokejimoDiena = 15;

        static void Main(string[] args)
        {
            Console.WriteLine("Programa paleista\n");

            List<lentele> lenteleList = new List<lentele>();

            DateTime currentDate = DateTime.Today;

            lenteleList.Add(new lentele
            {
                nr = 1,
                data = currentDate,
                pastabos = "Paskolos išmokėjimas"
            });

            List<double> palukanosIrPaskolosList = new List<double>();
            List<DateTime> datosList = new List<DateTime>();

            datosList.Add(currentDate);
            palukanosIrPaskolosList.Add(-paskolosSuma);

            double imoka = PMT(metinePalukanuNorma, terminas, paskolosSuma);
            double likutis = paskolosSuma;

            for (int n = 0; n < terminas; n++)
            {
                double palukanos = likutis * (metinePalukanuNorma / 100 / 12);
                double paskola = imoka - palukanos;

                currentDate = currentDate.AddMonths(1);

                int paskutineMenesioDiena = (int)DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
                int correctMokejimoDiena = mokejimoDiena;

                if (paskutineMenesioDiena < mokejimoDiena)
                {
                    correctMokejimoDiena = paskutineMenesioDiena;
                }

                likutis = likutis - paskola;

                DateTime data = new DateTime(currentDate.Year, currentDate.Month, correctMokejimoDiena);

                lenteleList.Add(new lentele
                {
                    nr = n + 2,
                    data = data,
                    pastabos = ""
                });

                palukanosIrPaskolosList.Add(palukanos + paskola);
                datosList.Add(data);
            }

            double bvkkmn = Financial.XIrr(palukanosIrPaskolosList, datosList, 0) * 100;

            Console.WriteLine("Lentelė:\n");
            Console.WriteLine("Nr.\tData\t\tPastabos");

            foreach (lentele element in lenteleList)
            {
                Console.WriteLine("{0}\t{1}\t{2}", element.nr, element.data.ToString("yyyy-MM-dd"), element.pastabos);
            }

            Console.WriteLine("\nApskaičiuotas BVKKMN: {0}\n", bvkkmn);
        }

        //suskaiciuoja ir grazina paskolos imoka pagal pastovaus dydzio mokejimus ir pastovu palukanu procenta
        static double PMT(double metinePalukanuNorma, int terminas, double paskolosSuma)
        {
            var norma = metinePalukanuNorma / 100 / 12;
            var vardiklis = Math.Pow((1 + norma), terminas) - 1;
            return (norma + (norma / vardiklis)) * paskolosSuma;
        }

    }

    class lentele
    {
        public int nr { get; set; }
        public DateTime data { get; set; }
        public string pastabos { get; set; }
    }
}
