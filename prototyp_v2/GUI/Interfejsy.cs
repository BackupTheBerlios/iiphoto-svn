
using System.Collections.Generic;
using System.Drawing;

namespace Photo
{
    /// <summary>
    /// Interfejs implementowany przez klasy kt�re chc� wykonywa� operacje na bitmapach.
    /// </summary>
    public interface IOperacja
    {
        /// <summary>
        /// Zwraca nazw� operacji.
        /// </summary>
        string NazwaOperacji
        {
            get;
        }
        /// <summary>
        /// Zwraca kod operacji, kod jest przydzielany przez aplikacj� podczas uruchamiania.
        /// S�u�y do jednoznacznej identyfikacji operacji (nazwa by� nie musi).
        /// </summary>
        int KodOperacji
        {
            get;
            set;
        }
        /// <summary>
        /// Ikona wy�wietlana w menu operacji.
        /// </summary>
        Image Ikona
        {
            get;
        }
        /// <summary>
        /// Autor implementacji.
        /// </summary>
        string Autor
        {
            get;
        }
        /// <summary>
        /// Wersja implementacji.
        /// </summary>
        string Wersja
        {
            get;
        }
        /// <summary>
        /// Kontakt do autora implementacji.
        /// </summary>
        string Kontakt
        {
            get;
        }
        /// <summary>
        /// Wywo�ywana gdy u�ytkownik chce wykona� operacj�. Ma na celu zebranie potrzebnych argument�w, np.
        /// przez wy�wietlenie okna dialogowego.
        /// </summary>
        /// <returns>Warto�ci argument�w ustalone przez u�ytkownika.</returns>
        Stack<object> PodajArgumenty();
        /// <summary>
        /// Wykonuje zaimplementowan� operacje graficzn� na bitmapie.
        /// </summary>
        /// <param name="Bitmap">Bitmapa na kt�rej ma zosta� wykonana operacja.</param>
        /// <param name="Argumenty">Argumenty dla operacji. Na dnie stosu le�y argument pierwszy.</param>
        void Wykonaj(Bitmap Bitmap, Stack<object> Argumenty);
    }

    /// <summary>
    /// Delegat informuj�cy o wybraniu zdj�cia z pewnego zbioru.
    /// </summary>
    /// <param name="zdjecie">Wybrany obiekt.</param>
    public delegate void WybranoZdjecieDelegate(IZdjecie zdjecie);

    /// <summary>
    /// Interfejs kt�ry przechowuje zbi�r zdj�� (element�w implementuj�cych interfejs IZdjecie).
    /// Powinien zna� podzbi�r zdj�� wybranych przez u�ytkownika.
    /// Posiada stan Edycja. Je�li stan Edycja jest aktywny opakowanie zbiera polecenia operacji 
    /// (PolecenieOperacji), kt�re wykona na zdj�ciach wybranych przez u�ytkownika przy zmianie stanu 
    /// Edycja na nieaktywny.
    /// </summary>
    /// <seealso cref="IZdjecie"/>
    public interface IOpakowanieZdjec
    {
        /// <summary>
        /// Indekser pozwalaj�cy uzyska� zdj�cie (IZdjecie) ze zbioru.
        /// </summary>
        /// <param name="numer">Indeks zdj�cia.</param>
        /// <returns></returns>
        IZdjecie this[int numer]
        {
            get;
        }
        /// <summary>
        /// Zwraca liczbe przechowywanych zdj��.
        /// </summary>
        int Ilosc
        {
            get;
        }
        /// <summary>
        /// Umieszcza obiekt typu IZdjecie w zbiorze.
        /// </summary>
        /// <param name="zdjecie">Obiekt kt�ry chcemy doda�.</param>
        void Dodaj(IZdjecie zdjecie);
        /// <summary>
        /// Usuwa zdj�cie ze zbioru.
        /// </summary>
        /// <param name="zdjecie">Obiekt kt�ry chcemy usun��.</param>
        void Usun(IZdjecie zdjecie);
        /// <summary>
        /// Powoduje ze opakowanie staje si� puste.
        /// </summary>
        void Oproznij();
        /// <summary>
        /// Zwraca zdj�cia wybrane przez u�ytkownika.
        /// </summary>
        /// <returns></returns>
        IZdjecie[] WybraneZdjecia();
        /// <summary>
        /// Rozpoczyna edycj� (stan Edycja staje sie aktywny).
        /// Je�li stan Edycja by� aktywny ju� wcze�niej - nie wykonuje nic.
        /// </summary>
        void RozpocznijEdycje();
        /// <summary>
        /// Zmienia stan Edycja na nieaktywny.
        /// Je�li stan edycja by� aktywny, wykonuje wszystkie polecenia operacji od momentu 
        /// ostatniej zmiany stanu Edycja z nieaktywnego na aktywny.
        /// </summary>
        void ZakonczEdycje();
        /// <summary>
        /// Dodaje polecenie operacji do zdj�� wybranych przez u�ytkownika.
        /// Je�li stan Edycja jest nieaktywny, to wykonuje operacj� natychmiast.
        /// </summary>
        /// <param name="operacja">Obiekt polecenia operacji kt�ra ma zosta� wykonana.</param>
        void DodajOperacje(PolecenieOperacji operacja);
        void UsunWszystkieOperacje();
        /// <summary>
        /// Zdarzenie informuj�ce o wskazaniu nowego zdj�cia przez u�ytkownika.
        /// Gdy nie jest wybrane �adne zdj�cie, delegaci otrzymuj� warto�� null jako argument.
        /// </summary>
        /// <seealso cref="WybranoZdjecieDelegate"/>
        event WybranoZdjecieDelegate WybranoZdjecie;
        /// <summary>
        /// Powoduje �e opakowanie wype�nia si� nowym zbiorem zdj��.
        /// </summary>
        /// <param name="zdjecia">Nowy zbi�r obiekt�w dla opakowania.</param>
        void Wypelnij(IZdjecie[] zdjecia);
    }

    public enum RodzajModyfikacjiZdjecia
    {
        Zawartosc,
        Metadane,
        Lokalizacja,
        UsunietoZBazy,
        UsunietoZDysku,
        DodanoDoBazy
    }

    /// <summary>
    /// Delagat zdarzenia modyfikacji zdj�cia.
    /// </summary>
    /// <param name="zdjecie">Zdj�cie kt�re zosta�o zmodyfikowane.</param>
    public delegate void ZmodyfikowanoZdjecieDelegate(IKontekst kontekst, IZdjecie zdjecie, RodzajModyfikacjiZdjecia rodzaj);

    /// <summary>
    /// Interfejs opisuj�cy zdj�cie.
    /// </summary>
    public interface IZdjecie
    {
        /// <summary>
        /// Dodaje polecenie operacji do zdj�cia. Lista operacji zostanie wykonana metod� WykonajOperacje.
        /// </summary>
        /// <param name="operacja">Obiekt polecenia operacji do wykonania.</param>
        void DodajOperacje(PolecenieOperacji operacja);
        /// <summary>
        /// Wykonuje list� operacji dodanych metod� DodajOperacje.
        /// </summary>
        void WykonajOperacje();
        /// <summary>
        /// Usuwa wszystkie polecenia operacji z listy.
        /// </summary>
        void UsunWszystkieOperacje();
        /// <summary>
        /// </summary>
        Bitmap Duze
        {
            get;
        }
        Bitmap Miniatura
        {
            get;
            set;
        }
        string NazwaPliku
        {
            get;
        }
        /// <summary>
        /// Zdarzenie informuj�ce o modyfikacji zdj�cia.
        /// </summary>
        /// <seealso cref="ZmodyfikowanoZdjecieDelegate"/>
        event ZmodyfikowanoZdjecieDelegate ZmodyfikowanoZdjecie;
    }

    /// <summary>
    /// Interfejs dla obiekt�w kt�re chc� by� kontekstem w kt�rym pojawia si� zdj�cie.
    /// Umo�liwia to reakcj� na zmiany zachodz�ce na obiekcie IZdjecie, kt�re znajduje
    /// si� w wielu kontekstach przez ka�dy z kontekst�w.
    /// </summary>
    public interface IKontekst
    {
        /// <summary>
        /// Umieszcza obiekt typu IZdjecie w kontek�cie.
        /// </summary>
        /// <param name="zdjecie">Obiekt kt�ry chcemy doda�.</param>
        void DodajDoKontekstu(IZdjecie zdjecie);
        /// <summary>
        /// Usuwa zdj�cie z tego kontekstu.
        /// </summary>
        /// <param name="zdjecie">Obiekt kt�ry chcemy usun��.</param>
        void UsunZKontekstu(IZdjecie zdjecie);
        /// <summary>
        /// Wywo�ywany w momencie modyfikacji zdj�cia przez kt�ry� z kontekst�w.
        /// </summary>
        /// <param name="kontekst">Kontekst kt�ry dokona� modyfikacji.</param>
        /// <param name="zdjecie">Zdj�cie kt�re zosta�o zmodyfikowane.</param>
        /// <param name="rodzaj">Rodzaj modyfikacji.</param>
        void ZmodyfikowanoZdjecie(IKontekst kontekst, IZdjecie zdjecie, RodzajModyfikacjiZdjecia rodzaj);
    }

    /// <summary>
    /// Delegat informuj�cy o zako�czeniu wyszukiwania.
    /// </summary>
    /// <param name="zdjecia">Lista zdj�� bed�ca wynikiem wyszukiwania.</param>
    public delegate void ZakonczonoWyszukiwanieDelegate(IZdjecie[] zdjecia);
    public delegate void RozpoczetoWyszukiwanieDelegate(IWyszukiwanie wyszukiwanie);
    public delegate void ZnalezionoZdjecieDelegate(IZdjecie zdjecie);

    /// <summary>
    /// Interfejs dla obiektu wyszukuj�cego zdj�cia.
    /// </summary>
    public interface IWyszukiwacz
    {
        /// <summary>
        /// Metoda wykonuje proces wyszukiwania. 
        /// </summary>
        /// <returns></returns>
        IWyszukiwanie Wyszukaj();
        event ZakonczonoWyszukiwanieDelegate ZakonczonoWyszukiwanie;
        event RozpoczetoWyszukiwanieDelegate RozpoczetoWyszukiwanie;
        event ZnalezionoZdjecieDelegate ZnalezionoZdjecie;
    }

    /// <summary>
    /// Interfejs dla obiekt�w wyra�e� wyszukuj�cych, kt�re mo�na bezpo�rednio zamieni� na 
    /// rezultat (zbi�r obiekt�w typu IZdj�cie) b�d�cy wynikiem tego wyra�enia.
    /// Umo�liwia sk�adanie wyra�e� sp�jnikami logicznymi.
    /// </summary>
    public interface IWyszukiwanie
    {
        /// <summary>
        /// Koniunkcja z innym wyra�eniem.
        /// </summary>
        /// <param name="W">IWyszukiwanie b�d�ce drugim argumentem koniunkcji.</param>
        void And(IWyszukiwanie W);
        /// <summary>
        /// Alternatywa z innym wyra�eniem.
        /// </summary>
        /// <param name="W">IWyszukiwanie b�d�ce drugim argumentem alternatywy.</param>
        void Or(IWyszukiwanie W);
        /// <summary>
        /// Koniunkcja tego wyra�enia z warunkiem dla relacji.
        /// Na przyk�ad: warunek dla 'data_wykonania' w relacji 'zdjecie'.
        /// </summary>
        /// <param name="Relacja"></param>
        /// <param name="Wartosc"></param>
        void And(string Relacja, string Warunek);
        void Or(string Relacja, string Warunek);
        void And(string Wyrazenie);
        void Or(string Wyrazenie);
        /// <summary>
        /// Zwraca tablic� zdj�c b�d�c� rezultatem tego wyra�enia wyszukuj�cego.
        /// </summary>
        /// <returns></returns>
        IZdjecie[] PodajWynik();
    }
}