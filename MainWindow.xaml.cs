using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Wumpus_171220087_AhmetCanAydemir
{
    public partial class MainWindow : Window
    {
		Oyun Oyun;
        public MainWindow()
        {
            InitializeComponent();

			Oyun = new Oyun();
			Oyun.OyunuBaslat();
			EkraniOlustur();
        }

		private void OyunBilgileriniDoldur()
		{
			chkSonrakiAdimdaOkAtilsin.IsEnabled = Oyun.Oyuncu.KalanOk > 0;
			txtKalanOk.Text = Oyun.Oyuncu.KalanOk.ToString();
			chkSonrakiAdimdaOkAtilsin.IsChecked = Oyun.Oyuncu.OkAtilacak;
			txtPuan.Text = Oyun.Oyuncu.Puan.ToString();
		}

		private void EkraniOlustur()
        {
			OyunBilgileriniDoldur();
			Alan.Children.Clear();
			Alan.RowDefinitions.Clear();
			Alan.ColumnDefinitions.Clear();
			for (int i = 0; i < Oyun.Boyut + 1; i++)
            {

				// İlk satır ve ilk sütun rakamlar olacağı için küçük olmasını istiyoruz
				var gridLength = i == 0 ? new GridLength(30) : new GridLength(1,GridUnitType.Star);
				
				Alan.ColumnDefinitions.Add(new ColumnDefinition() { Width = gridLength });
                Alan.RowDefinitions.Add(new RowDefinition() { Height = gridLength });
            }

			// Satır Numaraları
			for (int i = 1; i <= Oyun.Boyut; i++)
			{
				var labelSatir = new Label() { Content = i ,HorizontalAlignment = HorizontalAlignment.Right};
				var labelSutun = new Label() { Content = i , VerticalAlignment = VerticalAlignment.Bottom };

				Grid.SetColumn(labelSatir, 0);
				Grid.SetRow(labelSatir, i);
				Alan.Children.Add(labelSatir);

				Grid.SetColumn(labelSutun, i);
				Grid.SetRow(labelSutun, 0);
				Alan.Children.Add(labelSutun);
			}

			for (int i =0; i< Oyun.Boyut;i++)
			{
				for (int j = 0; j < Oyun.Boyut; j++)
				{
					var kare = Oyun.Kareler[j, i];

					// Kare henüz ziyaret edilmedi veya karetipi güvenli olduğu bilinmiyorsa ne olduğu görünmesin diye karartılıyor.
					if (!kare.ZiyaretEdildi)
					{
						if (!kare.GuvenliMi)
						{
							var border = new Border()
							{
								Background = new SolidColorBrush(Color.FromRgb(55, 55, 55))
							};
							Grid.SetRow(border, i + 1);
							Grid.SetColumn(border, j + 1);
							Alan.Children.Add(border);
						}
						if (kare.GuvenliMi) 
						{
							var border = new Border()
							{
								Background = new SolidColorBrush(Color.FromRgb(239, 255, 191)),
								
							};
							TextBlock yazi = new TextBlock()
							{
								Text = "Güvenli",
								TextAlignment = TextAlignment.Center,
								HorizontalAlignment = HorizontalAlignment.Center,
								VerticalAlignment = VerticalAlignment.Center,
								Foreground = new SolidColorBrush(Color.FromRgb(54, 217, 0)),
							};
							border.Child = yazi;
							Grid.SetRow(border, i + 1);
							Grid.SetColumn(border, j + 1);
							Alan.Children.Add(border);
						}

						continue;

					}

					Grid panel = new Grid();
					Grid.SetRow(panel, i + 1);
					Grid.SetColumn(panel, j + 1);

					if (kare.Esinti)
					{
						ResimYerlestir(panel, "esinti");
					}
					if (kare.Koku)
					{
						ResimYerlestir(panel, "koku");
					}
					if (Oyun.OyuncununBulunduguKare == kare)
					{

						if (Oyun.Oyuncu.Olu)
						{
							ResimYerlestir(panel, "karakter_olu");
						}
						else if (Oyun.Oyuncu.OkAtilacak)
						{
							ResimYerlestir(panel, "karakter");
						}
						else if (!Oyun.Oyuncu.OkAtilacak)
						{
							ResimYerlestir(panel, "karakter_oksuz");
						}
					}
					if (kare.KareTipi == KareTipi.Wumpus)
					{
						if (kare.BuKaredekiWumpusOlu)
							ResimYerlestir(panel, "wumpus_olu");
						else
							ResimYerlestir(panel, "wumpus");
					}
					if (kare.KareTipi == KareTipi.Altin)
					{
						ResimYerlestir(panel, "altin");
					}
					if (kare.KareTipi == KareTipi.Cukur)
					{
						ResimYerlestir(panel, "cukur");
					}

					Alan.Children.Add(panel);
				}
			}
			if(Oyun.OyuncununBulunduguKare == Oyun.Kareler[0,0] && Oyun.AltinAdet == Oyun.Oyuncu.ToplananAltinSayisi)
			{
				OyunBitti("Tebrikler tüm altınları toplayıp oyunu kazandın! Puan: " + txtPuan.Text + "\n Oyun yeniden başlatılsın mı?");
			}
			else if(Oyun.Oyuncu.Olu)
			{
				OyunBitti("Karakteriniz öldü. Puan: " + txtPuan.Text + "\n Oyun yeniden başlatılsın mı?");
			}

		}

		/// <summary>
		/// Klavye işlemleri
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// 

		private void Alan_KeyDown(object sender, KeyEventArgs e)
		{
			if (Oyun.Oyuncu.Olu)
				return;
			// Basılan tuşla ilgilenmiyorsak return ediyoruz.
			if (e.Key != System.Windows.Input.Key.Right
				&& e.Key != System.Windows.Input.Key.Left
				&& e.Key != System.Windows.Input.Key.Up
				&& e.Key != System.Windows.Input.Key.Down
				&& e.Key != System.Windows.Input.Key.Space
				&& e.Key != System.Windows.Input.Key.A)
				return;


			int eskiOkSayisi = Oyun.Oyuncu.KalanOk;
			if (e.Key == System.Windows.Input.Key.Right)
				Oyun.Oyuncu.SagaGit();
			else if (e.Key == System.Windows.Input.Key.Left)
				Oyun.Oyuncu.SolaGit();
			else if (e.Key == System.Windows.Input.Key.Up)
				Oyun.Oyuncu.YukariGit();
			else if (e.Key == System.Windows.Input.Key.Down)
				Oyun.Oyuncu.AsagiGit();
			else if (e.Key == System.Windows.Input.Key.Space)
			{
				if (chkSonrakiAdimdaOkAtilsin.IsEnabled)
				{
					chkSonrakiAdimdaOkAtilsin.IsChecked = !chkSonrakiAdimdaOkAtilsin.IsChecked;

					if (chkSonrakiAdimdaOkAtilsin.IsChecked ?? false)
						Oyun.Oyuncu.OkuHazirla();
					else
						Oyun.Oyuncu.OkAtilacak = false;
				}
			}
			else if (e.Key == System.Windows.Input.Key.A)
			{
				// Eğer kare'de altın varsa altını yerden alıyoruz.
				if (Oyun.OyuncununBulunduguKare.KareTipi == KareTipi.Altin)
				{
					Oyun.Oyuncu.AltiniAl();
					Oyun.OyuncununBulunduguKare.KareTipi = KareTipi.Bilinmiyor;
				}
			}

			// Oyuncunun yeni konumu ziyaret edildi olarak işaretleniyor.
			Oyun.OyuncununBulunduguKare.ZiyaretEdildi = true;
			e.Handled = true;

			if(Oyun.OyuncununBulunduguKare.KareTipi == KareTipi.Cukur)
			{
				Oyun.Oyuncu.Olu = true;

			}
			else if(Oyun.OyuncununBulunduguKare.KareTipi == KareTipi.Wumpus && !Oyun.OyuncununBulunduguKare.BuKaredekiWumpusOlu)
			{
				// Eğer ok atıldıysa wumpus ölmüş demektir.
				if (eskiOkSayisi > Oyun.Oyuncu.KalanOk)
					Oyun.OyuncununBulunduguKare.BuKaredekiWumpusOlu = true;
				else
					Oyun.Oyuncu.Olu = true;
			}

			// Hareket ettikten sonra UI yenileniyor.

			// Güvenli mi kontrol et
			rchKontrolLog.CaretPosition = rchKontrolLog.CaretPosition.GetPositionAtOffset(0, LogicalDirection.Backward);
			rchKontrolLog.CaretPosition.InsertTextInRun(Oyun.EtrafindakilerGuvenliMi());
			EkraniOlustur();
		}
		private void OyunBitti(string mesaj)
		{
			var sonuc = MessageBox.Show(mesaj, "Oyun Bitti", MessageBoxButton.YesNo, MessageBoxImage.Information);
			if(sonuc == MessageBoxResult.Yes)
			{
				YenidenBaslat();
			}
		}
		private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			
		}
		private void ResimYerlestir(Grid panel,string resimAdi)
		{
			panel.RowDefinitions.Add(new RowDefinition());

			var img = new Image()
			{
				Source = new BitmapImage(new Uri($"/Resim/{resimAdi}.png", UriKind.Relative)),
			};

			Grid.SetRow(img, panel.RowDefinitions.Count - 1);
			panel.Children.Add(img);
		}
		/// <summary>
		/// Oyun yeniden başlatılıyor.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnYenidenBaslat_Click(object sender, RoutedEventArgs e)
		{
			YenidenBaslat();
		}
		private void YenidenBaslat()
		{
			Oyun = new Oyun()
			{
				AltinAdet = int.Parse(txtAltinSayisi.Text),
				CukurAdet = int.Parse(txtCukurSayisi.Text),
				WumpusAdet = int.Parse(txtWumpusSayisi.Text),
				Boyut = int.Parse(txtKareSayisi.Text),
			};
			TextRange txt = new TextRange(rchKontrolLog.Document.ContentStart, rchKontrolLog.Document.ContentEnd);
			txt.Text = "";
			Oyun.OyunuBaslat();
			Alan.Focus();
			EkraniOlustur();
		}

		private void Alan_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			Alan.Focus();
		}

		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			Alan.Focus();

		}

		private void btnYenidenBaslat_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			e.Handled = true;
		}

		
	}
}
