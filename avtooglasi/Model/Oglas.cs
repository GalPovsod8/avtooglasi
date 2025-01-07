using System.ComponentModel;
using System.Xml.Serialization;

namespace avtooglasi.Model
{
    public enum TipPonudbe
    {
        Prodaja,
        Najem,
    }

    public enum Starost
    {
        Novo,
        Testno,
        Rabljeno
    }

    public enum KaroserijskaIzvedba
    {
        Limuzina,
        Kombilimuzina,
        Karavan,
        Enoprostorec,
        Coupe,
        Cabrio,
        SUV,
        Pick_up
    }

    [Serializable, XmlRoot("Oglas")]
    public class Oglas : INotifyPropertyChanged
    {
        private string _naziv;
        private string _opis;
        private double _cena;
        private string _prodajalec;
        private string? _thumbnailLink;
        private TipPonudbe _ponudba;
        private Starost _avtoStarost;
        private KaroserijskaIzvedba _karoserijskaIzvedba;
        private string _znamka;

        [XmlElement("Znamka")]
        public string Znamka
        {
            get => _znamka;
            set
            {
                if (_znamka != value)
                {
                    _znamka = value;
                    OnPropertyChanged(nameof(Znamka));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public string Naziv
        {
            get => _naziv;
            set
            {
                if (_naziv != value)
                {
                    _naziv = value;
                    OnPropertyChanged(nameof(Naziv));
                }
            }
        }

        public string Opis
        {
            get => _opis;
            set
            {
                if (_opis != value)
                {
                    _opis = value;
                    OnPropertyChanged(nameof(Opis));
                }
            }
        }

        public double Cena
        {
            get => _cena;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Cena mora biti večja od 0.");
                }
                _cena = value;
                OnPropertyChanged(nameof(Cena));
            }
        }

        public string Prodajalec
        {
            get => _prodajalec;
            set
            {
                if (_prodajalec != value)
                {
                    _prodajalec = value;
                    OnPropertyChanged(nameof(Prodajalec));
                }
            }
        }

        public string? ThumbnailLink
        {
            get => _thumbnailLink;
            set
            {
                if (_thumbnailLink != value)
                {
                    _thumbnailLink = value;
                    OnPropertyChanged(nameof(ThumbnailLink));
                }
            }
        }

        public TipPonudbe Ponudba
        {
            get => _ponudba;
            set
            {
                if (_ponudba != value)
                {
                    _ponudba = value;
                    OnPropertyChanged(nameof(Ponudba));
                }
            }
        }

        public Starost AvtoStarost
        {
            get => _avtoStarost;
            set
            {
                if (_avtoStarost != value)
                {
                    _avtoStarost = value;
                    OnPropertyChanged(nameof(AvtoStarost));
                }
            }
        }

        public KaroserijskaIzvedba KaroserijskaIzvedba
        {
            get => _karoserijskaIzvedba;
            set
            {
                if (_karoserijskaIzvedba != value)
                {
                    _karoserijskaIzvedba = value;
                    OnPropertyChanged(nameof(KaroserijskaIzvedba));
                }
            }
        }

        public Oglas() { }

        public Oglas(string naziv, string opis, double cena, string prodajalec, TipPonudbe ponudba, Starost starost, KaroserijskaIzvedba karoserijskaIzvedba, string znamka, string thumbnailLink = "")
        {
            _naziv = naziv;
            _opis = opis;
            _cena = cena;
            _prodajalec = prodajalec;
            _thumbnailLink = thumbnailLink;
            _ponudba = ponudba;
            _avtoStarost = starost;
            _karoserijskaIzvedba = karoserijskaIzvedba;
            _znamka = znamka;
        }
    }
}
