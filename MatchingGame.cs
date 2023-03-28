﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MatchingGame
{
    public partial class MatchingGame : Form
    {
        private Timer timer;
        private int sekunnit;
        private int minuutit;
        Label firstClicked = null;
        Label secondClicked = null;

        PrivateFontCollection omaFontti = new PrivateFontCollection();

        private bool helppopeliPelaa = false;
        private bool normaalipeliPelaa = false;
        private bool vaikeapeliPelaa = false;

        int score = 0;
        int yritykset = 0;

        int parasHelppoSekunti = 99;
        int parasHelppoMinuutti = 99;

        int parhaatHelpotYritykset = 9999999;
        int helppoHighScore = 0;
        int helpotVoitot = 0;

        int parhaatNormaalitYritykset = 9999999;
        int normaaliHighScore = 0;
        int normaalitVoitot = 0;

        int parhaatVaikeatYritykset = 9999999;
        int vaikeaHighScore = 0;
        int vaikeatVoitot = 0;

        Random random = new Random();

        List<string> helppoIcons = new List<string>();

        List<string> normaaliIcons = new List<string>();

        List<string> vaikeaIcons = new List<string>();

        public MatchingGame()
        {
            InitializeComponent();
            OmaFont();
            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            sekunnit++;

            if (sekunnit == 60)
            {
                sekunnit = 0;
                minuutit++;
            }

            string timeString = $"{minuutit:00}:{sekunnit:00}";
            aikaLabel.Text = timeString;
        }

        // Lisää projektiin custom fontin ja määrittää haluttujen kontrollien fonttia
        private void OmaFont()
        {
            int fontLength = Properties.Resources.NewRocker_Regular.Length;
            byte[] fontdata = Properties.Resources.NewRocker_Regular;
            System.IntPtr data = Marshal.AllocCoTaskMem(fontLength);
            Marshal.Copy(fontdata, 0, data, fontLength);
            omaFontti.AddMemoryFont(data, fontLength);

            aloitanappi.Font = new Font(omaFontti.Families[0], aloitanappi.Font.Size);
            helpponappi.Font = new Font(omaFontti.Families[0], helpponappi.Font.Size);
            normaalinappi.Font = new Font(omaFontti.Families[0], normaalinappi.Font.Size);
            vaikeanappi.Font = new Font(omaFontti.Families[0], vaikeanappi.Font.Size);
            takaisinnappi.Font = new Font(omaFontti.Families[0], takaisinnappi.Font.Size);
            tilastotnappi.Font = new Font(omaFontti.Families[0], tilastotnappi.Font.Size);
            aikaLabel.Font = new Font(omaFontti.Families[0], aikaLabel.Font.Size);
            helppoOtsikko.Font = new Font(omaFontti.Families[0], helppoOtsikko.Font.Size);
            normaaliOtsikko.Font = new Font(omaFontti.Families[0], normaaliOtsikko.Font.Size);
            vaikeaOtsikko.Font = new Font(omaFontti.Families[0], vaikeaOtsikko.Font.Size);
            voitot1.Font = new Font(omaFontti.Families[0], voitot1.Font.Size);
            voitot2.Font = new Font(omaFontti.Families[0], voitot2.Font.Size);
            voitot3.Font = new Font(omaFontti.Families[0], voitot3.Font.Size);
            parhaatPisteet1.Font = new Font(omaFontti.Families[0], parhaatPisteet1.Font.Size);
            parhaatPisteet2.Font = new Font(omaFontti.Families[0], parhaatPisteet2.Font.Size);
            parhaatPisteet3.Font = new Font(omaFontti.Families[0], parhaatPisteet3.Font.Size);
            uusipeliNappi.Font = new Font(omaFontti.Families[0], uusipeliNappi.Font.Size);
            paavalikkoonNappi.Font = new Font(omaFontti.Families[0], paavalikkoonNappi.Font.Size);
        }

        // Antaa korteille satunnaiset kuvat, kun helppo pelimuoto aloitetaan
        // Tämä metodi kutsutaan helpponappi_Click metodissa
        private void JaaHelpotKortit()
        {
            HelppoIconsAdd();
            sekunnit = 0;
            minuutit = 0;
            foreach (Control control in helppopeli.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    int randomNumber = random.Next(helppoIcons.Count);
                    iconLabel.Text = helppoIcons[randomNumber];
                    iconLabel.ForeColor = iconLabel.BackColor;
                    helppoIcons.RemoveAt(randomNumber);
                }
            }
        }

        // Antaa korteille satunnaiset kuvat, kun normaali pelimuoto aloitetaan
        // Tämä metodi kutsutaan normaalinappi_Click metodissa
        private void JaaNormaalitKortit()
        {
            NormaaliIconsAdd();
            sekunnit = 0;
            minuutit = 0;

            foreach (Control control in normaaliPeli.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    int randomNumber = random.Next(normaaliIcons.Count);
                    iconLabel.Text = normaaliIcons[randomNumber];
                    iconLabel.ForeColor = iconLabel.BackColor;
                    normaaliIcons.RemoveAt(randomNumber);
                }
            }
        }

        // Antaa korteille satunnaiset kuvat, kun vaikea pelimuoto aloitetaan
        // Tämä metodi kutsutaan vaikeanappi_Click metodissa
        private void JaaVaikeatKortit()
        {
            VaikeaIconsAdd();
            sekunnit = 0;
            minuutit = 0;

            foreach (Control control in vaikeaPeli.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    int randomNumber = random.Next(vaikeaIcons.Count);
                    iconLabel.Text = vaikeaIcons[randomNumber];
                    iconLabel.ForeColor = iconLabel.BackColor;
                    vaikeaIcons.RemoveAt(randomNumber);
                }
            }
        }

        // Tämä metodi tekee asioita, kun helpossa pelimuodossa painetaan korttia
        private void helppoLabel_Click(object sender, EventArgs e)
        {
            if (helppoTimer.Enabled == true)
                return;

            Label clickedLabel = sender as Label;

            if (clickedLabel != null)
            {
                if (clickedLabel.ForeColor == Color.Black)
                {
                    return;
                }

                if (firstClicked == null)
                {
                    timer.Start();
                    firstClicked = clickedLabel;
                    firstClicked.ForeColor = Color.Black;

                    return;
                }

                secondClicked = clickedLabel;
                secondClicked.ForeColor = Color.Black;

                yritykset++;

                CheckForEasyWinner();

                if (firstClicked.Text != secondClicked.Text)
                {
                    score -= 25;
                }

                if (firstClicked.Text == secondClicked.Text)
                {
                    score += 100;
                    firstClicked = null;
                    secondClicked = null;
                    return;
                }

                helppoTimer.Start();
            }
        }

        // Tämä metodi tekee asioita, kun normaalissa pelimuodossa painetaan korttia
        private void normaaliLabel_Click(object sender, EventArgs e)
        {
            if (normaaliTimer.Enabled == true)
                return;

            Label clickedLabel = sender as Label;

            if (clickedLabel != null)
            {
                if (clickedLabel.ForeColor == Color.Black)
                {
                    return;
                }

                if (firstClicked == null)
                {
                    timer.Start();
                    firstClicked = clickedLabel;
                    firstClicked.ForeColor = Color.Black;

                    return;
                }

                secondClicked = clickedLabel;
                secondClicked.ForeColor = Color.Black;

                yritykset++;

                CheckForNormalWinner();

                if (firstClicked.Text != secondClicked.Text)
                {
                    score -= 25;
                }

                if (firstClicked.Text == secondClicked.Text)
                {
                    score += 100;
                    firstClicked = null;
                    secondClicked = null;
                    return;
                }

                normaaliTimer.Start();
            }
        }

        // Tämä metodi tekee asioita, kun vaikeassa pelimuodossa painetaan korttia
        private void vaikeaLabel_Click(object sender, EventArgs e)
        {
            if (vaikeaTimer.Enabled == true)
                return;

            Label clickedLabel = sender as Label;

            if (clickedLabel != null)
            {
                if (clickedLabel.ForeColor == Color.Black)
                {
                    return;
                }

                if (firstClicked == null)
                {
                    timer.Start();
                    firstClicked = clickedLabel;
                    firstClicked.ForeColor = Color.Black;

                    return;
                }

                secondClicked = clickedLabel;
                secondClicked.ForeColor = Color.Black;

                yritykset++;

                CheckForHardWinner();

                if (firstClicked.Text != secondClicked.Text)
                {
                    score -= 25;
                }

                if (firstClicked.Text == secondClicked.Text)
                {
                    score += 100;
                    firstClicked = null;
                    secondClicked = null;
                    return;
                }

                vaikeaTimer.Start();
            }
        }

        // helppoTimer pitää huolen helpon pelimuodon korttien kääntönopeudesta
        private void helppoTimer_Tick(object sender, EventArgs e)
        {
            helppoTimer.Stop();

            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;

            firstClicked = null;
            secondClicked = null;
        }

        // normaaliTimer pitää huolen normaalin pelimuodon korttien kääntönopeudesta
        private void normaaliTimer_Tick(object sender, EventArgs e)
        {
            normaaliTimer.Stop();

            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;

            firstClicked = null;
            secondClicked = null;
        }

        // vaikeaTimer pitää huolen vaikean pelimuodon korttien kääntönopeudesta
        private void vaikeaTimer_Tick(object sender, EventArgs e)
        {
            vaikeaTimer.Stop();

            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;

            firstClicked = null;
            secondClicked = null;
        }

        // Tämä metodi katsoo, onko helppo pelimuoto voitettu
        // Metodi kutsutaan helppoLabel_Click metodissa, kun molemmat kortit on käännetty
        private void CheckForEasyWinner()
        {
            foreach (Control control in helppopeli.Controls)
            {
                Label iconLabel = control as Label;

                if (iconLabel != null)
                {
                    if (iconLabel.ForeColor == iconLabel.BackColor)
                        return;
                }
            }

            timer.Stop();
            helpotVoitot++;
            score += 200;

            if (yritykset < parhaatHelpotYritykset)
            {
                parhaatHelpotYritykset = yritykset;
            }
            if (score > helppoHighScore)
            {
                helppoHighScore = score;
            }

            MessageBox.Show("You won!\nYritykset: " + yritykset + "\nScore: " + score, "Congratulations!");
            yritykset = 0;
            score = 0;

            paavalikkoonNappi.Visible = true;
            uusipeliNappi.Visible = true;
            // LISÄÄ TÄHÄN KOODI, JOKA VIE PELAAJAN TAKAISIN ALOITUSRUUTUUN
        }

        // Tämä metodi katsoo, onko normaali pelimuoto voitettu
        // Metodi kutsutaan normaaliLabel_Click metodissa, kun molemmat kortit on käännetty
        private void CheckForNormalWinner()
        {
            foreach (Control control in normaaliPeli.Controls)
            {
                Label iconLabel = control as Label;

                if (iconLabel != null)
                {
                    if (iconLabel.ForeColor == iconLabel.BackColor)
                        return;
                }
            }
            timer.Stop();
            normaalitVoitot++;
            score += 200;

            if (yritykset < parhaatNormaalitYritykset)
            {
                parhaatNormaalitYritykset = yritykset;
            }
            if (score > normaaliHighScore)
            {
                normaaliHighScore = score;
            }

            MessageBox.Show("You won!", "Congratulations");
            yritykset = 0;
            score = 0;

            paavalikkoonNappi.Visible = true;
            uusipeliNappi.Visible = true;
            // LISÄÄ TÄHÄN KOODI, JOKA VIE PELAAJAN TAKAISIN ALOITUSRUUTUUN
        }

        // Tämä metodi katsoo, onko vaikea pelimuoto voitettu
        // Metodi kutsutaan vaikeaLabel_Click metodissa, kun molemmat kortit on käännetty
        private void CheckForHardWinner()
        {
            foreach (Control control in vaikeaPeli.Controls)
            {
                Label iconLabel = control as Label;

                if (iconLabel != null)
                {
                    if (iconLabel.ForeColor == iconLabel.BackColor)
                        return;
                }
            }
            timer.Stop();
            vaikeatVoitot++;
            score += 200;

            if (yritykset < parhaatVaikeatYritykset)
            {
                parhaatVaikeatYritykset = yritykset;
            }

            if (score > vaikeaHighScore)
            {
                vaikeaHighScore = score;
            }

            MessageBox.Show("You won!", "Congratulations");
            yritykset = 0;
            score = 0;

            paavalikkoonNappi.Visible = true;
            uusipeliNappi.Visible = true;
            // LISÄÄ TÄHÄN KOODI, JOKA VIE PELAAJAN TAKAISIN ALOITUSRUUTUUN
        }

        // Näyttää pelimuodot, kun "Aloita peli" painiketta painetaan
        private void aloitanappi_Click(object sender, EventArgs e)
        {
            helpponappi.Visible = true;
            normaalinappi.Visible = true;
            vaikeanappi.Visible = true;
            aloitanappi.Visible = false;
            tilastotnappi.Visible = false;
            takaisinnappi.Visible = true;
        }

        // Aloittaa helpon pelimuodon ja piilottaa valikon painikkeet
        private void helpponappi_Click(object sender, EventArgs e)
        {
            aikaLabel.Visible = true;
            helpponappi.Visible = false;
            normaalinappi.Visible = false;
            vaikeanappi.Visible = false;
            helppopeli.Visible = true;
            helppopeliPelaa = true;
            takaisinnappi.Visible = false;
            JaaHelpotKortit();
        }

        // Aloittaa normaalin pelimuodon ja piilottaa valikon painikkeet
        private void normaalinappi_Click(object sender, EventArgs e)
        {
            aikaLabel.Visible = true;
            helpponappi.Visible = false;
            normaalinappi.Visible = false;
            vaikeanappi.Visible = false;
            normaaliPeli.Visible = true;
            normaalipeliPelaa = true;
            takaisinnappi.Visible = false;
            JaaNormaalitKortit();
        }

        // Aloittaa vaikean pelimuodon ja piilottaa valikon painikkeet
        private void vaikeanappi_Click(object sender, EventArgs e)
        {
            aikaLabel.Visible = true;
            helpponappi.Visible = false;
            normaalinappi.Visible = false;
            vaikeanappi.Visible = false;
            vaikeaPeli.Visible = true;
            vaikeapeliPelaa = true;
            takaisinnappi.Visible = false;
            JaaVaikeatKortit();
        }

        // Tämä metodi näyttää helpon pelimuodon ruudukon koon, kun hiiri laitetaan painikkeen päälle
        private void helpponappi_MouseHover(object sender, EventArgs e)
        {
            helppopeli.Visible = true;
        }

        // Tämä metodi piilottaa helpon pelimuodon ruudukon koon, kun hiiri poistetaan painikkeen päältä
        private void helpponappi_MouseLeave(object sender, EventArgs e)
        {
            if (helppopeliPelaa == false)
            {
                helppopeli.Visible = false;
            }
        }

        // Tämä metodi näyttää normaalin pelimuodon ruudukon koon, kun hiiri laitetaan painikkeen päälle
        private void normaalinappi_MouseHover(object sender, EventArgs e)
        {
            normaaliPeli.Visible = true;
        }

        // Tämä metodi piilottaa normaalin pelimuodon ruudukon koon, kun hiiri poistetaan painikkeen päältä
        private void normaalinappi_MouseLeave(object sender, EventArgs e)
        {
            if (normaalipeliPelaa == false)
            {
                normaaliPeli.Visible = false;
            }
        }

        // Tämä metodi näyttää vaikean pelimuodon ruudukon koon, kun hiiri laitetaan painikkeen päälle
        private void vaikeanappi_MouseHover(object sender, EventArgs e)
        {
            vaikeaPeli.Visible = true;
        }

        // Tämä metodi piilottaa vaikean pelimuodon ruudukon koon, kun hiiri poistetaan painikkeen päältä
        private void vaikeanappi_MouseLeave(object sender, EventArgs e)
        {
            if (vaikeapeliPelaa == false)
            {
                vaikeaPeli.Visible = false;
            }
        }

        // Tämä metodi vie käyttäjän tilastoihin, piilottaa valikon painikkeet ja näyttää "takaisin" painikkeen
        private void tilastotnappi_Click(object sender, EventArgs e)
        {
            aloitanappi.Visible = false;
            tilastotnappi.Visible = false;
            takaisinnappi.Visible = true;
            helppoOtsikko.Visible = true;
            normaaliOtsikko.Visible = true;
            vaikeaOtsikko.Visible = true;
            voitot1.Visible = true;
            voitot2.Visible = true;
            voitot3.Visible = true;
            parhaatPisteet1.Visible = true;
            parhaatPisteet2.Visible = true;
            parhaatPisteet3.Visible = true;
        }

        // Tämä metodi vie käyttäjän takaisin edelliseen valikkoon
        private void takaisinnappi_Click(object sender, EventArgs e)
        {
            takaisinnappi.Visible = false;
            aloitanappi.Visible = true;
            tilastotnappi.Visible = true;
            if (helpponappi.Visible == true)
            {
                takaisinnappi.Visible = false;
                aloitanappi.Visible = true;
                helpponappi.Visible = false;
                normaalinappi.Visible = false;
                vaikeanappi.Visible = false;
            }
            helppoOtsikko.Visible = false;
            normaaliOtsikko.Visible = false;
            vaikeaOtsikko.Visible = false;
            voitot1.Visible = false;
            voitot2.Visible = false;
            voitot3.Visible = false;
            parhaatPisteet1.Visible = false;
            parhaatPisteet2.Visible = false;
            parhaatPisteet3.Visible = false;
        }

        private void paavalikkoonNappi_Click(object sender, EventArgs e)
        {
            tilastotnappi.Visible = true;
            aloitanappi.Visible = true;
            paavalikkoonNappi.Visible = false;
            uusipeliNappi.Visible = false;
            helppopeli.Visible = false;
            normaaliPeli.Visible = false;
            vaikeaPeli.Visible = false;
            aikaLabel.Visible = false;
            helppopeliPelaa = false;
            normaalipeliPelaa = false;
            vaikeapeliPelaa = false;
        }

        private void uusipeliNappi_Click(object sender, EventArgs e)
        {
            if (helppopeliPelaa == true)
            {
                JaaHelpotKortit();
                paavalikkoonNappi.Visible = false;
                uusipeliNappi.Visible = false;
            }
            if (normaalipeliPelaa == true)
            {
                JaaNormaalitKortit();
                paavalikkoonNappi.Visible = false;
                uusipeliNappi.Visible = false;
            }
            if (vaikeapeliPelaa == true)
            {
                JaaVaikeatKortit();
                paavalikkoonNappi.Visible = false;
                uusipeliNappi.Visible = false;
            }
        }

        private void HelppoIconsAdd()
        {
            helppoIcons.Add("a");
            helppoIcons.Add("a");
            helppoIcons.Add("b");
            helppoIcons.Add("b");
            helppoIcons.Add("c");
            helppoIcons.Add("c");
            helppoIcons.Add("d");
            helppoIcons.Add("d");
            helppoIcons.Add("e");
            helppoIcons.Add("e");
            helppoIcons.Add("f");
            helppoIcons.Add("f");
            helppoIcons.Add("g");
            helppoIcons.Add("g");
            helppoIcons.Add("h");
            helppoIcons.Add("h");
        }

        private void NormaaliIconsAdd()
        {
            normaaliIcons.Add("a");
            normaaliIcons.Add("a");
            normaaliIcons.Add("b");
            normaaliIcons.Add("b");
            normaaliIcons.Add("c");
            normaaliIcons.Add("c");
            normaaliIcons.Add("d");
            normaaliIcons.Add("d");
            normaaliIcons.Add("e");
            normaaliIcons.Add("e");
            normaaliIcons.Add("f");
            normaaliIcons.Add("f");
            normaaliIcons.Add("g");
            normaaliIcons.Add("g");
            normaaliIcons.Add("h");
            normaaliIcons.Add("h");
            normaaliIcons.Add("i");
            normaaliIcons.Add("i");
            normaaliIcons.Add("j");
            normaaliIcons.Add("j");
            normaaliIcons.Add("k");
            normaaliIcons.Add("k");
            normaaliIcons.Add("l");
            normaaliIcons.Add("l");
        }

        private void VaikeaIconsAdd()
        {
            vaikeaIcons.Add("a");
            vaikeaIcons.Add("a");
            vaikeaIcons.Add("b");
            vaikeaIcons.Add("b");
            vaikeaIcons.Add("c");
            vaikeaIcons.Add("c");
            vaikeaIcons.Add("d");
            vaikeaIcons.Add("d");
            vaikeaIcons.Add("e");
            vaikeaIcons.Add("e");
            vaikeaIcons.Add("f");
            vaikeaIcons.Add("f");
            vaikeaIcons.Add("g");
            vaikeaIcons.Add("g");
            vaikeaIcons.Add("h");
            vaikeaIcons.Add("h");
            vaikeaIcons.Add("i");
            vaikeaIcons.Add("i");
            vaikeaIcons.Add("j");
            vaikeaIcons.Add("j");
            vaikeaIcons.Add("k");
            vaikeaIcons.Add("k");
            vaikeaIcons.Add("l");
            vaikeaIcons.Add("l");
            vaikeaIcons.Add("m");
            vaikeaIcons.Add("m");
            vaikeaIcons.Add("n");
            vaikeaIcons.Add("n");
            vaikeaIcons.Add("o");
            vaikeaIcons.Add("o");
            vaikeaIcons.Add("p");
            vaikeaIcons.Add("p");
            vaikeaIcons.Add("q");
            vaikeaIcons.Add("q");
            vaikeaIcons.Add("r");
            vaikeaIcons.Add("r");
        }
    }
}
