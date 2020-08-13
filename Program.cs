using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZadacaRP3
{
    class Osoba : Fejs, IComparable<Osoba>
    {
        public string ime;
        public string prezime;
        private List<Osoba> Prijatelji = new List<Osoba>();
        public bool izbacena = false;
        private Osoba(string i, string p)
        {
            ime = i; prezime = p;
        }
        public static Osoba StvoriOsobu(string i, string p)
        {
            return new Osoba(i, p);
        }


        public int CompareTo(Osoba o)
        {
            if (this.brojPrijatelja() < o.brojPrijatelja())
                return -1;
            else if (this.brojPrijatelja() > o.brojPrijatelja())
                return 1;
            else
            {
                if (this.prezime == o.prezime)
                {
                    return this.ime.CompareTo(o.ime);
                }
                else
                {
                    return this.prezime.CompareTo(o.prezime);
                }
            }
        }

        // ugniježđena klasa koja implementira sučelje IComparer
        public class OsobaUsporedjivac : IComparer<Osoba>
        {
            // Enumeracija tipova za uspoređivanje
            public enum TipUsporedbe { Prijatelji, Prezime, Ime }
            // varijabla članica čuva odabrani tip usporedbe
            public Osoba.OsobaUsporedjivac.TipUsporedbe KojaUsporedba;

            // Implementacija sučelja IComparer<Osoba>
            public int Compare(Osoba x, Osoba y)
            {
                return x.CompareTo(y, this.KojaUsporedba);
            }
        }

        //posebna implementacija koju će pozvati prilagođeni uspoređivač
        public int CompareTo(Osoba o, Osoba.OsobaUsporedjivac.TipUsporedbe kojaUsporedba)
        {
            switch (kojaUsporedba)
            {
                case OsobaUsporedjivac.TipUsporedbe.Prijatelji:
                    {
                        if (this.Prijatelji.Count < o.Prijatelji.Count) return -1;
                        else if (this.Prijatelji.Count > o.Prijatelji.Count) return 1;
                        else return 0;
                    }
                case OsobaUsporedjivac.TipUsporedbe.Prezime:
                    return this.prezime.CompareTo(o.prezime);
                case OsobaUsporedjivac.TipUsporedbe.Ime:
                    return this.ime.CompareTo(o.ime);
            }
            return 0;
        }

        // statička metoda za uzimanje Comparer objekta 
        public static OsobaUsporedjivac DohvatiUsporedjivac()
        {
            return new OsobaUsporedjivac();
        }
        public int brojPrijatelja()
        {
            return this.Prijatelji.Count;
        }

        public List<Osoba> prijatelji()
        {
            return this.Prijatelji;
        }
        public List<Osoba> medjuPrijatelji(Osoba o2)
        {
            try
            {
                if (o2.izbacena) throw new System.Exception();
            }
            catch
            {
                Console.WriteLine("Koristimo izbacenu osobu.");
            }

            List<Osoba> MeduPrijatelji = new List<Osoba>();

            for (int i = 0; i < this.Prijatelji.Count; ++i)
                for (int j = 0; j < o2.Prijatelji.Count; ++j)
                {
                    if (this.Prijatelji[i] == o2.Prijatelji[j])
                        MeduPrijatelji.Add(this.Prijatelji[i]);
                }
            return MeduPrijatelji;

        }
        public static Osoba operator +(Osoba o1, Osoba o2)
        {
            try
            {
                if (o1.izbacena || o2.izbacena) throw new System.Exception();
            }
            catch
            {
                Console.WriteLine("Koristimo izbacenu osobu.");
            }

            if (o1.Prijatelji.IndexOf(o2) == -1)
            {
                o1.Prijatelji.Add(o2);
                o2.Prijatelji.Add(o1);
            }
            return o1;

        }
        public static Osoba operator -(Osoba o1, Osoba o2)
        {
            try
            {
                if (o1.izbacena || o2.izbacena) throw new System.Exception();
            }
            catch
            {
                Console.WriteLine("Koristimo izbacenu osobu.");
            };

            o1.Prijatelji.Remove(o2);
            o2.Prijatelji.Remove(o1);

            if (o1.brojPrijatelja() == 0)
            {
                osobe.Remove(o1);
                o1.izbacena = true;
            }

            if (o2.brojPrijatelja() == 0)
            {
                osobe.Remove(o2);
                o2.izbacena = true;
            }

            return o1;
        }

        public override string ToString()
        {
            return ime + " " + prezime + " " + this.brojPrijatelja();
        }
    }

    class IndeksPoImenu : IEnumerable<Osoba>
    {
        public List<Osoba> Lista = new List<Osoba>();
        public IndeksPoImenu(List<Osoba> lista)
        {
            foreach (Osoba o in lista)
                Lista.Add(o);
        }
        public Osoba this[string ime]
        {
            get
            {
                foreach (Osoba o in Lista)
                {
                    if (o.ime == ime)
                        return o;

                }
                return null;
            }
        }

        public IEnumerator<Osoba> GetEnumerator()
        {
            foreach (Osoba o in Lista)
                yield return o;
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }

    class Fejs : IEnumerable<Osoba>
    {
        static public List<Osoba> osobe = new List<Osoba>();

        public Osoba Dodaj(string i, string p)
        {
            Osoba o = Osoba.StvoriOsobu(i, p);
            osobe.Add(o);
            return o;
        }
        public IndeksPoImenu this[string p]
        {
            get
            {
                List<Osoba> lista = new List<Osoba>();
                for (int i = 0; i < osobe.Count(); i++)
                    if (osobe[i].prezime == p)
                        lista.Add(osobe[i]);
                return new IndeksPoImenu(lista);
            }
        }

        public void Izbaci(Osoba o)
        {
            try
            {
                if (o.izbacena) throw new System.Exception();
            }
            catch
            {
                Console.WriteLine("Koristimo izbacenu osobu.");
            }

            List<Osoba> prijateljiteosobe = new List<Osoba>();
            int brojPrijatelja = o.brojPrijatelja();
            for (int i = 0; i < brojPrijatelja; ++i)
            {
                prijateljiteosobe.Add(o.prijatelji()[i]);
            }
            for (int i = 0; i < brojPrijatelja; ++i)
            {
                prijateljiteosobe[i] -= o;
            }
            osobe.Remove(o);
            o.izbacena = true;
        }

        internal void Sort()
        {
            Osoba.OsobaUsporedjivac ou = Osoba.DohvatiUsporedjivac();
            ou.KojaUsporedba = Osoba.OsobaUsporedjivac.TipUsporedbe.Ime;
            osobe.Sort(ou);
            ou.KojaUsporedba = Osoba.OsobaUsporedjivac.TipUsporedbe.Prezime;
            osobe.Sort(ou);
            ou.KojaUsporedba = Osoba.OsobaUsporedjivac.TipUsporedbe.Prijatelji;
            osobe.Sort(ou);
        }

        internal void Sort(Osoba.OsobaUsporedjivac ou)
        {
            osobe.Sort(ou);
        }

        public IEnumerator<Osoba> GetEnumerator()
        {
            foreach (Osoba o in osobe)
                yield return o;
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Fejs fejs = new Fejs();
            Osoba A = fejs.Dodaj("Pero", "Peric");
            Osoba B = fejs.Dodaj("Ana", "Anic");
            Osoba C = fejs.Dodaj("Iva", "Ivic");
            Osoba D = fejs.Dodaj("Mirko", "Miric");
            Osoba E = fejs.Dodaj("Karla", "Karlic");
            Osoba F = fejs.Dodaj("Luka", "Lukic");

            //sprijateljimo neke osobe
            A += B; A += C; A += D; A += E;
            E += B; E += D; E += F;

            //ispisimo broj i imena prijatelja osobe A
            Console.WriteLine("Prijatelji osobe {0} {1} su: ", A.ime, A.prezime);
            foreach (Osoba o in A.prijatelji())
            {
                Console.WriteLine(o);
            }
            Console.WriteLine();

            //ispisimo broj i imena prijatelja osobe E
            Console.WriteLine("Prijatelji osobe {0} {1} su: ", E.ime, E.prezime);
            foreach (Osoba o in E.prijatelji())
            {
                Console.WriteLine(o);
            }
            Console.WriteLine();

            //ispisimo zajedničke prijatelje osoba A i E
            List<Osoba> zajednicki = A.medjuPrijatelji(E);
            Console.WriteLine("Zajednički prijatelji osoba {0} {1} i {2} {3} su: ", A.ime, A.prezime, E.ime, E.prezime);
            foreach (Osoba o in zajednicki)
            {
                Console.WriteLine(o);
            }
            Console.WriteLine();

            //indeksiranje
            Osoba G = fejs.Dodaj("Lana", "Lukic");
            Osoba H = fejs.Dodaj("Ines", "Lukic");
            Osoba I = fejs.Dodaj("Gloria", "Lukic");
            F += G; F += H; F += I;
            Console.WriteLine("Indeks po prezimenu Lukic: ");
            IndeksPoImenu pom = fejs["Lukic"];
            foreach (Osoba o in pom)
            {
                Console.WriteLine(o);
            }
            Console.WriteLine();
            Console.WriteLine("Indeks po imenu Lana: ");
            Console.WriteLine(pom["Lana"]);
            Console.WriteLine();

            //sortiranje samo po prezimenima
            Console.WriteLine("Sortiran ispis osoba na fejsu samo po prezimenu:");
            Osoba.OsobaUsporedjivac ou = Osoba.DohvatiUsporedjivac();
            ou.KojaUsporedba = Osoba.OsobaUsporedjivac.TipUsporedbe.Prezime;
            fejs.Sort(ou);
            foreach (Osoba o in fejs)
            {
                Console.WriteLine(o);
            }
            Console.WriteLine();

            //sortiranje
            Console.WriteLine("Sortiran ispis osoba na fejsu:");
            fejs.Sort();
            foreach (Osoba o in fejs)
            {
                Console.WriteLine(o);
            }
            Console.WriteLine();

            //izbaci osobu 
            Console.WriteLine("Izbacivanje osobe " + B);
            fejs.Izbaci(B);
            Console.WriteLine();
            Console.WriteLine("Nova lista prijatelja osobe {0} {1} je: ", A.ime, A.prezime);
            foreach (Osoba o in A.prijatelji())
            {
                Console.WriteLine(o);
            }
            Console.WriteLine();
            Console.WriteLine("Nova lista prijatelja osobe {0} {1} je: ", E.ime, E.prezime);
            foreach (Osoba o in E.prijatelji())
            {
                Console.WriteLine(o);
            }
            Console.WriteLine();

            //posvadajmo neke osobe
            Console.WriteLine("Stara lista prijatelja osobe {0} {1} je: ", F.ime, F.prezime);
            foreach (Osoba o in F.prijatelji())
            {
                Console.WriteLine(o);
            }
            Console.WriteLine();

            F -= H; F -= I;
            Console.WriteLine("Posvadali smo {0} {1} s {2} {3} i {4} {5}.", F.ime, F.prezime, H.ime, H.prezime, I.ime, I.prezime);
            Console.WriteLine();

            Console.WriteLine("Nova lista prijatelja osobe {0} {1} je: ", F.ime, F.prezime);
            foreach (Osoba o in F.prijatelji())
            {
                Console.WriteLine(o);
            }
            Console.WriteLine();

            //izbacili smo s fejsa osobe bez prijatelja - sortiran ispis
            Console.WriteLine("Novi popis osoba na fejsu (sortirano): ");
            fejs.Sort();
            foreach (Osoba o in fejs)
            {
                Console.WriteLine(o);
            }
            Console.WriteLine();

            //koristimo izbacenu osobu B, zatim H
            Console.WriteLine("Sprijateljimo osobe A i B:");
            A += B;
            Console.WriteLine();

            Console.WriteLine("Izbacimo osobu H:");
            fejs.Izbaci(H);
        }
    }
}

