using System.Collections.Generic;
using System.Drawing;
using System;

namespace Photo
{
    /// <summary>
    /// Interfejs implementowany przez klasy które chc¹ wykonywaæ operacje na bitmapach.
    /// </summary>
    public interface IOperacja
    {
        /// <summary>
        /// Zwraca nazwê operacji.
        /// </summary>
        string NazwaOperacji
        {
            get;
        }
        /// <summary>
        /// Zwraca kod operacji, kod jest przydzielany przez aplikacjê podczas uruchamiania.
        /// S³u¿y do jednoznacznej identyfikacji operacji (nazwa byæ nie musi).
        /// </summary>
        int KodOperacji
        {
            get;
            set;
        }
        /// <summary>
        /// Ikona wyœwietlana w menu operacji.
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
        /// Wywo³ywana gdy u¿ytkownik chce wykonaæ operacjê. Ma na celu zebranie potrzebnych argumentów, np.
        /// przez wyœwietlenie okna dialogowego.
        /// </summary>
        /// <returns>Wartoœci argumentów ustalone przez u¿ytkownika.</returns>
        Stack<object> PodajArgumenty();
        /// <summary>
        /// Wykonuje zaimplementowan¹ operacje graficzn¹ na bitmapie.
        /// </summary>
        /// <param name="z">Zdjecie na którym ma zostaæ wykonana operacja.</param>
        /// <param name="Argumenty">Argumenty dla operacji. Na dnie stosu le¿y argument pierwszy.</param>
        void Wykonaj(Zdjecie z, Stack<object> Argumenty);

        /// <summary>
        /// Metoda zwracajaca informacje, czy operacja ma zostac umieszczona
        /// na toolbarze, czy nie.
        /// </summary>
        /// <returns>Czy umiescic na toolbarze</returns>
        bool CzyNaToolbar();
    }

    /// <summary>
    /// Delegat informuj¹cy o wybraniu zdjêcia z pewnego zbioru.
    /// </summary>
    /// <param name="zdjecie">Wybrany obiekt.</param>
    public delegate void WybranoZdjecieDelegate(Zdjecie zdjecie);

    /// <summary>
    /// Delegat informuj¹cy o wybraniu katalogu z pewnego zbioru.
    /// </summary>
    /// <param name="katalog">Wybrany obiekt.</param>
    public delegate void WybranoKatalogDelegate(Katalog katalog);

    /// <summary>
    /// Delegat informuj¹cy o zaznaczeniu zdjêcia z pewnego zbioru.
    /// </summary>
    /// <param name="zdjecie">Wybrany obiekt.</param>
    public delegate void ZaznaczonoZdjecieDelegate(Zdjecie zdjecie);
    /// <summary>
    /// Delegat informuj¹cy o wlaczeniu widoku zdjecia
    /// </summary>
    public delegate void WlaczonoWidokZdjeciaDelegate();
    /// <summary>
    /// Delegat informuj¹cy o wylaczeniu widoku zdjecia
    /// </summary>
    public delegate void WylaczonoWidokZdjeciaDelegate();

    /// <summary>
    /// Delegat informuj¹cy o wylaczeniu widoku zdjecia
    /// </summary>
    public delegate bool ZabierzFocusDelegate();

    /// <summary>
    /// Delegat informuj¹cy o zmianie tagow
    /// </summary>
    public delegate void ZmieninoTagiDelegate(List<long> t);

    /// <summary>
    /// Interfejs który przechowuje zbiór zdjêæ (elementów implementuj¹cych interfejs IZdjecie).
    /// Powinien znaæ podzbiór zdjêæ wybranych przez u¿ytkownika.
    /// Posiada stan Edycja. Jeœli stan Edycja jest aktywny opakowanie zbiera polecenia operacji 
    /// (PolecenieOperacji), które wykona na zdjêciach wybranych przez u¿ytkownika przy zmianie stanu 
    /// Edycja na nieaktywny.
    /// </summary>
    /// <seealso cref="IZdjecie"/>
    public interface IOpakowanieZdjec
    {
        /// <summary>
        /// Indekser pozwalaj¹cy uzyskaæ zdjêcie (IZdjecie) ze zbioru.
        /// </summary>
        /// <param name="numer">Indeks zdjêcia.</param>
        /// <returns></returns>
        IZdjecie this[int numer]
        {
            get;
        }
        /// <summary>
        /// Zwraca liczbe przechowywanych zdjêæ.
        /// </summary>
        int Ilosc
        {
            get;
        }
        /// <summary>
        /// Umieszcza obiekt typu IZdjecie w zbiorze.
        /// </summary>
        /// <param name="zdjecie">Obiekt który chcemy dodaæ.</param>
        void Dodaj(IZdjecie zdjecie);
        /// <summary>
        /// Usuwa zdjêcie ze zbioru.
        /// </summary>
        /// <param name="zdjecie">Obiekt który chcemy usun¹æ.</param>
        void Usun(IZdjecie zdjecie);
        /// <summary>
        /// Powoduje ze opakowanie staje siê puste.
        /// </summary>
        void Oproznij();
        /// <summary>
        /// Zwraca zdjêcia wybrane przez u¿ytkownika.
        /// </summary>
        /// <returns></returns>
        IZdjecie[] WybraneZdjecia
        {
            get;
        }
        /// <summary>
        /// Rozpoczyna edycjê (stan Edycja staje sie aktywny).
        /// Jeœli stan Edycja by³ aktywny ju¿ wczeœniej - nie wykonuje nic.
        /// </summary>
        void RozpocznijEdycje();
        /// <summary>
        /// Zmienia stan Edycja na nieaktywny.
        /// Jeœli stan edycja by³ aktywny, wykonuje wszystkie polecenia operacji od momentu 
        /// ostatniej zmiany stanu Edycja z nieaktywnego na aktywny.
        /// </summary>
        void ZakonczEdycje();
        /// <summary>
        /// Dodaje polecenie operacji do zdjêæ wybranych przez u¿ytkownika.
        /// Jeœli stan Edycja jest nieaktywny, to wykonuje operacjê natychmiast.
        /// </summary>
        /// <param name="operacja">Obiekt polecenia operacji która ma zostaæ wykonana.</param>
        void DodajOperacje(PolecenieOperacji operacja);
        /// <summary>
        /// Metoda usuwajaca wszystkie operacje na zdjeciu/ach
        /// </summary>
        void UsunWszystkieOperacje();
        /// <summary>
        /// Zdarzenie informuj¹ce o wskazaniu nowego zdjêcia przez u¿ytkownika.
        /// Gdy nie jest wybrane ¿adne zdjêcie, delegaci otrzymuj¹ wartoœæ null jako argument.
        /// </summary>
        /// <seealso cref="WybranoZdjecieDelegate"/>
        event WybranoZdjecieDelegate WybranoZdjecie;
        /// <summary>
        /// Powoduje ¿e opakowanie wype³nia siê nowym zbiorem zdjêæ.
        /// </summary>
        /// <param name="zdjecia">Nowy zbiór zdjec dla opakowania.</param>
        /// <param name="katalogi">Nowy zbior katalogow dla opakowania</param>
        /// <param name="CzyZDrzewa">Czy zdjecia pochodza z drzewa katalogow</param>
        void Wypelnij(IZdjecie[] zdjecia, Katalog[] katalogi, bool CzyZDrzewa);
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
    /// Delagat zdarzenia modyfikacji zdjêcia.
    /// </summary>
    /// <param name="zdjecie">Zdjêcie które zosta³o zmodyfikowane.</param>
    /// <param name="kontekst">w jakim kontekscie zdjêcie zosta³o zdodyfikowane</param>
    /// <param name="rodzaj">jaki rodzaj modyfikacji</param>
    public delegate void ZmodyfikowanoZdjecieDelegate(IKontekst kontekst, IZdjecie zdjecie, RodzajModyfikacjiZdjecia rodzaj);

    /// <summary>
    /// Interfejs opisuj¹cy zdjêcie.
    /// </summary>
    public interface IZdjecie
    {
        /// <summary>
        /// Dodaje polecenie operacji do zdjêcia. Lista operacji zostanie wykonana metod¹ WykonajOperacje.
        /// </summary>
        /// <param name="operacja">Obiekt polecenia operacji do wykonania.</param>
        void DodajOperacje(PolecenieOperacji operacja);
        /// <summary>
        /// Wykonuje listê operacji dodanych metod¹ DodajOperacje.
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
        /// <summary>
        /// Propercja zwraca lub ustawia miniature zdjecia
        /// </summary>
        Bitmap Miniatura
        {
            get;
            set;
        }
        /// <summary>
        /// Propercja zwraca nazwe pliku
        /// </summary>
        string NazwaPliku
        {
            get;
        }

        /// <summary>
        /// Propercja zwraca rozmiar zdjecia
        /// </summary>
        Size Rozmiar
        {
            get;
        }

        /// <summary>
        /// Zdarzenie informuj¹ce o modyfikacji zdjêcia.
        /// </summary>
        /// <seealso cref="ZmodyfikowanoZdjecieDelegate"/>
        event ZmodyfikowanoZdjecieDelegate ZmodyfikowanoZdjecie;
    }

    /// <summary>
    /// Interfejs dla obiektów które chc¹ byæ kontekstem w którym pojawia siê zdjêcie.
    /// Umo¿liwia to reakcjê na zmiany zachodz¹ce na obiekcie IZdjecie, które znajduje
    /// siê w wielu kontekstach przez ka¿dy z kontekstów.
    /// </summary>
    public interface IKontekst
    {
        /// <summary>
        /// Umieszcza obiekt typu IZdjecie w kontekœcie.
        /// </summary>
        /// <param name="zdjecie">Obiekt który chcemy dodaæ.</param>
        void DodajDoKontekstu(IZdjecie zdjecie);
        /// <summary>
        /// Usuwa zdjêcie z tego kontekstu.
        /// </summary>
        /// <param name="zdjecie">Obiekt który chcemy usun¹æ.</param>
        void UsunZKontekstu(IZdjecie zdjecie);
        /// <summary>
        /// Wywo³ywany w momencie modyfikacji zdjêcia przez któryœ z kontekstów.
        /// </summary>
        /// <param name="kontekst">Kontekst który dokona³ modyfikacji.</param>
        /// <param name="zdjecie">Zdjêcie które zosta³o zmodyfikowane.</param>
        /// <param name="rodzaj">Rodzaj modyfikacji.</param>
        void ZmodyfikowanoZdjecie(IKontekst kontekst, IZdjecie zdjecie, RodzajModyfikacjiZdjecia rodzaj);
    }

    /// <summary>
    /// Delegat informuj¹cy o zakoñczeniu wyszukiwania.
    /// </summary>
    /// <param name="zdjecia">Lista zdjêæ bed¹ca wynikiem wyszukiwania.</param>
    public delegate void ZakonczonoWyszukiwanieDelegate(IZdjecie[] zdjecia, Katalog[] katalogi, bool CzyZDrzewa);

    /// <summary>
    /// Delegat informujacy o rozpoczeciu wyszukiwania
    /// </summary>
    /// <param name="wyszukiwanie">Rozpoczete wyszkukiwanie</param>
    public delegate void RozpoczetoWyszukiwanieDelegate(IWyszukiwanie wyszukiwanie);

    public delegate void ZnalezionoZdjecieDelegate(IZdjecie zdjecie);

    /// <summary>
    /// Delegat informujacy o zmianie zrodla zdjec
    /// </summary>
    /// <param name="dir">Nowe zrodlo zdjec</param>
    public delegate void ZmienionoZrodloDelegate(string dir);

    /// <summary>
    /// Delegat informujacy o zmianie tagow
    /// </summary>
    public delegate void ZmienionoTagiDelegate();

    /// <summary>
    /// Delegat informujacy o zmianie identyfikatora
    /// </summary>
    public delegate void ZmienionoIdsDelegate();

    /// <summary>
    /// Interfejs dla obiektu wyszukuj¹cego zdjêcia.
    /// </summary>
    public interface IWyszukiwacz
    {
        /// <summary>
        /// Metoda wykonuje proces wyszukiwania. 
        /// </summary>
        /// <returns></returns>
        IWyszukiwanie Wyszukaj();
        /// <summary>
        /// delegat informujê aplikacje ze wyszukiwanie zdjêæ siê zakoñczy³o
        /// </summary>
        event ZakonczonoWyszukiwanieDelegate ZakonczonoWyszukiwanie;
        /// <summary>
        /// delegat informujê aplikacje ze wyszukiwanie zdjêæ siê rozpocze³o
        /// </summary>
        event RozpoczetoWyszukiwanieDelegate RozpoczetoWyszukiwanie;
        //event ZnalezionoZdjecieDelegate ZnalezionoZdjecie;
    }

    /// <summary>
    /// Interfejs dla obiektów wyra¿eñ wyszukuj¹cych, które mo¿na bezpoœrednio zamieniæ na 
    /// rezultat (zbiór obiektów typu IZdjêcie) bêd¹cy wynikiem tego wyra¿enia.
    /// Umo¿liwia sk³adanie wyra¿eñ spójnikami logicznymi.
    /// </summary>
    public interface IWyszukiwanie
    {
        /// <summary>
        /// Koniunkcja z innym wyra¿eniem.
        /// </summary>
        /// <param name="W">IWyszukiwanie bêd¹ce drugim argumentem koniunkcji.</param>
        void And(IWyszukiwanie W);
        /// <summary>
        /// Alternatywa z innym wyra¿eniem.
        /// </summary>
        /// <param name="W">IWyszukiwanie bêd¹ce drugim argumentem alternatywy.</param>
        void Or(IWyszukiwanie W);
        /// <summary>
        /// Koniunkcja tego wyra¿enia z warunkiem dla relacji.
        /// Na przyk³ad: warunek dla 'data_wykonania' w relacji 'zdjecie'.
        /// </summary>
        /// <param name="Relacja"></param>
        /// <param name="Warunek"></param>
        void And(string Relacja, string Warunek);
        void Or(string Relacja, string Warunek);
        void And(string Wyrazenie);
        void Or(string Wyrazenie);
        /// <summary>
        /// Zwraca tablicê zdjêc bêd¹c¹ rezultatem tego wyra¿enia wyszukuj¹cego.
        /// </summary>
        /// <returns></returns>
        IZdjecie[] PodajWynik();
    }
}